using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oblik.FS;

namespace Oblik.Control
{
    public partial class OblikControl
    {
        /*-------------------------Private--------------------------------------*/
        ConnectionParams connectionParams;
        OblikFS oblikFS;

        /*-------------------------Constructors---------------------------------*/
        OblikControl(ConnectionParams connectionparams)
        {
            connectionParams = connectionparams;
            oblikFS = new OblikFS(connectionParams);
        }

        /*-------------------------Public---------------------------------------*/
        /// <summary>
        /// Парметры подключения к счетчику
        /// </summary>
        public ConnectionParams CurrentConnectionParams
        {
            get => connectionParams;
            set
            {
                connectionParams = value;
                oblikFS.CurrentConnectionParams = value;
            }
        }
        
        /// <summary>
        /// Журнал ошибок ввода-вывода
        /// </summary>
        public List<int> GetIOErrorsList  
        { 
            get => oblikFS.GetIOErrorsList; 
        }

    }
}
