using Oblik.FS;
using System;

namespace Oblik
{
    /// <summary>
    /// Параметры вычислений счетчика
    /// </summary>
    public class CalcUnits : Segment
    {
        public override int Size { get => 57; }
        public override int ReadSegmentID { get => 56; }
        public override int WriteSegmentID { get => 57; }

        #region Segment values

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

        #endregion Segment values
        #region Calculated values
        public float ener_cf { get => (float)Math.Pow(10, (Ener_unit - 6)); }
        public float powr_cf { get => (float)Math.Pow(10, Powr_unit); }
        public float curr_cf { get => (float)Math.Pow(10, Curr_unit); }
        public float volt_cf { get => (float)Math.Pow(10, (Volt_unit - 1)); }
        #endregion

        public CalcUnits(ConnectionParams connectionParams) : base(connectionParams) { }

        public CalcUnits(OblikFS oblikFS) : base(oblikFS) { }

        /// <summary>
        /// Преобразование структуры параметров вычислений в массив байт
        /// </summary>
        protected override void ToRaw()
        {
            int index = 0;

            Convert.ToBytes(Ener_fct).CopyTo(rawdata, index);
            index += sizeof(float);

            Convert.ToBytes(Powr_fct).CopyTo(rawdata, index);
            index += sizeof(float);

            Convert.ToBytes(Curr_fct).CopyTo(rawdata, index);
            index += sizeof(float);

            Convert.ToBytes(Volt_fct).CopyTo(rawdata, index);
            index += sizeof(float);

            //reserved1
            index += sizeof(float);

            rawdata[index] = (byte)(Ener_unit);
            index++;

            rawdata[index] = (byte)(Powr_unit);
            index++;

            rawdata[index] = (byte)(Curr_unit);
            index++;

            rawdata[index] = (byte)(Volt_unit);
            index++;

            Convert.ToBytes(Curr_1w).CopyTo(rawdata, index);
            index += sizeof(float);

            Convert.ToBytes(Curr_2w).CopyTo(rawdata, index);
            index += sizeof(float);

            Convert.ToBytes(Volt_1w).CopyTo(rawdata, index);
            index += sizeof(float);

            Convert.ToBytes(Volt_2w).CopyTo(rawdata, index);
            index += sizeof(float);

            rawdata[index] = Save_const;
            index++;

            Convert.ToBytes(Pwr_lim_A).CopyTo(rawdata, index);
            index += sizeof(float);

            Convert.ToBytes(Pwr_lim_B).CopyTo(rawdata, index);
            index += sizeof(float);

            Convert.ToBytes(Pwr_lim_C).CopyTo(rawdata, index);
            index += sizeof(float);

            Convert.ToBytes(Pwr_lim_D).CopyTo(rawdata, index);
        }

        /// <summary>
        /// Преобразование массива байт в стрктуру параметров вычислений
        /// </summary>
        protected override void FromRaw()
        {
            int index = 0;
            Ener_fct = Convert.ToValue<float>(rawdata, index);
            index += sizeof(float);

            Powr_fct = Convert.ToValue<float>(rawdata, index);
            index += sizeof(float);

            Curr_fct = Convert.ToValue<float>(rawdata, index);
            index += sizeof(float);

            Volt_fct = Convert.ToValue<float>(rawdata, index);
            index += sizeof(float);

            //reserved1
            index += sizeof(float);

            Ener_unit = (sbyte)rawdata[index];
            index++;

            Powr_unit = (sbyte)rawdata[index];
            index++;

            Curr_unit = (sbyte)rawdata[index];
            index++;

            Volt_unit = (sbyte)rawdata[index];
            index++;

            Curr_1w = Convert.ToValue<float>(rawdata, index);
            index += sizeof(float);

            Curr_2w = Convert.ToValue<float>(rawdata, index);
            index += sizeof(float);

            Volt_1w = Convert.ToValue<float>(rawdata, index);
            index += sizeof(float);

            Volt_2w = Convert.ToValue<float>(rawdata, index);
            index += sizeof(float);

            Save_const = rawdata[index];
            index++;

            Pwr_lim_A = Convert.ToValue<float>(rawdata, index);
            index += sizeof(float);

            Pwr_lim_B = Convert.ToValue<float>(rawdata, index);
            index += sizeof(float);

            Pwr_lim_C = Convert.ToValue<float>(rawdata, index);
            index += sizeof(float);

            Pwr_lim_D = Convert.ToValue<float>(rawdata, index);
        }
    }
}