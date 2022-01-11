using System.Runtime.InteropServices;

namespace Oblik
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct NetworkConfig
    {
        public ushort Divisor;
        public byte Addr;
    }
}