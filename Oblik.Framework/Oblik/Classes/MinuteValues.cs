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
        public const int Size = 40;

        public float act_ener { get; private set; }
        public float rea_ener { get; private set; }
        public uint[] channel { get; private set; }

        public MinuteValues(byte[] rawdata)
        {
            channel = new uint[8];
            if (rawdata.Length != Size)
                throw new ArgumentException($"MinuteValues raw data size must be {Size} bytes long");

            rawdata.CopyTo(rawdata, 0);
            int index = 0;
            act_ener = Utils.ConvertToVal<float>(rawdata, index);
            index += sizeof(uint);
            rea_ener = Utils.ConvertToVal<float>(rawdata, index);
            index += sizeof(uint);
            for (int i = 0; i < 8; i++)
            {
                channel[i] = Utils.ConvertToVal<uint>(rawdata, index);
            }
        }
    }
}
