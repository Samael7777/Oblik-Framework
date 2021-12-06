using System.Collections.Generic;

namespace Oblik.FS
{
    public interface IOblikFS
    {
        ConnectionParams CurrentConnectionParams { get; set; }
        byte[] ReadSegment(int segment, int offset, int len);
        void WriteSegment(int segment, int offset, byte[] data);
    }
}