using System;

namespace Oblik
{
    public class HalfHourGraphRow : Row
    {
        public override int RecordSize { get => 20; }

        public float act_ener { get; private set; }
        public float rea_ener { get; private set; }
        public UInt16[] channel { get; private set; }

        public HalfHourGraphRow(byte[] rawdata, int index) : base (rawdata, index)
        {
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