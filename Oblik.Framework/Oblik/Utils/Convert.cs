using System;
using System.Runtime.InteropServices;

namespace Oblik
{
    /// <summary>
    /// Группа преобразователей массива байт в различные типы данных и наоборот.
    /// Принимается, что старший байт имеет младший адрес (big-endian)
    /// </summary>
    public static class Convert
    {
        /// <summary>
        /// Преобразование массива байт (Big-Endian) в значение
        /// </summary>
        /// <typeparam name="T">Тип значения</typeparam>
        /// <param name="rawdata">Массив байт</param>
        /// <param name="index"> стартовый индекс</param>
        /// <returns>Значение</returns>
        public static T ToValue<T>(byte[] rawdata, int index)
        {
            T result = default;
            int size = Marshal.SizeOf(result);

            if ((rawdata.Length - index) < size)
                throw new ArgumentException("Not enough data for conversion");

            byte[] buf = new byte[size];
            Array.Copy(rawdata, index, buf, 0, size);
            Array.Reverse(buf);
            IntPtr bufptr = Marshal.AllocHGlobal(size);
            Marshal.Copy(buf, 0, bufptr, buf.Length);
            result = (T)Marshal.PtrToStructure(bufptr, typeof(T));
            Marshal.FreeHGlobal(bufptr);
            return result;
        }

        /// <summary>
        /// Преобразование значения в массив байт (Big-Endian)
        /// </summary>
        /// <typeparam name="T">тип данных на входе</typeparam>
        /// <param name="value"> значение</param>
        /// <returns>Массив байт (Big-Endian)</returns>
        public static byte[] ToBytes<T>(T value) where T : struct
        {
            int size = Marshal.SizeOf(typeof(T));
            byte[] result = new byte[size];
            IntPtr bufptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(value, bufptr, false);
            Marshal.Copy(bufptr, result, 0, size);
            Marshal.FreeHGlobal(bufptr);
            Array.Reverse(result);
            return result;
        }

        /*
        /// <summary>
        /// Преобразование массива байт в дату и время
        /// </summary>
        /// <param name="rawdata">Массив байт</param>
        /// <param name="index">Начальный индекс</param>
        /// <returns></returns>
        public static DateTime ToUTCTime(byte[] rawdata, int index)
        {
            DateTime baseTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);       //Базовая точка времени 01.01.1970 00:00 GMT
            return baseTime.AddSeconds(ToValue<uint>(rawdata, index));
        }

        /// <summary>
        /// Преобразование массива байт в uminiflo
        /// </summary>
        /// <param name="rawdata">Массив байт</param>
        /// <param name="index">Начальный индекс</param>
        /// <returns></returns>
        public static float ToUminiflo(byte[] rawdata, int index)
        {
            UInt16 buf = ToValue<UInt16>(rawdata, index);
            UInt16 man = (UInt16)(buf & 0x7FF);                                      //Мантисса - биты 0-10
            UInt16 exp = (UInt16)((buf & 0xF800) >> 11);                             //Порядок - биты 11-15
            float result = (float)(Math.Pow(2, exp - 15) * (1 + man / 2048f));      
            return result;
        }

        /// <summary>
        /// Преобразование массива байт в sminiflo
        /// </summary>
        /// <param name="rawdata">Массив байт</param>
        /// <param name="index">Начальный индекс</param>
        /// <returns></returns>
        public static float ToSminiflo(byte[] rawdata, int index)
        {
            UInt16 buf = ToValue<UInt16>(rawdata, index);
            UInt16 sig = (UInt16)(buf & (UInt16)1);                                             //Знак - бит 0
            UInt16 man = (UInt16)((buf & 0x7FE) >> 1);                                          //Мантисса - биты 1-10
            UInt16 exp = (UInt16)((buf & 0xF800) >> 11);                                        //Порядок - биты 11-15
            float result = (float)(Math.Pow(2, exp - 15) * (1 + (man / 2048f)) * Math.Pow(-1, sig));
            return result;  
        }

        /// <summary>
        /// Преобразование DateTime в массив байт согласно t_time
        /// </summary>
        /// <param name="Date"></param>
        /// <returns></returns>
        public static byte[] ToTime(DateTime Date)
        {
            DateTime BaseTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);      //Базовая точка времени 01.01.1970 00:00 GMT
            UInt32 Seconds = (UInt32)(Date - BaseTime).TotalSeconds;
            return ToBytes<uint>(Seconds);
        }
        */
    }
}