using System.Runtime.CompilerServices;

namespace Legion;

public static partial class InvocationHelper
{
	private const string _operation_timed_out = "Operation timed out";

	static readonly TimeSpan _defaultTimeout = TimeSpan.FromSeconds(5);

	public static Task WaitForTaskOrCancellationAsync(Task task, CancellationToken cancellationToken)
	{
		Throw.IfArgumentNull(task);
		Throw.IfArgumentDefault(cancellationToken);

		if (!cancellationToken.CanBeCanceled)
			return task;

		async Task WaitAsync()
		{
			using (RegisterTaskToCancellationToken(cancellationToken, out var cancelTask))
			{
				var completed = await Task.WhenAny(task, cancelTask).ConfigureAwait(false);
				if (completed != task)
				{
					IgnoreTaskUnobservedExceptions(task);

					throw new OperationCanceledException(cancellationToken);
				}

				await task;
			}
		}

		return WaitAsync();
	}

	public static Task<T> WaitForTaskOrCancellationAsync<T>(Task<T> task, CancellationToken cancellationToken)
	{
		Throw.IfArgumentNull(task);
		Throw.IfArgumentDefault(cancellationToken);

		if (!cancellationToken.CanBeCanceled)
			return task;

		async Task<T> WaitAsync()
		{
			using (RegisterTaskToCancellationToken(cancellationToken, out var cancelTask))
			{
				var completed = await Task.WhenAny(task, cancelTask).ConfigureAwait(false);
				if (completed != task)
				{
					IgnoreTaskUnobservedExceptions(task);

					throw new OperationCanceledException(cancellationToken);
				}

				return await task;
			}
		}

		return WaitAsync();
	}

	public static Task TaskWithTimeoutAsync(Task task, int ms = 0, int s = 0, int m = 0, int h = 0, int d = 0, CancellationToken cancellationToken = default,
		[CallerMemberName] string? memberName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int? lineNumber = null)
	{
		Throw.IfArgumentNull(task);

		var timeout = new TimeSpan(d, h, m, s, ms);
		if (timeout == TimeSpan.Zero)
			timeout = _defaultTimeout;

		return TaskWithTimeoutInternalAsync(task, timeout, memberName, filePath, lineNumber, cancellationToken);
	}

	public static Task TaskWithTimeoutAsync(Task task, TimeSpan timeout, CancellationToken cancellationToken = default,
		[CallerMemberName] string? memberName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int? lineNumber = null)
	{
		Throw.IfArgumentNull(task);

		return TaskWithTimeoutInternalAsync(task, timeout, memberName, filePath, lineNumber, cancellationToken);
	}

	private static Task TaskWithTimeoutInternalAsync(
		Task task,
		TimeSpan timeout,
		string? memberName,
		string? filePath,
		int? lineNumber,
		CancellationToken cancellationToken)
	{
		Throw.IfArgumentNull(task);

		if (task.IsCompleted)
			return task;

		async Task WaitAsync()
		{
			var cancel = new CancellationTokenSource();

			var registration = RegisterIfCanBeCanceled(cancel, cancellationToken);
			try
			{
				var delayTask = Task.Delay(/*Debugger.IsAttached ? Timeout.InfiniteTimeSpan :*/ timeout, cancel.Token);

				var completed = await Task.WhenAny(task, delayTask).ConfigureAwait(false);
				if (completed == delayTask)
				{
					IgnoreTaskUnobservedExceptions(task);

					throw new TimeoutException(_operation_timed_out /*FormatTimeoutMessage(memberName, filePath, lineNumber)*/);
				}

				await task;
			}
			finally
			{
#if NETSTANDARD2_0 || NETSTANDARD2_1
				registration.Dispose();
#else
				await registration.DisposeAsync();
#endif
				cancel.Cancel();
			}
		}

		return WaitAsync();
	}

	public static Task<T> TaskWithTimeoutAsync<T>(Task<T> task, int ms = 0, int s = 0, int m = 0, int h = 0, int d = 0,
		CancellationToken cancellationToken = default,
		[CallerMemberName] string? memberName = null, [CallerFilePath] string? filePath = null,
		[CallerLineNumber] int? lineNumber = null)
	{
		Throw.IfArgumentNull(task);

		var timeout = new TimeSpan(d, h, m, s, ms);
		if (timeout == TimeSpan.Zero)
			timeout = _defaultTimeout;

		return TaskWithTimeoutInternalAsync(task, timeout, memberName, filePath, lineNumber, cancellationToken);
	}

	public static Task<T> TaskWithTimeoutAsync<T>(Task<T> task, TimeSpan timeout, CancellationToken cancellationToken = default,
		[CallerMemberName] string? memberName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int? lineNumber = null)
	{
		Throw.IfArgumentNull(task);

		return TaskWithTimeoutInternalAsync(task, timeout, memberName, filePath, lineNumber, cancellationToken);
	}

	private static Task<T> TaskWithTimeoutInternalAsync<T>(Task<T> task, TimeSpan timeout, string? memberName, string? filePath,
		int? lineNumber, CancellationToken cancellationToken)
	{
		Throw.IfArgumentNull(task);

		if (task.IsCompleted)
			return task;

		async Task<T> WaitAsync()
		{
			var cancel = new CancellationTokenSource();

			var registration = RegisterIfCanBeCanceled(cancel, cancellationToken);
			try
			{
				var delayTask = Task.Delay(/*Debugger.IsAttached ? Timeout.InfiniteTimeSpan :*/ timeout, cancel.Token);

				var completed = await Task.WhenAny(task, delayTask).ConfigureAwait(false);
				if (completed == delayTask)
				{
					IgnoreTaskUnobservedExceptions(task);

					throw new TimeoutException(_operation_timed_out /*FormatTimeoutMessage(memberName, filePath, lineNumber)*/);
				}

				return await task;
			}
			finally
			{
#if NETSTANDARD2_0 || NETSTANDARD2_1
				registration.Dispose();
#else
				await registration.DisposeAsync();
#endif
				cancel.Cancel();
			}
		}

		return WaitAsync();
	}

	private static string FormatTimeoutMessage(string? memberName, string? filePath, int? lineNumber)
	{
		return !string.IsNullOrEmpty(memberName)
			? $"Operation in {memberName} timed out at {filePath}:{lineNumber}"
			: _operation_timed_out;
	}

	/// <summary>
	/// Returns true if a Task was ran to completion (without being cancelled or faulted)
	/// </summary>
	/// <param name="task"></param>
	/// <returns></returns>
	public static bool TaskIsCompletedSuccessfully(Task task)
	{
		Throw.IfArgumentNull(task);

		return task.Status == TaskStatus.RanToCompletion;
	}

	public static void IgnoreTaskUnobservedExceptions(Task task)
	{
		Throw.IfArgumentNull(task);

		if (task.IsCompleted)
		{
			if (task.IsFaulted)
			{
				var _ = task.Exception;
			}

			return;
		}

		task.ContinueWith(t =>
		{
			var _ = t.Exception;
		}, TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously);
	}

	/// <summary>
	/// Register a callback on the <paramref name="cancellationToken" /> which completes the resulting task.
	/// </summary>
	/// <param name="cancellationToken"></param>
	/// <param name="cancelTask"></param>
	/// <returns></returns>
	/// <exception cref="ArgumentException"></exception>
	public static CancellationTokenRegistration RegisterTaskToCancellationToken(CancellationToken cancellationToken, out Task cancelTask)
	{
		if (!cancellationToken.CanBeCanceled)
			throw new ArgumentException("The cancellationToken must support cancellation", nameof(cancellationToken));

		var source = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

		cancelTask = source.Task;

		return cancellationToken.Register(SetCompleted, source);
	}

	private static void SetCompleted(object? obj)
	{
		if (obj is TaskCompletionSource<bool> source)
			source.TrySetResult(true);
	}

	private static CancellationTokenRegistration RegisterIfCanBeCanceled(CancellationTokenSource source, CancellationToken cancellationToken)
	{
		Throw.IfArgumentNull(source);

		if (cancellationToken.CanBeCanceled)
			return cancellationToken.Register(Cancel, source);

		return default;
	}

	private static void Cancel(object? obj)
	{
		if (obj is CancellationTokenSource source)
			source.Cancel();
	}

#if NET8_0_OR_GREATER
	public static void FireAndForgetTask(Task task)
	{
		Throw.IfArgumentNull(task);

		if (!task.IsCompleted || task.IsFaulted)
		{
			_ = ForgetAwaited(task);
		}

		async static Task ForgetAwaited(Task task)
		{
			await task.ConfigureAwait(ConfigureAwaitOptions.SuppressThrowing);
		}
	}
#else
	public static void FireAndForgetTask(Task task)
	{
		Throw.IfArgumentNull(task);

		if (!task.IsCompleted || task.IsFaulted)
		{
			_ = ForgetAwaited(task);
		}

		async static Task ForgetAwaited(Task task)
		{
			try
			{
				await task.ConfigureAwait(false);
			}
			catch { }
		}
	}
#endif
}
