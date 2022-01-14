using System.Runtime.InteropServices;

namespace Oblik
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MeterNetworkConfig
    {
        public ushort Divisor;
        public byte Addr;
    }
}