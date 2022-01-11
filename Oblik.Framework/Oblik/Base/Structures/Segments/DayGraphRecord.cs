using System.Runtime.InteropServices;

namespace Oblik
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct DayGraphRecord
    {
        public ushort Channel_8;
        public ushort Channel_7;
        public ushort Channel_6;
        public ushort Channel_5;
        public ushort Channel_4;
        public ushort Channel_3;
        public ushort Channel_2;
        public ushort Channel_1;
        public UMiniFlo Rea_en_n;
        public UMiniFlo Rea_en_p;
        public UMiniFlo Act_en_n;
        public UMiniFlo Act_en_p;
        public Time TimeStamp;
    }
}
