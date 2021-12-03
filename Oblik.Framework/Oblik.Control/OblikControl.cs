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
        /// Журнал ошибок ввода-вывода
        /// </summary>
        public List<int> GetIOErrorsList  
        { 
            get => oblikFS.GetIOErrorsList; 
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
            get => Utils.ToUint16(oblikFS.ReadSegment(44, 0, 2));
        }

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
            int rec_size = DayGraphRow.Size;
            int max_packet = 255 / rec_size;                            //Максимально записей в 1 пакете
            int offset = index * rec_size;
            int recs_left = records;                                    //Осталось прочитать строк
            while (recs_left > 0)
            {
                int packet = (recs_left <= max_packet) ? (recs_left) : (max_packet);
                byte[] rawdata = oblikFS.ReadSegment(45, offset, packet * rec_size);
                for (int i = 0; i < packet; i++ )
                {
                    dayGraph.Add(new DayGraphRow(Utils.ArrayPart(rawdata, i * rec_size, rec_size)));
                }
                recs_left -= packet;
                offset += packet * rec_size;
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
            get => Utils.ToUTCTime(oblikFS.ReadSegment(64, 0, 4));
            set => oblikFS.WriteSegment(65, 0, Utils.ToTime(value));
        }

        /// <summary>
        /// Внутреннее время счетчика по локальному времени
        /// </summary>
        public DateTime CurrentTime
        {
            get => CurrentTimeUTC.ToLocalTime();
            set => CurrentTimeUTC = value.ToUniversalTime();
        }


    }
}
