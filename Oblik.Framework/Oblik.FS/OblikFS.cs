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

        /// <summary>
        /// Драйвер интерфейса связи
        /// </summary>
        private OblikDriver oblikDriver;

        /// <summary>
        /// Парметры подключения к счетчику
        /// </summary>
        private ConnectionParams connectionParams;

        /*-------------------------Constructors---------------------------------*/
        public OblikFS(ConnectionParams connectionParams)
        {
            this.connectionParams = connectionParams;
            oblikDriver = new OblikDriver(this.connectionParams);
            l1 = new byte[0];
            l2 = new byte[0];
        }

        /*-------------------------Public---------------------------------------*/
        /// <summary>
        /// Парметры подключения к счетчику
        /// </summary>
        public ConnectionParams CurrentConnectionParams
        {
            get => connectionParams;
            set
            {
                connectionParams = value;
                oblikDriver.CurrentConnectionParams = value;
            }
        }

        /// <summary>
        /// Чтение части сегмента счетчика
        /// </summary>
        /// <param name="segment">Сегмент счетчика</param>
        /// <param name="offset">Смещение относительно начала сегмента</param>
        /// <param name="len">Количество данных для чтения</param>
        /// <param name="data">Полученные данные</param>
        /// <returns>Успех операции</returns>
        public byte[] ReadSegment(int segment, int offset, int len)
        {
            byte[] result = new byte[0];
            PerformFrame((byte)segment, (ushort)offset, (byte)len, Access.Read);
            byte[] answer = oblikDriver.Request(l1);
            //Проверка на ошибки L2
            if (answer[0] != 0)
            {
                DecodeSegmentError(answer[2]);
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
                DecodeSegmentError(answer[2]);
            }
        }
    }
}