using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SK.RetryLib;
using System.Net;
using System.Collections.Generic;

namespace RetryLibTest
{
    [TestClass]
    public class WebRetryTest
    {
        [TestMethod]
        public void TimeoutRetry()
        {
            var retry = new Retry(3, 1000);
            retry.RetryPolicy = new WebRetryPolicy();
            var catchedExceptions = new List<Exception>();

            retry.OnExceptionCatch += 
                (object sender, Retry.ExceptionArgs args) =>
                {
                    catchedExceptions.Add(args.Ex);
                };

            int count = 0;
            try
            {
                retry.Func(
                    () =>
                    {
                        WebRequest request = WebRequest.Create("http://www.bing.com/");
                        request.Timeout = 10;
                        count++;
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        return response;
                    });
                Assert.Fail("Should catch exception here.");
            }
            catch { }

            Assert.AreEqual(3, count);
            foreach (Exception e in catchedExceptions)
            {
                Assert.IsInstanceOfType(e, typeof(WebException));
                Assert.AreEqual(WebExceptionStatus.Timeout, (e as WebException).Status);
            }
        }

        [TestMethod]
        public void WebNotFoundRetry()
        {
            var retry = new Retry(3, 1000);
            retry.RetryPolicy = new WebRetryPolicy();

            int count = 0;
            try
            {
                retry.Func(
                    () =>
                    {
                        WebRequest request = WebRequest.Create("http://www.facebook.com/");
                        count++;
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        return response;
                    });
                Assert.Fail("Should catch exception here.");
            }
            catch { }

            Assert.AreEqual(3, count);
        }

        [TestMethod]
        public void WebShouldNotRetry()
        {
            var retry = new Retry(3, 1000);
            retry.RetryPolicy = new WebRetryPolicy();

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
