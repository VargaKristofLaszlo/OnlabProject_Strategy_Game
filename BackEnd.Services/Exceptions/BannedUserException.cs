using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Exceptions
{
    public class BannedUserException : Exception
    {
        public BannedUserException()
        {
        }

        public BannedUserException(string message)
            : base(message)
        {
        }

        public BannedUserException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
