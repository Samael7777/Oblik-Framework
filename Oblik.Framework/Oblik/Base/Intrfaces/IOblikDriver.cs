using System.Collections.Generic;

namespace Oblik.Driver
{
    public interface IOblikDriver
    {
        /// <summary>
        /// Установка новой скорости соединения
        /// </summary>
        /// <param name="baudrate"></param>
        int Baudrate { get; set; }
        /// <summary>
        /// Запрос к счетчику
        /// </summary>
        /// <param name="l1">Пакет запроса L1</param>
        /// <returns>Ответ L2 счетчика</returns>
        byte[] Request(byte[] l1);
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