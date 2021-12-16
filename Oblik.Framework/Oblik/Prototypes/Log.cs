using System;
using System.Collections.Generic;
using Oblik.FS;


namespace Oblik
{
    public abstract class Log : Segment

    {   
        public abstract int MaxRecords { get; }
        public abstract int NumberOfRecords { get;}
        public abstract int RecordSize { get ; }
        public abstract int ClearSegmentID { get; }
        public abstract int PointerSegmentID { get; }

        protected Log() : base() { }
        
        public void GetLastRecords(int records)
        {           
            if (records > MaxRecords)
                throw new ArgumentOutOfRangeException($"Records to read must be below or equal {MaxRecords}");
            CleanRecords();
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
        public override void Read()
        {
            int currentRecords = NumberOfRecords;
            GetLastRecords(currentRecords);
        }
        protected abstract void CleanRecords();
        protected abstract void AddRecord(byte[] rawdata, int index);
        public virtual void CleanLog()
        {
            if (ClearSegmentID == 0)
                throw new OblikIOException("Not eraseable segment", (int)Error.NotEraseableSegError);

            byte[] req = new byte[2];
            req[1] = oblikFS.CurrentConnectionParams.Address;
            req[0] = (byte)(~req[1]);
            oblikFS.WriteSegment(ClearSegmentID, 0, req);
        }

    }
}




