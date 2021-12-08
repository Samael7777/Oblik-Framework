using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oblik
{
    public class HalfHourValues
    {
        /// <summary>
        /// Размер сырой структуры, байт
        /// </summary>
        public int Size { get => 80; }
        public float act_pw_cur { get; private set; }
        public float rea_pw_cur { get; private set; }
        public uint[] channel_cur { get; private set; }
        public float act_pw_old { get; private set; }
        public float rea_pw_old { get; private set; }
        public uint[] channel_old { get; private set; }

        public HalfHourValues(byte[] rawdata)
        {
            if (rawdata.Length != Size)
                throw new ArgumentException($"Half-hour values raw data size must be {Size} bytes long");

            channel_cur = new uint[8];
            channel_old = new uint[8];
            
            int index = 0;
            act_pw_cur = Convert.ToValue<float>(rawdata, index);
            index += sizeof(float);
            
            rea_pw_cur = Convert.ToValue<float>(rawdata, index);
            index += sizeof(float);
            
            for (int i = 0; i < 8; i++)
            {
                channel_cur[i] = Convert.ToValue<uint>(rawdata, index);
                index += sizeof(uint);
            }
            
            act_pw_old = Convert.ToValue<float>(rawdata, index);
            index += sizeof(float);
            
            rea_pw_old = Convert.ToValue<float>(rawdata, index);
            index += sizeof(float);
            
            for (int i = 0; i < 8; i++)
            {
                channel_old[i] = Convert.ToValue<uint>(rawdata, index);
                index += sizeof(uint);
            }
        }
    }
}
