using Polly;
using Polly.Retry;

namespace Common.ResiliencyPolicies
{
    public class GenericRetryingPolicy
    {
        public static AsyncRetryPolicy Get(string contextName, int retryDelayInSeconds = 3)
        {
            var retrySleepDuration = TimeSpan.FromSeconds(retryDelayInSeconds);

            var retryPolicy = Policy.Handle<Exception>()
                .WaitAndRetryAsync(
                   retryCount: 3,
                   sleepDurationProvider: (x) => retrySleepDuration,
                   onRetry: (exception, sleepDuration, attemptNumber, context) =>
                   {
                       Console.WriteLine($"{contextName}: Error during service communication. Retrying in {retrySleepDuration}. {attemptNumber} / {3}");
                   });

            return retryPolicy;
        }
    }
}
