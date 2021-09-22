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
        private static string ParseChannelError(int error)
        {
            string res;
            switch (error)
            {
                case 1:
                    res = Resources.Resources.L1OK;
                    break;
                case 0xff:
                    res = Resources.Resources.L1CSCError;
                    break;
                case 0xfe:
                    res = Resources.Resources.L1Overflow;
                    break;
                default:
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
        private static string ParseSegmentError(int error)
        {
            string res;
            switch (error)
            {
                case 0:
                    res = Resources.Resources.L2Err00;
                    break;
                case 0xff:
                    res = Resources.Resources.L2ErrFF;
                    break;
                case 0xfe:
                    res = Resources.Resources.L2ErrFE;
                    break;
                case 0xfd:
                    res = Resources.Resources.L2ErrFD;
                    break;
                case 0xfc:
                    res = Resources.Resources.L2ErrFC;
                    break;
                case 0xfb:
                    res = Resources.Resources.L2ErrFB;
                    break;
                case 0xfa:
                    res = Resources.Resources.L2ErrFA;
                    break;
                case 0xf9:
                    res = Resources.Resources.L2ErrF9;
                    break;
                case 0xf8:
                    res = Resources.Resources.L2ErrF8;
                    break;
                case 0xf7:
                    res = Resources.Resources.L2ErrF7;
                    break;
                case 0xf6:
                    res = Resources.Resources.L2ErrF6;
                    break;
                case 0xf5:
                    res = Resources.Resources.L2ErrF5;
                    break;
                default:
                    res = Resources.Resources.L2ErrUnk;
                    break;
            }
            return res;
        }
        
        /// <summary>
        /// Процедура шифрования данных L2
        /// </summary>
        /// <param name="l2">Ссылка на массив L2</param>
        /// <param name="passwd">Пароль</param>
        private static void Encode(ref byte[] l2, byte[] passwd)
        {
            //Шифрование полей "Данные" и "Пароль"
            byte x1 = 0x3A;
            for (int i = 0; i <= 7; i++) { x1 ^= passwd[i]; }
            int dpcsize = l2[4] + 8;                                //Размер "Данные + "Пароль" 
            int k = 4;
            for (int i = dpcsize - 1; i >= 0; i--)
            {
                byte x2 = l2[k++];
                l2[k] ^= x1;
                l2[k] ^= x2;
                l2[k] ^= passwd[i % 8];
                x1 += (byte)i;
            }
        }

    }

}
