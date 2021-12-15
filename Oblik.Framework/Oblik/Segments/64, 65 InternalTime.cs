using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oblik.FS;

namespace Oblik
{
    public class InternalTime : Segment
    {
        private DateTime time;

        public new static int Size { get => 4; }
        public new static int ReadSegmentID { get => 64; }
        public new static int WriteSegmentID { get => 65; }
        public DateTime Time
        {
            get
            {
                Read();
                return time;
            }
        }
        private void Init()
        {
            time = new DateTime();
        }
        public InternalTime(OblikFS oblikFS) : base(oblikFS)
        {
            Init();
        }
        public InternalTime(ConnectionParams connectionParams) : base(connectionParams)
        {
            Init();
        }
        public void SetCurrentTime()
        {   
            Write();
        }
        protected override void FromRaw()
        {
            time = Convert.ToUTCTime(rawdata, 0).ToLocalTime();
        }
        protected override void ToRaw()
        {
            time = DateTime.UtcNow;
            rawdata = Convert.ToTime(time);
        }

    }
}
