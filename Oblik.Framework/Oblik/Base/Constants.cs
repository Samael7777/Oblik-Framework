namespace Oblik
{
    /// <summary>
    /// Коды ошибок ввода-вывода
    /// </summary>
    public enum Error
    {
        NoError = 0,
        OpenPortError = 1,
        Timeout = 2,
        ReadError = 3,
        WriteError = 4,
        L1CSCError = 5,
        L1BufOverfowError = 6,
        L1UnkErrror = 7,
        L2ReqError = 8,
        L2SegIdError = 9,
        L2SegOpError = 10,
        L2UserAcsError = 11,
        L2DataPermisError = 12,
        L2SegOfstError = 13,
        L2WrReqError = 14,
        L2DataLenError = 15,
        L2PwdError = 16,
        L2DGCleanError = 17,
        L2PwdChngError = 18,
        L2UnkError = 19,
        NotWriteableSegError = 20,
        NotReadableSegError = 21,
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