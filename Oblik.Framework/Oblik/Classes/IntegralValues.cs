using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oblik
{
    public class IntegralValues
    {
        public const int Size = 240;

        /// <summary>
        /// Сырые данные
        /// </summary>
        private byte[] serialize;

        public uint act_en_p { get; private set; }
        public uint act_en_n { get; private set; }
        public uint rea_en_p { get; private set; }
        public uint rea_en_n { get; private set; }
        public uint[,] act_en_q { get; private set; }
        public uint[,] rea_en_q { get; private set; }
        public uint[,] channel_q { get; private set; }
        public uint[] exceed_q { get; private set; }
        public float[] max_exc_q { get; private set; }

        public byte[] RawData
        {
            set
            {
                CheckRawSize(value.Length);
                value.CopyTo(serialize, 0);
                FromRaw();
            }
        }

        public IntegralValues(byte[] rawdata)
        {
            CheckRawSize(rawdata.Length);
            act_en_q = new uint[4, 2];
            rea_en_q = new uint[4, 2];
            channel_q = new uint[8, 4];
            exceed_q = new uint[4];
            max_exc_q = new float[4];
            rawdata.CopyTo(serialize, 0);
            FromRaw();
        }

        private void FromRaw()
        {

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
