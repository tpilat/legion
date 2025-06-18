
namespace Legion.Policy;

internal class DelayRetryOptions : IRetryOptions
{
	public int? RetryCount { get; set; }

	public TimeSpan? RetryDelay { get; set; }

	public Func<object?, bool>? BreakFunction { get; set; }

	public Action<ResultBuilder>? NoBreakMatch { get; set; }

	public DelayRetryOptions(
		int? retryCount,
		TimeSpan? retryDelay,
		Func<object?, bool>? breakFunction,
		Action<ResultBuilder>? noBreakMatchDelegate)
	{
		if (retryCount.HasValue)
			Throw.IfArgumentIsLessThanOrEqual(retryCount.Value, 0);

		if (retryDelay < TimeSpan.Zero)
			retryDelay = TimeSpan.Zero;

		RetryCount = retryCount;
		RetryDelay = retryDelay;
		BreakFunction = breakFunction;
		NoBreakMatch = noBreakMatchDelegate;
	}

	public TimeSpan? RetryAfter(int retries)
	{
		if (retries == 0)
			return TimeSpan.Zero;

		if (RetryCount <= retries)
			return null;

		if (RetryDelay.HasValue)
			return RetryDelay;

		return TimeSpan.Zero;
	}
}
