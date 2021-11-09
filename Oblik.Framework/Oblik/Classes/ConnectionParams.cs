using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oblik
{
    /// <summary>
    /// Параметры подключения к счетчику
    /// </summary>
    public class ConnectionParams
    {
        private string password;
        private byte[] pwdBytes;

        /// <summary>
        /// Номер порта счетчика    
        /// </summary>
        public byte Port { get; set; }     

        /// <summary>
        /// Таймаут, мс
        /// </summary>
        public int Timeout { get; set; }

        /// <summary>
        /// Количество повторов
        /// </summary>
        public int Repeats { get; set; }

        /// <summary>
        /// Скорость соединения, бод
        /// </summary>
        public int Baudrate { get; set; }

        /// <summary>
        /// Адрес счетчика в сети RS-485
        /// </summary>
        public byte Address { get; set; }

        /// <summary>
        /// Пароль к счетчику
        /// </summary>
        public string Password { get => password;
            set 
            {
                //Очистка от мусора
                password = value.Trim();
                //Формирование длины в 8 символов
                if (password.Length >= 8)
                {
                    password.Substring(0, 8);
                } 
                else
                {
                    password.PadLeft(8, (char)0);
                }
                //Преобразование в массив байт
                pwdBytes = new byte[8];
                pwdBytes = Encoding.Default.GetBytes(password);
            } 
        }
        
        /// <summary>
        /// Пароль в виде массива байт
        /// </summary>
        public byte[] PasswordArray { get => pwdBytes; }
      
         
        /// <summary>
        /// Уровень доступа к сегментам счетчика
        /// </summary>
        public UserLevel User { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        public ConnectionParams()
        {
            Port = 1;
            Baudrate = 9600;
            Timeout = 500;
            Repeats = 2;
            Address = 0;
            Password = "";
            User = UserLevel.Energo;
        }
    }
}
