using System;

namespace CheckersCore
{
    public class InvalidMoveException : Exception
    {
        public InvalidMoveException(string message)
            : base(message)
        {

        }
    }
}
