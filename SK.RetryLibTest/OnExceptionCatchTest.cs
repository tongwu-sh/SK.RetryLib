using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SK.RetryLib;

namespace RetryLibTest
{
    [TestClass]
    public class OnExceptionCatchTest
    {
        [TestMethod]
        public void NoneOnExceptionCatch()
        {
            RetryTestHelper helper = new RetryTestHelper(new Exception("Test for NoneOnExceptionCatch"), 3);
            var retry = new Retry(3);

            retry.Action(() =>
            {
                helper.Action();
            });

            Assert.AreEqual(3, helper.CurrentCount);
        }

        [TestMethod]
        public void MultiOnExceptionCatch()
        {
            RetryTestHelper helper = new RetryTestHelper(new Exception("Test for MultiOnExceptionCatch"), 3);
            var retry = new Retry(3);
            int firstCount = 0, secondCount = 0;
            retry.OnExceptionCatch +=
                (sender, args) =>
                {
                    firstCount++;
                };

            retry.OnExceptionCatch +=
                (sender, args) =>
                {
                    secondCount++;
                };

            retry.Action(() =>
            {
                helper.Action();
            });

            Assert.AreEqual(2, firstCount);
            Assert.AreEqual(2, secondCount);
            Assert.AreEqual(3, helper.CurrentCount);
        }

        [TestMethod]
        public void SingleOnExceptionCatch()
        {
            RetryTestHelper helper = new RetryTestHelper(new Exception("Test for SingleOnExceptionCatch"), 3);
            var retry = new Retry(3);
            int count = 0;
            retry.OnExceptionCatch +=
                (object sender, Retry.ExceptionArgs args) =>
                {
                    count++;
                };
            retry.Action(() =>
            {
                helper.Action();
            });

            Assert.AreEqual(2, count);
            Assert.AreEqual(3, helper.CurrentCount);
        }
    
        [TestMethod]
        public void ThrowExceptionInOnExceptionCatch()
        {
            RetryTestHelper helper = new RetryTestHelper(new Exception("Test for MultiOnExceptionCatch"), 3);
            var retry = new Retry(3);
            retry.OnExceptionCatch +=
                (object sender, Retry.ExceptionArgs args) =>
                {
                    throw new Exception("first");
                };

            retry.OnExceptionCatch +=
                (object sender, Retry.ExceptionArgs args) =>
                {
                    throw new Exception("second");
                };

            try
            {
                retry.Action(() =>
                {
                    helper.Action();
                });
                Assert.Fail("Should Catch Exception.");
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(AggregateException));
                Assert.AreEqual(2, (ex as AggregateException).InnerExceptions.Count);
            }

            Assert.AreEqual(1, helper.CurrentCount);
        }
    }
}
