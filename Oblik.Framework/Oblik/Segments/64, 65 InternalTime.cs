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

        public override int Size { get => 4; }
        public override int ReadSegmentID { get => 64; }
        public override int WriteSegmentID { get => 65; }
        public DateTime Time
        {
            get
            {
                Read();
                return time;
            }
        }
        public InternalTime(IOblikFS oblikFS) : base(oblikFS)
        {
            time = new DateTime();
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
