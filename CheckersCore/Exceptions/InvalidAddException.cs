using System;

namespace CheckersCore
{
    public class InvalidAddException : Exception
    {
        public InvalidAddException(string message)
            : base(message)
        {

        }
    }
}
