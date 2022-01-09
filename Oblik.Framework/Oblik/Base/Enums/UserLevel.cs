using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oblik
{ 
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
