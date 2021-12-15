using System;
using System.Collections.Generic;
using Oblik.FS;

namespace Oblik
{
    /// <summary>
    /// Карта сегментов
    /// </summary>
    public class SegmentsMap : Segment
    {
        private const int recordSize = 5;
        public new int Size
        {
            get
            {
                return TotalSegments * recordSize;
            }
        }
        public new static int ReadSegmentID { get => 1; }
        public new static int WriteSegmentID { get => 0; }
        public int TotalSegments { get; private set; }
        public List<SegmentsMapRow> SegmentsMapList { get; private set; }

        private void Init()
        {
            TotalSegments = 0;
        }

        public SegmentsMap(ConnectionParams connectionParams) : base(connectionParams)
        {
            Init();
        }
        public SegmentsMap(OblikFS oblikFS) : base(oblikFS)
        {
            Init();
        }
        protected override void FromRaw()
        {
            SegmentsMapList = new List<SegmentsMapRow>();

            //Количество записей в карте
            TotalSegments = rawdata[0];
            //Заполнение карты
            for (int i = 0; i < TotalSegments; i++)
            {
                SegmentsMapRow record = new SegmentsMapRow(rawdata, i * recordSize);
                SegmentsMapList.Add(record);
            }
        }
    }
}