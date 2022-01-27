using System.Runtime.InteropServices;
using System;

namespace Oblik
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CalcUnits
    {
        public float Pwr_lim_D;
        public float Pwr_lim_C;
        public float Pwr_lim_B;
        public float Pwr_lim_A;
        public byte Save_const;
        private float volt_2w;
        private float volt_1w;
        private float curr_2w;
        private float curr_1w;
        public float Volt_2w { get => volt_2w; set { volt_2w = value; Calculate(ref this); } }
        public float Volt_1w { get => volt_1w; set { volt_1w = value; Calculate(ref this); } }
        public float Curr_2w { get => curr_2w; set { curr_2w = value; Calculate(ref this); } }
        public float Curr_1w { get => curr_1w; set { curr_1w = value; Calculate(ref this); } }
        public sbyte Volt_unit { get; private set; }
        public sbyte Curr_unit { get; private set; }
        public sbyte Powr_unit { get; private set; }
        public sbyte Ener_unit { get; private set; }
        public float reserved1;
        public float Volt_fct { get; private set; }
        public float Curr_fct { get; private set; }
        public float Powr_fct { get; private set; }
        public float Ener_fct { get; private set; }

        private static void Calculate(ref CalcUnits value)
        {
            float Ku = value.volt_1w / value.volt_2w;
            float Ki = value.curr_1w / value.curr_2w;
            float Kp = Ku * Ki;

            value.Volt_fct = Ku;
            value.Curr_fct = Ki;
            value.Powr_fct = Kp;

            sbyte pwr_unit = (sbyte)Math.Log10(1.56 * Kp / 0.4);
            value.Powr_unit = pwr_unit;
            value.Powr_fct /= (float)Math.Pow(10, pwr_unit);

            value.Ener_unit = (sbyte)(pwr_unit + 3);
            value.Ener_fct = value.Powr_fct / 1800f;

            sbyte volt_unit = (sbyte)(Math.Log10(value.Volt_fct * 100) - 1);
            value.Volt_unit = volt_unit;
            value.Volt_fct /= (float)Math.Pow(10, value.Volt_unit);

            sbyte curr_unit = (sbyte)Math.Log10(value.Curr_fct * 5);
            value.Curr_unit = ((curr_unit - 1) < 0) ? (sbyte)0 : (sbyte)(curr_unit - 1);
            value.Curr_fct /= (float)Math.Pow(10, value.Curr_unit);
        }
    }
}

/*
public struct CalcUnits
{
    public float Pwr_lim_D;
    public float Pwr_lim_C;
    public float Pwr_lim_B;
    public float Pwr_lim_A;
    public byte Save_const;
    public float Volt_2w;
    public float Volt_1w;
    public float Curr_2w;
    public float Curr_1w;
    public sbyte Volt_unit;
    public sbyte Curr_unit;
    public sbyte Powr_unit;
    public sbyte Ener_unit;
    public float reserved1;
    public float Volt_fct;
    public float Curr_fct;
    public float Powr_fct;
    public float Ener_fct;
}
*/