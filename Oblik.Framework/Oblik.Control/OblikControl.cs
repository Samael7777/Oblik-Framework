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
        ConnectionParams connectionParams;
        OblikFS oblikFS;

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
            get
            {
                return new CurrentValues(oblikFS.ReadSegment(36, 0, CurrentValues.Size));
            }
        }

        public CalcUnits CalculationUnits
        {
            get
            {
                return new CalcUnits(oblikFS.ReadSegment(56, 0, CalcUnits.Size));
            }
            set
            {
                oblikFS.WriteSegment(57, 0, value.RawData);
            }
        }
    }
}
