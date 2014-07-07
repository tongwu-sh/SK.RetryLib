using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SK.RetryLib;
using System.Threading.Tasks;

namespace RetryLibTest
{
    [TestClass]
    public class RetryStaticTest
    {
        [TestMethod]
        public void StaticActionRetry()
        {
            RetryTestHelper helper = new RetryTestHelper(new Exception("StaticActionRetry"), 3);
            Retry.Action(
                () =>
            {
                helper.Action();
            }, 3, 1000);
        }

        [TestMethod]
        public void StaticFuncRetry()
        {
            RetryTestHelper helper = new RetryTestHelper(new Exception("StaticFuncRetry"), 3);
            Retry.Func(
                () =>
                {
                    return helper.Function();
                }, 3, 1000);
        }

        [TestMethod]
        public void StaticActionAsyncRetry()
        {
            RetryTestHelper helper = new RetryTestHelper(new Exception("StaticActionAsyncRetry"), 3);
            Retry.ActionAsync(
                async () =>
                {
                    await Task.Run(() => helper.Function());
                }, 3, 1000).Wait();
        }

        [TestMethod]
        public void StaticFuncAsyncRetry()
        {
            RetryTestHelper helper = new RetryTestHelper(new Exception("StaticFuncAsyncRetry"), 3);
            Retry.FuncAsync(
                async () =>
                {
                    return await Task.Run( () => helper.Function());
                }, 3, 1000).Wait();
        }
    }
}
