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
                    res = "L2 OK";
                    break;
                case 0xff:
                    L2ErrorsLog.Add((int)Error.L2RequestError);
                    res = "L2 request error";
                    break;
                case 0xfe:
                    L2ErrorsLog.Add((int)Error.L2SegIDError);
                    res = "L2 segment ID error";
                    break;
                case 0xfd:
                    L2ErrorsLog.Add((int)Error.L2OperationError);
                    res = "L2 segment operation error";
                    break;
                case 0xfc:
                    L2ErrorsLog.Add((int)Error.L2UserLevelError);
                    res = "L2 user access level error";
                    break;
                case 0xfb:
                    L2ErrorsLog.Add((int)Error.L2PermissionError);
                    res = "L2 data permission error";
                    break;
                case 0xfa:
                    L2ErrorsLog.Add((int)Error.L2OffsetError);
                    res = "L2 segmnt offset error";
                    break;
                case 0xf9:
                    L2ErrorsLog.Add((int)Error.L2WriteReqError);
                    res = "L2 write request error";
                    break;
                case 0xf8:
                    L2ErrorsLog.Add((int)Error.L2DataLenError);
                    res = "L2 data lenght must be above zero";
                    break;
                case 0xf7:
                    L2ErrorsLog.Add((int)Error.L2PassError);
                    res = "L2 wrong password";
                    break;
                case 0xf6:
                    L2ErrorsLog.Add((int)Error.L2CleanError);
                    res = "L2 daygraph clear command is wrong";
                    break;
                case 0xf5:
                    L2ErrorsLog.Add((int)Error.L2PassChangeError);
                    res = "L2 password change restricted";
                    break;
                default:
                    L2ErrorsLog.Add((int)Error.L2Unknown);
                    res = "L2 unknown error";
                    break;
            }
            return res;
        }
    }
}
