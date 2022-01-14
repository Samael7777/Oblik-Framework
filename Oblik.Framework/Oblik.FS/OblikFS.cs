using System;
using System.Collections.Generic;
using Oblik.Driver;

namespace Oblik.FS
{
    public class OblikFS : IOblikFS
    {
        /*-------------------------Private--------------------------------------*/
        /// <summary>
        /// Флаг доступа к сегменту
        /// </summary>
        private enum Access
        {
            /// <summary>
            /// Доступ на чтение
            /// </summary>
            Read = 0,
            /// <summary>
            /// Доступ на запись
            /// </summary>
            Write = 1,
        }

        /// <summary>
        /// Массив данных L1
        /// </summary>
        private byte[] l1;

        /// <summary>
        /// Массив данных L2
        /// </summary>
        private byte[] l2;

        private readonly ConnectionParams connectionParams;

        private readonly IOblikDriver oblikDriver;

        /*-------------------------Constructors---------------------------------*/
        public OblikFS(ConnectionParams connectionParams, IOblikDriver oblikDriver)
        {
            this.oblikDriver = oblikDriver;
            this.connectionParams = connectionParams;
            l1 = new byte[0];
            l2 = new byte[0];
        }

        /*-------------------------Public---------------------------------------*/
        /// <summary>
        /// Драйвер счетчика
        /// </summary>
        public IOblikDriver OblikDriver { get => oblikDriver; }
        
        public int Baudrate 
        { 
            get => connectionParams.Baudrate;
            set => connectionParams.Baudrate = value; 
        }

        public int Address
        {
            get => connectionParams.Address;
            set => connectionParams.Address = (byte)value;
        }

        public int Timeout
        {
            get => connectionParams.Timeout;
            set => connectionParams.Timeout = value;
        }

        public UserLevel User
        {
            get => connectionParams.User;
            set => connectionParams.User = value;
        }
        public string Password
        {
            get => connectionParams.Password;
            set => connectionParams.Password = value;
        }
        /// <summary>
        /// Чтение части сегмента счетчика
        /// </summary>
        /// <param name="segment">Сегмент счетчика</param>
        /// <param name="offset">Смещение относительно начала сегмента</param>
        /// <param name="len">Количество данных для чтения</param>
        /// <returns>Успех операции</returns>
        public byte[] ReadSegment(int segment, int offset, int len)
        {
            PerformFrame(connectionParams.Address, (byte)segment, (ushort)offset, (byte)len, Access.Read);
            return oblikDriver.Request(l1, connectionParams.Baudrate, connectionParams.Timeout);
        }

        /// <summary>
        /// Чтение части сегмента счетчика
        /// </summary>
        /// <param name="segment">Сегмент счетчика</param>
        /// <param name="offset">Смещение относительно начала сегмента</param>
        /// <param name="len">Количество данных для чтения</param>
        /// <param name="packetsize">Размер пакета данных</param>
        /// <returns></returns>
        public byte[] ReadSegment(int segment, int offset, int len, int packetsize)
        {
            int maxPacket = 250 / packetsize;   //Максимальный размер пакета L2 в пакетах запроса
            int packetsLeft = len / packetsize; //Осталось пакетов для чтения
            int nextOffset = offset;            //Смещение следующего чтения
            byte[] result = new byte[len];
            int index = 0;                      //Смещение массива данных
            while (packetsLeft > 0)
            {
                int queryPackets = (packetsLeft > maxPacket) ? maxPacket : packetsLeft;
                int queryBytes = queryPackets * packetsize;
                byte[] answer = ReadSegment(segment, nextOffset, queryBytes);
                //Сохранение результата
                Array.Copy(answer, 0, result, index, queryPackets * packetsize);
                index += queryBytes;
                nextOffset += queryBytes;
                packetsLeft -= queryPackets;   
            }
            return result;
        }

        /// <summary>
        /// Запись в сегмент счетчика
        /// </summary>
        /// <param name="segment">Сегмент счетчика</param>
        /// <param name="offset">Смещение относительно начала сегмента</param>
        /// <param name="data">Данные для записи</param>
        public void WriteSegment(int segment, int offset, byte[] data)
        {           
            //Проверка на наличие данных для записи
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (data.Length > 255)
            {
                throw new ArgumentOutOfRangeException("Lenght must be below or equal 255");
            }

            byte address = ReadSegment(66, 0, 1)[0]; //Получение адреса RS-485 счетчика
           
            PerformFrame(address, (byte)segment, (ushort)offset, (byte)data.Length, Access.Write, data); //Подготовка фрейма L2
            oblikDriver.Request(l1, connectionParams.Baudrate, connectionParams.Timeout);
        }

        /// <summary>
        /// Запись в сегмент счетчика
        /// </summary>
        /// <param name="segment">Сегмент счетчика</param>
        /// <param name="offset">Смещение относительно начала сегмента</param>
        /// <param name="packetsize">Размер пакета данных</param>
        /// <param name="data">Данные для записи</param>
        public void WriteSegment(int segment, int offset, int packetsize, byte[] data)
        {
            //Проверка на наличие данных для записи
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            int maxPacket = 250 / packetsize;           //Максимальный размер пакета L2 в пакетах запроса
            int packetsLeft = data.Length / packetsize; //Осталось пакетов для записи
            int nextOffset = offset;                    //Смещение следующей записи
            int index = 0;                              //Смещение массива данных
            byte address = ReadSegment(66, 0, 1)[0];    //Получение адреса RS-485 счетчика
            while (packetsLeft > 0)
            {
                int queryPackets = (packetsLeft > maxPacket) ? maxPacket : packetsLeft;
                int queryBytes = queryPackets * packetsize;
                byte[] buffer = new byte[queryBytes];
                Array.Copy(data, index, buffer, 0, queryBytes);
                PerformFrame(address, (byte)segment, (ushort)nextOffset, (byte)queryBytes, Access.Write, buffer); //Подготовка фрейма L2
                oblikDriver.Request(l1, connectionParams.Baudrate, connectionParams.Timeout);
                index += queryBytes;
                nextOffset += queryBytes;
                packetsLeft -= queryPackets;
            }

        }
        #region Подготовка фреймов
        /// <summary>
        /// Процедура шифрования данных L2
        /// </summary>
        private void Encode()
        {
            //Шифрование полей "Данные" и "Пароль"
            byte[] password = new byte[8];         //Пароль
            Array.Copy(connectionParams.PasswordArray, password, 8);
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

        /// <summary>
        /// Подготовка фрейма запроса к счетчику
        /// </summary>
        /// <param name="address">Реальный адрес счетчика</param>
        /// <param name="segment">Сегмент счетчика</param>
        /// <param name="offset">Смещение относительно начала сегмента</param>
        /// <param name="len">Количество данных для чтения/записи</param>
        /// <param name="data">Данные для записи</param>
        /// <param name="access">Доступ на запись (1) или чтение (0)</param>
        /// <returns></returns>
        private void PerformFrame(byte address, byte segment, UInt16 offset, byte len, Access access, byte[] data = null)
        {
            //Формируем запрос L2
            l2 = new byte[5 + (len + 8) * (byte)access];                   //5 байт заголовка + 8 байт пароля + данные 
            l2[0] = (byte)((segment & 127) + (byte)access * 128);          //(биты 0 - 6 - номер сегмента, бит 7 = 1 - операция записи)
            l2[1] = (byte)connectionParams.User;                           //Указываем уровень доступа
            l2[2] = (byte)(offset >> 8);                                   //Старший байт смещения
            l2[3] = (byte)(offset & 0xFF);                                 //Младший байт смещения
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
            l1[2] = (oblikDriver.IsDirectConnected) ? (byte)0 : address; //                                             //Адрес счетчика
            l1[3] = (byte)(3 + l2.Length);                                  //Длина пакета L1 без ключей
            Array.Copy(l2, 0, l1, 4, l2.Length);                            //Вставляем запрос L2 в пакет L1

            //Вычисление контрольной суммы, побайтовое XOR, от поля "Адрес" до поля "L2"
            l1[l1.Length - 1] = 0;
            for (int i = 2; i < (l1.Length - 1); i++)
            {
                l1[l1.Length - 1] ^= l1[i];
            }
        }
        #endregion

        
    }
}