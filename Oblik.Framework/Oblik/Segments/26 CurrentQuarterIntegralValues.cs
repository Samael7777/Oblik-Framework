using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oblik.FS;

namespace Oblik
{
    public class CurrentQuarterIntegralValues : IntegralValues
    {
        public new static int Size { get => 240; }
        public new static int ReadSegmentID { get => 26; }
        public new static int WriteSegmentID { get => 0; }
        public CurrentQuarterIntegralValues(ConnectionParams connectionParams) : base(connectionParams) { }

        public CurrentQuarterIntegralValues(OblikFS oblikFS) : base(oblikFS) { }
    }
}
