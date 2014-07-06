using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RetryLib;

namespace RetryLibTest
{
    [TestClass]
    public class ActionAndFunctionRetryTest
    {
        #region TestMethod
        [TestMethod]
        public void TestExecuteActionLinear()
        {
            TestExecuteActionHelper(3, 1000, RetryWaitType.Linear);
        }

        [TestMethod]
        public void TestExecuteActionDouble()
        {
            TestExecuteActionHelper(3, 1000, RetryWaitType.Double);
        }
        [TestMethod]
        public void TestExecuteActionRandom()
        {
            TestExecuteActionHelper(3, 1000 * 5, RetryWaitType.Random);
        }

        [TestMethod]
        public void TestExecuteActionZero()
        {
            TestExecuteActionHelper(3, 1000 * 5, RetryWaitType.Zero);
        }

        [TestMethod]
        public void TestExecuteActionNagtiveCount()
        {
            TestExecuteActionHelper(-1, 1, RetryWaitType.Linear);
        }

        [TestMethod]
        public void TestExecuteActionNagtiveInterval()
        {
            try
            {
                TestExecuteActionHelper(-1, -1, RetryWaitType.Linear);
            }
            catch (ArgumentException)
            {
                Console.WriteLine("TestExecuteActionNagtiveInterval Passed");
            }
        }

        [TestMethod]
        public void TestExecuteFunction()
        {
            TestExecuteFuntionHelper(3, 1, RetryWaitType.Linear);
        }
        #endregion

        #region TestHelper
        private void TestExecuteFuntionHelper(int count, int interval, RetryWaitType type)
        {
            RetryTestHelper helper = new RetryTestHelper(new Exception("Exception for TestExecuteFunction"), count);
            int result = new Retry(count, interval, type).ExecuteFunc(() =>
            {
                return helper.Function();
            });

            if (count <= 0)
            {
                Assert.AreEqual(1, result);
            }
            else
            {
                Assert.AreEqual(count, result);
            }
        }

        private void TestExecuteActionHelper(int count, int interval, RetryWaitType type)
        {
            RetryTestHelper helper = new RetryTestHelper(new Exception("Exception for TestExecuteFunction"), count);
            new Retry(count, interval, type).ExecuteAction(helper.Action);

            if (count <= 0)
            {
                Assert.AreEqual(1, helper.CurrentCount);
            }
            else
            {
                Assert.AreEqual(count, helper.CurrentCount);
            }
        }
        #endregion
    }
}
