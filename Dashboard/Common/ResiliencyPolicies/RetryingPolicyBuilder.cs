using Polly;
using Polly.Retry;

namespace Common.ResiliencyPolicies
{
    public class RetryingPolicyBuilder<T>
    {
        private int _retryCount;
        private TimeSpan _retrySleepDuration;
        private string _contextName;
        private Action<int> _logDelegate;

        public RetryingPolicyBuilder()
        {
            _logDelegate = (attemptNumber) => {
                Console.WriteLine($"{_contextName}: Error during service communication. Retrying in {_retrySleepDuration}. {attemptNumber} / {_retryCount}"); 
            };

            WithContextName(typeof(T).Name);
            WithDelay(3);
            WithRetries(3);
        }

        public RetryingPolicyBuilder<T> WithContextName(string contextName)
        {
            _contextName = contextName;
            return this;
        }

        public RetryingPolicyBuilder<T> WithRetries(int retriesCount)
        {
            _retryCount = retriesCount;
            return this;
        }

        public RetryingPolicyBuilder<T> WithDelay(int retryDelayInSeconds)
        {
            _retrySleepDuration = TimeSpan.FromSeconds(retryDelayInSeconds);
            return this;
        }

        public RetryingPolicyBuilder<T> WithLogDelegate(Action<int> logAction)
        {
            _logDelegate = logAction;
            return this;
        }

        public AsyncRetryPolicy Build()
        {
            return Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(
                   retryCount: _retryCount,
                   sleepDurationProvider: (x) => _retrySleepDuration,
                   onRetry: (exception, sleepDuration, attemptNumber, context) =>
                   {
                       _logDelegate(attemptNumber);
                   });
        }
    }
}
