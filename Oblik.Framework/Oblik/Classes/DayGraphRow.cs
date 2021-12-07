using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oblik
{
    /// <summary>
    /// Строка суточного графика
    /// </summary>
    public class DayGraphRow
    {
        /// <summary>
        /// Размер сырой структуры, байт
        /// </summary>
        public const int Size = 28;
        /// <summary>
        /// Реактивная энергия "+" за период сохранения
        /// </summary>
        public float Rea_en_p { get; private set; }
        /// <summary>
        /// Время записи
        /// </summary>
        public DateTime Time { get; private set; }
        /// <summary>
        /// Активная энергия "+" за период сохранения
        /// </summary>
        public float Act_en_p { get; private set; }
        /// <summary>
        /// Активная энергия "-" за период сохранения
        /// </summary>
        public float Act_en_n { get; private set; }
        /// <summary>
        /// Реактивная энергия "-" за период сохранения
        /// </summary>
        public float Rea_en_n { get; private set; }
        /// <summary>
        /// Количество импульсов по каналам
        /// </summary>
        public int[] Channel { get; private set; }

        /// <summary>
        /// Конструктор с преобразованим массива байт в строку суточного графика
        /// </summary>
        public DayGraphRow(byte[] rawdata)
        {
            //Проверка корректности входных данных
            CheckRawSize(rawdata.Length);
            int index = 0;
            //time (4 байта)
            Time = Utils.ToUTCTime(rawdata, index).ToLocalTime();
            index += 4;
            //act_en_p (2 байта)
            Act_en_p = Utils.ToUminiflo(rawdata, index);
            index += 2;
            //act_en_n (2 байта)
            Act_en_n = Utils.ToUminiflo(rawdata, index);
            index += 2;
            //rea_en_p (2 байта)
            Rea_en_p = Utils.ToUminiflo(rawdata, index);
            index += 2;
            //rea_en_n (2 байта)
            Rea_en_n = Utils.ToUminiflo(rawdata, index);
            index += 2;
            Channel = new int[8];
            for (int i = 0; i <= 7; i++)
            {
                Channel[i] = (int)Utils.ConvertToVal<UInt16>(rawdata, index);
                index += 2;
            }
        }
        /// <summary>
        /// Проверка размера сырых данных
        /// </summary>
        /// <param name="size">Размер сырых данных</param>
        /// <exception cref="ArgumentException"></exception>
        private void CheckRawSize(int size)
        {
            if (size != Size)
                throw new ArgumentException($"DayGraphRow raw data size must be {Size} bytes long");
        }
    }
}
