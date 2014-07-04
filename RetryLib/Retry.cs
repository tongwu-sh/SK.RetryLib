using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RetryLib
{
    public enum RetryType{
        Linear,
        Double,
        Zero
    }

    public class Retry
    {
        const int DefaultRetryCount = 3;
        const int DefaultInternalSecond = 5;

        public Predicate<Exception> RetryPolicy { get; set; }

        public int RetryCount { get; set; }

        public TimeSpan InternalTime { get; set; }

        public RetryType RetryType { get; set; }

        public Retry(int retryCount, TimeSpan internalTime, RetryType retryType = RetryType.Linear)
        {
            RetryCount = retryCount;
            InternalTime = internalTime;
            RetryType = retryType;
        }

        public Retry(int retryCount = DefaultRetryCount, 
                     int internalSecond = DefaultInternalSecond, 
                     RetryType retryType = RetryType.Linear)
            : this(retryCount, TimeSpan.FromSeconds(internalSecond), retryType)
        {  }

        public void ExecuteAction(Action action)
        {
            ExecuteFunc<bool>(() =>
            {
                action();
                return true;
            });
        }

        public T ExecuteFunc<T>(Func<T> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException("function is null");
            }

            int currentRetryCount = 0;
            while (true)
            {
                try
                {
                    return func();
                }
                catch (Exception ex)
                {
                    currentRetryCount++;
                    if ((RetryPolicy == null || RetryPolicy(ex)) && currentRetryCount < RetryCount)
                    {
                        Thread.Sleep(WaitingTime(currentRetryCount));
                        continue;
                    }
                    else throw;
                }
            }
        }

        protected virtual TimeSpan WaitingTime(int currentRetryTime)
        {
            switch (RetryType)
            {
                case RetryType.Double:
                    return TimeSpan.FromSeconds(InternalTime.Seconds * Math.Pow(2, currentRetryTime));
                case RetryType.Zero:
                    return TimeSpan.Zero;

                default:
                    return InternalTime;
            }

        }
    }
}
