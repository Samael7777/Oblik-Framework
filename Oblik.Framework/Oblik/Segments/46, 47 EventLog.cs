﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oblik.FS;

namespace Oblik
{
    public class EventLog : Log
    {
        public override int Size { get => 4000; }
        public override int ReadSegmentID { get => 45; }
        public override int WriteSegmentID { get => 0; }
        public int ClearSegmentID { get => 89; }
        public int PointerSegmentID { get => 44; }
        public override int NumberOfRecords
        {
            get => Convert.ToValue<UInt16>(oblikFS.ReadSegment(PointerSegmentID, 0, 2), 0);
        }
        public override int MaxRecords { get => 800; }
        public override int RecordSize { get => 5; }
        public List<EventLogRow> Records { get; protected set; }
        public EventLog(OblikFS oblikFS) : base(oblikFS)
        {
            Init();
        }
        public EventLog(ConnectionParams connectionParams) : base(connectionParams)
        {
            Init();
        }
        protected override void CleanLog()
        {
            Records.Clear();
        }
        private void Init()
        {
            Records = new List<EventLogRow>();
        }
        public override void Read()
        {
            int currentRecords = NumberOfRecords;
            GetLastRecords(currentRecords);
        }
        protected override void AddRecord(byte[] rawdata, int index)
        {
            Records.Add(new EventLogRow(rawdata, index));
        }
        public void CleanEventLog()
        {
            byte[] req = new byte[2];
            req[1] = oblikFS.CurrentConnectionParams.Address;
            req[0] = (byte)(~req[1]);
            oblikFS.WriteSegment(ClearSegmentID, 0, req);
        }
    }
}