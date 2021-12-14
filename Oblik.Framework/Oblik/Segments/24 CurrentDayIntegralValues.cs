using Oblik.FS;

namespace Oblik
{
    public class CurrentDayIntegralValues : IntegralValues
    {
        public override int Size { get => 240; }
        public override int ReadSegmentID { get => 24; }
        public override int WriteSegmentID { get => 0; }
        public CurrentDayIntegralValues(ConnectionParams connectionParams) : base(connectionParams) { }

        public CurrentDayIntegralValues(OblikFS oblikFS) : base(oblikFS) { }        
    }
}
