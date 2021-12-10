using System;


namespace Oblik
{
    /// <summary>
    /// Параметры вычислений счетчика
    /// </summary>
    public class CalcUnits
    {
        /// <summary>
        /// Размер сырой структуры, байт
        /// </summary>
        public static int Size { get => 57; }
        public float Ener_fct { get; set; }
        public float Powr_fct { get; set; }
        public float Curr_fct { get; set; }
        public float Volt_fct { get; set; }
        public float Curr_1w { get; set; }
        public float Curr_2w { get; set; }
        public float Volt_1w { get; set; }
        public float Volt_2w { get; set; }
        public float Pwr_lim_A { get; set; }
        public float Pwr_lim_B { get; set; }
        public float Pwr_lim_C { get; set; }
        public float Pwr_lim_D { get; set; }
        public byte Save_const { get; set; }
        public sbyte Ener_unit { get; set; }
        public sbyte Powr_unit { get; set; }
        public sbyte Curr_unit { get; set; }
        public sbyte Volt_unit { get; set; }
        public float ener_cf { get => (float)Math.Pow(10, (Ener_unit - 6)); }
        public float powr_cf { get => (float)Math.Pow(10, Powr_unit); }
        public float curr_cf { get => (float)Math.Pow(10, Curr_unit); }
        public float volt_cf { get => (float)Math.Pow(10, (Volt_unit - 1)); }
        public byte[] RawData
        {
            get
            {
                ToRaw();
                return serialize;
            }
            set
            {
                CheckRawSize(value.Length);
                value.CopyTo(serialize, 0);
                FromRaw();
            }
        }
        
        /// <summary>
        /// Сырые данные
        /// </summary>
        private byte[] serialize;

        public CalcUnits()
        {
            serialize = new byte[Size];
        }

        public CalcUnits(byte[] rawdata) : this()
        {
            CheckRawSize(rawdata.Length);
            rawdata.CopyTo(serialize, 0);
            FromRaw();
        }

        /// <summary>
        /// Преобразование структуры параметров вычислений в массив байт
        /// </summary>
        private void ToRaw()
        {
            int index = 0;

            Convert.ToBytes(Ener_fct).CopyTo(serialize, index);
            index += sizeof(float);

            Convert.ToBytes(Powr_fct).CopyTo(serialize, index);
            index += sizeof(float);

            Convert.ToBytes(Curr_fct).CopyTo(serialize, index);
            index += sizeof(float);

            Convert.ToBytes(Volt_fct).CopyTo(serialize, index);
            index += sizeof(float);

            //reserved1
            index += sizeof(float);

            serialize[index] = (byte)(Ener_unit);
            index++;

            serialize[index] = (byte)(Powr_unit);
            index++;

            serialize[index] = (byte)(Curr_unit);
            index++;

            serialize[index] = (byte)(Volt_unit);
            index++;

            Convert.ToBytes(Curr_1w).CopyTo(serialize, index);
            index += sizeof(float);

            Convert.ToBytes(Curr_2w).CopyTo(serialize, index);
            index += sizeof(float);

            Convert.ToBytes(Volt_1w).CopyTo(serialize, index);
            index += sizeof(float);

            Convert.ToBytes(Volt_2w).CopyTo(serialize, index);
            index += sizeof(float);

            serialize[index] = Save_const;
            index++;

            Convert.ToBytes(Pwr_lim_A).CopyTo(serialize, index);
            index += sizeof(float);

            Convert.ToBytes(Pwr_lim_B).CopyTo(serialize, index);
            index += sizeof(float);

            Convert.ToBytes(Pwr_lim_C).CopyTo(serialize, index);
            index += sizeof(float);

            Convert.ToBytes(Pwr_lim_D).CopyTo(serialize, index);
        }
        
        /// <summary>
        /// Преобразование массива байт в стрктуру параметров вычислений
        /// </summary>
        private void FromRaw()
        {
            int index = 0;
            Ener_fct = Convert.ToValue<float>(serialize, index);
            index += sizeof(float);

            Powr_fct = Convert.ToValue<float>(serialize, index);
            index += sizeof(float);

            Curr_fct = Convert.ToValue<float>(serialize, index);
            index += sizeof(float);

            Volt_fct = Convert.ToValue<float>(serialize, index);
            index += sizeof(float);

            //reserved1
            index += sizeof(float);

            Ener_unit = (sbyte)serialize[index];
            index++;

            Powr_unit = (sbyte)serialize[index];
            index++;

            Curr_unit = (sbyte)serialize[index];
            index++;

            Volt_unit = (sbyte)serialize[index];
            index++;

            Curr_1w = Convert.ToValue<float>(serialize, index);
            index += sizeof(float);

            Curr_2w = Convert.ToValue<float>(serialize, index);
            index += sizeof(float);

            Volt_1w = Convert.ToValue<float>(serialize, index);
            index += sizeof(float);

            Volt_2w = Convert.ToValue<float>(serialize, index);
            index += sizeof(float);

            Save_const = serialize[index];
            index++;

            Pwr_lim_A = Convert.ToValue<float>(serialize, index);
            index += sizeof(float);

            Pwr_lim_B = Convert.ToValue<float>(serialize, index);
            index += sizeof(float);

            Pwr_lim_C = Convert.ToValue<float>(serialize, index);
            index += sizeof(float);

            Pwr_lim_D = Convert.ToValue<float>(serialize, index);
        }

        /// <summary>
        /// Проверка размера сырых данных
        /// </summary>
        /// <param name="size">Размер сырых данных</param>
        /// <exception cref="ArgumentException"></exception>
        private void CheckRawSize(int size)
        {
            if (size != Size)
                throw new ArgumentException($"Calculation Units raw data size must be {Size} bytes long");
        }
    }
}