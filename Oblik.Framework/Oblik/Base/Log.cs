using System;
using System.Collections.Generic;
using Oblik.FS;


namespace Oblik
{
    public abstract class Log : Segment

    {
        public abstract int MaxRecords { get; }
        public abstract int NumberOfRecords { get;}
        public abstract int RecordSize { get; }
        protected Log() : base()
        {
        }
        public void GetLastRecords(int records)
        {           
            if (records > MaxRecords)
                throw new ArgumentOutOfRangeException($"Records to read must be below or equal {MaxRecords}");
            CleanLog();
            int maxPacketSize = 255 / RecordSize;                         //Максимально записей в 1 пакете
            int startIndex = MaxRecords - records;
            int offset = startIndex * RecordSize;
            int recordsLeft = records;                                    //Осталось прочитать строк
            while (recordsLeft > 0)
            {
                int packet = (recordsLeft <= maxPacketSize) ? (recordsLeft) : (maxPacketSize);
                byte[] rawdata = oblikFS.ReadSegment(ReadSegmentID, offset, packet * RecordSize);
                for (int i = 0; i < packet; i++)
                {
                    AddRecord(rawdata, i * RecordSize);
                }
                recordsLeft -= packet;
                offset += packet * RecordSize;
            }
        }
        
        public Log(ConnectionParams connectionParams) : base(connectionParams) { }
        public Log(OblikFS oblikFS) : base(oblikFS) { }

        protected abstract void AddRecord(byte[] rawdata, int index);
        protected abstract void CleanLog();

    }
}




