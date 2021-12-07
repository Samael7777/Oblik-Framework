using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oblik
{
    public class IntegralValues
    {
        /// <summary>
        /// Размер сырой структуры, байт
        /// </summary>
        public const int Size = 240;  

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
            CheckRawSize(rawdata.Length);
            channel = new uint[8, 4];
            
            int index = 0;

            act_en_p = Utils.ConvertToVal<uint>(rawdata, index);
            index += sizeof(uint);
            act_en_n = Utils.ConvertToVal<uint>(rawdata, index);
            index += sizeof(uint);
            rea_en_p = Utils.ConvertToVal<uint>(rawdata, index);
            index += sizeof(uint);
            rea_en_n = Utils.ConvertToVal<uint>(rawdata, index);
            index += sizeof(uint);
            act_en_a_p = Utils.ConvertToVal<uint>(rawdata, index);
            index += sizeof(uint);
            act_en_a_n = Utils.ConvertToVal<uint>(rawdata, index);
            index += sizeof(uint);
            act_en_b_p = Utils.ConvertToVal<uint>(rawdata, index);
            index += sizeof(uint);
            act_en_b_n = Utils.ConvertToVal<uint>(rawdata, index);
            index += sizeof(uint);
            act_en_c_p = Utils.ConvertToVal<uint>(rawdata, index);
            index += sizeof(uint);
            act_en_c_n = Utils.ConvertToVal<uint>(rawdata, index);
            index += sizeof(uint);
            act_en_d_p = Utils.ConvertToVal<uint>(rawdata, index);
            index += sizeof(uint);
            act_en_d_n = Utils.ConvertToVal<uint>(rawdata, index);
            index += sizeof(uint);
            rea_en_a_p = Utils.ConvertToVal<uint>(rawdata, index);
            index += sizeof(uint);
            rea_en_a_n = Utils.ConvertToVal<uint>(rawdata, index);
            index += sizeof(uint);
            rea_en_b_p = Utils.ConvertToVal<uint>(rawdata, index);
            index += sizeof(uint);
            rea_en_b_n = Utils.ConvertToVal<uint>(rawdata, index);
            index += sizeof(uint);
            rea_en_c_p = Utils.ConvertToVal<uint>(rawdata, index);
            index += sizeof(uint);
            rea_en_c_n = Utils.ConvertToVal<uint>(rawdata, index);
            index += sizeof(uint);
            rea_en_d_p = Utils.ConvertToVal<uint>(rawdata, index);
            index += sizeof(uint);
            rea_en_d_n = Utils.ConvertToVal<uint>(rawdata, index);
            index += sizeof(uint);
            for (int i = 0; i < 8; i++)
            {
                for (int k = 0; k < 4; k++)
                {
                    channel[i, k] = Utils.ConvertToVal<uint>(rawdata, index);
                    index += sizeof(uint);
                }
            }
            exceed_a = Utils.ConvertToVal<uint>(rawdata, index);
            index += sizeof(uint);
            exceed_b = Utils.ConvertToVal<uint>(rawdata, index);
            index += sizeof(uint);
            exceed_c = Utils.ConvertToVal<uint>(rawdata, index);
            index += sizeof(uint);
            exceed_d = Utils.ConvertToVal<uint>(rawdata, index);
            index += sizeof(uint);
            max_exc_a = Utils.ConvertToVal<float>(rawdata, index);
            index += sizeof(float);
            max_exc_b = Utils.ConvertToVal<float>(rawdata, index);
            index += sizeof(float);
            max_exc_c = Utils.ConvertToVal<float>(rawdata, index);
            index += sizeof(float);
            max_exc_d = Utils.ConvertToVal<float>(rawdata, index);
        }

        /// <summary>
        /// Проверка размера сырых данных
        /// </summary>
        /// <param name="size">Размер сырых данных</param>
        /// <exception cref="ArgumentException"></exception>
        private void CheckRawSize(int size)
        {
            if (size != Size)
                throw new ArgumentException($"Integral Values raw data size must be {Size} bytes long");
        }
    }
}


