using System;
using Oblik.FS;

namespace Oblik
{
    /// <summary>
    /// Текущие измерения
    /// </summary>
    public class CurrentValues : Segment
    {
       
        public new static int Size { get => 24; }
        public new static int ReadSegmentID { get => 36; }
        public new static int WriteSegmentID { get => 0; }
        
        #region Values
        public float Curr1 { get; private set; }
        public float Curr2 { get; private set; }
        public float Curr3 { get; private set; }
        public float Volt1 { get; private set; }
        public float Volt2 { get; private set; }
        public float Volt3 { get; private set; }
        public float Act_pw { get; private set; }
        public float Rea_pw { get; private set; }
        public ushort Freq { get; private set; }
        #endregion

        public CurrentValues(OblikFS oblikFS) : base(oblikFS) { }
        public CurrentValues(ConnectionParams connectionParams) : base(connectionParams) { }

        protected override void FromRaw()
        {
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