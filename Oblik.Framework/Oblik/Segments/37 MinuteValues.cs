using System;
using Oblik.FS;

namespace Oblik
{
    public class MinuteValues : Segment
    {
        public override int Size { get => 40; }
        public override int ReadSegmentID { get => 37; }
        public override int WriteSegmentID { get => 0; }

        #region Values
        public float act_ener { get; private set; }
        public float rea_ener { get; private set; }
        public uint[] channel { get; private set; }
        #endregion

        private void Init()
        {
            channel = new uint[8];
        }
        public MinuteValues(OblikFS oblikFS) : base(oblikFS)
        {
            Init();
        }
        public MinuteValues(ConnectionParams connectionParams) : base(connectionParams)
        {
            Init();
        }
        protected override void FromRaw()
        {
            
            channel = new uint[8];
            int index = 0;

            act_ener = Convert.ToValue<float>(rawdata, index);
            index += sizeof(uint);

            rea_ener = Convert.ToValue<float>(rawdata, index);
            index += sizeof(uint);

            for (int i = 0; i < 8; i++)
            {
                channel[i] = Convert.ToValue<uint>(rawdata, index);
            }
        }
    }
}