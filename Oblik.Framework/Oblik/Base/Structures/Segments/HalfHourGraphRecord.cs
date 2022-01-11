using System.Runtime.InteropServices;

namespace Oblik
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct HalfHourGraphRecord
    {
        public ushort Channel_8;
        public ushort Channel_7;
        public ushort Channel_6;
        public ushort Channel_5;
        public ushort Channel_4;
        public ushort Channel_3;
        public ushort Channel_2;
        public ushort Channel_1;
        public SMiniFlo Rea_ener;
        public SMiniFlo Act_ener;
    }
}
