namespace Legion.Razor.UI;

internal class Debouncer
{
	private System.Timers.Timer? timer;
	DateTime TimerStarted { get; set; } = GlobalContext.Instance.UtcNow.AddYears(-1);

	public void Debounce(int interval, Func<Task> action)
	{
		timer?.Stop();
		timer = null;

		timer = new System.Timers.Timer() { Interval = interval, Enabled = false, AutoReset = false };
		timer.Elapsed += (s, e) =>
		{
			if (timer == null)
			{
				return;
			}

			timer?.Stop();
			timer = null;

			try
			{
				Task.Run(action);
			}
			catch (TaskCanceledException)
			{
				//
			}
		};

		timer.Start();
	}

	public void Throttle(int interval, Func<Task> action)
	{
		timer?.Stop();
		timer = null;

		var curTime = GlobalContext.Instance.UtcNow;

		if (curTime.Subtract(TimerStarted).TotalMilliseconds < interval)
		{
			interval -= (int)curTime.Subtract(TimerStarted).TotalMilliseconds;
		}

		timer = new System.Timers.Timer() { Interval = interval, Enabled = false, AutoReset = false };
		timer.Elapsed += (s, e) =>
		{
			if (timer == null)
			{
				return;
			}

			timer?.Stop();
			timer = null;

			try
			{
				Task.Run(action);
			}
			catch (TaskCanceledException)
			{
				//
			}
		};

		timer.Start();
		TimerStarted = curTime;
	}
}

