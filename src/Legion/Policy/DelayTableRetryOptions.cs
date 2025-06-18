
namespace Legion.Policy;

internal class DelayTableRetryOptions : IRetryOptions
{
	public int? RetryCount { get; set; }

	public RetryTable RetryTable { get; set; }

	public Func<object?, bool>? BreakFunction { get; set; }

	public Action<ResultBuilder>? NoBreakMatch { get; set; }

	public DelayTableRetryOptions(
		int? retryCount,
		Dictionary<int, TimeSpan> retryDelayTable,
		Func<object?, bool>? breakFunction,
		Action<ResultBuilder>? noBreakMatchDelegate)
	{
		if (retryCount.HasValue)
			Throw.IfArgumentIsLessThanOrEqual(retryCount.Value, 0);

		RetryCount = retryCount;
		RetryTable = new RetryTable(retryDelayTable);
		BreakFunction = breakFunction;
		NoBreakMatch = noBreakMatchDelegate;
	}

	public TimeSpan? RetryAfter(int retries)
	{
		if (retries == 0)
			return TimeSpan.Zero;

		if (RetryCount <= retries)
			return null;

		return RetryTable.GetRetryTimeSpan(retries);
	}
}
