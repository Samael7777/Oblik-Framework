using System.Runtime.InteropServices;

namespace Oblik
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CurrentValues
    {
        public ushort reserved3;
        public ushort reserved2;
        public ushort Freq;
        public ushort reserved1;
        public SMiniFlo Rea_pw;
        public SMiniFlo Act_pw;
        public UMiniFlo Volt3;
        public UMiniFlo Volt2;
        public UMiniFlo Volt1;
        public UMiniFlo Curr3;
        public UMiniFlo Curr2;
        public UMiniFlo Curr1;  
    }
}
