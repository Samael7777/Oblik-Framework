using Oblik.FS;

namespace Oblik
{
    public abstract class Segment
    {
        protected byte[] rawdata;
        protected OblikFS oblikFS;
        public static int Size { get; }
        public static int WriteSegmentID { get; }
        public static int ReadSegmentID { get; }

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

        public virtual void Read()
        {
            if (ReadSegmentID == 0)
                throw new OblikIOException("Not readable segment", (int)Error.NotReadableSegError);

            rawdata = oblikFS.ReadSegment(ReadSegmentID, 0, Size);
            FromRaw();
        }

        public virtual void Write()
        {
            if (WriteSegmentID == 0)
                throw new OblikIOException("Not writeable segment", (int)Error.NotWriteableSegError);

            ToRaw();
            oblikFS.WriteSegment(WriteSegmentID, 0, rawdata);
        }

        protected virtual void ToRaw() { }

        protected virtual void FromRaw() { }
    }
}