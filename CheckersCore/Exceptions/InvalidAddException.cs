using System;

namespace CheckersCore.Exceptions
{
    public class InvalidAddException : Exception
    {
        public InvalidAddException(string message)
            : base(message)
        {

        }
    }
}
