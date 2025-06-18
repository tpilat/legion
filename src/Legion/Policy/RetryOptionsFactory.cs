namespace Legion.Policy;

public static class RetryOptionsFactory
{
	public static IRetryOptions Create(
		Func<object?, bool>? breakFunction = null,
		Action<ResultBuilder>? noBreakMatchDelegate = null)
		=> new DelayRetryOptions(null, null, breakFunction, noBreakMatchDelegate);

	public static IRetryOptions Create(
		int retryCount,
		Func<object?, bool>? breakFunction = null,
		Action<ResultBuilder>? noBreakMatchDelegate = null)
		=> new DelayRetryOptions(retryCount, null, breakFunction, noBreakMatchDelegate);

	public static IRetryOptions Create(
		int? retryCount,
		TimeSpan retryDelay,
		Func<object?, bool>? breakFunction = null,
		Action<ResultBuilder>? noBreakMatchDelegate = null)
		=> new DelayRetryOptions(retryCount, retryDelay, breakFunction, noBreakMatchDelegate);

	public static IRetryOptions Create(
		int? retryCount,
		List<TimeSpan> retryDelayList,
		Func<object?, bool>? breakFunction = null,
		Action<ResultBuilder>? noBreakMatchDelegate = null)
		=> retryDelayList == null
			? new DelayRetryOptions(retryCount, null, breakFunction, noBreakMatchDelegate)
			: new DelayListRetryOptions(retryCount, retryDelayList, breakFunction, noBreakMatchDelegate);

	public static IRetryOptions Create(
		int? retryCount,
		Dictionary<int, TimeSpan> retryDelayTable,
		Func<object?, bool>? breakFunction = null,
		Action<ResultBuilder>? noBreakMatchDelegate = null)
		=> retryDelayTable == null
			? new DelayRetryOptions(retryCount, null, breakFunction, noBreakMatchDelegate)
			: new DelayTableRetryOptions(retryCount, retryDelayTable, breakFunction, noBreakMatchDelegate);
}

