using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oblik.FS;

namespace Oblik
{
    public class HalfHourGraph : Log
    {
        public override int Size { get => 600; }
        public override int ReadSegmentID { get => 49; }
        public override int WriteSegmentID { get => 0; }
        public override int ClearSegmentID { get => 0; }
        public override int PointerSegmentID { get => 48; }
        public override int MaxRecords { get => 30; }
        public override int RecordSize { get => 20; }
        public List<HalfHourGraphRow> Records { get; protected set; }

        public HalfHourGraph(IOblikFS oblikFS) : base(oblikFS)
        {
            Records = new List<HalfHourGraphRow>();
        }

        protected override void CleanRecords()
        {
            Records.Clear();
        }
        protected override void AddRecord(byte[] rawdata, int index)
        {
            Records.Add(new HalfHourGraphRow(rawdata, index));
        }
        public override void ReadRecords()
        {
            numOfRecs = oblikFS.ReadSegment(PointerSegmentID, 0, 1)[0];
        }
    }
}
