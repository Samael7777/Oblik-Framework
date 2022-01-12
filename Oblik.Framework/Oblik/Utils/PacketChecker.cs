using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oblik
{
    internal static class PacketChecker
    {
        /// <summary>
        /// Парсер ошибок L1, при ошибке вызывает исключение OblikIOException
        /// </summary>
        /// <param name="errorcode">Код ошибки L1</param>
        internal static void L1Error(int errorcode)
        {
            switch (errorcode)
            {
                case 1:
                    break;  //No errors
                case 0xff:
                    throw new OblikIOException("L1 Checksum Error", Error.L1CSCError);
                case 0xfe:
                    throw new OblikIOException("L1 Meter input buffer overflow", Error.L1BufOverfowError);
                default:
                    throw new OblikIOException("L1 Unknown error", Error.L1UnkErrror);
            }
        }

        /// <summary>
        /// Парсер ошибок L2, при ошибке вызывает исключение OblikIOException
        /// </summary>
        /// <param name="errorcode">Код ошибки L2</param>
        internal static void L2Error(int errorcode)
        {
            switch (errorcode)
            {
                case 0:
                    break;      //No errors
                case 0xff:
                    throw new OblikIOException("L2 request error", Error.L2ReqError);
                case 0xfe:
                    throw new OblikIOException("L2 segment ID error", Error.L2SegIdError);
                case 0xfd:
                    throw new OblikIOException("L2 segment operation error", Error.L2SegOpError);
                case 0xfc:
                    throw new OblikIOException("L2 user access level error", Error.L2UserAcsError);
                case 0xfb:
                    throw new OblikIOException("L2 data permission error", Error.L2DataPermisError);
                case 0xfa:
                    throw new OblikIOException("L2 segmnt offset error", Error.L2SegOfstError);
                case 0xf9:
                    throw new OblikIOException("L2 write request error", Error.L2WrReqError);
                case 0xf8:
                    throw new OblikIOException("L2 data lenght must be above zero", Error.L2DataLenError);
                case 0xf7:
                    throw new OblikIOException("L2 wrong password", Error.L2PwdError);
                case 0xf6:
                    throw new OblikIOException("L2 daygraph clear command is wrong", Error.L2DGCleanError);
                case 0xf5:
                    throw new OblikIOException("L2 password change restricted", Error.L2PwdChngError);
                default:
                    throw new OblikIOException("L2 unknown error", Error.L2UnkError);
            }
        }
    }
}
