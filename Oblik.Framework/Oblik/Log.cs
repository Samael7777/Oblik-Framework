using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oblik.Oblik
{
    public abstract class Log<T> : Segment
    {
        public abstract int TotalRecords { get; }
        public List<T> Records { get; protected set; }
        
        protected Log() : base()
        {
            Records = new List<T>();
        }
        
        public void GetLastRecords(int records)
        {
            if (records > TotalRecords)
                throw new ArgumentOutOfRangeException($"Records to read must be below or equal {TotalRecords}");

        }

    }
}

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