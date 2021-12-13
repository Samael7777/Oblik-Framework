using System;

namespace Oblik
{
    public struct SminiFlo //: IComparable, IFormattable, IConvertible, IComparable<SminiFlo>, IEquatable<SminiFlo>
    {
        internal ushort Value;

        #region Constructors

        public SminiFlo(ushort value)
        {
            Value = value;
        }

        #endregion Constructors

        #region BitConverter & Math methods for Half

        public static SminiFlo ToSminiFlo(ushort bits)
        {
            return new SminiFlo { Value = bits };
        }

        public static SminiFlo ToSminiFlo(byte[] rawdata, int index)
        {
            return ToSminiFlo(BitConverter.ToUInt16(rawdata, index));
        }

        public static ushort GetBits(SminiFlo value)
        {
            return value.Value;
        }

        public static byte[] GetBytes(SminiFlo value)
        {
            return BitConverter.GetBytes(value.Value);
        }

        #endregion BitConverter & Math methods for Half
    }

    public static class SminifloHelper
    {
        public static float ToSingle(SminiFlo value)
        {
            ushort signum = (ushort)(value.Value & 1);
            ushort mantissa = (ushort)((value.Value & 0x7FE) >> 1);
            ushort exponent = (ushort)((value.Value & 0xF800) >> 11);
            return (float)(Math.Pow(2, exponent - 15) * (1 + (mantissa / 2048)) * Math.Pow(-1, signum));
        }

        /*
        public static SminiFlo ToSminiFlo(float value)
        {
            uint bits = BitConverter.ToUInt32(BitConverter.GetBytes(value), 0);
            int signum = (int)((bits & 0x80000000) >> 31);
            int exponent = (int)((bits & 0x7FC00000) >> 23) - 127;
            int mantissa = (int)(bits & 0x7FFFFF);
        }
        */
    }
}