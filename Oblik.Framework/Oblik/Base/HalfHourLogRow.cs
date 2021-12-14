using System;

namespace Oblik
{
    public class HalfHourLogRow
    {
        public static int Size { get => 20; }
        public float act_ener { get; private set; }
        public float rea_ener { get; private set; }
        public UInt16[] channel { get; private set; }

        public HalfHourLogRow(byte[] rawdata, int index)
        {
            if ((rawdata.Length - index) < Size)
                throw new ArgumentException($"Raw data size must be {Size} bytes long");

            channel = new UInt16[8];

            act_ener = Convert.ToSminiflo(rawdata, index);
            index += 2;
            rea_ener = Convert.ToSminiflo(rawdata, index);
            index += 2;
            for (int i = 0; i < 8; i++)
            {
                channel[i] = Convert.ToValue<UInt16>(rawdata, index);
                index += 2;
            }
        }
    }
}