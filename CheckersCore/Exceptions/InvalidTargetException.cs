using System;

namespace CheckersCore
{
    public class InvalidTargetException : Exception
    {
        public InvalidTargetException(string message)
            : base(message)
        {

        }
    }
}
