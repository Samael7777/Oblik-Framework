using System.Text;
using System;

namespace Oblik
{
    /// <summary>
    /// Параметры подключения к счетчику
    /// </summary>
    public class SerialConnectionParams
    {
        public bool IsDirectConnected { get; set; }
        /// <summary>
        /// Порт счетчика
        /// </summary>
        public string Port { get; set; }

        /// <summary>
        /// Таймаут, мс
        /// </summary>
        public int Timeout { get; set; }

        /// <summary>
        /// Скорость соединения, бод
        /// </summary>
        public int Baudrate { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        public SerialConnectionParams()
        {
            Port = "COM1";
            Baudrate = 9600;
            Timeout = 500;
            IsDirectConnected = false;
        }
    }
}