using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oblik.FS;

namespace Oblik
{
    public  class LastYearIntegralValues : IntegralValues
    {
        public override int Size { get => 240; }
        public override int ReadSegmentID { get => 31; }
        public override int WriteSegmentID { get => 0; }
        public LastYearIntegralValues(ConnectionParams connectionParams) : base(connectionParams) { }

        public LastYearIntegralValues(OblikFS oblikFS) : base(oblikFS) { }
    }
}
