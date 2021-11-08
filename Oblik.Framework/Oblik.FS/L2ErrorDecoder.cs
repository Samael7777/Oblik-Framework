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
                    res = "L2 request error";
                    break;
                case 0xfe:
                    res = "L2 segment ID error";
                    break;
                case 0xfd:
                    res = "L2 segment operation error";
                    break;
                case 0xfc:
                    res = "L2 user access level error";
                    break;
                case 0xfb:
                    res = "L2 data permission error";
                    break;
                case 0xfa:
                    res = "L2 segmnt offset error";
                    break;
                case 0xf9:
                    res = "L2 write request error";
                    break;
                case 0xf8:
                    res = "L2 data lenght must be above zero";
                    break;
                case 0xf7:
                    res = "L2 wrong password";
                    break;
                case 0xf6:
                    res = "L2 daygraph clear command is wrong";
                    break;
                case 0xf5:
                    res = "L2 password change restricted";
                    break;
                default:
                    res = "L2 unknown error";
                    break;
            }
            return res;
        }
    }
}
