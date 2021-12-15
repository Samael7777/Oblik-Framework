using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oblik.FS;

namespace Oblik
{
    public class LastDayIntegralValues : IntegralValues
    {
        public new static int Size { get => 240; }
        public new static int ReadSegmentID { get => 28; }
        public new static int WriteSegmentID { get => 0; }

        public LastDayIntegralValues(ConnectionParams connectionParams) : base(connectionParams) { }
        public LastDayIntegralValues(OblikFS oblikFS) : base(oblikFS) { }
    }
}
