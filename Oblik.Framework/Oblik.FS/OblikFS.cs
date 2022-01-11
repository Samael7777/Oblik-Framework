using System;
using System.Collections.Generic;
using Oblik.Driver;

namespace Oblik.FS
{
    public partial class OblikFS : IOblikFS
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

        private ConnectionParams connectionParams;

        private IOblikDriver oblikDriver;

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
        public IOblikDriver OblikDriver { get => OblikDriver; }
        
        public int Baudrate
        {
            get => oblikDriver.Baudrate;
            set => oblikDriver.Baudrate = value;
        }

        public int Address
        {
            get => connectionParams.Address;
            set => connectionParams.Address = (byte)value;
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
            byte[] result = new byte[len];
            PerformFrame(connectionParams.Address, (byte)segment, (ushort)offset, (byte)len, Access.Read);
            byte[] buffer = oblikDriver.Request(l1);
            DecodeSegmentError(buffer[0]);      //Проверка на ошибки L2
            //Копирование данных без заголовков L2
            Array.Copy(buffer, 2, result, 0, len);
            return result;
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
            byte[] answer = oblikDriver.Request(l1);
            //Проверка на ошибки L2
            DecodeSegmentError(answer[0]);
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
                byte[] answer = oblikDriver.Request(l1);
                //Проверка на ошибки L2
                DecodeSegmentError(answer[0]);
                index += queryBytes;
                nextOffset += queryBytes;
                packetsLeft -= queryPackets;
            }

        }
    }
}