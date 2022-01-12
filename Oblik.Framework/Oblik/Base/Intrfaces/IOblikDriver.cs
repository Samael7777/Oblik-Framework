using System.Collections.Generic;

namespace Oblik.Driver
{
    public interface IOblikDriver
    {
        /// <summary>
        /// Запрос к счетчику
        /// </summary>
        /// <param name="l1">Пакет запроса L1</param>
        /// <param name="baudrate">Скорость соединения</param>
        /// <param name="timeout">Таймаут записи/чтения</param>
        /// <returns>Ответ L2 счетчика</returns>
        byte[] Request(byte[] l1, int baudrate, int timeout);
        
        /// <summary>
        /// Истина, если подкючение по протоколу RS-232
        /// </summary>
        bool IsDirectConnected { get; }

        /// <summary>
        /// Порт подключения счетчика
        /// </summary>
        string Port { get; }

    }
}