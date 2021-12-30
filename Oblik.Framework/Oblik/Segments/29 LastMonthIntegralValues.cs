using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oblik.FS;

namespace Oblik
{
    public class LastMonthIntegralValues : IntegralValues
    {
        public override int Size { get => 240; }
        public override int ReadSegmentID { get => 29; }
        public override int WriteSegmentID { get => 0; }

        public LastMonthIntegralValues(IOblikFS oblikFS) : base(oblikFS) { }
    }
}
