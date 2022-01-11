using System.Runtime.InteropServices;

namespace Oblik
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MinuteValues
    {
        public uint Channel_8;
        public uint Channel_7;
        public uint Channel_6;
        public uint Channel_5;
        public uint Channel_4;
        public uint Channel_3;
        public uint Channel_2;
        public uint Channel_1;
        public float Rea_ener;
        public float Act_ener;
    }
}