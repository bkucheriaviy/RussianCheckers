using System;

namespace CheckersCore.Exceptions
{
    public class InvalidMoveException : Exception
    {
        public InvalidMoveException(string message)
            : base(message)
        {

        }
    }
}
