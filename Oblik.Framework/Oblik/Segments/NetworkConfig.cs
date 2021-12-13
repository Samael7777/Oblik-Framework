using System;

namespace Oblik
{
    /// <summary>
    /// Сетевая конфигурация счетчика
    /// </summary>
    public class NetworkConfig
    {
        /// <summary>
        /// Размер сырой структуры, байт
        /// </summary>
        public const int Size = 3;

        /// <summary>
        /// Массив сырых данных сегмента
        /// </summary>
        private byte[] serialize;

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
                Div2Speed();
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
                Speed2Div();
            }
        }

        /// <summary>
        /// Поток байт, соответствующий структуре
        /// </summary>
        public byte[] RawData
        {
            get
            {
                ToRaw();
                return serialize;
            }
            set
            {
                CheckRawSize(value.Length);
                value.CopyTo(serialize, 0);
                FromRaw();
            }
        }

        public NetworkConfig()
        {
            serialize = new byte[Size];
        }

        public NetworkConfig(byte[] rawdata) : this()
        {
            CheckRawSize(rawdata.Length);
            rawdata.CopyTo(serialize, 0);
        }

        public NetworkConfig(int speed, byte addr) : this()
        {
            Addr = addr;
            Speed = speed;
            Speed2Div();
        }

        /// <summary>
        /// Преобразование потока байт в структуру
        /// </summary>
        private void FromRaw()
        {
            Addr = serialize[0];
            Divisor = (ushort)(serialize[1] << 8 + serialize[2]);
            Div2Speed();
        }

        /// <summary>
        /// Преобразование структуры в поток байт
        /// </summary>
        private void ToRaw()
        {
            serialize[0] = Addr;
            Convert.ToBytes(Divisor).CopyTo(serialize, 1);
        }

        /// <summary>
        /// Пересчет делителя от скорости
        /// </summary>
        private void Speed2Div()
        {
            Divisor = (ushort)(115200 / Speed);
        }

        /// <summary>
        /// Пересчет скорости от делителя
        /// </summary>
        private void Div2Speed()
        {
            Speed = 115200 / Divisor;
        }

        /// <summary>
        /// Проверка размера сырых данных
        /// </summary>
        /// <param name="size">Размер сырых данных</param>
        /// <exception cref="ArgumentException"></exception>
        private void CheckRawSize(int size)
        {
            if (size != Size)
                throw new ArgumentException($"Network config raw data size must be {Size} bytes long");
        }
    }
}