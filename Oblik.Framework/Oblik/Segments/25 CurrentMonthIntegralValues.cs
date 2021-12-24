using Oblik.FS;

namespace Oblik
{
    public class CurrentMonthIntegralValues : IntegralValues
    {
        public override int Size { get => 240; }
        public override int ReadSegmentID { get => 25; }
        public override int WriteSegmentID { get => 0; }
        
        public CurrentMonthIntegralValues(ConnectionParams connectionParams) : base(connectionParams) { }
        public CurrentMonthIntegralValues(OblikFS oblikFS) : base(oblikFS) { }
    }
}
