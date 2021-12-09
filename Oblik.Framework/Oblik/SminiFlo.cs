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
        #endregion

        #region BitConverter & Math methods for Half
        public static SminiFlo ToSminiFlo(ushort bits)
        {
            return new SminiFlo { Value = bits };
        }

        public static SminiFlo ToSminiFlo(byte[] rawdata, int index)
        {
            byte[] buffer = new byte[2];
            Array.Copy(rawdata, index, buffer, 0, 2);
            Array.Reverse(buffer);
            return ToSminiFlo(BitConverter.ToUInt16(buffer, 0));
        }

        public static ushort GetBits(SminiFlo value)
        {
            return value.Value;
        }
        public static byte[] GetBytes(SminiFlo value)
        {
            byte[] buffer = BitConverter.GetBytes(value.Value);
            Array.Reverse(buffer);
            return buffer;
        }
        
        #endregion


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

        public static SminiFlo(float value)
        {
            
        }

    }
}