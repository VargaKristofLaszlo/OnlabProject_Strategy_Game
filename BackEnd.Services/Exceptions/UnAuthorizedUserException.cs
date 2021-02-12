using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Exceptions
{
    public class UnAuthorizedUserException : Exception
    {
        public UnAuthorizedUserException()
        {
        }

        public UnAuthorizedUserException(string message)
            : base(message)
        {
        }

        public UnAuthorizedUserException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
