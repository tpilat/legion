using Legion.Extensions;
using System.Runtime.CompilerServices;

namespace Legion;

public static partial class InvocationHelper
{
	public static void ActionWithTimeout(
		Action action,
		TimeSpan timeout,
		[CallerMemberName] string? memberName = null,
		[CallerFilePath] string? filePath = null,
		[CallerLineNumber] int? lineNumber = null)
	{
		Throw.IfArgumentNull(action);

		var task =
			Task
				.Run(() => action())
				.OrTimeoutAsync(timeout, cancellationToken: default, memberName, filePath, lineNumber);
		
		task.GetAwaiter().GetResult();
	}

	public static void ActionWithTimeout<T>(
		Action<T> action,
		T arg,
		TimeSpan timeout,
		[CallerMemberName] string? memberName = null,
		[CallerFilePath] string? filePath = null,
		[CallerLineNumber] int? lineNumber = null)
	{
		Throw.IfArgumentNull(action);

		var task =
			Task
				.Run(() => action(arg))
				.OrTimeoutAsync(timeout, cancellationToken: default, memberName, filePath, lineNumber);

		task.GetAwaiter().GetResult();
	}

	public static void ActionWithTimeout<T1, T2>(
		Action<T1, T2> action,
		T1 arg1,
		T2 arg2,
		TimeSpan timeout,
		[CallerMemberName] string? memberName = null,
		[CallerFilePath] string? filePath = null,
		[CallerLineNumber] int? lineNumber = null)
	{
		Throw.IfArgumentNull(action);

		var task =
			Task
				.Run(() => action(arg1, arg2))
				.OrTimeoutAsync(timeout, cancellationToken: default, memberName, filePath, lineNumber);

		task.GetAwaiter().GetResult();
	}

	public static void ActionWithTimeout<T1, T2, T3>(
		Action<T1, T2, T3> action,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		TimeSpan timeout,
		[CallerMemberName] string? memberName = null,
		[CallerFilePath] string? filePath = null,
		[CallerLineNumber] int? lineNumber = null)
	{
		Throw.IfArgumentNull(action);

		var task =
			Task
				.Run(() => action(arg1, arg2, arg3))
				.OrTimeoutAsync(timeout, cancellationToken: default, memberName, filePath, lineNumber);

		task.GetAwaiter().GetResult();
	}

	public static void ActionWithTimeout<T1, T2, T3, T4>(
		Action<T1, T2, T3, T4> action,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		TimeSpan timeout,
		[CallerMemberName] string? memberName = null,
		[CallerFilePath] string? filePath = null,
		[CallerLineNumber] int? lineNumber = null)
	{
		Throw.IfArgumentNull(action);

		var task =
			Task
				.Run(() => action(arg1, arg2, arg3, arg4))
				.OrTimeoutAsync(timeout, cancellationToken: default, memberName, filePath, lineNumber);

		task.GetAwaiter().GetResult();
	}

	public static void ActionWithTimeout<T1, T2, T3, T4, T5>(
		Action<T1, T2, T3, T4, T5> action,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		T5 arg5,
		TimeSpan timeout,
		[CallerMemberName] string? memberName = null,
		[CallerFilePath] string? filePath = null,
		[CallerLineNumber] int? lineNumber = null)
	{
		Throw.IfArgumentNull(action);

		var task =
			Task
				.Run(() => action(arg1, arg2, arg3, arg4, arg5))
				.OrTimeoutAsync(timeout, cancellationToken: default, memberName, filePath, lineNumber);

		task.GetAwaiter().GetResult();
	}

	public static void ActionWithTimeout<T1, T2, T3, T4, T5, T6>(
		Action<T1, T2, T3, T4, T5, T6> action,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		T5 arg5,
		T6 arg6,
		TimeSpan timeout,
		[CallerMemberName] string? memberName = null,
		[CallerFilePath] string? filePath = null,
		[CallerLineNumber] int? lineNumber = null)
	{
		Throw.IfArgumentNull(action);

		var task =
			Task
				.Run(() => action(arg1, arg2, arg3, arg4, arg5, arg6))
				.OrTimeoutAsync(timeout, cancellationToken: default, memberName, filePath, lineNumber);

		task.GetAwaiter().GetResult();
	}

	public static void ActionWithTimeout<T1, T2, T3, T4, T5, T6, T7>(
		Action<T1, T2, T3, T4, T5, T6, T7> action,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		T5 arg5,
		T6 arg6,
		T7 arg7,
		TimeSpan timeout,
		[CallerMemberName] string? memberName = null,
		[CallerFilePath] string? filePath = null,
		[CallerLineNumber] int? lineNumber = null)
	{
		Throw.IfArgumentNull(action);

		var task =
			Task
				.Run(() => action(arg1, arg2, arg3, arg4, arg5, arg6, arg7))
				.OrTimeoutAsync(timeout, cancellationToken: default, memberName, filePath, lineNumber);

		task.GetAwaiter().GetResult();
	}

	public static void ActionWithTimeout<T1, T2, T3, T4, T5, T6, T7, T8>(
		Action<T1, T2, T3, T4, T5, T6, T7, T8> action,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		T5 arg5,
		T6 arg6,
		T7 arg7,
		T8 arg8,
		TimeSpan timeout,
		[CallerMemberName] string? memberName = null,
		[CallerFilePath] string? filePath = null,
		[CallerLineNumber] int? lineNumber = null)
	{
		Throw.IfArgumentNull(action);

		var task =
			Task
				.Run(() => action(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8))
				.OrTimeoutAsync(timeout, cancellationToken: default, memberName, filePath, lineNumber);

		task.GetAwaiter().GetResult();
	}

	public static void ActionWithTimeout<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
		Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		T5 arg5,
		T6 arg6,
		T7 arg7,
		T8 arg8,
		T9 arg9,
		TimeSpan timeout,
		[CallerMemberName] string? memberName = null,
		[CallerFilePath] string? filePath = null,
		[CallerLineNumber] int? lineNumber = null)
	{
		Throw.IfArgumentNull(action);

		var task =
			Task
				.Run(() => action(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9))
				.OrTimeoutAsync(timeout, cancellationToken: default, memberName, filePath, lineNumber);

		task.GetAwaiter().GetResult();
	}

	public static void ActionWithTimeout<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
		Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		T5 arg5,
		T6 arg6,
		T7 arg7,
		T8 arg8,
		T9 arg9,
		T10 arg10,
		TimeSpan timeout,
		[CallerMemberName] string? memberName = null,
		[CallerFilePath] string? filePath = null,
		[CallerLineNumber] int? lineNumber = null)
	{
		Throw.IfArgumentNull(action);

		var task =
			Task
				.Run(() => action(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10))
				.OrTimeoutAsync(timeout, cancellationToken: default, memberName, filePath, lineNumber);

		task.GetAwaiter().GetResult();
	}

	public static void ActionWithTimeout<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
		Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		T5 arg5,
		T6 arg6,
		T7 arg7,
		T8 arg8,
		T9 arg9,
		T10 arg10,
		T11 arg11,
		TimeSpan timeout,
		[CallerMemberName] string? memberName = null,
		[CallerFilePath] string? filePath = null,
		[CallerLineNumber] int? lineNumber = null)
	{
		Throw.IfArgumentNull(action);

		var task =
			Task
				.Run(() => action(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11))
				.OrTimeoutAsync(timeout, cancellationToken: default, memberName, filePath, lineNumber);

		task.GetAwaiter().GetResult();
	}

	public static void ActionWithTimeout<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
		Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		T5 arg5,
		T6 arg6,
		T7 arg7,
		T8 arg8,
		T9 arg9,
		T10 arg10,
		T11 arg11,
		T12 arg12,
		TimeSpan timeout,
		[CallerMemberName] string? memberName = null,
		[CallerFilePath] string? filePath = null,
		[CallerLineNumber] int? lineNumber = null)
	{
		Throw.IfArgumentNull(action);

		var task =
			Task
				.Run(() => action(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12))
				.OrTimeoutAsync(timeout, cancellationToken: default, memberName, filePath, lineNumber);

		task.GetAwaiter().GetResult();
	}

	public static void ActionWithTimeout<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
		Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		T5 arg5,
		T6 arg6,
		T7 arg7,
		T8 arg8,
		T9 arg9,
		T10 arg10,
		T11 arg11,
		T12 arg12,
		T13 arg13,
		TimeSpan timeout,
		[CallerMemberName] string? memberName = null,
		[CallerFilePath] string? filePath = null,
		[CallerLineNumber] int? lineNumber = null)
	{
		Throw.IfArgumentNull(action);

		var task =
			Task
				.Run(() => action(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13))
				.OrTimeoutAsync(timeout, cancellationToken: default, memberName, filePath, lineNumber);

		task.GetAwaiter().GetResult();
	}

	public static void ActionWithTimeout<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
		Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		T5 arg5,
		T6 arg6,
		T7 arg7,
		T8 arg8,
		T9 arg9,
		T10 arg10,
		T11 arg11,
		T12 arg12,
		T13 arg13,
		T14 arg14,
		TimeSpan timeout,
		[CallerMemberName] string? memberName = null,
		[CallerFilePath] string? filePath = null,
		[CallerLineNumber] int? lineNumber = null)
	{
		Throw.IfArgumentNull(action);

		var task =
			Task
				.Run(() => action(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14))
				.OrTimeoutAsync(timeout, cancellationToken: default, memberName, filePath, lineNumber);

		task.GetAwaiter().GetResult();
	}

	public static void ActionWithTimeout<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
		Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		T5 arg5,
		T6 arg6,
		T7 arg7,
		T8 arg8,
		T9 arg9,
		T10 arg10,
		T11 arg11,
		T12 arg12,
		T13 arg13,
		T14 arg14,
		T15 arg15,
		TimeSpan timeout,
		[CallerMemberName] string? memberName = null,
		[CallerFilePath] string? filePath = null,
		[CallerLineNumber] int? lineNumber = null)
	{
		Throw.IfArgumentNull(action);

		var task =
			Task
				.Run(() => action(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15))
				.OrTimeoutAsync(timeout, cancellationToken: default, memberName, filePath, lineNumber);

		task.GetAwaiter().GetResult();
	}

	public static void ActionWithTimeout<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
		Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		T5 arg5,
		T6 arg6,
		T7 arg7,
		T8 arg8,
		T9 arg9,
		T10 arg10,
		T11 arg11,
		T12 arg12,
		T13 arg13,
		T14 arg14,
		T15 arg15,
		T16 arg16,
		TimeSpan timeout,
		[CallerMemberName] string? memberName = null,
		[CallerFilePath] string? filePath = null,
		[CallerLineNumber] int? lineNumber = null)
	{
		Throw.IfArgumentNull(action);

		var task =
			Task
				.Run(() => action(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16))
				.OrTimeoutAsync(timeout, cancellationToken: default, memberName, filePath, lineNumber);

		task.GetAwaiter().GetResult();
	}

	public static TResult FuncWithTimeout<TResult>(
		Func<TResult> func,
		TimeSpan timeout,
		[CallerMemberName] string? memberName = null,
		[CallerFilePath] string? filePath = null,
		[CallerLineNumber] int? lineNumber = null)
	{
		Throw.IfArgumentNull(func);

		var task =
			Task
				.Run(() => func())
				.OrTimeoutAsync(timeout, cancellationToken: default, memberName, filePath, lineNumber);
		
		var result = task.GetAwaiter().GetResult();
		return result;
	}

	public static TResult FuncWithTimeout<T, TResult>(
		Func<T, TResult> func,
		T arg,
		TimeSpan timeout,
		[CallerMemberName] string? memberName = null,
		[CallerFilePath] string? filePath = null,
		[CallerLineNumber] int? lineNumber = null)
	{
		Throw.IfArgumentNull(func);

		var task =
			Task
				.Run(() => func(arg))
				.OrTimeoutAsync(timeout, cancellationToken: default, memberName, filePath, lineNumber);

		var result = task.GetAwaiter().GetResult();
		return result;
	}

	public static TResult FuncWithTimeout<T1, T2, TResult>(
		Func<T1, T2, TResult> func,
		T1 arg1,
		T2 arg2,
		TimeSpan timeout,
		[CallerMemberName] string? memberName = null,
		[CallerFilePath] string? filePath = null,
		[CallerLineNumber] int? lineNumber = null)
	{
		Throw.IfArgumentNull(func);

		var task =
			Task
				.Run(() => func(arg1, arg2))
				.OrTimeoutAsync(timeout, cancellationToken: default, memberName, filePath, lineNumber);

		var result = task.GetAwaiter().GetResult();
		return result;
	}

	public static TResult FuncWithTimeout<T1, T2, T3, TResult>(
		Func<T1, T2, T3, TResult> func,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		TimeSpan timeout,
		[CallerMemberName] string? memberName = null,
		[CallerFilePath] string? filePath = null,
		[CallerLineNumber] int? lineNumber = null)
	{
		Throw.IfArgumentNull(func);

		var task =
			Task
				.Run(() => func(arg1, arg2, arg3))
				.OrTimeoutAsync(timeout, cancellationToken: default, memberName, filePath, lineNumber);

		var result = task.GetAwaiter().GetResult();
		return result;
	}

	public static TResult FuncWithTimeout<T1, T2, T3, T4, TResult>(
		Func<T1, T2, T3, T4, TResult> func,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		TimeSpan timeout,
		[CallerMemberName] string? memberName = null,
		[CallerFilePath] string? filePath = null,
		[CallerLineNumber] int? lineNumber = null)
	{
		Throw.IfArgumentNull(func);

		var task =
			Task
				.Run(() => func(arg1, arg2, arg3, arg4))
				.OrTimeoutAsync(timeout, cancellationToken: default, memberName, filePath, lineNumber);

		var result = task.GetAwaiter().GetResult();
		return result;
	}

	public static TResult FuncWithTimeout<T1, T2, T3, T4, T5, TResult>(
		Func<T1, T2, T3, T4, T5, TResult> func,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		T5 arg5,
		TimeSpan timeout,
		[CallerMemberName] string? memberName = null,
		[CallerFilePath] string? filePath = null,
		[CallerLineNumber] int? lineNumber = null)
	{
		Throw.IfArgumentNull(func);

		var task =
			Task
				.Run(() => func(arg1, arg2, arg3, arg4, arg5))
				.OrTimeoutAsync(timeout, cancellationToken: default, memberName, filePath, lineNumber);

		var result = task.GetAwaiter().GetResult();
		return result;
	}

	public static TResult FuncWithTimeout<T1, T2, T3, T4, T5, T6, TResult>(
		Func<T1, T2, T3, T4, T5, T6, TResult> func,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		T5 arg5,
		T6 arg6,
		TimeSpan timeout,
		[CallerMemberName] string? memberName = null,
		[CallerFilePath] string? filePath = null,
		[CallerLineNumber] int? lineNumber = null)
	{
		Throw.IfArgumentNull(func);

		var task =
			Task
				.Run(() => func(arg1, arg2, arg3, arg4, arg5, arg6))
				.OrTimeoutAsync(timeout, cancellationToken: default, memberName, filePath, lineNumber);

		var result = task.GetAwaiter().GetResult();
		return result;
	}

	public static TResult FuncWithTimeout<T1, T2, T3, T4, T5, T6, T7, TResult>(
		Func<T1, T2, T3, T4, T5, T6, T7, TResult> func,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		T5 arg5,
		T6 arg6,
		T7 arg7,
		TimeSpan timeout,
		[CallerMemberName] string? memberName = null,
		[CallerFilePath] string? filePath = null,
		[CallerLineNumber] int? lineNumber = null)
	{
		Throw.IfArgumentNull(func);

		var task =
			Task
				.Run(() => func(arg1, arg2, arg3, arg4, arg5, arg6, arg7))
				.OrTimeoutAsync(timeout, cancellationToken: default, memberName, filePath, lineNumber);

		var result = task.GetAwaiter().GetResult();
		return result;
	}

	public static TResult FuncWithTimeout<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(
		Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> func,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		T5 arg5,
		T6 arg6,
		T7 arg7,
		T8 arg8,
		TimeSpan timeout,
		[CallerMemberName] string? memberName = null,
		[CallerFilePath] string? filePath = null,
		[CallerLineNumber] int? lineNumber = null)
	{
		Throw.IfArgumentNull(func);

		var task =
			Task
				.Run(() => func(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8))
				.OrTimeoutAsync(timeout, cancellationToken: default, memberName, filePath, lineNumber);

		var result = task.GetAwaiter().GetResult();
		return result;
	}

	public static TResult FuncWithTimeout<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(
		Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> func,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		T5 arg5,
		T6 arg6,
		T7 arg7,
		T8 arg8,
		T9 arg9,
		TimeSpan timeout,
		[CallerMemberName] string? memberName = null,
		[CallerFilePath] string? filePath = null,
		[CallerLineNumber] int? lineNumber = null)
	{
		Throw.IfArgumentNull(func);

		var task =
			Task
				.Run(() => func(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9))
				.OrTimeoutAsync(timeout, cancellationToken: default, memberName, filePath, lineNumber);

		var result = task.GetAwaiter().GetResult();
		return result;
	}

	public static TResult FuncWithTimeout<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(
		Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> func,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		T5 arg5,
		T6 arg6,
		T7 arg7,
		T8 arg8,
		T9 arg9,
		T10 arg10,
		TimeSpan timeout,
		[CallerMemberName] string? memberName = null,
		[CallerFilePath] string? filePath = null,
		[CallerLineNumber] int? lineNumber = null)
	{
		Throw.IfArgumentNull(func);

		var task =
			Task
				.Run(() => func(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10))
				.OrTimeoutAsync(timeout, cancellationToken: default, memberName, filePath, lineNumber);

		var result = task.GetAwaiter().GetResult();
		return result;
	}

	public static TResult FuncWithTimeout<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(
		Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> func,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		T5 arg5,
		T6 arg6,
		T7 arg7,
		T8 arg8,
		T9 arg9,
		T10 arg10,
		T11 arg11,
		TimeSpan timeout,
		[CallerMemberName] string? memberName = null,
		[CallerFilePath] string? filePath = null,
		[CallerLineNumber] int? lineNumber = null)
	{
		Throw.IfArgumentNull(func);

		var task =
			Task
				.Run(() => func(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11))
				.OrTimeoutAsync(timeout, cancellationToken: default, memberName, filePath, lineNumber);

		var result = task.GetAwaiter().GetResult();
		return result;
	}

	public static TResult FuncWithTimeout<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(
		Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> func,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		T5 arg5,
		T6 arg6,
		T7 arg7,
		T8 arg8,
		T9 arg9,
		T10 arg10,
		T11 arg11,
		T12 arg12,
		TimeSpan timeout,
		[CallerMemberName] string? memberName = null,
		[CallerFilePath] string? filePath = null,
		[CallerLineNumber] int? lineNumber = null)
	{
		Throw.IfArgumentNull(func);

		var task =
			Task
				.Run(() => func(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12))
				.OrTimeoutAsync(timeout, cancellationToken: default, memberName, filePath, lineNumber);

		var result = task.GetAwaiter().GetResult();
		return result;
	}

	public static TResult FuncWithTimeout<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(
		Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> func,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		T5 arg5,
		T6 arg6,
		T7 arg7,
		T8 arg8,
		T9 arg9,
		T10 arg10,
		T11 arg11,
		T12 arg12,
		T13 arg13,
		TimeSpan timeout,
		[CallerMemberName] string? memberName = null,
		[CallerFilePath] string? filePath = null,
		[CallerLineNumber] int? lineNumber = null)
	{
		Throw.IfArgumentNull(func);

		var task =
			Task
				.Run(() => func(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13))
				.OrTimeoutAsync(timeout, cancellationToken: default, memberName, filePath, lineNumber);

		var result = task.GetAwaiter().GetResult();
		return result;
	}

	public static TResult FuncWithTimeout<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(
		Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> func,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		T5 arg5,
		T6 arg6,
		T7 arg7,
		T8 arg8,
		T9 arg9,
		T10 arg10,
		T11 arg11,
		T12 arg12,
		T13 arg13,
		T14 arg14,
		TimeSpan timeout,
		[CallerMemberName] string? memberName = null,
		[CallerFilePath] string? filePath = null,
		[CallerLineNumber] int? lineNumber = null)
	{
		Throw.IfArgumentNull(func);

		var task =
			Task
				.Run(() => func(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14))
				.OrTimeoutAsync(timeout, cancellationToken: default, memberName, filePath, lineNumber);

		var result = task.GetAwaiter().GetResult();
		return result;
	}

	public static TResult FuncWithTimeout<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(
		Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> func,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		T5 arg5,
		T6 arg6,
		T7 arg7,
		T8 arg8,
		T9 arg9,
		T10 arg10,
		T11 arg11,
		T12 arg12,
		T13 arg13,
		T14 arg14,
		T15 arg15,
		TimeSpan timeout,
		[CallerMemberName] string? memberName = null,
		[CallerFilePath] string? filePath = null,
		[CallerLineNumber] int? lineNumber = null)
	{
		Throw.IfArgumentNull(func);

		var task =
			Task
				.Run(() => func(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15))
				.OrTimeoutAsync(timeout, cancellationToken: default, memberName, filePath, lineNumber);

		var result = task.GetAwaiter().GetResult();
		return result;
	}

	public static TResult FuncWithTimeout<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(
		Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> func,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		T5 arg5,
		T6 arg6,
		T7 arg7,
		T8 arg8,
		T9 arg9,
		T10 arg10,
		T11 arg11,
		T12 arg12,
		T13 arg13,
		T14 arg14,
		T15 arg15,
		T16 arg16,
		TimeSpan timeout,
		[CallerMemberName] string? memberName = null,
		[CallerFilePath] string? filePath = null,
		[CallerLineNumber] int? lineNumber = null)
	{
		Throw.IfArgumentNull(func);

		var task =
			Task
				.Run(() => func(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16))
				.OrTimeoutAsync(timeout, cancellationToken: default, memberName, filePath, lineNumber);

		var result = task.GetAwaiter().GetResult();
		return result;
	}

	public static void FireAndForget(Action action)
	{
		Task.Run(() => action)
			.FireAndForget();
	}

	public static void FireAndForget<T>(
		Action<T> action,
		T arg)
	{
		Task.Run(() => action(arg))
			.FireAndForget();
	}

	public static void FireAndForget<T1, T2>(
		Action<T1, T2> action,
		T1 arg1,
		T2 arg2)
	{
		Task.Run(() => action(arg1, arg2))
			.FireAndForget();
	}

	public static void FireAndForget<T1, T2, T3>(
		Action<T1, T2, T3> action,
		T1 arg1,
		T2 arg2,
		T3 arg3)
	{
		Task.Run(() => action(arg1, arg2, arg3))
			.FireAndForget();
	}

	public static void FireAndForget<T1, T2, T3, T4>(
		Action<T1, T2, T3, T4> action,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4)
	{
		Task.Run(() => action(arg1, arg2, arg3, arg4))
			.FireAndForget();
	}

	public static void FireAndForget<T1, T2, T3, T4, T5>(
		Action<T1, T2, T3, T4, T5> action,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		T5 arg5)
	{
		Task.Run(() => action(arg1, arg2, arg3, arg4, arg5))
			.FireAndForget();
	}

	public static void FireAndForget<T1, T2, T3, T4, T5, T6>(
		Action<T1, T2, T3, T4, T5, T6> action,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		T5 arg5,
		T6 arg6)
	{
		Task.Run(() => action(arg1, arg2, arg3, arg4, arg5, arg6))
			.FireAndForget();
	}

	public static void FireAndForget<T1, T2, T3, T4, T5, T6, T7>(
		Action<T1, T2, T3, T4, T5, T6, T7> action,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		T5 arg5,
		T6 arg6,
		T7 arg7)
	{
		Task.Run(() => action(arg1, arg2, arg3, arg4, arg5, arg6, arg7))
			.FireAndForget();
	}

	public static void FireAndForget<T1, T2, T3, T4, T5, T6, T7, T8>(
		Action<T1, T2, T3, T4, T5, T6, T7, T8> action,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		T5 arg5,
		T6 arg6,
		T7 arg7,
		T8 arg8)
	{
		Task.Run(() => action(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8))
			.FireAndForget();
	}

	public static void FireAndForget<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
		Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		T5 arg5,
		T6 arg6,
		T7 arg7,
		T8 arg8,
		T9 arg9)
	{
		Task.Run(() => action(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9))
			.FireAndForget();
	}

	public static void FireAndForget<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
		Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		T5 arg5,
		T6 arg6,
		T7 arg7,
		T8 arg8,
		T9 arg9,
		T10 arg10)
	{
		Task.Run(() => action(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10))
			.FireAndForget();
	}

	public static void FireAndForget<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
		Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		T5 arg5,
		T6 arg6,
		T7 arg7,
		T8 arg8,
		T9 arg9,
		T10 arg10,
		T11 arg11)
	{
		Task.Run(() => action(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11))
			.FireAndForget();
	}

	public static void FireAndForget<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
		Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		T5 arg5,
		T6 arg6,
		T7 arg7,
		T8 arg8,
		T9 arg9,
		T10 arg10,
		T11 arg11,
		T12 arg12)
	{
		Task.Run(() => action(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12))
			.FireAndForget();
	}

	public static void FireAndForget<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
		Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		T5 arg5,
		T6 arg6,
		T7 arg7,
		T8 arg8,
		T9 arg9,
		T10 arg10,
		T11 arg11,
		T12 arg12,
		T13 arg13)
	{
		Task.Run(() => action(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13))
			.FireAndForget();
	}

	public static void FireAndForget<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
		Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		T5 arg5,
		T6 arg6,
		T7 arg7,
		T8 arg8,
		T9 arg9,
		T10 arg10,
		T11 arg11,
		T12 arg12,
		T13 arg13,
		T14 arg14)
	{
		Task.Run(() => action(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14))
			.FireAndForget();
	}

	public static void FireAndForget<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
		Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		T5 arg5,
		T6 arg6,
		T7 arg7,
		T8 arg8,
		T9 arg9,
		T10 arg10,
		T11 arg11,
		T12 arg12,
		T13 arg13,
		T14 arg14,
		T15 arg15)
	{
		Task.Run(() => action(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15))
			.FireAndForget();
	}

	public static void FireAndForget<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
		Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		T5 arg5,
		T6 arg6,
		T7 arg7,
		T8 arg8,
		T9 arg9,
		T10 arg10,
		T11 arg11,
		T12 arg12,
		T13 arg13,
		T14 arg14,
		T15 arg15,
		T16 arg16)
	{
		Task.Run(() => action(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16))
			.FireAndForget();
	}

	public static void FireAndForget<TResult>(
		Func<TResult> func)
	{
		Task.Run(() => func())
			.FireAndForget();
	}

	public static void FireAndForget<T, TResult>(
		Func<T, TResult> func,
		T arg)
	{
		Task.Run(() => func(arg))
			.FireAndForget();
	}

	public static void FireAndForget<T1, T2, TResult>(
		Func<T1, T2, TResult> func,
		T1 arg1,
		T2 arg2)
	{
		Task.Run(() => func(arg1, arg2))
			.FireAndForget();
	}

	public static void FireAndForget<T1, T2, T3, TResult>(
		Func<T1, T2, T3, TResult> func,
		T1 arg1,
		T2 arg2,
		T3 arg3)
	{
		Task.Run(() => func(arg1, arg2, arg3))
			.FireAndForget();
	}

	public static void FireAndForget<T1, T2, T3, T4, TResult>(
		Func<T1, T2, T3, T4, TResult> func,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4)
	{
		Task.Run(() => func(arg1, arg2, arg3, arg4))
			.FireAndForget();
	}

	public static void FireAndForget<T1, T2, T3, T4, T5, TResult>(
		Func<T1, T2, T3, T4, T5, TResult> func,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		T5 arg5)
	{
		Task.Run(() => func(arg1, arg2, arg3, arg4, arg5))
			.FireAndForget();
	}

	public static void FireAndForget<T1, T2, T3, T4, T5, T6, TResult>(
		Func<T1, T2, T3, T4, T5, T6, TResult> func,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		T5 arg5,
		T6 arg6)
	{
		Task.Run(() => func(arg1, arg2, arg3, arg4, arg5, arg6))
			.FireAndForget();
	}

	public static void FireAndForget<T1, T2, T3, T4, T5, T6, T7, TResult>(
		Func<T1, T2, T3, T4, T5, T6, T7, TResult> func,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		T5 arg5,
		T6 arg6,
		T7 arg7)
	{
		Task.Run(() => func(arg1, arg2, arg3, arg4, arg5, arg6, arg7))
			.FireAndForget();
	}

	public static void FireAndForget<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(
		Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> func,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		T5 arg5,
		T6 arg6,
		T7 arg7,
		T8 arg8)
	{
		Task.Run(() => func(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8))
			.FireAndForget();
	}

	public static void FireAndForget<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(
		Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> func,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		T5 arg5,
		T6 arg6,
		T7 arg7,
		T8 arg8,
		T9 arg9)
	{
		Task.Run(() => func(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9))
			.FireAndForget();
	}

	public static void FireAndForget<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(
		Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> func,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		T5 arg5,
		T6 arg6,
		T7 arg7,
		T8 arg8,
		T9 arg9,
		T10 arg10)
	{
		Task.Run(() => func(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10))
			.FireAndForget();
	}

	public static void FireAndForget<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(
		Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> func,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		T5 arg5,
		T6 arg6,
		T7 arg7,
		T8 arg8,
		T9 arg9,
		T10 arg10,
		T11 arg11)
	{
		Task.Run(() => func(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11))
			.FireAndForget();
	}

	public static void FireAndForget<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(
		Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> func,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		T5 arg5,
		T6 arg6,
		T7 arg7,
		T8 arg8,
		T9 arg9,
		T10 arg10,
		T11 arg11,
		T12 arg12)
	{
		Task.Run(() => func(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12))
			.FireAndForget();
	}

	public static void FireAndForget<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(
		Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> func,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		T5 arg5,
		T6 arg6,
		T7 arg7,
		T8 arg8,
		T9 arg9,
		T10 arg10,
		T11 arg11,
		T12 arg12,
		T13 arg13)
	{
		Task.Run(() => func(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13))
			.FireAndForget();
	}

	public static void FireAndForget<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(
		Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> func,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		T5 arg5,
		T6 arg6,
		T7 arg7,
		T8 arg8,
		T9 arg9,
		T10 arg10,
		T11 arg11,
		T12 arg12,
		T13 arg13,
		T14 arg14)
	{
		Task.Run(() => func(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14))
			.FireAndForget();
	}

	public static void FireAndForget<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(
		Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> func,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		T5 arg5,
		T6 arg6,
		T7 arg7,
		T8 arg8,
		T9 arg9,
		T10 arg10,
		T11 arg11,
		T12 arg12,
		T13 arg13,
		T14 arg14,
		T15 arg15)
	{
		Task.Run(() => func(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15))
			.FireAndForget();
	}

	public static void FireAndForget<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(
		Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> func,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		T5 arg5,
		T6 arg6,
		T7 arg7,
		T8 arg8,
		T9 arg9,
		T10 arg10,
		T11 arg11,
		T12 arg12,
		T13 arg13,
		T14 arg14,
		T15 arg15,
		T16 arg16)
	{
		Task.Run(() => func(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16))
			.FireAndForget();
	}
}
