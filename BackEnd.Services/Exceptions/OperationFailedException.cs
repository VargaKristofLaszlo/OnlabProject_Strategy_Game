using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Exceptions
{
    public class OperationFailedException : Exception
    {
        public IEnumerable<string> Errors { get; private set; }

        public OperationFailedException(string message):base(message)
        {}

        public OperationFailedException()
        {

        }

        public OperationFailedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
