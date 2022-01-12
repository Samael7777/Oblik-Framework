using System.Text;
using System;

namespace Oblik
{
    /// <summary>
    /// Параметры подключения к счетчику
    /// </summary>
    public class SerialConnectionParams
    {
        /// <summary>
        /// Истина, в случае подключения по RS-232
        /// </summary>
        public bool IsDirectConnected { get; set; }
        
        /// <summary>
        /// Порт счетчика
        /// </summary>
        public string Port { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        public SerialConnectionParams()
        {
            Port = "COM1";
            IsDirectConnected = false;
        }
    }
}