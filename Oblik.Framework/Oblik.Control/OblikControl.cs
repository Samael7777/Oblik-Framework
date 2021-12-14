using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oblik.FS;

namespace Oblik.Control
{
    public partial class OblikControl
    {    
        
        /// <summary>
        /// Указатель получасового графика
        /// </summary>
        public int HalfHourGraphPtr
        {
            get => (int)Convert.ToValue<byte>(oblikFS.ReadSegment(48, 0, 1), 0);
        }

        /// <summary>
        /// Получасовой график
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, HalfHourLogRow> HalfHourLog()
        {
           
            Dictionary<int, HalfHourLogRow> halfHourLog = new Dictionary<int, HalfHourLogRow>();
            int recordSize = HalfHourLogRow.Size;
            int maxPacketSize = 255 / recordSize;                         //Максимально записей в 1 пакете
            int offset = 0;
            int recordsLeft = 30;                                         //Осталось прочитать строк
            while (recordsLeft > 0)
            {
                int packet = (recordsLeft <= maxPacketSize) ? (recordsLeft) : (maxPacketSize);
                byte[] rawdata = oblikFS.ReadSegment(45, offset, packet * recordSize);
                for (int i = 0; i < packet; i++)
                {
                    halfHourLog.Add(30 - recordsLeft, new HalfHourLogRow(rawdata, i * recordSize));
                    recordsLeft--;
                }               
                offset += packet * recordSize;
            }
            return halfHourLog;
        }
    }
}