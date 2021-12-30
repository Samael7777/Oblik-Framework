using System.Collections.Generic;

namespace Oblik.Driver
{
    public interface IOblikDriver
    {
        /// <summary>
        /// Текущий адрес счетчика
        /// </summary>
        int Address { get; }
        /// <summary>
        /// Текущий пользователь
        /// </summary>
        UserLevel User { get; }
        /// <summary>
        /// Текущий пароль
        /// </summary>
        byte[] Password { get; }
        /// <summary>
        /// Запрос к счетчику
        /// </summary>
        /// <param name="l1">Пакет запроса L1</param>
        /// <returns>Ответ L2 счетчика</returns>
        byte[] Request(byte[] l1);

    }
}