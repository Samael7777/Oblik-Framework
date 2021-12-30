﻿using System;
using System.Collections.Generic;
using Oblik.Driver;

namespace Oblik.FS
{
    public partial class OblikFS : IOblikFS
    {
        /*-------------------------Private--------------------------------------*/
        /// <summary>
        /// Флаг доступа к сегменту
        /// </summary>
        private enum Access
        {
            /// <summary>
            /// Доступ на чтение
            /// </summary>
            Read = 0,
            /// <summary>
            /// Доступ на запись
            /// </summary>
            Write = 1,
        }

        /// <summary>
        /// Массив данных L1
        /// </summary>
        private byte[] l1;

        /// <summary>
        /// Массив данных L2
        /// </summary>
        private byte[] l2;

        /// <summary>
        /// Драйвер интерфейса связи
        /// </summary>
        private IOblikDriver oblikDriver;

        /*-------------------------Constructors---------------------------------*/
        public OblikFS(IOblikDriver oblikDriver)
        {

            this.oblikDriver = oblikDriver;
            l1 = new byte[0];
            l2 = new byte[0];
        }

        /*-------------------------Public---------------------------------------*/
        /// <summary>
        /// Драйвер счетчика
        /// </summary>
        public IOblikDriver OblikDriver { get => oblikDriver; }
        
        /// <summary>
        /// Чтение части сегмента счетчика
        /// </summary>
        /// <param name="segment">Сегмент счетчика</param>
        /// <param name="offset">Смещение относительно начала сегмента</param>
        /// <param name="len">Количество данных для чтения</param>
        /// <param name="data">Полученные данные</param>
        /// <returns>Успех операции</returns>
        public byte[] ReadSegment(int segment, int offset, int len)
        {
            const int maxPacket = 250;      //Максимальный размер пакета L2
            int bytesLeft = len;            //Осталось байт для чтения
            int nextOffset = offset;        //Смещение следующего чтения
            byte[] result = new byte[0];

            while (bytesLeft > 0)
            {
                int querySize = (bytesLeft > maxPacket) ? maxPacket : bytesLeft;
                PerformFrame((byte)segment, (ushort)nextOffset, (byte)querySize, Access.Read);
                byte[] answer = oblikDriver.Request(l1);      
                DecodeSegmentError(answer[0]);      //Проверка на ошибки L2
                nextOffset += querySize;
                bytesLeft -= querySize;
                //Сохранение результата
                int nextIndex = result.Length;
                Array.Resize(ref result, answer[1] + nextIndex);
                Array.Copy(answer, 2, result, nextIndex, answer[1]);

               
            }
            return result;
        }

        /// <summary>
        /// Запись в сегмент счетчика
        /// </summary>
        /// <param name="segment">Сегмент счетчика</param>
        /// <param name="offset">Смещение относительно начала сегмента</param>
        /// <param name="data">Данные для записи</param>
        /// <returns>Успех операции</returns>
        public void WriteSegment(int segment, int offset, byte[] data)
        {
            //Обрезка аргументов
            segment &= 0xFF;
            offset &= 0xFFFF;

            //Проверка на наличие данных для записи
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }
            PerformFrame((byte)segment, (ushort)offset, (byte)data.Length, Access.Write, data);
            byte[] answer = oblikDriver.Request(l1);
            //Проверка на ошибки L2
            if (answer[0] != 0)
            {
                DecodeSegmentError(answer[2]);
            }
        }
    }
}