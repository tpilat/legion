namespace Legion.Policy;

public interface IRetryOptions
{
	int? RetryCount { get; }

	//TimeSpan? RetryTimeout { get; }

	TimeSpan? RetryAfter(int retries);

	Func<object?, bool>? BreakFunction { get; }

	Action<ResultBuilder>? NoBreakMatch { get; }
}
