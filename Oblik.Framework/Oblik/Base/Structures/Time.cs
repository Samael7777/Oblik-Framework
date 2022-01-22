using System;
using System.Globalization;

namespace Oblik
{
    [Serializable]
    public struct Time
    {
        //Базовая точка времени 01.01.1970 00:00 GMT
        private static readonly DateTime baseTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        private uint value;

        public Time(uint value)
        {
            this.value = value;
        }
        public Time(DateTime value)
        {
            this.value = ToTime(value);
        }
        public static uint GetBits(Time value)
        {
            return value.value;
        }

        #region Type casting operators
        public static TypeCode GetTypeCode()
        {
            return (TypeCode)253;
        }
        public static explicit operator DateTime(Time value)
        {
            return ToUTCTime(value);
        }
        public static implicit operator Time(DateTime value)
        {
            return new Time(value);
        }
        #endregion
        public override string ToString()
        {
            return ((DateTime)this).ToString(CultureInfo.InvariantCulture);
        }
        public string ToString(IFormatProvider formatProvider)
        {
            return ((DateTime)this).ToString(formatProvider);
        }
        public string ToString(string format)
        {
            return ((DateTime)this).ToString(format, CultureInfo.InvariantCulture);
        }
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return ((DateTime)this).ToString(format, formatProvider);
        }

        private static DateTime ToUTCTime(Time value)
        {
            return baseTime.AddSeconds(Time.GetBits(value));
        }
        private static uint ToTime(DateTime value)
        {
            return (uint)(value - baseTime).TotalSeconds;
        }
    }
}
