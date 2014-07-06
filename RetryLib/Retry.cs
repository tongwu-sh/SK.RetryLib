using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RetryLib
{
    public enum RetryWaitType{
        Linear, // wait Xs, Xs, Xs....
        Double, // wait Xs, 2Xs, 4Xs...
        Random, // wait (0, X)s
        Zero    // never wait
    }

    /// <summary>
    /// Retry simple usage:
    ///     var result = Retry.ExecuteFunc(
    ///                     () =>
    ///                     {
    ///                         // Do Something here.
    ///                         return ....;
    ///                     }, retryCount:3, intervalMilliSecond:1000);
    /// </summary>
    public partial class Retry
    {
        const int DefaultIntervalMilliSecond = 5 * 1000;

        #region OnExceptionCatch Event
        public class ExceptionArgs : EventArgs
        {
            public ExceptionArgs(Exception ex)
            {
                Ex = ex;
            }

            public Exception Ex { get; set; }
        }

        public event EventHandler<ExceptionArgs> OnExceptionCatch = null;
        #endregion

        #region Properties
        public IRetryPolicy RetryPolicy { get; set; }

        public int RetryCount { get; set; }

        public TimeSpan IntervalTime { get; set; }

        public RetryWaitType RetryType { get; set; }
        #endregion

        #region Constructor
        public Retry(int retryCount, TimeSpan intervalTime, RetryWaitType retryWaitType = RetryWaitType.Linear)
        {
            Init(retryCount, intervalTime, retryWaitType);
        }

        public Retry(int retryCount,
                     int intervalMilliSecond = DefaultIntervalMilliSecond,
                     RetryWaitType retryWaitType = RetryWaitType.Linear)
        {
            if (intervalMilliSecond < 0)
            {
                throw new ArgumentException("intervalMillionSecond should >= 0");
            }

            Init(retryCount, TimeSpan.FromMilliseconds(intervalMilliSecond), retryWaitType);
        }

        private void Init(int retryCount, TimeSpan intervalTime, RetryWaitType retryType)
        {
            RetryCount = retryCount;
            IntervalTime = intervalTime;
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
                    // => Will run every OnExceptionCatchHandler and throw AggregateException for Interval Exception
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

        public async Task ExecuteActionAsync(Func<Task> asyncAction)
        {
            await ExecuteFuncAsync(async () =>
            {
                await asyncAction();
                return true;
            });
        }

        public async Task<T> ExecuteFuncAsync<T>(Func<Task<T>> asyncFunc)
        {
            if (asyncFunc == null)
            {
                throw new ArgumentNullException("asyncFunc is null");
            }

            int currentRetryCount = 0;
            while (true)
            {
                try
                {
                    return await asyncFunc();
                }
                catch (Exception ex)
                {
                    // Step 1: Call event handlers for catched Exception
                    // => Will run every OnExceptionCatchHandler and throw AggregateException for Interval Exception
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
                    return TimeSpan.FromTicks(IntervalTime.Ticks * (long)Math.Pow(2, currentRetryTime));
                case RetryWaitType.Zero:
                    return TimeSpan.Zero;
                case RetryWaitType.Random:
                    Random intervalTicksRandom = new Random();
                    long intervalTicks = (long)(intervalTicksRandom.NextDouble() * (double)IntervalTime.Ticks);
                    return TimeSpan.FromTicks(intervalTicks);
                default:
                    return IntervalTime;
            }
        }

        #endregion
    }
}
