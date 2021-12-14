using System;
using Oblik.FS;

namespace Oblik
{
    /// <summary>
    /// Сетевая конфигурация счетчика
    /// </summary>
    public class NetworkConfig : Segment
    {
        public override int Size { get => 3; }
        public override int ReadSegmentID { get => 66; }
        public override int WriteSegmentID { get => 67; }

        /// <summary>
        /// Сетевой адрес по протоколу RS-48
        /// </summary>
        public byte Addr { get; set; }

        /// <summary>
        /// Скорость соединения, делитель от 115200
        /// </summary>
        public UInt16 Divisor
        {
            get => Divisor;
            set
            {
                Divisor = value;
                Speed = 115200 / Divisor;
            }
        }

        /// <summary>
        /// Скорость соединения
        /// </summary>
        public int Speed
        {
            get => Speed;
            set
            {
                Speed = value;
                Divisor = (ushort)(115200 / Speed);
            }
        }

        public NetworkConfig(OblikFS oblikFS) : base(oblikFS) { }
        public NetworkConfig(ConnectionParams connectionParams) : base(connectionParams) { }

        /// <summary>
        /// Преобразование потока байт в структуру
        /// </summary>
        protected override void FromRaw()
        {
            Addr = rawdata[0];
            Divisor = (ushort)Convert.ToValue<ushort>(rawdata, 1);
        }

        /// <summary>
        /// Преобразование структуры в поток байт
        /// </summary>
        protected override void ToRaw()
        {
            rawdata[0] = Addr;
            Convert.ToBytes(Divisor).CopyTo(rawdata, 1);
        }
    }
}