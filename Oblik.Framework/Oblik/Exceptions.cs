using System;


namespace Oblik
{
    /// <summary>
    /// Исключение операций ввода/вывода
    /// </summary>
    public class OblikIOException : Exception
    {
        public OblikIOException(string message) : base(message) { }
        public OblikIOException() : base() { }
        public OblikIOException(string message, Exception innerException) : base(message, innerException) { }
    }
}
