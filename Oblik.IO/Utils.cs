/*
 * Общие утилиты
 */

using System;

namespace Oblik.IO
{
    public partial class OblikConnector
    {
        /// <summary>
        /// Возвращает количество миллисекунд для данного экземпляра
        /// </summary>
        /// <returns>Количество миллисекунд для данного экземпляра</returns>
        private static ulong GetTickCount()
        {
            return (ulong)DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        /// <summary>
        /// Парсер ошибок L1
        /// </summary>
        /// <param name="error">Код ошибки L1</param>
        /// <returns>Строка с текстом ошибки</returns>
        private string DecodeChannelError(int error)
        {
            string res;
            switch (error)
            {
                case 1:
                    res = Resources.Resources.L1OK;
                    break;
                case 0xff:
                    ErrorsLog.Add((int)Error.L1CSCError);
                    res = Resources.Resources.L1CSCError;
                    break;
                case 0xfe:
                    ErrorsLog.Add((int)Error.L1OverFlow);
                    res = Resources.Resources.L1Overflow;
                    break;
                default:
                    ErrorsLog.Add((int)Error.L1Unknown);
                    res = Resources.Resources.L1Unk;
                    break;
            }
            return res;
        }

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
                    res = Resources.Resources.L2Err00;
                    break;
                case 0xff:
                    ErrorsLog.Add((int)Error.L2RequestError);
                    res = Resources.Resources.L2ErrFF;
                    break;
                case 0xfe:
                    ErrorsLog.Add((int)Error.L2SegIDError);
                    res = Resources.Resources.L2ErrFE;
                    break;
                case 0xfd:
                    ErrorsLog.Add((int)Error.L2OperationError);
                    res = Resources.Resources.L2ErrFD;
                    break;
                case 0xfc:
                    ErrorsLog.Add((int)Error.L2UserLevelError);
                    res = Resources.Resources.L2ErrFC;
                    break;
                case 0xfb:
                    ErrorsLog.Add((int)Error.L2PermissionError);
                    res = Resources.Resources.L2ErrFB;
                    break;
                case 0xfa:
                    ErrorsLog.Add((int)Error.L2OffsetError);
                    res = Resources.Resources.L2ErrFA;
                    break;
                case 0xf9:
                    ErrorsLog.Add((int)Error.L2WriteReqError);
                    res = Resources.Resources.L2ErrF9;
                    break;
                case 0xf8:
                    ErrorsLog.Add((int)Error.L2DataLenError);
                    res = Resources.Resources.L2ErrF8;
                    break;
                case 0xf7:
                    ErrorsLog.Add((int)Error.L2PassError);
                    res = Resources.Resources.L2ErrF7;
                    break;
                case 0xf6:
                    ErrorsLog.Add((int)Error.L2CleanError);
                    res = Resources.Resources.L2ErrF6;
                    break;
                case 0xf5:
                    ErrorsLog.Add((int)Error.L2PassChangeError);
                    res = Resources.Resources.L2ErrF5;
                    break;
                default:
                    ErrorsLog.Add((int)Error.L2Unknown);
                    res = Resources.Resources.L2ErrUnk;
                    break;
            }
            return res;
        }

        /// <summary>
        /// Отдает массив заданной длины, начинающийся с заданного индекса исходного массива
        /// </summary>
        /// <param name="array">Источник</param>
        /// <param name="StartIndex">Начальный индекс</param>
        /// <param name="Lenght">Длина</param>
        /// <returns>Массив байт</returns>
        private static byte[] ArrayPart(byte[] array, int StartIndex, int Lenght)
        {
            byte[] res = new byte[Lenght];
            Array.Copy(array, StartIndex, res, 0, Lenght);
            return res;
        }
    }

}
