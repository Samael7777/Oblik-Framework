using System;
using System.IO.Ports;
using System.Threading;

namespace Oblik.Driver
{
    public class OblikSerialDriver : IOblikDriver
    {
        /*-------------------------Private--------------------------------*/

        private readonly Mutex mutex;
        /// <summary>
        /// Параметры подключения
        /// </summary>
        private readonly SerialConnectionParams connectionParams;

        /// <summary>
        /// COM-порт
        /// </summary>
        private readonly SerialPort sp;

        /*-------------------------Constructors------------------------------*/

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="connectionParams">Параметры подключения</param>
        public OblikSerialDriver(SerialConnectionParams connectionParams)
        {
            this.connectionParams = connectionParams;
            mutex = new Mutex(false);
            //Настройка соединения с COM портом
            sp = new SerialPort
            {
                PortName = this.connectionParams.Port,
                Parity = Parity.None,
                DataBits = 8,
                StopBits = StopBits.One,
                DtrEnable = false,
                RtsEnable = false,
                Handshake = Handshake.None
            };
        }

        /// <summary>
        /// Деструктор
        /// </summary>
        ~OblikSerialDriver()
        {
            if (sp != null)
            {
                if (sp.IsOpen)
                    sp.Close();
                sp.Dispose();
            }
        }

        /*----------------------Public------------------------------------------*/

        public string Port { get => sp.PortName; }
        public bool IsDirectConnected
        {
            get => connectionParams.IsDirectConnected;
            set => connectionParams.IsDirectConnected = value;
        }

        /// <summary>
        /// Параметры текущего соединения
        /// </summary>
        public SerialConnectionParams CurrentConnectionParams
        {
            get => connectionParams;
        }

        /// <summary>
        /// Отправка запроса L1 к счетчику и получение данных
        /// </summary>
        /// <param name="l1">Запрос L1 к счетчику</param>
        /// <returns>Ответ счетчика в формате массива L2</returns>
        public byte[] Request(byte[] l1, int baudrate, int timeout)
        {
            //Исключение при пустом запросе
            if (l1 == null)
            {
                throw new ArgumentNullException(paramName: nameof(l1));
            }
            
            byte[] result = new byte[0];

            sp.BaudRate = (connectionParams.IsDirectConnected) ? 9600 : baudrate;
            sp.WriteTimeout = timeout;
            sp.ReadTimeout = timeout + 100;
            int checkSum = 0;       //Контрольная сумма
            //Блокировка потока
            mutex.WaitOne();

            //Очистка буферов
            try
            {
                if (!sp.IsOpen) sp.Open();
                sp.DiscardOutBuffer();
                sp.DiscardInBuffer();
            }
            catch (Exception e)
            {
                mutex.ReleaseMutex();
                throw new OblikIOException(e.Message, Error.OpenPortError);
            }
            //Отправка запроса
            try
            {
                sp.Write(l1, 0, l1.Length);
            }
            catch (Exception e)
            {
                sp.Close();
                mutex.ReleaseMutex();
                throw new OblikIOException(e.Message, Error.WriteError);
            }

            //Получение результата L1
            int L1Err = ReadAnswer(1)[0];

            //Проверка на ошибки L1
            if (L1Err != 1)
            {
                sp.Close();
                mutex.ReleaseMutex();
                PacketHelper.CheckL1Error(L1Err);
            }

            checkSum ^= L1Err;  //Контрольная сумма

            //Получение количества байт в ответе
            int L2len = ReadAnswer(1)[0];
            checkSum ^= L2len;

            //Получение оставшихся данных
            byte[] l2Packet = ReadAnswer(L2len + 1);
            sp.Close();

            //Проверка контрольной суммы ответа
            for (int i = 0; i <= L2len; i++)
            {
                checkSum ^= l2Packet[i];
            }

            //Ошибка контрольной суммы ответа
            if (checkSum != 0)
            {
                mutex.ReleaseMutex();
                throw new OblikIOException("Answer L1 CSC Error", Error.L1CSCError);
            }

            //Проверка ответа L2
            if (l2Packet[0] != 0)
            {
                mutex.ReleaseMutex();
                PacketHelper.CheckL2Error(l2Packet[0]);
            }

            //Формирование ответа
            int l2DataSize = l2Packet.Length - 3;
            Array.Resize(ref result, l2DataSize);
            Array.Copy(l2Packet, 2, result, 0, l2DataSize);
            mutex.ReleaseMutex();
            return result;
        }
        
        //----------------------------Private methods----------------------------------------
        
        /// <summary>
        /// Чтение данных из порта
        /// </summary>
        /// <param name="BytesToRead">Количество байт для чтения</param>
        /// <param name="answer">Массив с полученными данными</param>
        /// <returns></returns>
        private byte[] ReadAnswer(int BytesToRead)
        {
            int timeout = sp.ReadTimeout - 100;
            byte[] buffer = new byte[BytesToRead];      //Буфер для чтения
            int BytesGot;                               //Получено байт
            int count = BytesToRead;
            int offset = 0;

            ulong start = GetTickCount();
            
            while (offset < BytesToRead)
            {
                if ((GetTickCount() - start) > (ulong)timeout)
                {
                    //Таймаут
                    sp.Close();
                    mutex.ReleaseMutex();
                    throw new OblikIOException("Timeout", Error.Timeout);
                }
                try
                {
                    BytesGot = sp.Read(buffer, offset, count);
                }
                catch
                {
                    BytesGot = 0;
                }
                count -= BytesGot;
                offset += BytesGot;
            }
            if (offset != BytesToRead)
            {
                //Ошибка чтения порта
                sp.Close();
                mutex.ReleaseMutex();
                throw new OblikIOException("Port read error", Error.ReadError);
            }
            return buffer;
        }
        
        /// <summary>
        /// Возвращает количество миллисекунд для данного экземпляра
        /// </summary>
        /// <returns>Количество миллисекунд для данного экземпляра</returns>
        private static ulong GetTickCount()
        {
            return (ulong)DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }  
    }
}



    
