using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RetryLib
{
    public enum RetryWaitType{
        Linear,
        Double,
        Random,
        Zero
    }

    public class Retry
    {
        const int DefaultInternalMilliSecond = 5 * 1000;

        #region OnExceptionCatch Event
        public class ExceptionArgs : EventArgs
        {
            public ExceptionArgs(Exception ex)
            {
                Ex = ex;
            }

            public Exception Ex { get; set; }
        }

        public event EventHandler<ExceptionArgs> OnExceptionCatch = delegate { };
        #endregion

        #region Properties
        public IRetryPolicy RetryPolicy { get; set; }

        public int RetryCount { get; set; }

        public TimeSpan InternalTime { get; set; }

        public RetryWaitType RetryType { get; set; }
        #endregion

        #region Constructor
        public Retry(int retryCount, TimeSpan internalTime, RetryWaitType retryWaitType = RetryWaitType.Linear)
        {
            Init(retryCount, internalTime, retryWaitType);
        }

        public Retry(int retryCount,
                     int internalMilliSecond = DefaultInternalMilliSecond,
                     RetryWaitType retryWaitType = RetryWaitType.Linear)
        {
            if (internalMilliSecond < 0)
            {
                throw new ArgumentException("internalMillionSecond should >= 0");
            }

            Init(retryCount, TimeSpan.FromMilliseconds(internalMilliSecond), retryWaitType);
        }

        private void Init(int retryCount, TimeSpan internalTime, RetryWaitType retryType)
        {
            RetryCount = retryCount;
            InternalTime = internalTime;
            RetryType = retryType;
        }
        #endregion

        #region Method
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
                    // Step 1: Call event handlers for catched Exception
                    // => Will run every OnExceptionCatchHandler and throw AggregateException for Internal Exception
                    if (OnExceptionCatch != null)
                    {
                        var eventExceptions = new List<Exception>();

                        foreach (EventHandler<ExceptionArgs> handler in OnExceptionCatch.GetInvocationList())
                        {
                            try
                            {
                                handler(this, new ExceptionArgs(ex));
                            }
                            catch (Exception eventEx)
                            {
                                eventExceptions.Add(eventEx);
                            }
                        }

                        if (eventExceptions.Count > 0)
                        {
                            throw new AggregateException("AggregateException thrown by OnExceptionCatch", eventExceptions);
                        }
                    }

                    // Step 2: Add currentRetryCount => "wait and continue" or "break and stop execution"
                    currentRetryCount++;
                    if ((RetryPolicy == null || RetryPolicy.ShouldRetry(ex)) && currentRetryCount < RetryCount)
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
                case RetryWaitType.Double:
                    return TimeSpan.FromTicks(InternalTime.Ticks * (long)Math.Pow(2, currentRetryTime));
                case RetryWaitType.Zero:
                    return TimeSpan.Zero;
                case RetryWaitType.Random:
                    Random intervalTicksRandom = new Random();
                    long intervalTicks = (long)(intervalTicksRandom.NextDouble() * (double)InternalTime.Ticks);
                    return TimeSpan.FromTicks(intervalTicks);
                default:
                    return InternalTime;
            }
        }

        #endregion
    }
}
