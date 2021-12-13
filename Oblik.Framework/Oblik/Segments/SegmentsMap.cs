using System;
using System.Collections.Generic;

namespace Oblik
{
    /// <summary>
    /// Запись карты сегментов
    /// </summary>
    public class SegmentsMapRec
    {
        /// <summary>
        /// Размер сырой структуры, байт
        /// </summary>
        public static int Size { get => 5; }

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

        public SegmentsMapRec(byte[] rawdata, int index)
        {
            if ((rawdata.Length - index) < Size)
                throw new ArgumentException($"SegmentsMapRec raw data size must be {Size} bytes long");

            Num = rawdata[index];
            Right = (byte)(rawdata[index + 1] & 15);
            Access = (rawdata[index + 1] & 128) >> 7;
            SegSize = Convert.ToValue<UInt16>(rawdata, 2);
        }
    }

    /// <summary>
    /// Карта сегментов
    /// </summary>
    public class SegmentsMap
    {
        public int totalSegments { get; private set; }
        public List<SegmentsMapRec> SegmentsMapList { get; private set; }

        public SegmentsMap(byte[] rawdata)
        {
            SegmentsMapList = new List<SegmentsMapRec>();

            //Количество записей в карте
            totalSegments = rawdata[0];
            //Заполнение карты
            for (int i = 0; i < totalSegments; i++)
            {
                SegmentsMapRec record = new SegmentsMapRec(rawdata, i * SegmentsMapRec.Size);
                SegmentsMapList.Add(record);
            }
        }
    }
}