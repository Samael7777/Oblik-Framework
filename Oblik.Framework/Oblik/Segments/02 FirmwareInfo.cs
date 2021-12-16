using Oblik.FS;

namespace Oblik
{ 
    public class FirmwareInfo : Segment
    {
        public override int Size { get => 2; }
        public override int ReadSegmentID { get => 2; }
        public override int WriteSegmentID { get => 0; }

        public int Version { get; protected set; }
        public int Build { get; protected set; }

        public FirmwareInfo(ConnectionParams connectionParams) : base(connectionParams) { }
        public FirmwareInfo(OblikFS oblikFS) : base(oblikFS) { }

        protected override void FromRaw()
        {
            Version = rawdata[0];
            Build = rawdata[1];
        }
    }
}
