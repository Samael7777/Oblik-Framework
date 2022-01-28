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
        public float Volt_2w { get => volt_2w; set { volt_2w = value; Calculate(); } }
        public float Volt_1w { get => volt_1w; set { volt_1w = value; Calculate(); } }
        public float Curr_2w { get => curr_2w; set { curr_2w = value; Calculate(); } }
        public float Curr_1w { get => curr_1w; set { curr_1w = value; Calculate(); } }
        public sbyte Volt_unit { get; private set; }
        public sbyte Curr_unit { get; private set; }
        public sbyte Powr_unit { get; private set; }
        public sbyte Ener_unit { get; private set; }
        public float reserved1;
        public float Volt_fct { get; private set; }
        public float Curr_fct { get; private set; }
        public float Powr_fct { get; private set; }
        public float Ener_fct { get; private set; }

        private void Calculate()
        {
            Volt_fct = volt_1w / volt_2w;
            Curr_fct = curr_1w / curr_2w;
            Powr_fct = Volt_fct * Curr_fct;

            Powr_unit = (sbyte)Math.Log10(3.9 * Powr_fct);
            Powr_fct /= (float)Math.Pow(10, Powr_unit);

            Ener_unit = (sbyte)(Powr_unit + 3);
            Ener_fct = Powr_fct / 1800f;

            Volt_unit = (sbyte)(Math.Log10(Volt_fct * 100) - 1);
            Volt_fct /= (float)Math.Pow(10, Volt_unit);

            Curr_unit = (sbyte)(Math.Log10(Curr_fct * 5) - 1);
            Curr_unit = (Curr_unit < 0) ? (sbyte)0 : Curr_unit;
            Curr_fct /= (float)Math.Pow(10, Curr_unit);
        }
    }
}