using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetryLib
{
    /// <summary>
    /// Retry.Static contains static method for easy use.
    /// </summary>
    public partial class Retry
    {
        #region static method for retry easy use
        public static void ExecuteAction(Action action,
                                         int retryCount,
                                         TimeSpan intervalTime,
                                         RetryWaitType waitType = RetryWaitType.Linear,
                                         IRetryPolicy retryPolicy = null,
                                         EventHandler<ExceptionArgs> exceptionHandler = null)
        {
            var retry = CreateRetry(retryCount, intervalTime, waitType, retryPolicy, exceptionHandler);
            retry.ExecuteAction(action);
        }

        public static void ExecuteAction(Action action,
                                         int retryCount,
                                         int intervalMilliSecond = DefaultIntervalMilliSecond,
                                         RetryWaitType waitType = RetryWaitType.Linear,
                                         IRetryPolicy retryPolicy = null,
                                         EventHandler<ExceptionArgs> exceptionHandler = null)
        {
            var retry = CreateRetry(retryCount, intervalMilliSecond, waitType, retryPolicy, exceptionHandler);
            retry.ExecuteAction(action);
        }

        public static async Task ExecuteActionAsync(Func<Task> action,
                                         int retryCount,
                                         TimeSpan intervalTime,
                                         RetryWaitType waitType = RetryWaitType.Linear,
                                         IRetryPolicy retryPolicy = null,
                                         EventHandler<ExceptionArgs> exceptionHandler = null)
        {
            var retry = CreateRetry(retryCount, intervalTime, waitType, retryPolicy, exceptionHandler);
            await retry.ExecuteActionAsync(action);
        }

        public static async Task ExecuteActionAsync(Func<Task> action,
                                         int retryCount,
                                         int intervalMilliSecond = DefaultIntervalMilliSecond,
                                         RetryWaitType waitType = RetryWaitType.Linear,
                                         IRetryPolicy retryPolicy = null,
                                         EventHandler<ExceptionArgs> exceptionHandler = null)
        {
            var retry = CreateRetry(retryCount, intervalMilliSecond, waitType, retryPolicy, exceptionHandler);
            await retry.ExecuteActionAsync(action);
        }

        public static T ExecuteFunc<T>(Func<T> func,
                                       int retryCount,
                                       TimeSpan intervalTime,
                                       RetryWaitType waitType = RetryWaitType.Linear,
                                       IRetryPolicy retryPolicy = null,
                                       EventHandler<ExceptionArgs> exceptionHandler = null)
        {
            var retry = CreateRetry(retryCount, intervalTime, waitType, retryPolicy, exceptionHandler);
            return retry.ExecuteFunc(func);
        }

        public static T ExecuteFunc<T>(Func<T> func,
                                       int retryCount,
                                       int intervalMilliSecond = DefaultIntervalMilliSecond,
                                       RetryWaitType waitType = RetryWaitType.Linear,
                                       IRetryPolicy retryPolicy = null,
                                       EventHandler<ExceptionArgs> exceptionHandler = null)
        {
            var retry = CreateRetry(retryCount, intervalMilliSecond, waitType, retryPolicy, exceptionHandler);
            return retry.ExecuteFunc(func);
        }

        public static async Task<T> ExecuteFuncAsync<T>(Func<Task<T>> func,
                                       int retryCount,
                                       int intervalMilliSecond = DefaultIntervalMilliSecond,
                                       RetryWaitType waitType = RetryWaitType.Linear,
                                       IRetryPolicy retryPolicy = null,
                                       EventHandler<ExceptionArgs> exceptionHandler = null)
        {
            var retry = CreateRetry(retryCount, intervalMilliSecond, waitType, retryPolicy, exceptionHandler);
            return await retry.ExecuteFuncAsync(func);
        }

        public static async Task<T> ExecuteFuncAsync<T>(Func<Task<T>> func,
                                       int retryCount,
                                       TimeSpan intervalTime,
                                       RetryWaitType waitType = RetryWaitType.Linear,
                                       IRetryPolicy retryPolicy = null,
                                       EventHandler<ExceptionArgs> exceptionHandler = null)
        {
            var retry = CreateRetry(retryCount, intervalTime, waitType, retryPolicy, exceptionHandler);
            return await retry.ExecuteFuncAsync(func);
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
