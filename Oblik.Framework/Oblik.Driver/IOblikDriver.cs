using System.Collections.Generic;

namespace Oblik.Driver
{
    public interface IOblikDriver
    {
        ConnectionParams CurrentConnectionParams { get; set; }
        byte[] Request(byte[] l1);
    }
}