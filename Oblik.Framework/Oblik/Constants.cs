namespace Oblik
{
    /// <summary>
    /// Коды ошибок ввода-вывода
    /// </summary>
    public enum Error
    {
        NoError         = 0,
        OpenPortError   = 1,
        Timeout         = 2,
        ReadError       = 3,
        WriteError      = 4,
        CSCError        = 5,
    }

    /// <summary>
    /// Уровень доступа
    /// 0 - пользователь;
    /// 1 - администратор;
    /// 2 - энергонадзор; 
    /// 3 - служебный пользователь.
    /// </summary>
    public enum UserLevel
    {
        /// <summary>
        /// Пользователь
        /// </summary>
        User = 0,
        /// <summary>
        /// Администратор
        /// </summary>
        Admin = 1,
        /// <summary>
        /// Энергонадзор
        /// </summary>
        Energo = 2,
        /// <summary>
        /// Системный пользователь
        /// </summary>
        System = 3
    }

}
