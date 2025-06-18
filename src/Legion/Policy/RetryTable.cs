using Legion.Extensions;

namespace Legion.Policy;

public class RetryTable : IRetryTable
{
	private List<int> _orderedRetries;

	public Dictionary<int, TimeSpan> IterationRetryTable { get; set; } //Dictionary<MaxRetryCount, TimeSpan>
	IReadOnlyDictionary<int, TimeSpan> IRetryTable.IterationRetryTable => IterationRetryTable;

	public RetryTable(Dictionary<int, TimeSpan> delayTable)
	{
		Throw.IfArgumentNullOrEmpty(delayTable);

		IterationRetryTable = new Dictionary<int, TimeSpan>(delayTable);
		_orderedRetries = IterationRetryTable.Keys.OrderBy(x => x).ToList();
	}

	public RetryTable Add(int iterationCount, TimeSpan delay, bool force = true)
	{
		Throw.IfArgumentIsGreaterThanOrEqual(iterationCount, 0);

		if (force)
		{
			IterationRetryTable[iterationCount] = delay;
			_orderedRetries = IterationRetryTable.Keys.OrderBy(x => x).ToList();
			return this;
		}
		else
		{
			var result = IterationRetryTable.TryAdd(iterationCount, delay);
			_orderedRetries = IterationRetryTable.Keys.OrderBy(x => x).ToList();
			return this;
		}
	}

	public TimeSpan GetRetryTimeSpan(int currentRetryCount)
	{
		var orderedRetries = _orderedRetries;
		var key = orderedRetries[0];
		for (int i = 1; i < orderedRetries.Count; i++)
		{
			var retry = orderedRetries[i];
			if (retry <= currentRetryCount)
				key = retry;
			else
				break;
		}

		return IterationRetryTable[key];
	}
}
