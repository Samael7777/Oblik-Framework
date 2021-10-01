using System.Collections.Generic;

namespace Oblik.Driver
{
    public interface IOblikDriver
    {
        ConnectionParams CurrentConnectionParams { get; set; }
        List<int> GetConnectionErrors { get; }
        byte[] Request(byte[] l1);
    }
}