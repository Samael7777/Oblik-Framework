using System;

namespace Oblik.FS
{
    public partial class OblikFS
    {
        /// <summary>
        /// Чтение сегмента счетчика
        /// </summary>
        /// <param name="segment">Сегмент счетчика</param>
        /// <param name="offset">Смещение относительно начала сегмента</param>
        /// <param name="len">Количество данных для чтения</param>
        /// <param name="data">Полученные данные</param>
        /// <returns>Успех операции</returns>
        public byte[] ReadSegment(int segment, int offset, int len)
        {
            //Обрезка аргументов
            segment &= 0xFF;
            offset &= 0xFFFF;
            len &= 0xFF;

            byte[] result = new byte[0];
            PerformFrame((byte)segment, (ushort)offset, (byte)len, Access.Read);
            byte[] answer = oblikDriver.Request(l1);
            //Проверка на ошибки L2
            if (answer[0] != 0)
            {
                throw new OblikIOException(DecodeSegmentError(answer[2]));
            }

            Array.Copy(answer, 2, result, 0, answer[1]);
            return result;                     
        }

        /// <summary>
        /// Запись в сегмент счетчика
        /// </summary>
        /// <param name="segment">Сегмент счетчика</param>
        /// <param name="offset">Смещение относительно начала сегмента</param>
        /// <param name="data">Данные для записи</param>
        /// <returns>Успех операции</returns>
        public void WriteSegment(int segment, int offset, byte[] data)
        {
            //Обрезка аргументов
            segment &= 0xFF;
            offset &= 0xFFFF;

            //Проверка на наличие данных для записи
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }
            PerformFrame((byte)segment, (ushort)offset, (byte)data.Length, Access.Write, data);
            byte[] answer = oblikDriver.Request(l1);
            //Проверка на ошибки L2
            if (answer[0] != 0)
            {
                throw new OblikIOException(DecodeSegmentError(answer[2]));
            }
        }

        /// <summary>
        /// Процедура шифрования данных L2
        /// </summary>
        private void Encode()
        {
            //Шифрование полей "Данные" и "Пароль"
            byte[] passwd = connectionParams.PasswordArray;         //Пароль
            byte x1 = 0x3A;
            for (int i = 0; i <= 7; i++) { x1 ^= passwd[i]; }
            int dpcsize = l2[4] + 8;                                //Размер "Данные + "Пароль" 
            int k = 4;
            for (int i = dpcsize - 1; i >= 0; i--)
            {
                byte x2 = l2[k++];
                l2[k] ^= x1;
                l2[k] ^= x2;
                l2[k] ^= passwd[i % 8];
                x1 += (byte)i;
            }
        }

        /// <summary>
        /// Подготовка фрейма запроса к счетчику
        /// </summary>
        /// <param name="segment">Сегмент счетчика</param>
        /// <param name="offset">Смещение относительно начала сегмента</param>
        /// <param name="len">Количество данных для чтения/записи</param>
        /// <param name="data">Данные для записи</param>
        /// <param name="access">Доступ на запись (1) или чтение (0)</param>
        /// <returns></returns>
        private void PerformFrame(byte segment, UInt16 offset, byte len, Access access, byte[] data = null)
        {
            //Формируем запрос L2
            l2 = new byte[5 + (len + 8) * (int)access];                    //5 байт заголовка + 8 байт пароля + данные 
            l2[0] = (byte)((segment & 127) + (int)access * 128);           //(биты 0 - 6 - номер сегмента, бит 7 = 1 - операция записи)
            l2[1] = (byte)connectionParams.User;                           //Указываем уровень доступа
            l2[2] = BitConverter.GetBytes(offset)[1];                      //Старший байт смещения
            l2[3] = BitConverter.GetBytes(offset)[0];                      //Младший байт смещения
            l2[4] = len;                                                   //Размер считываемых данных

            //В случае записи в сегмент
            if (access == Access.Write)
            {
                Array.Copy(data, 0, l2, 5, len);                                //Копируем данные в L2
                Array.Copy(connectionParams.PasswordArray, 0, l2, len + 5, 8);  //Копируем пароль в L2
                Encode();                                                       //Шифруем данные и пароль L2
            }

            //Формируем фрейм L1
            l1 = new byte[5 + l2.Length];
            l1[0] = 0xA5;                                                   //Заголовок пакета
            l1[1] = 0x5A;                                                   //Заголовок пакета
            l1[2] = (byte)(connectionParams.Address & 0xff);                //Адрес счетчика
            l1[3] = (byte)(3 + l2.Length);                                  //Длина пакета L1 без ключей
            Array.Copy(l2, 0, l1, 4, l2.Length);                            //Вставляем запрос L2 в пакет L1

            //Вычисление контрольной суммы, побайтовое XOR, от поля "Адрес" до поля "L2"
            l1[l1.Length - 1] = 0;
            for (int i = 2; i < (l1.Length - 1); i++)
            {
                l1[l1.Length - 1] ^= (byte)l1[i];
            }

        }

    }
}