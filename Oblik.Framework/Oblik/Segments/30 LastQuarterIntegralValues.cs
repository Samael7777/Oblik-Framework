using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oblik.FS;

namespace Oblik
{
    public class LastQuarterIntegralValues : IntegralValues
    {
        public new static int Size { get => 240; }
        public new static int ReadSegmentID { get => 30; }
        public new static int WriteSegmentID { get => 0; }
        public LastQuarterIntegralValues(ConnectionParams connectionParams) : base(connectionParams) { }

        public LastQuarterIntegralValues(OblikFS oblikFS) : base(oblikFS) { }
    }
}
