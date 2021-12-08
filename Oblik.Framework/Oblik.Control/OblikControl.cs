using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oblik.FS;

namespace Oblik.Control
{
    public partial class OblikControl
    {
        /*-------------------------Private--------------------------------------*/
        private ConnectionParams connectionParams;
        private OblikFS oblikFS;
        

        /*-------------------------Constructors---------------------------------*/
        public OblikControl(ConnectionParams connectionparams)
        {
            connectionParams = connectionparams;
            oblikFS = new OblikFS(connectionParams);
        }

        /*-------------------------Public---------------------------------------*/

        /// <summary>
        /// Парметры подключения к счетчику
        /// </summary>
        public ConnectionParams CurrentConnectionParams
        {
            get => connectionParams;
            set
            {
                connectionParams = value;
                oblikFS.CurrentConnectionParams = value;
            }
        }
       
        /// <summary>
        /// Карта сегментов
        /// </summary>
        public SegmentsMap SegmentsList
        {
            get
            {
                int numsegments = oblikFS.ReadSegment(1, 0, 1)[0];
                return new SegmentsMap(oblikFS.ReadSegment(1, 0, numsegments * 5 + 1));
            }
            
        }
        
        /// <summary>
        /// Текущие значения измерений
        /// </summary>
        public CurrentValues CurrentVals
        {
            get => new CurrentValues(oblikFS.ReadSegment(36, 0, CurrentValues.Size));
        }

        /// <summary>
        /// Параметры вычислений
        /// </summary>
        public CalcUnits CalculationUnits
        {
            get => new CalcUnits(oblikFS.ReadSegment(56, 0, CalcUnits.Size));
            set => oblikFS.WriteSegment(57, 0, value.RawData);
        }

        /// <summary>
        /// Настройки сети
        /// </summary>
        public NetworkConfig NetConfig
        {
            get => new NetworkConfig(oblikFS.ReadSegment(66, 0, NetworkConfig.Size));
            set => oblikFS.WriteSegment(67, 0, value.RawData);
        }

        /// <summary>
        /// Количество записей суточного графика
        /// </summary>
        public int DayGraphRecords
        {
            get => Convert.ToValue<UInt16>(oblikFS.ReadSegment(44, 0, 2), 0);
        }

        // <summary>
        /// Количество записей протокола событий
        /// </summary>
        public int EventLogRecords
        {
            get => Convert.ToValue<UInt16>(oblikFS.ReadSegment(46, 0, 2), 0);
        }

        /// <summary>
        /// Протокол событий
        /// </summary>
        /// <param name="records">Прочитать записей</param>
        /// <param name="index">Начальная запись</param>
        /// <returns></returns>
        public List<EventLogRow> EventLogList(int records, int index)
        {
            if ((index + records) > 800)
                throw new ArgumentException("Index and records combination are out of range");

            List<EventLogRow> eventLog = new List<EventLogRow>();

        }

        private List<T> GetRecordsPack 

        /// <summary>
        /// Суточный график
        /// </summary>
        /// <param name="records">Прочитать записей</param>
        /// <param name="index">Начальная запись</param>
        /// <returns></returns>
        public List<DayGraphRow> DayGraphList(int records, int index)
        {
            if ((index + records) > 1750)
                throw new ArgumentException("Index and records combination are out of range");

            List<DayGraphRow> dayGraph = new List<DayGraphRow>();
            int recordSize = DayGraphRow.Size;
            int maxPacketSize = 255 / recordSize;                         //Максимально записей в 1 пакете
            int offset = index * recordSize;
            int recordsLeft = records;                                    //Осталось прочитать строк
            while (recordsLeft > 0)
            {
                int packet = (recordsLeft <= maxPacketSize) ? (recordsLeft) : (maxPacketSize);
                byte[] rawdata = oblikFS.ReadSegment(45, offset, packet * recordSize);
                for (int i = 0; i < packet; i++)
                {
                    dayGraph.Add(new DayGraphRow(rawdata, i * recordSize));
                }
                recordsLeft -= packet;
                offset += packet * recordSize;
            }
            return dayGraph;
        }

        /// <summary>
        /// Очистка суточного графика
        /// </summary>
        public void CleanDayGraph()
        {
            byte[] req = new byte[2];
            req[1] = connectionParams.Address;
            req[0] = (byte)(~ req[1]);
            oblikFS.WriteSegment(88, 0, req);
        }
        
        /// <summary>
        /// Очистка протокола событий
        /// </summary>
        public void CleanEventLog()
        {
            byte[] req = new byte[2];
            req[1] = connectionParams.Address;
            req[0] = (byte)(~req[1]);
            oblikFS.WriteSegment(89, 0, req);
        }

        /// <summary>
        /// Внутреннее время счетчика по Гринвичу
        /// </summary>
        public DateTime CurrentTimeUTC
        {
            get => Convert.ToUTCTime(oblikFS.ReadSegment(64, 0, 4), 0);
            set => oblikFS.WriteSegment(65, 0, Convert.ToTime(value));
        }

        /// <summary>
        /// Внутреннее время счетчика по локальному времени
        /// </summary>
        public DateTime CurrentTime
        {
            get => CurrentTimeUTC.ToLocalTime();
            set => CurrentTimeUTC = value.ToUniversalTime();
        }

        /// <summary>
        /// Интегральные показатели за текущие сутки
        /// </summary>
        public IntegralValues CurrentDayValues
        {
            get => new IntegralValues(oblikFS.ReadSegment(24, 0, IntegralValues.Size));
        }
        
        /// <summary>
        /// Интегральные показатели за текущий месяц
        /// </summary>
        public IntegralValues CurrentMonthValues
        {
            get => new IntegralValues(oblikFS.ReadSegment(25, 0, IntegralValues.Size));
        }
        
        /// <summary>
        /// Интегральные показатели за текущий квартал
        /// </summary>
        public IntegralValues CurrentQuartValues
        {
            get => new IntegralValues(oblikFS.ReadSegment(26, 0, IntegralValues.Size));
        }
        
        /// <summary>
        /// Интегральные показатели за текущий год
        /// </summary>
        public IntegralValues CurrentYearValues
        {
            get => new IntegralValues(oblikFS.ReadSegment(27, 0, IntegralValues.Size));
        }

        /// <summary>
        /// Интегральные показатели за прошедшие сутки
        /// </summary>
        public IntegralValues PrevDayValues
        {
            get => new IntegralValues(oblikFS.ReadSegment(28, 0, IntegralValues.Size));
        }
        
        /// <summary>
        /// Интегральные показатели за прошедший месяц
        /// </summary>
        public IntegralValues PrevMonthValues
        {
            get => new IntegralValues(oblikFS.ReadSegment(29, 0, IntegralValues.Size));
        }
        
        /// <summary>
        /// Интегральные показатели за прошедший квартал
        /// </summary>
        public IntegralValues PrevQuartValues
        {
            get => new IntegralValues(oblikFS.ReadSegment(30, 0, IntegralValues.Size));
        }
        
        /// <summary>
        /// Интегральные показатели за прошедший год
        /// </summary>
        public IntegralValues PrevYearValues
        {
            get => new IntegralValues(oblikFS.ReadSegment(31, 0, IntegralValues.Size));
        }

        
        /// <summary>
        /// Получасовые значения
        /// </summary>
        public HalfHourValues HalfHourVals
        {
            get => new HalfHourValues(oblikFS.ReadSegment(38, 0, HalfHourValues.Size));
        }

        // <summary>
        /// Указатель получасового графика
        /// </summary>
        public int HalfHourGraphPtr
        {
            get => (int)Convert.ToValue<byte>(oblikFS.ReadSegment(48, 0, 1), 0);
        }
    }
}
