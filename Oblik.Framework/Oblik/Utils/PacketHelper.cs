using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oblik
{
    internal static class PacketHelper
    {

        /// <summary>
        /// Парсер ошибок L1, при ошибке вызывает исключение OblikIOException
        /// </summary>
        /// <param name="errorcode">Код ошибки L1</param>
        internal static void CheckL1Error(int errorcode)
        {
            switch (errorcode)
            {
                case 1:
                    break;  //No errors
                case 0xff:
                    throw new OblikIOException("L1 Checksum Error", Error.L1CSCError);
                case 0xfe:
                    throw new OblikIOException("L1 Meter input buffer overflow", Error.L1BufOverfowError);
                default:
                    throw new OblikIOException("L1 Unknown error", Error.L1UnkErrror);
            }
        }

        /// <summary>
        /// Парсер ошибок L2, при ошибке вызывает исключение OblikIOException
        /// </summary>
        /// <param name="errorcode">Код ошибки L2</param>
        internal static void CheckL2Error(int errorcode)
        {
            switch (errorcode)
            {
                case 0:
                    break;      //No errors
                case 0xff:
                    throw new OblikIOException("L2 request error", Error.L2ReqError);
                case 0xfe:
                    throw new OblikIOException("L2 segment ID error", Error.L2SegIdError);
                case 0xfd:
                    throw new OblikIOException("L2 segment operation error", Error.L2SegOpError);
                case 0xfc:
                    throw new OblikIOException("L2 user access level error", Error.L2UserAcsError);
                case 0xfb:
                    throw new OblikIOException("L2 data permission error", Error.L2DataPermisError);
                case 0xfa:
                    throw new OblikIOException("L2 segmnt offset error", Error.L2SegOfstError);
                case 0xf9:
                    throw new OblikIOException("L2 write request error", Error.L2WrReqError);
                case 0xf8:
                    throw new OblikIOException("L2 data lenght must be above zero", Error.L2DataLenError);
                case 0xf7:
                    throw new OblikIOException("L2 wrong password", Error.L2PwdError);
                case 0xf6:
                    throw new OblikIOException("L2 daygraph clear command is wrong", Error.L2DGCleanError);
                case 0xf5:
                    throw new OblikIOException("L2 password change restricted", Error.L2PwdChngError);
                default:
                    throw new OblikIOException("L2 unknown error", Error.L2UnkError);
            }
        }

        /// <summary>
        /// Подготовка фрейма запроса к счетчику
        /// </summary>
        /// <param name="address">Адрес счетчика в сети RS-485</param>
        /// <param name="segment">Запрашиваемый сегмент</param>
        /// <param name="offset">Смещение относительно начала сегмента</param>
        /// <param name="len">Количество данных для чтения/записи</param>
        /// <param name="access">Доступ на запись (1) или чтение (0)</param>
        /// <param name="user">Права доступа к сегменту</param>
        /// <param name="password"></param>
        /// <param name="data">Данные для записи</param>
        /// <returns>Сформированный пакеи запроса L1</returns>
        internal static byte[] CreatePacket(byte address, byte segment, ushort offset, byte len, byte access, UserLevel user, string password = "", byte[] data = null)
        {          
            access &= 1; //Маскировка лишних бит
            //Формируем запрос L2
            byte[] l2 = new byte[5 + (len + 8) * access];                  //5 байт заголовка + 8 байт пароля + данные 
            l2[0] = (byte)((segment & 127) + access * 128);                //(биты 0 - 6 - номер сегмента, бит 7 = 1 - операция записи)
            l2[1] = (byte)user;                                            //Указываем уровень доступа
            l2[2] = (byte)(offset >> 8);                                   //Старший байт смещения
            l2[3] = (byte)(offset & 0xFF);                                 //Младший байт смещения
            l2[4] = len;                                                   //Размер считываемых данных

            //В случае записи в сегмент
            if (access == 1)
            {
                //Подготовка пароля
                password.Trim();                                            //Очистка от мусора
                password = password.PadLeft(8, (char)0).Substring(0, 8);    //Формирование длины в 8 символов
                byte[] pwdBytes = Encoding.Default.GetBytes(password);      //Формирование массива байт с паролем
                Array.Copy(data, 0, l2, 5, len);                            //Копируем данные в L2
                Array.Copy(pwdBytes, 0, l2, len + 5, 8);                    //Копируем пароль в L2
                Encode(ref l2, pwdBytes);                                   //Шифруем данные и пароль L2
            }

            //Формируем фрейм L1
            byte[] l1 = new byte[5 + l2.Length];
            l1[0] = 0xA5;                                                   //Заголовок пакета
            l1[1] = 0x5A;                                                   //Заголовок пакета
            l1[2] = address;                                                //Адрес счетчика
            l1[3] = (byte)(3 + l2.Length);                                  //Длина пакета L1 без ключей
            Array.Copy(l2, 0, l1, 4, l2.Length);                            //Вставляем запрос L2 в пакет L1

            //Вычисление контрольной суммы, побайтовое XOR, от поля "Адрес" до поля "L2"
            l1[l1.Length - 1] = 0;
            for (int i = 2; i < (l1.Length - 1); i++)
            {
                l1[l1.Length - 1] ^= l1[i];
            }
            
            return l1;
        }

        /// <summary>
        /// Процедура шифрования данных L2
        /// </summary>
        /// <param name="l2">Паект L2</param>
        /// <param name="password">Массив с паролем</param>
        private static void Encode(ref byte[] l2, byte[] password)
        {
            //Шифрование полей "Данные" и "Пароль"
            byte x1 = 0x3A;
            for (int i = 0; i <= 7; i++) { x1 ^= password[i]; }
            int dpcsize = l2[4] + 8;                                //Размер "Данные + "Пароль" 
            int index = 4;
            for (int i = dpcsize - 1; i >= 0; i--)
            {
                byte x2 = l2[index++];
                l2[index] ^= x1;
                l2[index] ^= x2;
                l2[index] ^= password[i % 8];
                x1 += (byte)i;
            }
        }
    }
}