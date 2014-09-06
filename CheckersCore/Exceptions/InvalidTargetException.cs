using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
