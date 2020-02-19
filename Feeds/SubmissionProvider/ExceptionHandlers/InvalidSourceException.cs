using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubmissionProvider.Exceptions
{
    public class InvalidSourceException : Exception
    {
        public InvalidSourceException(string msg)
        {
            throw new Exception(msg);
        }        
    }
}
