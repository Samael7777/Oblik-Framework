using System;

namespace Oblik
{
    /// <summary>
    /// Исключение операций ввода/вывода
    /// </summary>
    public class OblikIOException : Exception
    {
        public int errorcode
        {
            get; private set;
        }

        public OblikIOException(string message) : base(message)
        {
            errorcode = 0;
        }

        public OblikIOException() : base()
        {
            errorcode = 0;
        }

        public OblikIOException(string message, Exception innerException) : base(message, innerException)
        {
            errorcode = 0;
        }

        public OblikIOException(string message, int errorcode) : base(message)
        {
            this.errorcode = errorcode;
        }
    }
}