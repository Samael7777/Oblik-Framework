using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oblik.FS;

namespace Oblik
{
    public class CurrentYearIntegralValues : IntegralValues
    {
        public override int Size { get => 240; }
        public override int ReadSegmentID { get => 27; }
        public override int WriteSegmentID { get => 0; }
        public CurrentYearIntegralValues(ConnectionParams connectionParams) : base(connectionParams) { }

        public CurrentYearIntegralValues(OblikFS oblikFS) : base(oblikFS) { }
    }
}
