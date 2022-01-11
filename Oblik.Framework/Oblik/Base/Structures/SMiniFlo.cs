using System;
using System.Globalization;

namespace Oblik
{
    [Serializable]
    public struct SMiniFlo
    {
        private ushort value;

        public SMiniFlo(ushort value)
        {
            this.value = value;
        }

        public static ushort GetBits(SMiniFlo value)
        {
            return value.value;
        }
        
        #region Type casting operators
        public static TypeCode GetTypeCode()
        {
            return (TypeCode)255;
        }       
        public static implicit operator float(SMiniFlo value) 
        { 
            return SMiniFloHelper.SMiniFloToFloat(value); 
        }
        public static implicit operator SMiniFlo(ushort value)
        {
            return new SMiniFlo(value);
        }
        #endregion
        
        public override string ToString()
        {
            return ((float)this).ToString(CultureInfo.InvariantCulture);
        }
        public string ToString(IFormatProvider formatProvider)
        {
            return ((float)this).ToString(formatProvider);
        }
        public string ToString(string format)
        {
            return ((float)this).ToString(format, CultureInfo.InvariantCulture);
        }
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return ((float)this).ToString(format, formatProvider);
        }

    }

    internal static class SMiniFloHelper
    {
        public static float SMiniFloToFloat(SMiniFlo value)
        {
            ushort buf = SMiniFlo.GetBits(value);
            UInt16 sig = (UInt16)(buf & (UInt16)1);                                             //Знак - бит 0
            UInt16 man = (UInt16)((buf & 0x7FE) >> 1);                                          //Мантисса - биты 1-10
            UInt16 exp = (UInt16)((buf & 0xF800) >> 11);                                        //Порядок - биты 11-15
            float result = (float)(Math.Pow(2, exp - 15) * (1 + (man / 2048f)) * Math.Pow(-1, sig));
            return result;
        }
    }
}
