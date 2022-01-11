using System.Runtime.InteropServices;

namespace Oblik
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct EventLogRecord
    {
        public byte Code;
        public Time TimeStamp;
    }
}
