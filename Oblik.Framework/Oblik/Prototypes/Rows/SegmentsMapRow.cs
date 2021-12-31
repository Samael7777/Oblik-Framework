﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oblik
{
    public class SegmentsMapRow
    {

        /// <summary>
        /// Размер сырой структуры, байт
        /// </summary>
        public int RecordSize { get => 5; }

        /// <summary>
        /// Номер сегмента
        /// </summary>
        public int Num { get; private set; }

        /// <summary>
        /// Права доступа
        /// </summary>
        public int Rights { get; private set; }

        /// <summary>
        /// Доступ: 0 - чтение, 1 - запись
        /// </summary>
        public int Access { get; private set; }

        /// <summary>
        /// Размер сегмента, байт
        /// </summary>
        public int Size { get; private set; }

        public SegmentsMapRow(byte[] rawdata, int index)
        {
            Num = rawdata[index];
            Rights = (byte)(rawdata[index + 1] & 15);
            Access = (rawdata[index + 1] & 128) >> 7;
            Size = Convert.ToValue<UInt16>(rawdata, index + 2);
        }
    }
}
