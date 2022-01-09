using Oblik.FS;

namespace Oblik
{
    public abstract class Segment
    {
        protected byte[] rawdata;
        protected IOblikFS oblikFS;
        public abstract int Size { get; }
        public abstract int WriteSegmentID { get; }
        public abstract int ReadSegmentID { get; }


        public Segment(IOblikFS oblikFS)
        {
            this.oblikFS = oblikFS;
            rawdata = new byte[Size];
        }

        public virtual void Read()
        {
            if (ReadSegmentID == 0)
                throw new OblikIOException("Not readable segment", Error.NotReadableSegError);

            rawdata = oblikFS.ReadSegment(ReadSegmentID, 0, Size);
            FromRaw();
        }

        public virtual void Write()
        {
            if (WriteSegmentID == 0)
                throw new OblikIOException("Not writeable segment", Error.NotWriteableSegError);

            ToRaw();
            oblikFS.WriteSegment(WriteSegmentID, 0, rawdata);
        }

        protected virtual void ToRaw() { }
        protected virtual void FromRaw() { }
    }
}