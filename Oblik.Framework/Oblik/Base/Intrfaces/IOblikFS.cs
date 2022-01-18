using Oblik.Driver;

namespace Oblik.FS
{
    public interface IOblikFS
    {
        byte[] ReadSegment(int segment, int offset, int len);
        void WriteSegment(int segment, int offset, byte[] data);
        int Address { get; set; }
        int Baudrate { get; set; }
        int Timeout { get; set; }
        UserLevel User { get; set; }
        string Password { get; set; }
        IOblikDriver OblikDriver { get; }
    }
}