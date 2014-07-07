using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using SK.RetryLib;

namespace RetryLibTest
{
    [TestClass]
    public class IORetryTest
    {
        [TestMethod]
        public void IONotFoundRetry()
        {
            var retry = new Retry(3, 1000);
            retry.RetryPolicy = new IORetryPolicy();

            int count = 0;
            try
            {
                retry.Action(
                    () =>
                    {
                        count++;
                        new FileStream("NotExist", FileMode.Open);
                    });
                Assert.Fail("Should catch exception here.");
            }
            catch { }

            Assert.AreEqual(3, count);
        }

        [TestMethod]
        public void FileInUseRetry()
        {
            var retry = new Retry(3, 1000);
            retry.RetryPolicy = new IORetryPolicy();

            int count = 0;
            try
            {
                new FileStream("InUse", FileMode.OpenOrCreate);
                retry.Action(
                    () =>
                    {
                        count++;
                        new FileStream("InUse", FileMode.Open);
                    });
                Assert.Fail("Should catch exception here.");
            }
            catch { }

            Assert.AreEqual(3, count);
        }

        [TestMethod]
        public void IOShouldNotRetry()
        {
            var retry = new Retry(3, 1000);
            retry.RetryPolicy = new IORetryPolicy();

            int count = 0;
            try
            {
                retry.Action(
                    () =>
                    {
                        count++;
                        throw new ArgumentException("Should not retry");
                    });
                Assert.Fail("Should catch exception here.");
            }
            catch { }

            Assert.AreEqual(1, count);
        }
    }
}
