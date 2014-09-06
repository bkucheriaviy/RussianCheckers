using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
