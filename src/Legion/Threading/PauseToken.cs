namespace Legion.Threading;

public class PauseToken
{
	private readonly object _lock = new();
	private TaskCompletionSource<bool>? _resumeTcs;
	private bool _isPaused;

	public bool IsPaused
	{
		get
		{
			lock (_lock) return _isPaused;
		}
	}

	public void Pause()
	{
		lock (_lock)
		{
			if (_isPaused) return;
			_isPaused = true;
			_resumeTcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
		}
	}

	public void Resume()
	{
		TaskCompletionSource<bool>? tcs = null;

		lock (_lock)
		{
			if (!_isPaused) return;
			_isPaused = false;
			tcs = _resumeTcs;
			_resumeTcs = null;
		}

		tcs?.TrySetResult(true);
	}

	public async Task WaitWhilePausedAsync(CancellationToken cancellationToken)
	{
		Task? waitTask = null;

		lock (_lock)
		{
			if (!_isPaused) return;
			waitTask = _resumeTcs?.Task;
		}

		if (waitTask != null)
		{
			using (cancellationToken.Register(() =>
			{
				lock (_lock)
				{
					_resumeTcs?.TrySetCanceled(cancellationToken);
				}
			}))
			{
				await waitTask;
			}
		}
	}
}

