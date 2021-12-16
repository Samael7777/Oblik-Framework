using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oblik.FS;

namespace Oblik
{
    public class LastQuarterIntegralValues : IntegralValues
    {
        public override int Size { get => 240; }
        public override int ReadSegmentID { get => 30; }
        public override int WriteSegmentID { get => 0; }
        public LastQuarterIntegralValues(ConnectionParams connectionParams) : base(connectionParams) { }

        public LastQuarterIntegralValues(OblikFS oblikFS) : base(oblikFS) { }
    }
}
