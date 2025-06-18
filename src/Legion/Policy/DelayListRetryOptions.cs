
namespace Legion.Policy;

internal class DelayListRetryOptions : IRetryOptions
{
	private readonly TimeSpan _last;

	public int? RetryCount { get; set; }

	public List<TimeSpan> RetryDelayList { get; set; }

	public Func<object?, bool>? BreakFunction { get; set; }

	public Action<ResultBuilder>? NoBreakMatch { get; set; }

	public DelayListRetryOptions(
		int? retryCount,
		List<TimeSpan> retryDelayList,
		Func<object?, bool>? breakFunction,
		Action<ResultBuilder>? noBreakMatchDelegate)
	{
		if (retryCount.HasValue)
			Throw.IfArgumentIsLessThanOrEqual(retryCount.Value, 0);

		Throw.IfArgumentNullOrEmpty(retryDelayList);

		RetryCount = retryCount;
		RetryDelayList = retryDelayList;
		_last = RetryDelayList[RetryDelayList.Count - 1];
		BreakFunction = breakFunction;
		NoBreakMatch = noBreakMatchDelegate;
	}

	public TimeSpan? RetryAfter(int retries)
	{
		if (retries == 0)
			return TimeSpan.Zero;

		if (RetryCount <= retries)
			return null;

		if (retries < RetryDelayList.Count)
			return RetryDelayList[retries];

		return _last;
	}
}
