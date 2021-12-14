using System;
using System.Collections.Generic;
using Oblik.FS;

namespace Oblik
{
    public class DayGraph : Log
    {
        public override int Size { get => 49000; }
        public override int ReadSegmentID { get => 45; }
        public override int WriteSegmentID { get => 0; }
        public int ClearSegmentID { get => 88; }
        public int PointerSegmentID { get => 44; }
        public override int NumberOfRecords
        {
            get => Convert.ToValue<UInt16>(oblikFS.ReadSegment(PointerSegmentID, 0, 2), 0);
        }
        public override int MaxRecords { get => 1750; }
        public override int RecordSize { get => 28; }
        public List<DayGraphRow> Records { get; protected set; }
        public DayGraph(OblikFS oblikFS) : base(oblikFS) 
        {
            Init();
        }
        public DayGraph(ConnectionParams connectionParams) : base(connectionParams)
        {
            Init();
        }
        protected override void CleanLog()
        {
            Records.Clear();
        }
        private void Init()
        {
            Records = new List<DayGraphRow>();
        }
        public override void Read()
        {
            int currentRecords = NumberOfRecords;
            GetLastRecords(currentRecords);
        }
        protected override void AddRecord(byte[] rawdata, int index)
        {
            Records.Add(new DayGraphRow(rawdata, index));
        }
        public void CleanDayGraph()
        {
            byte[] req = new byte[2];
            req[1] = oblikFS.CurrentConnectionParams.Address;
            req[0] = (byte)(~req[1]);
            oblikFS.WriteSegment(ClearSegmentID, 0, req);
        }
    }
}
