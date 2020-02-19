using SubmissionProvider.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubmissionProvider
{
    public class Logger : ILogger
    {
        public Logger()
        {

        }

        public void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}
