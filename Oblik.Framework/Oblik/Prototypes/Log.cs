using System;
using System.Collections.Generic;
using Oblik.FS;


namespace Oblik
{
    public abstract class Log : Segment

    {
        protected int numOfRecs;
        public abstract int MaxRecords { get; }
        public virtual int NumberOfRecords 
        {
            get => numOfRecs;
        }
        public abstract int RecordSize { get ; }
        public abstract int ClearSegmentID { get; }
        public abstract int PointerSegmentID { get; }
      
        public void GetLastRecords(int records)
        {           
            if (records > MaxRecords)
                throw new ArgumentOutOfRangeException($"Records to read must be below or equal {MaxRecords}");
            CleanRecords();
            int maxPacketSize = 250 / RecordSize;                         //Максимально записей в 1 пакете
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
        
        public Log(IOblikFS oblikFS) : base(oblikFS) { }
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
                throw new OblikIOException("Not eraseable segment", Error.NotEraseableSegError);

            byte[] req = new byte[2];
            req[1] = oblikFS.ReadSegment(66, 0, 1)[0]; //Адрес RS-485 счетчика
            req[0] = (byte)(~req[1]);
            oblikFS.WriteSegment(ClearSegmentID, 0, req);
        }
        public virtual void ReadRecords()
        {
            numOfRecs = Convert.ToValue<UInt16>(oblikFS.ReadSegment(PointerSegmentID, 0, 2), 0);
        }

    }
}




