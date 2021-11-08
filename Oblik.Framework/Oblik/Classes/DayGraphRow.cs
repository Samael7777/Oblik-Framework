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
        /// Реактивная энергия "+" за период сохранения
        /// </summary>
        public float Rea_en_p { get; }
        /// <summary>
        /// Время записи
        /// </summary>
        public DateTime Time { get; }
        /// <summary>
        /// Активная энергия "+" за период сохранения
        /// </summary>
        public float Act_en_p { get; }
        /// <summary>
        /// Активная энергия "-" за период сохранения
        /// </summary>
        public float Act_en_n { get; }
        /// <summary>
        /// Реактивная энергия "-" за период сохранения
        /// </summary>
        public float Rea_en_n { get; }
        /// <summary>
        /// Количество импульсов по каналам
        /// </summary>
        public int[] Channel { get; }

        /// <summary>
        /// Конструктор с преобразованим массива байт в строку суточного графика
        /// </summary>
        DayGraphRow(byte[] data)
        {
            //Проверка корректности входных данных
            if (data.Length != 20)
                throw new ArgumentException("DayGraphRow raw data size must be 20 bytes long");

            int index = 0;
            //time (4 байта)
            Time = Utils.ToUTCTime(Utils.ArrayPart(data, index, 4)).ToLocalTime();
            index += 4;
            //act_en_p (2 байта)
            Act_en_p = Utils.ToUminiflo(Utils.ArrayPart(data, index, 2));
            index += 2;
            //act_en_n (2 байта)
            Act_en_n = Utils.ToUminiflo(Utils.ArrayPart(data, index, 2));
            index += 2;
            //rea_en_p (2 байта)
            Rea_en_p = Utils.ToUminiflo(Utils.ArrayPart(data, index, 2));
            index += 2;
            //rea_en_n (2 байта)
            Rea_en_n = Utils.ToUminiflo(Utils.ArrayPart(data, index, 2));
            index += 2;
            Channel = new int[8];
            for (int i = 0; i <= 7; i++)
            {
                Channel[i] = (int)Utils.ToUint16(Utils.ArrayPart(data, index, sizeof(ushort)));
                index += 2;
            }
        }
    }
}
