using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oblik
{
    public class MinuteValues
    {
        /// <summary>
        /// Размер сырой структуры, байт
        /// </summary>
        public static int Size { get => 40; }
        public float act_ener { get; private set; }
        public float rea_ener { get; private set; }
        public uint[] channel { get; private set; }

        public MinuteValues(byte[] rawdata)
        {
            if (rawdata.Length != Size)
                throw new ArgumentException($"MinuteValues raw data size must be {Size} bytes long");

            channel = new uint[8];
            int index = 0;
            
            act_ener = Convert.ToValue<float>(rawdata, index);
            index += sizeof(uint);
            
            rea_ener = Convert.ToValue<float>(rawdata, index);
            index += sizeof(uint);
            
            for (int i = 0; i < 8; i++)
            {
                channel[i] = Convert.ToValue<uint>(rawdata, index);
            }
        }
    }
}
