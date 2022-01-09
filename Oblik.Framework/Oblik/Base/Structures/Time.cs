using System;


namespace Oblik
{
    [Serializable]
    public struct Time
    {
        private UInt32 value;

        public Time(uint value)
        {
            this.value = value;
        }
        public Time(DateTime value)
        {
            this.value = TimeHelper.ToTime(value);
        }
        public static UInt32 GetBits(Time value)
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
            return TimeHelper.ToUTCTime(value);
        }
        public static implicit operator Time(DateTime value)
        {
            return new Time(value);
        }
        #endregion
    }

    internal static class TimeHelper
    {
        //Базовая точка времени 01.01.1970 00:00 GMT
        private static DateTime baseTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc); 
        
        public static DateTime ToUTCTime (Time value)
        {   
            return baseTime.AddSeconds(Time.GetBits(value));
        }
        public static UInt32 ToTime(DateTime value)
        {
            return (UInt32)(value - baseTime).TotalSeconds;
        }
    }
}
