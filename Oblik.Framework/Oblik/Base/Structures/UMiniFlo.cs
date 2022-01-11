using System;
using System.Globalization;

namespace Oblik
{
    [Serializable]
    public struct UMiniFlo
    {
        private ushort value;

        public UMiniFlo(ushort value)
        {
            this.value = value;
        }

        public static ushort GetBits(UMiniFlo value)
        {
            return value.value;
        }

        #region Type casting operators
        public static TypeCode GetTypeCode()
        {
            return (TypeCode)254;
        }
        public static implicit operator float (UMiniFlo value)
        {
            return UMiniFloHelper.UMiniFloToFloat(value);
        }
        public static implicit operator UMiniFlo(ushort value)
        {
            return new UMiniFlo(value);
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

    internal static class UMiniFloHelper
    {
        public static float UMiniFloToFloat(UMiniFlo value)
        {
            UInt16 buf = UMiniFlo.GetBits(value);
            UInt16 man = (UInt16)(buf & 0x7FF);                                      //Мантисса - биты 0-10
            UInt16 exp = (UInt16)((buf & 0xF800) >> 11);                             //Порядок - биты 11-15
            float result = (float)(Math.Pow(2, exp - 15) * (1 + man / 2048f));
            return result;
        }
    }
}
