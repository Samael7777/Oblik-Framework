using Oblik.FS;

namespace Oblik
{
    public class CurrentDayIntegralValues : IntegralValues
    {
        public new static int Size { get => 240; }
        public new static int ReadSegmentID { get => 24; }
        public new static int WriteSegmentID { get => 0; }
        public CurrentDayIntegralValues(ConnectionParams connectionParams) : base(connectionParams) { }

        public CurrentDayIntegralValues(OblikFS oblikFS) : base(oblikFS) { }        
    }
}
