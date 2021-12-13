using System;

namespace Oblik
{
    public class IntegralValues
    {
        /// <summary>
        /// Размер сырой структуры, байт
        /// </summary>
        public static int Size { get => 240; }

        public uint act_en_p { get; private set; }
        public uint act_en_n { get; private set; }
        public uint rea_en_p { get; private set; }
        public uint rea_en_n { get; private set; }
        public uint act_en_a_p { get; private set; }
        public uint act_en_a_n { get; private set; }
        public uint act_en_b_p { get; private set; }
        public uint act_en_b_n { get; private set; }
        public uint act_en_c_p { get; private set; }
        public uint act_en_c_n { get; private set; }
        public uint act_en_d_p { get; private set; }
        public uint act_en_d_n { get; private set; }
        public uint rea_en_a_p { get; private set; }
        public uint rea_en_a_n { get; private set; }
        public uint rea_en_b_p { get; private set; }
        public uint rea_en_b_n { get; private set; }
        public uint rea_en_c_p { get; private set; }
        public uint rea_en_c_n { get; private set; }
        public uint rea_en_d_p { get; private set; }
        public uint rea_en_d_n { get; private set; }
        public uint[,] channel { get; private set; }
        public uint exceed_a { get; private set; }
        public uint exceed_b { get; private set; }
        public uint exceed_c { get; private set; }
        public uint exceed_d { get; private set; }
        public float max_exc_a { get; private set; }
        public float max_exc_b { get; private set; }
        public float max_exc_c { get; private set; }
        public float max_exc_d { get; private set; }

        public IntegralValues(byte[] rawdata)
        {
            if (rawdata.Length != Size)
                throw new ArgumentException($"Integral Values raw data size must be {Size} bytes long");

            channel = new uint[8, 4];

            int index = 0;

            act_en_p = Convert.ToValue<uint>(rawdata, index);
            index += sizeof(uint);

            act_en_n = Convert.ToValue<uint>(rawdata, index);
            index += sizeof(uint);

            rea_en_p = Convert.ToValue<uint>(rawdata, index);
            index += sizeof(uint);

            rea_en_n = Convert.ToValue<uint>(rawdata, index);
            index += sizeof(uint);

            act_en_a_p = Convert.ToValue<uint>(rawdata, index);
            index += sizeof(uint);

            act_en_a_n = Convert.ToValue<uint>(rawdata, index);
            index += sizeof(uint);

            act_en_b_p = Convert.ToValue<uint>(rawdata, index);
            index += sizeof(uint);

            act_en_b_n = Convert.ToValue<uint>(rawdata, index);
            index += sizeof(uint);

            act_en_c_p = Convert.ToValue<uint>(rawdata, index);
            index += sizeof(uint);

            act_en_c_n = Convert.ToValue<uint>(rawdata, index);
            index += sizeof(uint);

            act_en_d_p = Convert.ToValue<uint>(rawdata, index);
            index += sizeof(uint);

            act_en_d_n = Convert.ToValue<uint>(rawdata, index);
            index += sizeof(uint);

            rea_en_a_p = Convert.ToValue<uint>(rawdata, index);
            index += sizeof(uint);

            rea_en_a_n = Convert.ToValue<uint>(rawdata, index);
            index += sizeof(uint);

            rea_en_b_p = Convert.ToValue<uint>(rawdata, index);
            index += sizeof(uint);

            rea_en_b_n = Convert.ToValue<uint>(rawdata, index);
            index += sizeof(uint);

            rea_en_c_p = Convert.ToValue<uint>(rawdata, index);
            index += sizeof(uint);

            rea_en_c_n = Convert.ToValue<uint>(rawdata, index);
            index += sizeof(uint);

            rea_en_d_p = Convert.ToValue<uint>(rawdata, index);
            index += sizeof(uint);

            rea_en_d_n = Convert.ToValue<uint>(rawdata, index);
            index += sizeof(uint);

            for (int i = 0; i < 8; i++)
            {
                for (int k = 0; k < 4; k++)
                {
                    channel[i, k] = Convert.ToValue<uint>(rawdata, index);
                    index += sizeof(uint);
                }
            }

            exceed_a = Convert.ToValue<uint>(rawdata, index);
            index += sizeof(uint);

            exceed_b = Convert.ToValue<uint>(rawdata, index);
            index += sizeof(uint);

            exceed_c = Convert.ToValue<uint>(rawdata, index);
            index += sizeof(uint);

            exceed_d = Convert.ToValue<uint>(rawdata, index);
            index += sizeof(uint);

            max_exc_a = Convert.ToValue<float>(rawdata, index);
            index += sizeof(float);

            max_exc_b = Convert.ToValue<float>(rawdata, index);
            index += sizeof(float);

            max_exc_c = Convert.ToValue<float>(rawdata, index);
            index += sizeof(float);

            max_exc_d = Convert.ToValue<float>(rawdata, index);
        }
    }
}