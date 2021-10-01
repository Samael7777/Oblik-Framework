namespace Oblik
{
    public enum Error
    {
        NoError = 0,
        OpenPortError = 1,
        Timeout = 2,
        ReadError = 3,
        WriteError = 4,
        L1CSCError = 5,
        L1OverFlow = 6,
        L1Unknown = 7,
        L2Unknown = 8,
        L2RequestError = 9,
        L2SegIDError = 10,
        L2OperationError = 11,
        L2UserLevelError = 12,
        L2PermissionError = 13,
        L2OffsetError = 14,
        L2WriteReqError = 15,
        L2DataLenError = 16,
        L2PassError = 17,
        L2CleanError = 18,
        L2PassChangeError = 19,
        SegmetAccessError = 20,
        CSCError = 21,
        QueryError = 22,
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
