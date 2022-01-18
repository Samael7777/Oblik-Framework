using System;
using System.Text;


namespace Oblik
{
    public class ConnectionParams
    {
        /// <summary>
        /// Адрес счетчика в сети RS-485
        /// </summary>
        public byte Address { get; set; }
        
        /// <summary>
        /// Таймаут, мс
        /// </summary>
        public int Timeout { get; set; }

        /// <summary>
        /// Скорость соединения, бод
        /// </summary>
        public int Baudrate { get; set; }
        
        /// <summary>
        /// Пароль счетчика
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Уровень доступа к сегментам счетчика
        /// </summary>
        public UserLevel User { get; set; }

        public ConnectionParams()
        {
            Address = 0;
            Password = "";
            User = UserLevel.Energo;
            Timeout = 300;
            Baudrate = 9600;
        }
    }
}
