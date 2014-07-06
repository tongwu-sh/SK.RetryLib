using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RetryLib;
using System.Threading.Tasks;

namespace RetryLibTest
{
    [TestClass]
    public class AsyncRetryTest
    {
        [TestMethod]
        public void AsyncFunctionRetry()
        {
            RetryTestHelper helper = new RetryTestHelper(new Exception("Exception for TestExecuteFunction"), 3);
            Task<int> resultTask = new Retry(3, 1000, RetryWaitType.Linear).ExecuteFuncAsync(() =>
            {
                return Task.Run(() => helper.Function());
            });

            int result = resultTask.Result;
            Assert.AreEqual(3, result);
        }

        [TestMethod]
        public void AsyncActionRetry()
        {
            RetryTestHelper helper = new RetryTestHelper(new Exception("Exception for TestExecuteFunction"), 3);
            var retry = new Retry(3, 1000, RetryWaitType.Linear);
            int exceptionCount = 0;
            retry.OnExceptionCatch +=
                (sender, args) =>
                {
                    exceptionCount++;
                };
            Task resultTask = retry.ExecuteActionAsync(async () =>
            {
                await Task.Run(() =>
                {
                    helper.Action();
                });
            });

            resultTask.Wait();
            Assert.AreEqual(3, helper.CurrentCount);
            Assert.AreEqual(2, exceptionCount);
        }
    }
}
