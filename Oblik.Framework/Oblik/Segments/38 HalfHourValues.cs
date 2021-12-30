using System;
using Oblik.FS;

namespace Oblik
{
    public class HalfHourValues : Segment
    {
        
        public override int Size { get => 80; }
        public override int ReadSegmentID { get => 38; }
        public override int WriteSegmentID { get => 0; }
        
        #region Values
        public float act_pw_cur { get; private set; }
        public float rea_pw_cur { get; private set; }
        public uint[] channel_cur { get; private set; }
        public float act_pw_old { get; private set; }
        public float rea_pw_old { get; private set; }
        public uint[] channel_old { get; private set; }
        #endregion

        public HalfHourValues(IOblikFS oblikFS) : base(oblikFS) 
        {
            channel_cur = new uint[8];
            channel_old = new uint[8];
        }

        protected override void FromRaw()
        { 
            int index = 0;
            act_pw_cur = Convert.ToValue<float>(rawdata, index);
            index += sizeof(float);

            rea_pw_cur = Convert.ToValue<float>(rawdata, index);
            index += sizeof(float);

            for (int i = 0; i < 8; i++)
            {
                channel_cur[i] = Convert.ToValue<uint>(rawdata, index);
                index += sizeof(uint);
            }

            act_pw_old = Convert.ToValue<float>(rawdata, index);
            index += sizeof(float);

            rea_pw_old = Convert.ToValue<float>(rawdata, index);
            index += sizeof(float);

            for (int i = 0; i < 8; i++)
            {
                channel_old[i] = Convert.ToValue<uint>(rawdata, index);
                index += sizeof(uint);
            }
        }
    }
}