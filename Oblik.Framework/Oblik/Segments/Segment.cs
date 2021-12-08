using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oblik
{
    abstract class Segment
    {
        protected List<object> propertyList;
        protected byte[] rawdata;
      
        abstract public int Size { get; }
        public Segment(byte[] rawdata, int index)
        {
            if ((rawdata.Length - index) < Size)
                throw new ArgumentException($"Raw data size must be {Size} bytes long");
            this.rawdata = new byte[Size];
            Array.Copy(rawdata, index, this.rawdata, 0, Size);
        }
        protected void Convert()
        {

        }

 
    }
}
