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
        private const int recordSize = 4;
        public override int Size
        {
            get
            {
                return TotalSegments * recordSize;
            }
        }
        public override int ReadSegmentID { get => 1; }
        public override int WriteSegmentID { get => 0; }
        public int TotalSegments { get; private set; }
        public List<SegmentsMapRow> SegmentsMapList { get; private set; }

        public SegmentsMap(IOblikFS oblikFS) : base(oblikFS)
        {
            TotalSegments = 0;
            SegmentsMapList = new List<SegmentsMapRow>();
        }

        public override void Read()
        {
            SegmentsMapList.Clear();    //Очистка списка
            TotalSegments = oblikFS.ReadSegment(ReadSegmentID, 0, 1)[0];    //Всего сегментов
            
            int maxPacketSize = 250 / recordSize;             //Максимально записей в 1 пакете
            int offset = 1;                                   //Начальное смещение
            int recordsLeft = TotalSegments;                  //Осталось прочитать строк
            while (recordsLeft > 0)
            {
                int packet = (recordsLeft <= maxPacketSize) ? (recordsLeft) : (maxPacketSize);
                int dataLenght = packet * recordSize;             
                rawdata = oblikFS.ReadSegment(ReadSegmentID, offset, dataLenght);
                //Заполнение карты
                for (int i = 0; i < packet; i++)
                {
                    SegmentsMapRow record = new SegmentsMapRow(rawdata, i * recordSize);
                    SegmentsMapList.Add(record);
                }
                recordsLeft -= packet;
                offset += packet * recordSize;
            }
        }
    }
}