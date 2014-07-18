using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetryLibTest
{
    public class RetryTestHelper
    {
        public Exception Ex { set; get; }
        public int RetryCount { set; get; }

        public int CurrentCount;

        public RetryTestHelper(Exception ex, int retryCount)
        {
            Ex = ex;
            RetryCount = retryCount;
            CurrentCount = 0;
        }

        public void Action()
        {
            if (++CurrentCount % RetryCount != 0)
                throw Ex;
        }

        public int Function()
        {
            Action();
            return CurrentCount;
        }
    }
}
