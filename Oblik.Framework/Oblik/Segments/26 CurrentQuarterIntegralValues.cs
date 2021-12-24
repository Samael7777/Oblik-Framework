using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oblik.FS;

namespace Oblik
{
    public class CurrentQuarterIntegralValues : IntegralValues
    {
        public override int Size { get => 240; }
        public override int ReadSegmentID { get => 26; }
        public override int WriteSegmentID { get => 0; }
        
        public CurrentQuarterIntegralValues(ConnectionParams connectionParams) : base(connectionParams) { }
        public CurrentQuarterIntegralValues(OblikFS oblikFS) : base(oblikFS) { }
    }
}
