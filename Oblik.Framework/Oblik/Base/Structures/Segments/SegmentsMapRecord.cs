using System.Runtime.InteropServices;

namespace Oblik
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SegmentsMapRecord
    {
        public ushort Size;
        public byte Rights;
        public byte Id;
    }
}
