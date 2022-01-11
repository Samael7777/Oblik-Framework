using System.Runtime.InteropServices;

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
}