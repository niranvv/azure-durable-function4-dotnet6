using Learn.DurableFunction.Exceptions;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using System;

namespace Learn.DurableFunction.Helpers
{
    public static class FunctionRetryOptions
    {
        public static RetryOptions options = new RetryOptions(firstRetryInterval: TimeSpan.FromSeconds(2),
                                                              maxNumberOfAttempts: 5)
        {
            Handle = (exception) =>
              {
                  return exception.InnerException is TooManyRequestsException;
              }
        };
    }
}
