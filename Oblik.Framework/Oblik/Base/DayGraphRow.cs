using System;

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
        public int RecordSize { get => 28; }

        #region Values
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
        #endregion

        /// <summary>
        /// Конструктор с преобразованим массива байт в строку суточного графика
        /// </summary>
        /// <param name="rawdata">Массив байт</param>
        /// <param name="index">Начальный индекс</param>
        public DayGraphRow(byte[] rawdata, int index) 
        {
            Time = Convert.ToUTCTime(rawdata, index).ToLocalTime();
            index += 4;

            Act_en_p = Convert.ToUminiflo(rawdata, index);
            index += 2;

            Act_en_n = Convert.ToUminiflo(rawdata, index);
            index += 2;

            Rea_en_p = Convert.ToUminiflo(rawdata, index);
            index += 2;

            Rea_en_n = Convert.ToUminiflo(rawdata, index);
            index += 2;

            Channel = new int[8];
            for (int i = 0; i <= 7; i++)
            {
                Channel[i] = (int)Convert.ToValue<UInt16>(rawdata, index);
                index += 2;
            }
        }
    }
}