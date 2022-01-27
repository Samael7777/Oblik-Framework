using System.Runtime.InteropServices;

namespace Oblik
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct FirmwareInfo
    {
        public byte Build;
        public byte Version;
        public override string ToString()
        {
            return InfoBuilder(this);
        }

        private static string InfoBuilder(FirmwareInfo fw)
        {
            int major = fw.Version & 15;    //Версия ПО (major)
            int type = fw.Version & 240;    //Тип счетчика
            string typestring = string.Empty;
            switch (type)
            {
                case 0:
                    typestring = "ЛО-3Tx-xMx";
                    break;
                case 1:
                    typestring = "ЛО-3Tx-xPx";
                    break;
                case 2:
                    typestring = "Тарификатор с RS-485";
                    break;
                case 3:
                    typestring = "Тарификатор без RS-485";
                    break;
            }
            return $"V.{major}.{fw.Build} mod. {type} {typestring}";
        }
    }
}
