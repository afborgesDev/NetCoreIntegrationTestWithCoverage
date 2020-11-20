using System;
using Polly;

namespace EntryPoint.Processor.Services
{
    public class PolicyForRabbitCreation
    {
        public const int DefaultRetryCount = 10;

        public static readonly Policy MainPolicy = Policy.Handle<TimeoutException>()
                                                         .WaitAndRetry(DefaultRetryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }
}
