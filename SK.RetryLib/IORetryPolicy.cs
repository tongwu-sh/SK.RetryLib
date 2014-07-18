using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SK.RetryLib
{
    /// <summary>
    /// IORetryPolicy used for IO Operation retry, only retry for IO Transit Exception 
    /// </summary>
    public class IORetryPolicy : IRetryPolicy
    {
        private static HashSet<Type> NotRetryIOException = new HashSet<Type>()
        {
            typeof(EndOfStreamException),
            typeof(FileLoadException),
            typeof(PathTooLongException),
            typeof(DriveNotFoundException)
        };

        public virtual bool ShouldRetry(Exception Ex)
        {
            // Only retry IOException
            if (!(Ex is IOException))
                return false;

            var ioEx = Ex as IOException;
            if (NotRetryIOException.Contains(ioEx.GetType()))
            {
                return false;
            }

            return true;
        }
    }
}
