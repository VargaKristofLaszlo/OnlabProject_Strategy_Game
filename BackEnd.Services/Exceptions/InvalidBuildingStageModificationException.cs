using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Exceptions
{
    public class InvalidBuildingStageModificationException : Exception
    {
        public InvalidBuildingStageModificationException()
        {
        }

        public InvalidBuildingStageModificationException(string message)
            : base(message)
        {
        }

        public InvalidBuildingStageModificationException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
