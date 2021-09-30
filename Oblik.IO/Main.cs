using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;

namespace Oblik.IO
{
    public partial class OblikConnector
    {
        /*-------------------------Private variables---------------------------*/
        /// <summary>
        /// Таймаут, мс
        /// </summary>
        private int timeout;

        /// <summary>
        /// Количество повторов
        /// </summary>
        private int repeats;

        /// <summary>
        /// Пароль к счетчику
        /// </summary>
        private string password;

        /// <summary>
        /// Пароль к счетчику в виде массива байт
        /// </summary>
        private byte[] passwd;

        /// <summary>
        /// COM-порт
        /// </summary>
        private SerialPort sp;

        /// <summary>
        /// Журнал ошибок последней операции
        /// </summary>
        private List<int> ErrorsLog;

        /// <summary>
        /// Уровень доступа
        /// </summary>
        private UserLevel user;

        /// <summary>
        /// Адрес счетчика в сети
        /// </summary>
        private byte address;

        /// <summary>
        /// Номер порта счетчика
        /// </summary>
        private byte port;

        /// <summary>
        /// Массив данных L1
        /// </summary>
        private byte[] l1;

        /// <summary>
        /// Массив данных L2
        /// </summary>
        private byte[] l2;
        /*-------------------------Constructors------------------------------*/

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="port">Номер порта</param>
        /// <param name="baudrate">Скорость соединения</param>
        /// <param name="timeout">Таймаут, мс</param>
        /// <param name="repeats">Количество повторов</param>
        /// <param name="userlevel">Уровень доступа</param>
        /// <param name="password">Пароль к счетчику</param>
        OblikConnector(byte port, byte address, int baudrate, int timeout, int repeats, UserLevel userlevel, string password)
        {
            this.timeout = timeout;
            this.repeats = repeats;
            this.port = port;
            passwd = new byte[8];
 
            Address = address;
            User = userlevel;
            Password = password;

            ErrorsLog = new List<int>();    //Инициализация журнала ошибок

            //Настройка соединения с COM портом
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
        OblikConnector(byte port, byte address) : this(port, address, 9600, 500, 3, UserLevel.Energo, "") { }

        /*----------------------Public methods---------------------------------*/

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
        /// Получить список кодов ошибок
        /// </summary>
        /// <returns></returns>
        public List<int> GetErrorsLog() => ErrorsLog;

        /// <summary>
        /// Пароль к счетчику
        /// </summary>
        public string Password
        {
            get => password;
            set
            {
                value.Trim();                       //Очистка от возможного мусора
                password = value.Substring(0, 8);   //Ограничение длины в 8 символов
                //Преобразование строки пароля в массив байт
                passwd = Encoding.Default.GetBytes(password);
                Array.Resize(ref passwd, 8);
            }
        }

        /// <summary>
        /// Уровень доступа
        /// </summary>
        public UserLevel User
        {
            get => user;
            set
            {
                user = value;
            }
        }
        /// <summary>
        /// Адрес счетчика в сети
        /// </summary>
        public byte Address
        {
            get => address;
            set
            {
                address = value;
            }
        }

        /// <summary>
        /// Номер порта счетчика
        /// </summary>
        public byte Port
        {
            get => port;
            set
            {
                port = value;
                sp.PortName = "COM" + port.ToString();
            }
        }
    }
}
