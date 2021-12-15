using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oblik.FS;

namespace Oblik
{
    public class LastMonthIntegralValues : IntegralValues
    {
        public new static int Size { get => 240; }
        public new static int ReadSegmentID { get => 29; }
        public new static int WriteSegmentID { get => 0; }
        public LastMonthIntegralValues(ConnectionParams connectionParams) : base(connectionParams) { }
        public LastMonthIntegralValues(OblikFS oblikFS) : base(oblikFS) { }
    }
}
