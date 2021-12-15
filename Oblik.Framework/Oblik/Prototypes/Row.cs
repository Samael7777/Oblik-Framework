using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oblik
{
    public abstract class Row
    {
        public static int RecordSize { get; }
        public Row(byte[] rawdata, int index)
        {
            if ((rawdata.Length - index) < RecordSize)
                throw new ArgumentException($"Raw data size must be {RecordSize} bytes long");
        }
    }
}
