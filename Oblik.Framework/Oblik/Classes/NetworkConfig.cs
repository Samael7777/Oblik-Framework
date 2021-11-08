using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oblik
{
    /// <summary>
    /// Сетевая конфигурация счетчика
    /// </summary>
    public class NetworkConfig
    {
        /// <summary>
        /// Сетевой адрес по протоколу RS-48
        /// </summary>
        public byte Addr { get; set; }
        
        /// <summary>
        /// Скорость соединения, делитель от 115200
        /// </summary>
        public ushort Divisor { get; set; }
        
        /// <summary>
        /// Скорость соединения
        /// </summary>
        public int Speed { get; set; }

        NetworkConfig (byte[] data)
        {

        } 

        NetworkConfig(int speed, byte addr)
        {
                        
        }

        private void FromRaw(byte[] data)
        {

        }
        
    }
}
