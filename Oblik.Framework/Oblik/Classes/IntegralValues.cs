using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oblik
{
    public class IntegralValues
    {
        public const int Size = 240;    //Длина пакета

        public uint act_en_p { get; private set; }
        public uint act_en_n { get; private set; }
        public uint rea_en_p { get; private set; }
        public uint rea_en_n { get; private set; }
        public uint[,] act_en_q { get; private set; }
        public uint[,] rea_en_q { get; private set; }
        public uint[,] channel_q { get; private set; }
        public uint[] exceed_q { get; private set; }
        public float[] max_exc_q { get; private set; }

        public IntegralValues(byte[] rawdata)
        {
            CheckRawSize(rawdata.Length);
            act_en_q = new uint[4, 2];
            rea_en_q = new uint[4, 2];
            channel_q = new uint[8, 4];
            exceed_q = new uint[4];
            max_exc_q = new float[4];
            
            int index = 0;

            act_en_p = Utils.ToUint32(Utils.ArrayPart(rawdata, index, sizeof(uint)));
            index += sizeof(uint);
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
