using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oblik
{
    /// <summary>
    /// Запись карты сегментов
    /// </summary>
    public class SegmentsMapRec
    {
        public const int Size = 5;
        /// <summary>
        /// Номер сегмента
        /// </summary>
        public int Num { get; private set; }
        /// <summary>
        /// Права доступа
        /// </summary>
        public int Right { get; private set; }
        /// <summary>
        /// Доступ: 0 - чтение, 1 - запись
        /// </summary>
        public int Access { get; private set; }
        /// <summary>
        /// Размер сегмента, байт
        /// </summary>
        public int SegSize { get; private set; }

        public SegmentsMapRec (byte [] rawdata)
        {
            CheckRawSize(rawdata.Length);
            Num = rawdata[0];
            Right = (byte)(rawdata[1] & 15);
            Access = (rawdata[1] & 128) >> 7;
            SegSize = Utils.ToUint16(Utils.ArrayPart(rawdata, 2, 2));

        }
        /// <summary>
        /// Проверка размера сырых данных
        /// </summary>
        /// <param name="size">Размер сырых данных</param>
        /// <exception cref="ArgumentException"></exception>
        private void CheckRawSize(int size)
        {
            if (size != Size)
                throw new ArgumentException($"SegmentsMapRec raw data size must be {Size} bytes long");
        }
    }

    /// <summary>
    /// Карта сегментов
    /// </summary>
    public class SegmentsMap
    {
        public int NumSegments { get; private set; }
        public List<SegmentsMapRec> SegMap { get; private set; }
        public SegmentsMap (byte[] rawdata)
        {
            SegMap = new List<SegmentsMapRec>();

            //Количество записей в карте
            NumSegments = rawdata[0];
            int size = SegmentsMapRec.Size;
            //Заполнение карты
            for (int i = 0; i < NumSegments; i++)
            {
                SegmentsMapRec record = new SegmentsMapRec(Utils.ArrayPart(rawdata, (i * size + 1), size));
                SegMap.Add(record);
            }
        }

    }
}