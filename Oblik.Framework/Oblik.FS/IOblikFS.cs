using System.Collections.Generic;

namespace Oblik.FS
{
    public interface IOblikFS
    {
        ConnectionParams CurrentConnectionParams { get; set; }
        List<int> GetIOErrorsList { get; }
        byte[] ReadSegment(byte segment, ushort offset, byte len);
        void WriteSegment(byte segment, ushort offset, byte[] data);
    }
}