using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oblik
{
    public class HalfHourGraph : Log
    {
        public new static int Size { get => 600; }
        public new static int ReadSegmentID { get => 49; }
        public new static int WriteSegmentID { get => 0; }
        public new static int ClearSegmentID { get => 0; }
        public new static int PointerSegmentID { get => 48; }
        public override int NumberOfRecords
        {
            get => Convert.ToValue<UInt16>(oblikFS.ReadSegment(PointerSegmentID, 0, 2), 0);
        }
        public new static int MaxRecords { get => 30; }
        public new static int RecordSize { get => 20; }
        public List<HalfHourGraphRow> Records { get; protected set; }
        protected override void CleanRecords()
        {
            Records.Clear();
        }
        protected override void AddRecord(byte[] rawdata, int index)
        {
            Records.Add(new HalfHourGraphRow(rawdata, index));
        }
    }
}
