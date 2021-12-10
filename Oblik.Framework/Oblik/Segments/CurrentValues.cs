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
        public static int Size { get => 24; }
        public float Curr1 { get; private set; }
        public float Curr2 { get; private set; }
        public float Curr3 { get; private set; }
        public float Volt1 { get; private set; }
        public float Volt2 { get; private set; }
        public float Volt3 { get; private set; }
        public float Act_pw { get; private set; }
        public float Rea_pw { get; private set; }
        public ushort Freq { get; private set; }
        
        public CurrentValues(byte[] rawdata)
        {
            if (rawdata.Length != Size)
                throw new ArgumentException($"CurrentValues raw data size must be {Size} bytes long");

            int index = 0;
            Curr1 = Convert.ToUminiflo(rawdata, index);
            index += 2;

            Curr2 = Convert.ToUminiflo(rawdata, index);
            index += 2;

            Curr3 = Convert.ToUminiflo(rawdata, index);
            index += 2;

            Volt1 = Convert.ToUminiflo(rawdata, index);
            index += 2;

            Volt2 = Convert.ToUminiflo(rawdata, index);
            index += 2;

            Volt3 = Convert.ToUminiflo(rawdata, index);
            index += 2;

            Act_pw = Convert.ToUminiflo(rawdata, index);
            index += 2;

            Rea_pw = Convert.ToUminiflo(rawdata, index);
            index += 2;

            //Reserved1
            index += 2;

            Freq = Convert.ToValue<UInt16>(rawdata, index);

        }
   
    }
}