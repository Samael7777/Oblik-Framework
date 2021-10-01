using System;
using System.Collections.Generic;
using System.Text;
using Oblik;
using Oblik.Driver;

namespace Oblik.FS
{
    public partial class OblikFS
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

        /// <summary>
        /// Журнал ошибок L2
        /// </summary>
        private List<int> L2ErrorsLog;

        /*-------------------------Constructors---------------------------------*/
        public OblikFS(ConnectionParams connectionParams)
        {
            this.connectionParams = connectionParams;
            oblikDriver = new OblikDriver(this.connectionParams);
            L2ErrorsLog = new List<int>();
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
        /// Журнал ошибок L1 последней операции
        /// </summary>
        public List<int> GetL1ErrorsList { get => oblikDriver.GetConnectionErrors; }

        /// <summary>
        /// Журнал ошибок L2 последней операции
        /// </summary>
        public List<int> GetL2ErrorsList { get => L2ErrorsLog; }
    }
}

                
               
