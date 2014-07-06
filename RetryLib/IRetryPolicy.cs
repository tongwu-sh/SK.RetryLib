using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SK.RetryLib
{
    public interface IRetryPolicy
    {
        bool ShouldRetry(Exception Ex);
    }
}
