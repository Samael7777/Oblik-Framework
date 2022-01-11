using System.Runtime.InteropServices;

namespace Oblik
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct FirmwareInfo
    {
        public byte Build;
        public byte Version;
    }
}
