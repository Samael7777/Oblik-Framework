using System;

namespace Oblik
{
    /// <summary>
    /// Исключение операций ввода/вывода
    /// </summary>
    public class OblikIOException : Exception
    {
        public Error ErrorCode
        {
            get; private set;
        }

        public OblikIOException(string message) : base(message)
        {
            ErrorCode = Error.UnknownError;
        }

        public OblikIOException() : base()
        {
            ErrorCode = Error.UnknownError;
        }

        public OblikIOException(string message, Exception innerException) : base(message, innerException)
        {
            ErrorCode = Error.UnknownError;
        }

        public OblikIOException(string message, Error errorcode) : base(message)
        {
            ErrorCode = errorcode;
        }
    }
}