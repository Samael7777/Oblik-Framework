using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oblik.FS;
using Oblik.Driver;

namespace Oblik
{
    public class Meter
    {
        //Соединение
        private IOblikFS oblikFS;

        /*-------------------СЕГМЕНТЫ---------------------*/
        public SegmentsMap SegmentsList { get; private set; }
        public FirmwareInfo Firmware { get; private set; }
        public CurrentDayIntegralValues CurrentDayIntegralVals { get; private set; }
        public CurrentMonthIntegralValues CurrentMonthIntegralVals { get; private set; }
        public CurrentQuarterIntegralValues CurrentQuarterIntegralVals { get; private set; }
        public CurrentYearIntegralValues CurrentYearIntegralVals { get; private set; }
        public LastDayIntegralValues LastDayIntegralVals { get; private set; }
        public LastMonthIntegralValues LastMonthIntegralVals { get; private set; }
        public LastQuarterIntegralValues LastQuarterIntegralVals { get; private set; }
        public LastYearIntegralValues LastYearIntegralVals { get; private set; }
        public CurrentValues CurrentVals { get; private set; }
        public MinuteValues MinuteVals { get; private set; }
        public HalfHourValues HalfHourVals { get; private set; }
        public CalcUnits CalculationUnits { get; private set; }
        public DayGraph DayGraphRecords { get; private set; }
        public EventLog EventRecords { get; private set; }
        public HalfHourGraph HalfHourGraphVals { get; private set; }
        public InternalTime MeterTime { get; private set; }
        public NetworkConfig MeterNetwork { get; private set; }

        /*--------------------Конструкторы---------------------------------*/
        public Meter(IOblikFS oblikFS)
        {
            this.oblikFS = oblikFS;
            Init();
        }
        public Meter(IOblikDriver oblikDriver)
        {
            oblikFS = new OblikFS(oblikDriver);
            Init();
        }
        private void Init()
        {
            SegmentsList = new SegmentsMap(oblikFS);
            Firmware = new FirmwareInfo(oblikFS);
            CurrentDayIntegralVals = new CurrentDayIntegralValues(oblikFS);
            CurrentMonthIntegralVals = new CurrentMonthIntegralValues(oblikFS);
            CurrentQuarterIntegralVals = new CurrentQuarterIntegralValues(oblikFS);
            CurrentYearIntegralVals = new CurrentYearIntegralValues(oblikFS);
            LastDayIntegralVals = new LastDayIntegralValues(oblikFS);
            LastMonthIntegralVals = new LastMonthIntegralValues(oblikFS);
            LastQuarterIntegralVals = new LastQuarterIntegralValues(oblikFS);
            LastYearIntegralVals = new LastYearIntegralValues(oblikFS);
            CurrentVals = new CurrentValues(oblikFS);
            MinuteVals = new MinuteValues(oblikFS);
            HalfHourVals = new HalfHourValues(oblikFS);
            CalculationUnits = new CalcUnits(oblikFS);
            DayGraphRecords = new DayGraph(oblikFS);
            EventRecords = new EventLog(oblikFS);
            HalfHourGraphVals = new HalfHourGraph(oblikFS);
            MeterTime = new InternalTime(oblikFS);
            MeterNetwork = new NetworkConfig(oblikFS);
        }

        /*--------------------Методы---------------------------------*/
        /// <summary>
        /// Получить основную информацию о счетчике
        /// </summary>
        public void ReadGeneralInfo()
        {
            Firmware.Read();
            CalculationUnits.Read();
            MeterNetwork.Read();
        }
    }
}
