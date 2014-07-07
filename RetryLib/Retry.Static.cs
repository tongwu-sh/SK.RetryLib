using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SK.RetryLib
{
    /// <summary>
    /// Retry.Static contains static method for easy use.
    /// </summary>
    public partial class Retry
    {
        #region static method for retry easy use
        public static void Action(Action action,
                                int retryCount,
                                TimeSpan intervalTime,
                                RetryWaitType waitType = RetryWaitType.Linear,
                                IRetryPolicy retryPolicy = null,
                                EventHandler<ExceptionArgs> exceptionHandler = null)
        {
            var retry = CreateRetry(retryCount, intervalTime, waitType, retryPolicy, exceptionHandler);
            retry.Action(action);
        }

        public static void Action(Action action,
                                int retryCount,
                                int intervalMilliSecond = DefaultIntervalMilliSecond,
                                RetryWaitType waitType = RetryWaitType.Linear,
                                IRetryPolicy retryPolicy = null,
                                EventHandler<ExceptionArgs> exceptionHandler = null)
        {
            var retry = CreateRetry(retryCount, intervalMilliSecond, waitType, retryPolicy, exceptionHandler);
            retry.Action(action);
        }

        public static async Task ActionAsync(Func<Task> action,
                                             int retryCount,
                                             TimeSpan intervalTime,
                                             RetryWaitType waitType = RetryWaitType.Linear,
                                             IRetryPolicy retryPolicy = null,
                                             EventHandler<ExceptionArgs> exceptionHandler = null)
        {
            var retry = CreateRetry(retryCount, intervalTime, waitType, retryPolicy, exceptionHandler);
            await retry.ActionAsync(action);
        }

        public static async Task ActionAsync(Func<Task> action,
                                             int retryCount,
                                             int intervalMilliSecond = DefaultIntervalMilliSecond,
                                             RetryWaitType waitType = RetryWaitType.Linear,
                                             IRetryPolicy retryPolicy = null,
                                             EventHandler<ExceptionArgs> exceptionHandler = null)
        {
            var retry = CreateRetry(retryCount, intervalMilliSecond, waitType, retryPolicy, exceptionHandler);
            await retry.ActionAsync(action);
        }

        public static T Func<T>(Func<T> func,
                                int retryCount,
                                TimeSpan intervalTime,
                                RetryWaitType waitType = RetryWaitType.Linear,
                                IRetryPolicy retryPolicy = null,
                                EventHandler<ExceptionArgs> exceptionHandler = null)
        {
            var retry = CreateRetry(retryCount, intervalTime, waitType, retryPolicy, exceptionHandler);
            return retry.Func(func);
        }

        public static T Func<T>(Func<T> func,
                                int retryCount,
                                int intervalMilliSecond = DefaultIntervalMilliSecond,
                                RetryWaitType waitType = RetryWaitType.Linear,
                                IRetryPolicy retryPolicy = null,
                                EventHandler<ExceptionArgs> exceptionHandler = null)
        {
            var retry = CreateRetry(retryCount, intervalMilliSecond, waitType, retryPolicy, exceptionHandler);
            return retry.Func(func);
        }

        public static async Task<T> FuncAsync<T>(Func<Task<T>> func,
                                               int retryCount,
                                               int intervalMilliSecond = DefaultIntervalMilliSecond,
                                               RetryWaitType waitType = RetryWaitType.Linear,
                                               IRetryPolicy retryPolicy = null,
                                               EventHandler<ExceptionArgs> exceptionHandler = null)
        {
            var retry = CreateRetry(retryCount, intervalMilliSecond, waitType, retryPolicy, exceptionHandler);
            return await retry.FuncAsync(func);
        }

        public static async Task<T> FuncAsync<T>(Func<Task<T>> func,
                                               int retryCount,
                                               TimeSpan intervalTime,
                                               RetryWaitType waitType = RetryWaitType.Linear,
                                               IRetryPolicy retryPolicy = null,
                                               EventHandler<ExceptionArgs> exceptionHandler = null)
        {
            var retry = CreateRetry(retryCount, intervalTime, waitType, retryPolicy, exceptionHandler);
            return await retry.FuncAsync(func);
        }
        #endregion

        #region Create Retry
        public static Retry CreateRetry(int retryCount,
                                        TimeSpan intervalTime,
                                        RetryWaitType waitType = RetryWaitType.Linear,
                                        IRetryPolicy retryPolicy = null,
                                        EventHandler<ExceptionArgs> exceptionHandler = null)
        {
            var retry = new Retry(retryCount, intervalTime, waitType);

            if (retryPolicy != null)
            {
                retry.RetryPolicy = retryPolicy;
            }
            if (exceptionHandler != null)
            {
                retry.OnExceptionCatch += exceptionHandler;
            }

            return retry;
        }

        public static Retry CreateRetry(int retryCount,
                                        int intervalMilliSecond = DefaultIntervalMilliSecond,
                                        RetryWaitType waitType = RetryWaitType.Linear,
                                        IRetryPolicy retryPolicy = null,
                                        EventHandler<ExceptionArgs> exceptionHandler = null)
        {
            var retry = new Retry(retryCount, intervalMilliSecond, waitType);

            if (retryPolicy != null)
            {
                retry.RetryPolicy = retryPolicy;
            }
            if (exceptionHandler != null)
            {
                retry.OnExceptionCatch += exceptionHandler;
            }

            return retry;
        }
        #endregion
    }
}
