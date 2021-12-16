﻿using System.Collections.Generic;
using System.IO.Ports;
using Oblik;

namespace Oblik.Driver
{
    public partial class OblikDriver : IOblikDriver
    {
        /*-------------------------Private--------------------------------*/

        /// <summary>
        /// Параметры подключения
        /// </summary>
        private ConnectionParams connectionParams;

        /// <summary>
        /// COM-порт
        /// </summary>
        private SerialPort sp;

        /*-------------------------Constructors------------------------------*/

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="connectionParams">Параметры подключения</param>
        public OblikDriver(ConnectionParams connectionParams)
        {
            this.connectionParams = connectionParams;

            //Настройка соединения с COM портом
            sp = new SerialPort
            {
                PortName = this.connectionParams.Port,
                BaudRate = this.connectionParams.Baudrate,
                Parity = Parity.None,
                DataBits = 8,
                StopBits = StopBits.One,
                ReadTimeout = (int)this.connectionParams.Timeout + 100,
                WriteTimeout = (int)this.connectionParams.Timeout + 100,
                DtrEnable = false,
                RtsEnable = false,
                Handshake = Handshake.None
            };
        }

        /// <summary>
        /// Деструктор
        /// </summary>
        ~OblikDriver()
        {
            if (sp != null)
            {
                if (sp.IsOpen)
                    sp.Close();
                sp.Dispose();
            }
        }

        /*----------------------Public------------------------------------------*/

        /// <summary>
        /// Параметры текущего соединения
        /// </summary>
        public ConnectionParams CurrentConnectionParams
        {
            get => connectionParams;
            set
            {
                connectionParams = value;
                sp.PortName = "COM" + connectionParams.Port.ToString();
                sp.BaudRate = connectionParams.Baudrate;
                sp.WriteTimeout = (int)connectionParams.Timeout + 100;
                sp.ReadTimeout = (int)connectionParams.Timeout + 100;
            }
        }

    }
}



    
