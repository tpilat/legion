namespace Legion.Tasks;

public static class TaskHelper
{
	public static async Task<TResult> Retry<TResult>(
		this Func<Task<TResult>> taskMethod,
		Func<TResult, bool> resultOK,
		int retries)
	{
		while (true)
		{
			var result = await taskMethod();
			if (retries-- == 0 || resultOK(result))
				return result;
		}
	}

	internal static async Task Test(string s, TimeSpan ts, Guid g)
	{
		await TaskHelper.Retry<int>(
			() => MyMethod(s, ts, g),
			r => false,
			100);
	}

	internal static async Task<int> MyMethod(string s, TimeSpan ts, Guid g)
	{
		await Task.Delay(ts);
		return 5;
	}
}
