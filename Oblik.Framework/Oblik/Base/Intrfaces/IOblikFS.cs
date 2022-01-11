using Oblik.Driver;

namespace Oblik.FS
{
    public interface IOblikFS
    {
        byte[] ReadSegment(int segment, int offset, int len);
        byte[] ReadSegment(int segment, int offset, int len, int packetsize);
        void WriteSegment(int segment, int offset, byte[] data);
        void WriteSegment(int segment, int offset, int packetsize, byte[] data);
        int Address { get; set; }
        int Baudrate { get; set; }
        UserLevel User { get; set; }
        string Password { get; set; }
        IOblikDriver OblikDriver { get; }
    }
}