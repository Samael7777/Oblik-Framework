using System;
using System.Text;


namespace Oblik
{
    public class ConnectionParams
    {
        private string password;
        private byte[] passwordBytes;

        /// <summary>
        /// Адрес счетчика в сети RS-485
        /// </summary>
        public byte Address { get; set; }
        public string Password
        {
            get => password;
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
                passwordBytes = Encoding.Default.GetBytes(password);
                if (passwordBytes.Length < 8)
                {
                    Array.Resize(ref passwordBytes, 8);
                }
            }
        }

        /// <summary>
        /// Пароль в виде массива байт
        /// </summary>
        public byte[] PasswordArray { get => passwordBytes; }

        /// <summary>
        /// Уровень доступа к сегментам счетчика
        /// </summary>
        public UserLevel User { get; set; }

        public ConnectionParams()
        {
            Address = 0;
            password = "";
            User = UserLevel.Energo;
            passwordBytes = new byte[8];
        }
    }
}
