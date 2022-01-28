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
    }
}