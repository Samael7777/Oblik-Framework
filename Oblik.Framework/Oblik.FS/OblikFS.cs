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
        /// Журнал ошибок ввода-вывода
        /// </summary>
        public List<int> GetIOErrorsList => oblikDriver.GetConnectionErrors;
    }
}