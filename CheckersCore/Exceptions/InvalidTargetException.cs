using System;

namespace CheckersCore.Exceptions
{
    public class InvalidTargetException : Exception
    {
        public InvalidTargetException(string message)
            : base(message)
        {

        }
    }
}
