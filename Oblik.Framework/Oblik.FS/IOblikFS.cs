using System.Collections.Generic;
using Oblik.Driver;

namespace Oblik.FS
{
    public interface IOblikFS
    {
        byte[] ReadSegment(int segment, int offset, int len);
        void WriteSegment(int segment, int offset, byte[] data);
        IOblikDriver OblikDriver { get; }
    }
}