using Oblik.FS;

namespace Oblik
{
    public abstract class Segment
    {
        protected byte[] rawdata;
        protected OblikFS oblikFS;
        public abstract int Size { get; }
        public abstract int WriteSegment { get; }
        public abstract int ReadSegment { get; }

        protected Segment()
        {
            rawdata = new byte[Size];
        }

        public Segment(ConnectionParams connectionParams) : this()
        {
            oblikFS = new OblikFS(connectionParams);
        }

        public Segment(OblikFS oblikFS) : this()
        {
            this.oblikFS = oblikFS;
        }

        public void Read()
        {
            rawdata = oblikFS.ReadSegment(ReadSegment, 0, Size);
            FromRaw();
        }

        public void Write()
        {
            ToRaw();
            oblikFS.WriteSegment(WriteSegment, 0, rawdata);
        }

        protected abstract void ToRaw();

        protected abstract void FromRaw();
    }
}