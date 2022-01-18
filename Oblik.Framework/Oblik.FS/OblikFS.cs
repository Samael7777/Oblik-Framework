using System;
using System.Collections.Generic;
using Oblik.Driver;

namespace Oblik.FS
{
    public class OblikFS : IOblikFS
    {
        /*-------------------------Private--------------------------------------*/

        private readonly ConnectionParams connectionParams;

        private readonly IOblikDriver oblikDriver;

        /*-------------------------Constructors---------------------------------*/
        public OblikFS(ConnectionParams connectionParams, IOblikDriver oblikDriver)
        {
            this.oblikDriver = oblikDriver;
            this.connectionParams = connectionParams;
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
        /// <returns></returns>
        public byte[] ReadSegment(int segment, int offset, int len)
        {
            return AccessSegment(segment, offset, len);
        }

        /// <summary>
        /// Запись в сегмент счетчика
        /// </summary>
        /// <param name="segment">Сегмент счетчика</param>
        /// <param name="offset">Смещение относительно начала сегмента</param>
        /// <param name="data">Данные для записи</param>
        public void WriteSegment(int segment, int offset, byte[] data)
        {
            AccessSegment(segment, offset, data.Length, true, data);
        }

        /// <summary>
        /// Доступ к сегменту счетчика
        /// </summary>
        /// <param name="segment">Сегмент счетчика</param>
        /// <param name="offset">Смещение относительно начала сегмента</param>
        /// <param name="len">Количество байт для чтения/записи</param>
        /// <param name="write">Истина при доступа на запись</param>
        /// <param name="data">Данные для записи</param>
        /// <returns>Считанные данные</returns>
        private byte[] AccessSegment(int segment, int offset, int len, bool write = false, byte[] data = null)
        {
            byte access = (write) ? (byte)1 : (byte)0;                                  //Доступ на запись/чтение
            byte[] result = new byte[(write) ? 0 : len];                                //Инициализация ответного массива для чтения
            byte address = (oblikDriver.IsDirectConnected)? (byte)0 : (byte)Address;    //Получение адреса счетчика
            int maxPacket = 0x0F;                                                       //Максимальный размер пакета данных
            int index = 0;                                                              //Смещение массива с данными
            int bytesLeft = len;                                                        //Остаток данных для передачи
            byte[] writebuffer = null;
            while (bytesLeft > 0)
            {
                int queryBytes = (bytesLeft > maxPacket) ? maxPacket : bytesLeft;
                if (write)
                {
                    writebuffer = new byte[queryBytes];
                    Array.Copy(data, index, writebuffer, 0, queryBytes);
                }
                byte[] request = PacketHelper.CreatePacket(address, (byte)segment, (ushort)offset, (byte)queryBytes, access, User, Password, writebuffer);
                byte[] readbuffer = OblikDriver.Request(request, Baudrate, Timeout);
                if (!write)
                {
                    Array.Copy(readbuffer, 0, result, index, queryBytes);
                }
                index += queryBytes;
                offset += queryBytes;
                bytesLeft -= queryBytes;
            }
            return result;        
        }
    }
}