using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oblik
{
    /// <summary>
    /// Текущие измерения
    /// </summary>
    public class CurrentValues
    {
        /// <summary>
        /// Размер сырой структуры, байт
        /// </summary>
        public const int Size = 24;
        
        /// <summary>
        /// Сырые данные
        /// </summary>
        private byte[] serialize;

        public float Curr1 { get; private set; }
        public float Curr2 { get; private set; }
        public float Curr3 { get; private set; }
        public float Volt1 { get; private set; }
        public float Volt2 { get; private set; }
        public float Volt3 { get; private set; }
        public float Act_pw { get; private set; }
        public float Rea_pw { get; private set; }
        public ushort Freq { get; private set; }
        public byte[] RawData
        {
            set
            {
                CheckRawSize(value.Length);
                value.CopyTo(serialize, 0);
                FromRaw();
            }
        }

        

        public CurrentValues() 
        {
            serialize = new byte[Size];
        }
        public CurrentValues(byte[] rawdata) : this()
        {
            CheckRawSize(rawdata.Length);
            rawdata.CopyTo(serialize, 0);
            FromRaw();
        }
                
        /// <summary>
        /// Преобразование массива байт в стрктуру
        /// </summary>
        private void FromRaw()
        {
            int index = 0;
            Curr1 = Utils.ToUminiflo(Utils.ArrayPart(serialize, index, 2));
            index += 2;

            Curr2 = Utils.ToUminiflo(Utils.ArrayPart(serialize, index, 2));
            index += 2;

            Curr3 = Utils.ToUminiflo(Utils.ArrayPart(serialize, index, 2));
            index += 2;

            Volt1 = Utils.ToUminiflo(Utils.ArrayPart(serialize, index, 2));
            index += 2;

            Volt2 = Utils.ToUminiflo(Utils.ArrayPart(serialize, index, 2));
            index += 2;

            Volt3 = Utils.ToUminiflo(Utils.ArrayPart(serialize, index, 2));
            index += 2;

            Act_pw = Utils.ToUminiflo(Utils.ArrayPart(serialize, index, 2));
            index += 2;

            Rea_pw = Utils.ToUminiflo(Utils.ArrayPart(serialize, index, 2));
            index += 2;

            //Reserved1
            index += 2;

            Freq = Utils.ToUint16(Utils.ArrayPart(serialize, index, 2));
        }
        
        /// <summary>
        /// Проверка размера сырых данных
        /// </summary>
        /// <param name="size">Размер сырых данных</param>
        /// <exception cref="ArgumentException"></exception>
        private void CheckRawSize(int size)
        {
            if (size != Size)
                throw new ArgumentException($"CurrentValues raw data size must be {Size} bytes long");
        }
    }
}