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
        /// Парсер ошибок L2, при ошибке вызывает исключение OblikIOException
        /// </summary>
        /// <param name="error">Код ошибки L2</param>
        private void DecodeSegmentError(int error)
        {
            switch (error)
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
