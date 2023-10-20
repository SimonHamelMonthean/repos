using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarteAuTresor
{
    public class BadInputFileException : Exception
    {
        public BadInputFileException()
        {
        }

        public BadInputFileException(string message)
            : base(message)
        {
        }

        public BadInputFileException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
