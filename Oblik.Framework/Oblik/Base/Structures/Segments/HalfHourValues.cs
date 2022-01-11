using System.Runtime.InteropServices;

namespace Oblik
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct HalfHourValues
    {
        public uint Channel_8_old;
        public uint Channel_7_old;
        public uint Channel_6_old;
        public uint Channel_5_old;
        public uint Channel_4_old;
        public uint Channel_3_old;
        public uint Channel_2_old;
        public uint Channel_1_old;
        public float Rea_pw_old;
        public float Act_pw_old;
        public uint Channel_8_cur;
        public uint Channel_7_cur;
        public uint Channel_6_cur;
        public uint Channel_5_cur;
        public uint Channel_4_cur;
        public uint Channel_3_cur;
        public uint Channel_2_cur;
        public uint Channel_1_cur;
        public float Rea_pw_cur;
        public float Act_pw_cur;
    }
}
