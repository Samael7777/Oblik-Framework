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
    }
}
