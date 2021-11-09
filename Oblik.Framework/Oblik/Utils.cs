using System;
using System.IO;


namespace Oblik
{

    /// <summary>
    /// Набор инструментов.
    /// Группа преобразователей массива байт в различные типы данных и наоборот. 
    /// Принимается, что старший байт имеет младший адрес (big-endian)
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Преобразование массива байт в UInt32
        /// </summary>
        /// <param name="array"></param>
        /// <returns>Число UInt32</returns>
        public static UInt32 ToUint32(byte[] array)
        {
            Array.Reverse(array);
            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream(array);
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
        /// <param name="array"></param>
        /// <returns></returns>
        public static float ToFloat(byte[] array)
        {
            Array.Reverse(array);
            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream(array);
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
        /// <param name="array"></param>
        /// <returns></returns>
        public static UInt16 ToUint16(byte[] array)
        {
            Array.Reverse(array);
            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream(array);
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

        /// <summary>
        /// Преобразование массива байт в дату и время
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static DateTime ToUTCTime(byte[] array)
        {
            UInt32 _ctime;  //Время по стандарту t_time
            DateTime BaseTime;
            BaseTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);       //Базовая точка времени 01.01.1970 00:00 GMT
            _ctime = ToUint32(array);                                             //Время в формате C (time_t) 
            return BaseTime.AddSeconds(_ctime);
        }

        /// <summary>
        /// Преобразование массива байт в uminiflo 
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static float ToUminiflo(byte[] array)
        {
            UInt16 _data = ToUint16(array);
            UInt16 man, exp;
            float res;
            man = (UInt16)(_data & 0x7FF);                                      //Мантисса - биты 0-10
            exp = (UInt16)((_data & 0xF800) >> 11);                             //Порядок - биты 11-15
            res = (float)System.Math.Pow(2, (exp - 15)) * (1 + man / 2048);     //Pow - возведение в степень
            return res;
        }

        /// <summary>
        /// Преобразование массива байт в sminiflo
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static float ToSminiflo(byte[] array)
        {
            UInt16 _data = ToUint16(array);
            UInt16 sig = (UInt16)(_data & (UInt16)1);                                  //Знак - бит 0
            UInt16 man = (UInt16)((_data & 0x7FE) >> 1);                               //Мантисса - биты 1-10
            UInt16 exp = (UInt16)((_data & 0xF800) >> 11);                             //Порядок - биты 11-15
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
            UInt32 Time = (UInt32)(Date - BaseTime).TotalSeconds;
            return UInt32ToByte(Time);
        }

        /// <summary>
        /// Отдает массив заданной длины, начинающийся с заданного индекса исходного массива
        /// </summary>
        /// <param name="array">Источник</param>
        /// <param name="StartIndex">Начальный индекс</param>
        /// <param name="Lenght">Длина</param>
        /// <returns>Массив байт</returns>
        public static byte[] ArrayPart(byte[] array, int StartIndex, int Lenght)
        {
            byte[] res = new byte[Lenght];
            Array.Copy(array, StartIndex, res, 0, Lenght);
            return res;
        }
    }
    
}
