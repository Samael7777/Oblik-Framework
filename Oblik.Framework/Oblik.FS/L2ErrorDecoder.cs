using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oblik;

namespace Oblik.FS
{
    public partial class OblikFS
    {
        /// <summary>
        /// Парсер ошибок L2
        /// </summary>
        /// <param name="error">Код ошибки L2</param>
        /// <returns>Строка с текстом ошибки</returns>
        private string DecodeSegmentError(int error)
        {
            string res;
            switch (error)
            {
                case 0:
                    res = Resources.L2Err00;
                    break;
                case 0xff:
                    L2ErrorsLog.Add((int)Error.L2RequestError);
                    res = Resources.L2ErrFF;
                    break;
                case 0xfe:
                    L2ErrorsLog.Add((int)Error.L2SegIDError);
                    res = Resources.L2ErrFE;
                    break;
                case 0xfd:
                    L2ErrorsLog.Add((int)Error.L2OperationError);
                    res = Resources.L2ErrFD;
                    break;
                case 0xfc:
                    L2ErrorsLog.Add((int)Error.L2UserLevelError);
                    res = Resources.L2ErrFC;
                    break;
                case 0xfb:
                    L2ErrorsLog.Add((int)Error.L2PermissionError);
                    res = Resources.L2ErrFB;
                    break;
                case 0xfa:
                    L2ErrorsLog.Add((int)Error.L2OffsetError);
                    res = Resources.L2ErrFA;
                    break;
                case 0xf9:
                    L2ErrorsLog.Add((int)Error.L2WriteReqError);
                    res = Resources.L2ErrF9;
                    break;
                case 0xf8:
                    L2ErrorsLog.Add((int)Error.L2DataLenError);
                    res = Resources.L2ErrF8;
                    break;
                case 0xf7:
                    L2ErrorsLog.Add((int)Error.L2PassError);
                    res = Resources.L2ErrF7;
                    break;
                case 0xf6:
                    L2ErrorsLog.Add((int)Error.L2CleanError);
                    res = Resources.L2ErrF6;
                    break;
                case 0xf5:
                    L2ErrorsLog.Add((int)Error.L2PassChangeError);
                    res = Resources.L2ErrF5;
                    break;
                default:
                    L2ErrorsLog.Add((int)Error.L2Unknown);
                    res = Resources.L2ErrUnk;
                    break;
            }
            return res;
        }
    }
}
