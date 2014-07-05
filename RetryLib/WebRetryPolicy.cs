using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RetryLib
{
    public class WebRetryPolicy : IRetryPolicy
    {
        private static HashSet<WebExceptionStatus> NotRetryWebExceptionStatus = new HashSet<WebExceptionStatus>()
        {
            WebExceptionStatus.MessageLengthLimitExceeded,
            WebExceptionStatus.ProtocolError,
            WebExceptionStatus.RequestProhibitedByCachePolicy,
            WebExceptionStatus.RequestProhibitedByProxy,
            WebExceptionStatus.SecureChannelFailure,
            WebExceptionStatus.TrustFailure
        };

        public virtual bool ShouldRetry(Exception Ex)
        {
            // Only retry WebException
            if (!(Ex is WebException))
                return false;

            var webEx = Ex as WebException;
            if (NotRetryWebExceptionStatus.Contains(webEx.Status))
            {
                return false;
            }

            return true;
        }
    }
}
