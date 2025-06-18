namespace Legion.Policy;

public class RetryState
{
	public IRetryOptions RetryOptions { get; }
	public int PreviousRetryCount { get; private set; }

	public RetryState(IRetryOptions retryOptions)
	{
		Throw.IfArgumentNull(retryOptions);

		PreviousRetryCount = -1;
		RetryOptions = retryOptions;
	}

	public bool CallBreak(object? obj)
		=> RetryOptions.BreakFunction?.Invoke(obj) ?? true;

	public void CallNoBreakMatch(IResult retryResult)
	{
		if (RetryOptions.NoBreakMatch != null
			&& RetryOptions.BreakFunction != null
			&& !RetryOptions.RetryAfter(PreviousRetryCount + 1).HasValue)
		{
			var builder = new ResultBuilder(retryResult);
			RetryOptions.NoBreakMatch.Invoke(builder);
		}
	}

	public bool RetryWithDelay()
	{
		PreviousRetryCount++;
		if (PreviousRetryCount == 0)
			return true;

		var delay = RetryOptions.RetryAfter(PreviousRetryCount);
		if (!delay.HasValue)
			return false;

		if (delay != TimeSpan.Zero)
			Thread.Sleep(delay.Value);

		return true;
	}

	public async Task<bool> RetryWithDelayAsync()
	{
		PreviousRetryCount++;
		if (PreviousRetryCount == 0)
			return true;

		var delay = RetryOptions.RetryAfter(PreviousRetryCount);
		if (!delay.HasValue)
			return false;

		if (delay != TimeSpan.Zero)
			await Task.Delay(delay.Value);

		return true;
	}
}
