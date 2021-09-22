using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using Oblik.Resources;

namespace Oblik.IO
{
    public partial class OblikConnector
    {             
        /// <summary>
        /// Таймаут, мс
        /// </summary>
        private int timeout;
        
        /// <summary>
        /// Количество повторов
        /// </summary>
        private int repeats;
      
        /// <summary>
        /// COM-порт
        /// </summary>
        private SerialPort sp;

        private string LastReceiverError;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="port">Номер порта</param>
        /// <param name="baudrate">Скорость соединения</param>
        /// <param name="timeout">Таймаут, мс</param>
        /// <param name="repeats">Количество повторов</param>
        OblikConnector(int port, int baudrate, int timeout, int repeats)
        {
            this.timeout = timeout;
            this.repeats = repeats;
            //Настройка соединения COM портом
            sp = new SerialPort
            {
                PortName = "COM" + port.ToString(),
                BaudRate = baudrate,
                Parity = Parity.None,
                DataBits = 8,
                StopBits = StopBits.One,
                ReadTimeout = 500,
                WriteTimeout = 500,
                DtrEnable = false,
                RtsEnable = false,
                Handshake = Handshake.None
            };
        }
        
        /// <summary>
        /// Конструктор (скорость 9600 бод, таймаут 500мс, 3 повтора)
        /// </summary>
        /// <param name="port">Номер порта</param>
        OblikConnector(int port) : this(port, 9600, 500, 3) {}

        /// <summary>
        /// Таймаут, мс
        /// </summary>
        public int Timeout
        {
            get => timeout;
            set => timeout = Timeout;
        }

        /// <summary>
        /// Количество повторов
        /// </summary>
        public int Repeats
        {
            get => repeats;
            set => repeats = Repeats;
        }
        
        /// <summary>
        /// Скорость соединения, бод
        /// </summary>
        public int Baudrate
        {
            get => sp.BaudRate;
            set => sp.BaudRate = Baudrate;
        }

        /// <summary>
        /// Получить последнюю ошибку приема
        /// </summary>
        /// <returns></returns>
        public string GetLastReceiverError() => LastReceiverError;

    }
}
