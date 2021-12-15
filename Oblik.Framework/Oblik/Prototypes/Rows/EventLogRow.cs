using System;

namespace Oblik
{
    public class EventLogRow : Row
    {
        public new static int RecordSize { get => 5; }
        public DateTime Time { get; private set; }
        public int Code { get; private set; }

        public EventLogRow(byte[] rawdata, int index) : base(rawdata, index)
        {
            Time = Convert.ToUTCTime(rawdata, index);
            index += 4;
            Code = rawdata[index];
        }
    }
}