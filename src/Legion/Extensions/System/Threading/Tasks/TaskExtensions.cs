using System.Runtime.CompilerServices;

namespace Legion.Extensions;

public static class TaskExtensions
{
	static readonly TimeSpan _defaultTimeout = TimeSpan.FromSeconds(5);

	public static Task OrCanceledAsync(this Task task, CancellationToken cancellationToken)
		=> InvocationHelper.WaitForTaskOrCancellationAsync(task, cancellationToken);

	public static Task<T> OrCanceledAsync<T>(this Task<T> task, CancellationToken cancellationToken)
		=> InvocationHelper.WaitForTaskOrCancellationAsync(task, cancellationToken);

	public static Task OrTimeoutAsync(this Task task, int ms = 0, int s = 0, int m = 0, int h = 0, int d = 0, CancellationToken cancellationToken = default,
		[CallerMemberName] string? memberName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int? lineNumber = null)
		=> InvocationHelper.TaskWithTimeoutAsync(task, ms, s, m, h, d, cancellationToken, memberName, filePath, lineNumber);

	public static Task OrTimeoutAsync(this Task task, TimeSpan timeout, CancellationToken cancellationToken = default,
		[CallerMemberName] string? memberName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int? lineNumber = null)
		=> InvocationHelper.TaskWithTimeoutAsync(task, timeout, cancellationToken, memberName, filePath, lineNumber);

	public static Task<T> OrTimeoutAsync<T>(this Task<T> task, int ms = 0, int s = 0, int m = 0, int h = 0, int d = 0,
		CancellationToken cancellationToken = default,
		[CallerMemberName] string? memberName = null, [CallerFilePath] string? filePath = null,
		[CallerLineNumber] int? lineNumber = null)
		=> InvocationHelper.TaskWithTimeoutAsync(task, ms, s, m, h, d, cancellationToken, memberName, filePath, lineNumber);

	public static Task<T> OrTimeoutAsync<T>(this Task<T> task, TimeSpan timeout, CancellationToken cancellationToken = default,
		[CallerMemberName] string? memberName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int? lineNumber = null)
		=> InvocationHelper.TaskWithTimeoutAsync(task, timeout, cancellationToken, memberName, filePath, lineNumber);

	/// <summary>
	/// Returns true if a Task was ran to completion (without being cancelled or faulted)
	/// </summary>
	/// <param name="task"></param>
	/// <returns></returns>
	public static bool IsCompletedSuccessfully(this Task task)
		=> InvocationHelper.TaskIsCompletedSuccessfully(task);

	public static void IgnoreUnobservedExceptions(this Task task)
		=> InvocationHelper.IgnoreTaskUnobservedExceptions(task);

	/// <summary>
	/// Register a callback on the <paramref name="cancellationToken" /> which completes the resulting task.
	/// </summary>
	/// <param name="cancellationToken"></param>
	/// <param name="cancelTask"></param>
	/// <returns></returns>
	/// <exception cref="ArgumentException"></exception>
	public static CancellationTokenRegistration RegisterTask(CancellationToken cancellationToken, out Task cancelTask)
		=> InvocationHelper.RegisterTaskToCancellationToken(cancellationToken, out cancelTask);

	public static void FireAndForget(this Task task)
		=> InvocationHelper.FireAndForgetTask(task);
}
