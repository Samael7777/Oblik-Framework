using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oblik
{
    public abstract class Segment
    {
 
        public static int Size { get; }
        public static int SegmentID { get; }
        public Segment(byte[] rawdata, int index)
        {
            if ((rawdata.Length - index) < Size)
                throw new ArgumentException($"Raw data size must be {Size} bytes long");
        }
    }
}
