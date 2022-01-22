using System;
using System.Runtime.InteropServices;
using Oblik.FS;
using Oblik.Driver;

namespace Oblik
{
    public class Meter
    {
        
        private readonly OblikFS oblikFS; //Драйвер файловой системы счетчика

        //Конструктор
        public Meter(IOblikDriver oblikDriver, ConnectionParams connectionParams)
        {
            oblikFS = new OblikFS(connectionParams, oblikDriver);
        }

        /// <summary>
        /// Истина, если счетчик подключен по RS-232
        /// </summary>
        public bool IsDirectConnected
        {
            get => oblikFS.OblikDriver.IsDirectConnected;
        }

        #region Сегменты

        /// <summary>
        /// Версия прошивки
        /// </summary>
        public FirmwareInfo Firmware
        {
            get => ReadData<FirmwareInfo>(2, 0);     
        }

        /// <summary>
        /// Карта сегментов
        /// </summary>
        public SegmentsMapRecord[] SegmentsMap
        {
            get
            {
                int segments = ReadData<byte>(1, 0);    //Количество записей в карте сегментов
                SegmentsMapRecord[] map = ReadData<SegmentsMapRecord>(1, 1, segments);
                return map;
            }
        }

        /// <summary>
        /// Интегральные показатели за текущие сутки
        /// </summary>
        public IntegralValues CurrentDayIntegralValues
        {
            get => ReadData<IntegralValues>(24, 0);
        }
        
        /// <summary>
        /// Интегральные показатели за текущий месяц
        /// </summary>
        public IntegralValues CurrentMonthIntegralValues
        {
            get => ReadData<IntegralValues>(25, 0);
        }

        /// <summary>
        /// Интегральные показатели за текущий квартал
        /// </summary>
        public IntegralValues CurrentQuarterIntegralValues
        {
            get => ReadData<IntegralValues>(26, 0);
        }

        /// <summary>
        /// Интегральные показатели за текущий год
        /// </summary>
        public IntegralValues CurrentYearIntegralValues
        {
            get => ReadData<IntegralValues>(27, 0);
        }

        /// <summary>
        /// Интегральные показатели за прошлые сутки
        /// </summary>
        public IntegralValues LastDayIntegralValues
        {
            get => ReadData<IntegralValues>(28, 0);
        }

        /// <summary>
        /// Интегральные показатели за прошлый месяц
        /// </summary>
        public IntegralValues LastMonthIntegralValues
        {
            get => ReadData<IntegralValues>(29, 0);
        }

        /// <summary>
        /// Интегральные показатели за прошлый квартал
        /// </summary>
        public IntegralValues LastQuarterIntegralValues
        {
            get => ReadData<IntegralValues>(30, 0);
        }

        /// <summary>
        /// Интегральные показатели за прошлый год
        /// </summary>
        public IntegralValues LastYearIntegralValues
        {
            get => ReadData<IntegralValues>(31, 0);
        }

        /// <summary>
        /// Текущие значения
        /// </summary>
        public CurrentValues CurrentValues
        {
            get => ReadData<CurrentValues>(36, 0);
        }

        /// <summary>
        /// Минутные значения
        /// </summary>
        public MinuteValues MinuteValues
        {
            get => ReadData<MinuteValues>(37, 0);
        }

        /// <summary>
        /// Получасовые значения
        /// </summary>
        public HalfHourValues HalfHourValues
        {
            get => ReadData<HalfHourValues>(38, 0);
        }

        //Суточный график

        /// <summary>
        /// Указатель суточного графика
        /// </summary>
        public int DayGraphPointer
        {
            get => ReadData<ushort>(44, 0);
        }

        /// <summary>
        /// Получить суточный график
        /// </summary>
        /// <param name="records">Количество последних записей</param>
        /// <returns></returns>
        public DayGraphRecord[] GetDayGraphRecords(int records)
        {
            int aviableRecords = DayGraphPointer;
            if (records > aviableRecords)
                throw new ArgumentOutOfRangeException("Number of records must be below or equal aviable records");
            
            int recordSize = Marshal.SizeOf(new DayGraphRecord());
            int offset = (aviableRecords - records) * recordSize;
            return ReadData<DayGraphRecord>(45, offset, records);
        }
        
        /// <summary>
        /// Очистка суточного графика
        /// </summary>
        public void CleanDayGraph()
        {
            CleanLog(88);
        }

        //Протокол событий
        
        /// <summary>
        /// Указатель протокола событий
        /// </summary>
        public int EventLogPointer
        {
            get => ReadData<ushort>(46, 0);
        }

        /// <summary>
        /// Получить протокол событий
        /// </summary>
        /// <param name="records">Количество последних записей</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public EventLogRecord[] GetEventLogRecords(int records)
        {
            int aviableRecords = EventLogPointer;
            if (records > aviableRecords)
                throw new ArgumentOutOfRangeException("Number of records must be below or equal aviable records");

            int recordSize = Marshal.SizeOf(new EventLogRecord());
            int offset = (aviableRecords - records) * recordSize;
            return ReadData<EventLogRecord>(47, offset, records);
        }
        
        /// <summary>
        /// Очистка протокола событий
        /// </summary>
        public void CleanEventLog()
        {
            CleanLog(89);
        }

        //Получасовой график

        /// <summary>
        /// Указатель получасового графика, указывает на самую старую запись
        /// </summary>
        public int HalfHourGraphPointer
        {
            get => ReadData<ushort>(48, 0);
        }

        /// <summary>
        /// Получить получасовой график
        /// </summary>
        /// <returns></returns>
        public HalfHourGraphRecord[] GetHalfHourGraph()
        {
            return ReadData<HalfHourGraphRecord>(49, 0, 30);
        }

        /// <summary>
        /// Параметры вычислений
        /// </summary>
        public CalcUnits CalculationParams
        {
            get => ReadData<CalcUnits>(56, 0);
            set => WriteData(57, 0, value);
        }

        /// <summary>
        /// Текущее время счетчика по UTC
        /// </summary>
        public Time CurrentTimeUTC
        {
            get => ReadData<Time>(64, 0);
            set => WriteData(65, 0, value);
        }

        /// <summary>
        /// Настройка сети счетчика
        /// </summary>
        public NetworkConfig NetworkConfig
        {
            get
            {
                MeterNetworkConfig meternetconfig = ReadData<MeterNetworkConfig>(66, 0);
                NetworkConfig netconfig = new NetworkConfig
                {
                    Address = meternetconfig.Addr,
                    Baudrate = (meternetconfig.Divisor == 0) ? 57600 : 115200 / meternetconfig.Divisor
                };
                return netconfig;
            } 
                
            set
            {
                MeterNetworkConfig meternetconfig = new MeterNetworkConfig
                {
                    Addr = (byte)value.Address,
                    Divisor = (ushort)((value.Baudrate == 0)? 2: (115200 / value.Baudrate))
                };

                WriteData(67, 0, meternetconfig);
                
                //Переход на новые настройки сети
                if (!IsDirectConnected)
                {
                    oblikFS.Address = value.Address;
                    oblikFS.Baudrate = (value.Baudrate == 0)? 57600: value.Baudrate;
                }
            }
        }
        #endregion


        #region Утилиты
        /// <summary>
        /// Прочитать сегмент
        /// </summary>
        /// <typeparam name="T">Структура данных сегмента</typeparam>
        /// <param name="segment">Сегмент</param>
        /// <param name="offset">Начальное смещение в байтах</param>
        /// <returns></returns>
        private T ReadData<T>(int segment, int offset) 
            where T: struct
        {
            T result = default;
            int lenght = Marshal.SizeOf(result);   
            byte[] buffer = oblikFS.ReadSegment(segment, offset, lenght);
            result = Convert.ToValue<T>(buffer, 0);
            return result;
        }

        /// <summary>
        /// Прочитать пакеты данных из сегмента
        /// </summary>
        /// <typeparam name="T">Структура данных сегмента</typeparam>
        /// <param name="segment">Сегмент</param>
        /// <param name="offset">Начальное смещение в байтах</param>
        /// <param name="packets">Количество пакетов данных</param>
        /// <returns>Массив структур данных сегмента</returns>
        private T[] ReadData<T>(int segment, int offset, int packets)
            where T: struct
        {
            T item = default;
            T[] result = new T[packets];
            int packetSize = Marshal.SizeOf(item);

            //Получение данных со счетчика
            byte[] buffer = oblikFS.ReadSegment(segment, offset, packetSize * packets);
            
            //Преобразование сырых данных в массив структур
            for (int i = 0; i < packets; i++)
            {
                item = Convert.ToValue<T>(buffer, i * packetSize);
                result[i] = item;
            }
            return result;
        }

        /// <summary>
        /// Записать данные в сегмент
        /// </summary>
        /// <typeparam name="T">Структура данных сегмента</typeparam>
        /// <param name="segment">Сегмент</param>
        /// <param name="offset">Начальное смещение в байтах</param>
        /// <param name="data">Данные</param>
        private void WriteData<T>(int segment, int offset, T data)
            where T: struct
        {
            byte[] buffer = Convert.ToBytes<T>(data);
            oblikFS.WriteSegment(segment, offset, buffer);
        }

        /// <summary>
        /// Записать массив данных в сегмент
        /// </summary>
        /// <typeparam name="T">Структура данных сегмента</typeparam>
        /// <param name="segment">Сегмент</param>
        /// <param name="offset">Начальное смещение в байтах</param>
        /// <param name="data">Массив данных</param>
        private void WriteData<T>(int segment, int offset, T[] data)
            where T : struct
        {
            T item = default;
            int packets = data.Length;
            int packetSize = Marshal.SizeOf(item);
            //Преобразование массива данных в массив байт
            byte[] buffer = new byte[packets * packetSize];     //Общий буфер для записи
            for (int i = 0; i < packets; i++)
            {
                item = data[i];
                byte[] packet = Convert.ToBytes<T>(item);
                Array.Copy(packet, 0, buffer, i * packetSize, packet.Length);
            }
            //Запись данных в счетчик
            oblikFS.WriteSegment(segment, offset, buffer);
        }

        /// <summary>
        /// Очистка графика
        /// </summary>
        /// <param name="segment">сегмент команды очистки</param>
        private void CleanLog(int segment)
        {
            byte address = (byte)oblikFS.Address;
            byte[] command = { (byte)~address, address };
            oblikFS.WriteSegment(segment, 0, command);
        }
        #endregion
    }
}
