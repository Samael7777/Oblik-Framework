using System;
using System.Collections.Generic;
using Oblik.FS;

namespace Oblik
{
    public class DayGraph : Log
    {
        public new static int Size { get => 49000; }
        public new static int ReadSegmentID { get => 45; }
        public new static int WriteSegmentID { get => 0; }
        public new static int ClearSegmentID { get => 88; }
        public new static int PointerSegmentID { get => 44; }
        public override int NumberOfRecords
        {
            get => Convert.ToValue<UInt16>(oblikFS.ReadSegment(PointerSegmentID, 0, 2), 0);
        }
        public new static int MaxRecords { get => 1750; }
        public new static int RecordSize { get => 28; }
       
        public List<DayGraphRow> Records { get; protected set; }
        public DayGraph(OblikFS oblikFS) : base(oblikFS) 
        {
            Init();
        }
        public DayGraph(ConnectionParams connectionParams) : base(connectionParams)
        {
            Init();
        }
        protected override void CleanRecords()
        {
            Records.Clear();
        }
        private void Init()
        {
            Records = new List<DayGraphRow>();
        }
        protected override void AddRecord(byte[] rawdata, int index)
        {
            Records.Add(new DayGraphRow(rawdata, index));
        }
    }
}
