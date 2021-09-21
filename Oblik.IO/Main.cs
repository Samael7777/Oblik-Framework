using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

namespace Oblik.IO
{
    public partial class OblikConnector
    {
        /// <summary>
        /// Таймаут, мс
        /// </summary>
        private int Timeout;
        
        /// <summary>
        /// Количество повторов
        /// </summary>
        private int Repeats;
      
        /// <summary>
        /// COM-порт
        /// </summary>
        private SerialPort sp;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="port">Номер порта</param>
        /// <param name="baudrate">Скорость соединения</param>
        /// <param name="timeout">Таймаут, мс</param>
        /// <param name="repeats">Количество повторов</param>
        OblikConnector(int port, int baudrate, int timeout, int repeats)
        {
            Timeout = timeout;
            Repeats = repeats;

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


    }
}
