using System;
using System.IO;
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
        public static byte[] ToBytes<T>(T value) where T: struct
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
            UInt16 man, exp;
            float res;
            man = (UInt16)(buf & 0x7FF);                                      //Мантисса - биты 0-10
            exp = (UInt16)((buf & 0xF800) >> 11);                             //Порядок - биты 11-15
            res = (float)Math.Pow(2, (exp - 15)) * (1 + man / 2048);          //Pow - возведение в степень
            return res;
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
            return (float)(Math.Pow(2, exp - 15) * (1 + (man / 2048)) * Math.Pow(-1, sig));     //Pow - возведение в степень
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


        //----------------Старое, после проверки удалить!-----------------------//

        /*
         /// <summary>
        /// Отдает массив заданной длины, начинающийся с заданного индекса исходного массива
        /// </summary>
        /// <param name="array">Источник</param>
        /// <param name="index">Начальный индекс</param>
        /// <param name="len">Длина</param>
        /// <returns>Массив байт</returns>
        public static byte[] ArrayPart(byte[] array, int index, int len)
        {
            byte[] result = new byte[len];
            Array.Copy(array, index, result, 0, len);
            return result;
        }
        /// <summary>
        /// Преобразование массива байт в UInt32
        /// </summary>
        /// <param name="rawdata"></param>
        /// <returns>Число UInt32</returns>
        public static UInt32 ToUint32(byte[] rawdata)
        {
            Array.Reverse(rawdata);
            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream(rawdata);
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    stream = null;
                    return reader.ReadUInt32();
                }
            }
            finally
            {
                if (stream != null)
                {
                    stream.Dispose();
                }
            }
        }

        /// <summary>
        /// Преобразование UInt32 в массив байт
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] UInt32ToByte(UInt32 data)
        {
            byte[] res = new byte[sizeof(UInt32)];
            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream(res);
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    stream = null;
                    writer.Write(data);
                    Array.Reverse(res);
                    return res;
                }
            }
            finally
            {
                if (stream != null)
                {
                    stream.Dispose();
                }
            }
        }

        /// <summary>
        /// Преобразование массива байт в float
        /// </summary>
        /// <param name="rawdata"></param>
        /// <returns></returns>
        public static float ToFloat(byte[] rawdata)
        {
            Array.Reverse(rawdata);
            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream(rawdata);
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    stream = null;
                    return reader.ReadSingle();
                }
            }
            finally
            {
                if (stream != null)
                {
                    stream.Dispose();
                }
            }
        }

        /// <summary>
        /// Преобразование float в массив байт
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] FloatToByte(float data)
        {
            byte[] res = new byte[sizeof(float)];
            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream(res);
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    stream = null;
                    writer.Write(data);
                    Array.Reverse(res);
                    return res;
                }
            }
            finally
            {
                if (stream != null)
                {
                    stream.Dispose();
                }
            }
        }

        /// <summary>
        /// Преобразование массива байт в word (оно же uint16)
        /// </summary>
        /// <param name="rawdata"></param>
        /// <returns></returns>
        public static UInt16 ToUint16(byte[] rawdata)
        {
            Array.Reverse(rawdata);
            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream(rawdata);
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    stream = null;
                    return reader.ReadUInt16();
                }
            }
            finally
            {
                if (stream != null)
                {
                    stream.Dispose();
                }
            }
        }

        /// <summary>
        /// Преобразование word(UInt16) в массив байт
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] UInt16ToByte(UInt16 data)
        {
            byte[] res = new byte[2];
            res[0] = (byte)((data & 0xFF00) >> 8);
            res[1] = (byte)(data & 0x00FF);
            return res;
        }
        */
    }

}
