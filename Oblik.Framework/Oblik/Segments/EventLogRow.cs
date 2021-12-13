using System;

namespace Oblik
{
    public class EventLogRow
    {
        public static int Size { get => 5; }
        public DateTime Time { get; private set; }
        public int Code { get; private set; }

        public EventLogRow(byte[] rawdata, int index)
        {
            if ((rawdata.Length - index) < Size)
                throw new ArgumentException($"Raw data size must be {Size} bytes long");

            Time = Convert.ToUTCTime(rawdata, index);
            index += 4;
            Code = rawdata[index];
        }
    }
}