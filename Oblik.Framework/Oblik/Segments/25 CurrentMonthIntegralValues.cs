using Oblik.FS;

namespace Oblik
{
    public class CurrentMonthIntegralValues : IntegralValues
    {
        public new static int Size { get => 240; }
        public new static int ReadSegmentID { get => 25; }
        public new static int WriteSegmentID { get => 0; }
        public CurrentMonthIntegralValues(ConnectionParams connectionParams) : base(connectionParams) { }
        public CurrentMonthIntegralValues(OblikFS oblikFS) : base(oblikFS) { }
    }
}
