using Legion.Logging;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Legion;

public partial class Result : IResult
{
	public static IResult Call<TInvocationContext>(
		Action<TInvocationContext> @delegate,
		TInvocationContext invocationContext,
		[CallerArgumentExpression(nameof(@delegate))] string? delegateName = null)
		where TInvocationContext : IInvocationContext
	{
		var result = new ResultBuilder();

		var delegateWasCalled = false;
		var isUnhandledException = false;
		Stopwatch? stopwatch = null;
		IInvocationContext iinvocationContext = invocationContext;
		InvocationContext? disposableInvocationContext = null;

		try
		{
			Throw.IfNull(iinvocationContext);

			if (iinvocationContext.InvocationResultSyncCallback != null)
				stopwatch = Stopwatch.StartNew();

			if (result.IsArgumentNull(iinvocationContext.InvocationCreateNew(), @delegate))
			{
				var logResult = iinvocationContext.LogResultErrorMessages(result.Build());
				result.MergeErrors(logResult);

				return result.Build();
			}

			delegateWasCalled = true;

			@delegate(invocationContext);

			return result.Build();
		}
		catch (Exception ex)
		{
			isUnhandledException = true;
			stopwatch?.Stop();

			if (iinvocationContext == null)
			{
				disposableInvocationContext = new InvocationContextBuilder(ScopeContext.Create($"{nameof(Legion)} > {nameof(Result)}")).Build();
				iinvocationContext = disposableInvocationContext;
			}

			result.WithError(
				iinvocationContext.InvocationCreateNew(),
				iinvocationContext.UnhandledErrorCode ?? Exceptions.Internal.ErrorCodes.ResultException.Unhandled,
				x => x.ExceptionInfo(ex));

			var res = result.Build();

			var logResult = iinvocationContext.LogResultErrorMessages(res);
			result.MergeErrors(logResult);

			if (iinvocationContext.InvocationResultSyncCallback != null)
			{
				try
				{
					iinvocationContext.InvocationResultSyncCallback(new InvocationResult
					{
						InvocationContext = iinvocationContext.InvocationCreateNew(),
						DelegateWasCalled = delegateWasCalled,
						Result = result.Build(),
						IsUnhandledException = isUnhandledException,
						ElapsedMilliseconds = stopwatch?.ElapsedMilliseconds ?? -1,
						DelegateMethodName = string.IsNullOrWhiteSpace(delegateName)
							? iinvocationContext.TraceFrameStack.LastFrame ?? new TraceFrame().Frame
							: delegateName!,
					});
				}
				catch (Exception profilerEx)
				{
					var error = new ErrorMessageBuilder(
						iinvocationContext.InvocationCreateNew(),
						Exceptions.Internal.ErrorCodes.ResultException.InvocationResult)
						.ExceptionInfo(profilerEx)
						.Build();

					result.WithError(error);

					try
					{
						iinvocationContext.Logger?.LogErrorMessage(error);
					}
					catch (Exception loggerEx)
					{
						result.WithError(
							iinvocationContext.InvocationCreateNew(),
							Exceptions.Internal.ErrorCodes.ResultException.InvocationResult,
							x => x.ExceptionInfo(loggerEx));
					}
				}
			}

			return res;
		}
		finally
		{
			if (stopwatch != null && !isUnhandledException)
			{
				stopwatch.Stop();
				if (iinvocationContext.InvocationResultSyncCallback != null)
				{
					try
					{
						iinvocationContext.InvocationResultSyncCallback(new InvocationResult
						{
							InvocationContext = iinvocationContext.InvocationCreateNew(),
							DelegateWasCalled = delegateWasCalled,
							Result = result.Build(),
							IsUnhandledException = isUnhandledException,
							ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
							DelegateMethodName = string.IsNullOrWhiteSpace(delegateName)
								? iinvocationContext.TraceFrameStack.LastFrame ?? new TraceFrame().Frame
								: delegateName!,
						});
					}
					catch (Exception profilerEx)
					{
						var error = new ErrorMessageBuilder(
							iinvocationContext.InvocationCreateNew(),
							Exceptions.Internal.ErrorCodes.ResultException.InvocationResult)
							.ExceptionInfo(profilerEx)
							.Build();

						result.WithError(error);

						try
						{
							iinvocationContext.Logger?.LogErrorMessage(error);
						}
						catch (Exception loggerEx)
						{
							result.WithError(
								iinvocationContext.InvocationCreateNew(),
								Exceptions.Internal.ErrorCodes.ResultException.InvocationResult,
								x => x.ExceptionInfo(loggerEx));
						}
					}
				}
			}

			//disposableInvocationContext?.Dispose();
		}
	}

	public static IResult Call<TInvocationContext, T>(
		Action<TInvocationContext, T> @delegate,
		TInvocationContext invocationContext,
		T arg,
		[CallerArgumentExpression(nameof(@delegate))] string? delegateName = null)
		where TInvocationContext : IInvocationContext
	{
		var result = new ResultBuilder();

		var delegateWasCalled = false;
		var isUnhandledException = false;
		Stopwatch? stopwatch = null;
		IInvocationContext iinvocationContext = invocationContext;
		InvocationContext? disposableInvocationContext = null;

		try
		{
			Throw.IfNull(iinvocationContext);

			if (iinvocationContext.InvocationResultSyncCallback != null)
				stopwatch = Stopwatch.StartNew();

			if (result.IsArgumentNull(iinvocationContext.InvocationCreateNew(), @delegate))
			{
				var logResult = iinvocationContext.LogResultErrorMessages(result.Build());
				result.MergeErrors(logResult);

				return result.Build();
			}

			delegateWasCalled = true;

			@delegate(invocationContext, arg);

			return result.Build();
		}
		catch (Exception ex)
		{
			isUnhandledException = true;
			stopwatch?.Stop();

			if (iinvocationContext == null)
			{
				disposableInvocationContext = new InvocationContextBuilder(ScopeContext.Create($"{nameof(Legion)} > {nameof(Result)}")).Build();
				iinvocationContext = disposableInvocationContext;
			}

			result.WithError(
				iinvocationContext.InvocationCreateNew(),
				iinvocationContext.UnhandledErrorCode ?? Exceptions.Internal.ErrorCodes.ResultException.Unhandled,
				x => x.ExceptionInfo(ex));

			var res = result.Build();

			var logResult = iinvocationContext.LogResultErrorMessages(res);
			result.MergeErrors(logResult);

			if (iinvocationContext.InvocationResultSyncCallback != null)
			{
				try
				{
					iinvocationContext.InvocationResultSyncCallback(new InvocationResult
					{
						InvocationContext = iinvocationContext.InvocationCreateNew(),
						DelegateWasCalled = delegateWasCalled,
						Result = result.Build(),
						IsUnhandledException = isUnhandledException,
						ElapsedMilliseconds = stopwatch?.ElapsedMilliseconds ?? -1,
						DelegateMethodName = string.IsNullOrWhiteSpace(delegateName)
							? iinvocationContext.TraceFrameStack.LastFrame ?? new TraceFrame().Frame
							: delegateName!,
					});
				}
				catch (Exception profilerEx)
				{
					var error = new ErrorMessageBuilder(
						iinvocationContext.InvocationCreateNew(),
						Exceptions.Internal.ErrorCodes.ResultException.InvocationResult)
						.ExceptionInfo(profilerEx)
						.Build();

					result.WithError(error);

					try
					{
						iinvocationContext.Logger?.LogErrorMessage(error);
					}
					catch (Exception loggerEx)
					{
						result.WithError(
							iinvocationContext.InvocationCreateNew(),
							Exceptions.Internal.ErrorCodes.ResultException.InvocationResult,
							x => x.ExceptionInfo(loggerEx));
					}
				}
			}

			return res;
		}
		finally
		{
			if (stopwatch != null && !isUnhandledException)
			{
				stopwatch.Stop();
				if (iinvocationContext.InvocationResultSyncCallback != null)
				{
					try
					{
						iinvocationContext.InvocationResultSyncCallback(new InvocationResult
						{
							InvocationContext = iinvocationContext.InvocationCreateNew(),
							DelegateWasCalled = delegateWasCalled,
							Result = result.Build(),
							IsUnhandledException = isUnhandledException,
							ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
							DelegateMethodName = string.IsNullOrWhiteSpace(delegateName)
								? iinvocationContext.TraceFrameStack.LastFrame ?? new TraceFrame().Frame
								: delegateName!,
						});
					}
					catch (Exception profilerEx)
					{
						var error = new ErrorMessageBuilder(
							iinvocationContext.InvocationCreateNew(),
							Exceptions.Internal.ErrorCodes.ResultException.InvocationResult)
							.ExceptionInfo(profilerEx)
							.Build();

						result.WithError(error);

						try
						{
							iinvocationContext.Logger?.LogErrorMessage(error);
						}
						catch (Exception loggerEx)
						{
							result.WithError(
								iinvocationContext.InvocationCreateNew(),
								Exceptions.Internal.ErrorCodes.ResultException.InvocationResult,
								x => x.ExceptionInfo(loggerEx));
						}
					}
				}
			}

			//disposableInvocationContext?.Dispose();
		}
	}

	public static IResult Call<TInvocationContext, T1, T2>(
		Action<TInvocationContext, T1, T2> @delegate,
		TInvocationContext invocationContext,
		T1 arg1,
		T2 arg2,
		[CallerArgumentExpression(nameof(@delegate))] string? delegateName = null)
		where TInvocationContext : IInvocationContext
	{
		var result = new ResultBuilder();

		var delegateWasCalled = false;
		var isUnhandledException = false;
		Stopwatch? stopwatch = null;
		IInvocationContext iinvocationContext = invocationContext;
		InvocationContext? disposableInvocationContext = null;

		try
		{
			Throw.IfNull(iinvocationContext);

			if (iinvocationContext.InvocationResultSyncCallback != null)
				stopwatch = Stopwatch.StartNew();

			if (result.IsArgumentNull(iinvocationContext.InvocationCreateNew(), @delegate))
			{
				var logResult = iinvocationContext.LogResultErrorMessages(result.Build());
				result.MergeErrors(logResult);

				return result.Build();
			}

			delegateWasCalled = true;

			@delegate(invocationContext, arg1, arg2);

			return result.Build();
		}
		catch (Exception ex)
		{
			isUnhandledException = true;
			stopwatch?.Stop();

			if (iinvocationContext == null)
			{
				disposableInvocationContext = new InvocationContextBuilder(ScopeContext.Create($"{nameof(Legion)} > {nameof(Result)}")).Build();
				iinvocationContext = disposableInvocationContext;
			}

			result.WithError(
				iinvocationContext.InvocationCreateNew(),
				iinvocationContext.UnhandledErrorCode ?? Exceptions.Internal.ErrorCodes.ResultException.Unhandled,
				x => x.ExceptionInfo(ex));

			var res = result.Build();

			var logResult = iinvocationContext.LogResultErrorMessages(res);
			result.MergeErrors(logResult);

			if (iinvocationContext.InvocationResultSyncCallback != null)
			{
				try
				{
					iinvocationContext.InvocationResultSyncCallback(new InvocationResult
					{
						InvocationContext = iinvocationContext.InvocationCreateNew(),
						DelegateWasCalled = delegateWasCalled,
						Result = result.Build(),
						IsUnhandledException = isUnhandledException,
						ElapsedMilliseconds = stopwatch?.ElapsedMilliseconds ?? -1,
						DelegateMethodName = string.IsNullOrWhiteSpace(delegateName)
							? iinvocationContext.TraceFrameStack.LastFrame ?? new TraceFrame().Frame
							: delegateName!,
					});
				}
				catch (Exception profilerEx)
				{
					var error = new ErrorMessageBuilder(
						iinvocationContext.InvocationCreateNew(),
						Exceptions.Internal.ErrorCodes.ResultException.InvocationResult)
						.ExceptionInfo(profilerEx)
						.Build();

					result.WithError(error);

					try
					{
						iinvocationContext.Logger?.LogErrorMessage(error);
					}
					catch (Exception loggerEx)
					{
						result.WithError(
							iinvocationContext.InvocationCreateNew(),
							Exceptions.Internal.ErrorCodes.ResultException.InvocationResult,
							x => x.ExceptionInfo(loggerEx));
					}
				}
			}

			return res;
		}
		finally
		{
			if (stopwatch != null && !isUnhandledException)
			{
				stopwatch.Stop();
				if (iinvocationContext.InvocationResultSyncCallback != null)
				{
					try
					{
						iinvocationContext.InvocationResultSyncCallback(new InvocationResult
						{
							InvocationContext = iinvocationContext.InvocationCreateNew(),
							DelegateWasCalled = delegateWasCalled,
							Result = result.Build(),
							IsUnhandledException = isUnhandledException,
							ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
							DelegateMethodName = string.IsNullOrWhiteSpace(delegateName)
								? iinvocationContext.TraceFrameStack.LastFrame ?? new TraceFrame().Frame
								: delegateName!,
						});
					}
					catch (Exception profilerEx)
					{
						var error = new ErrorMessageBuilder(
							iinvocationContext.InvocationCreateNew(),
							Exceptions.Internal.ErrorCodes.ResultException.InvocationResult)
							.ExceptionInfo(profilerEx)
							.Build();

						result.WithError(error);

						try
						{
							iinvocationContext.Logger?.LogErrorMessage(error);
						}
						catch (Exception loggerEx)
						{
							result.WithError(
								iinvocationContext.InvocationCreateNew(),
								Exceptions.Internal.ErrorCodes.ResultException.InvocationResult,
								x => x.ExceptionInfo(loggerEx));
						}
					}
				}
			}

			//disposableInvocationContext?.Dispose();
		}
	}

	public static IResult Call<TInvocationContext, T1, T2, T3>(
		Action<TInvocationContext, T1, T2, T3> @delegate,
		TInvocationContext invocationContext,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		[CallerArgumentExpression(nameof(@delegate))] string? delegateName = null)
		where TInvocationContext : IInvocationContext
	{
		var result = new ResultBuilder();

		var delegateWasCalled = false;
		var isUnhandledException = false;
		Stopwatch? stopwatch = null;
		IInvocationContext iinvocationContext = invocationContext;
		InvocationContext? disposableInvocationContext = null;

		try
		{
			Throw.IfNull(iinvocationContext);

			if (iinvocationContext.InvocationResultSyncCallback != null)
				stopwatch = Stopwatch.StartNew();

			if (result.IsArgumentNull(iinvocationContext.InvocationCreateNew(), @delegate))
			{
				var logResult = iinvocationContext.LogResultErrorMessages(result.Build());
				result.MergeErrors(logResult);

				return result.Build();
			}

			delegateWasCalled = true;

			@delegate(invocationContext, arg1, arg2, arg3);

			return result.Build();
		}
		catch (Exception ex)
		{
			isUnhandledException = true;
			stopwatch?.Stop();

			if (iinvocationContext == null)
			{
				disposableInvocationContext = new InvocationContextBuilder(ScopeContext.Create($"{nameof(Legion)} > {nameof(Result)}")).Build();
				iinvocationContext = disposableInvocationContext;
			}

			result.WithError(
				iinvocationContext.InvocationCreateNew(),
				iinvocationContext.UnhandledErrorCode ?? Exceptions.Internal.ErrorCodes.ResultException.Unhandled,
				x => x.ExceptionInfo(ex));

			var res = result.Build();

			var logResult = iinvocationContext.LogResultErrorMessages(res);
			result.MergeErrors(logResult);

			if (iinvocationContext.InvocationResultSyncCallback != null)
			{
				try
				{
					iinvocationContext.InvocationResultSyncCallback(new InvocationResult
					{
						InvocationContext = iinvocationContext.InvocationCreateNew(),
						DelegateWasCalled = delegateWasCalled,
						Result = result.Build(),
						IsUnhandledException = isUnhandledException,
						ElapsedMilliseconds = stopwatch?.ElapsedMilliseconds ?? -1,
						DelegateMethodName = string.IsNullOrWhiteSpace(delegateName)
							? iinvocationContext.TraceFrameStack.LastFrame ?? new TraceFrame().Frame
							: delegateName!,
					});
				}
				catch (Exception profilerEx)
				{
					var error = new ErrorMessageBuilder(
						iinvocationContext.InvocationCreateNew(),
						Exceptions.Internal.ErrorCodes.ResultException.InvocationResult)
						.ExceptionInfo(profilerEx)
						.Build();

					result.WithError(error);

					try
					{
						iinvocationContext.Logger?.LogErrorMessage(error);
					}
					catch (Exception loggerEx)
					{
						result.WithError(
							iinvocationContext.InvocationCreateNew(),
							Exceptions.Internal.ErrorCodes.ResultException.InvocationResult,
							x => x.ExceptionInfo(loggerEx));
					}
				}
			}

			return res;
		}
		finally
		{
			if (stopwatch != null && !isUnhandledException)
			{
				stopwatch.Stop();
				if (iinvocationContext.InvocationResultSyncCallback != null)
				{
					try
					{
						iinvocationContext.InvocationResultSyncCallback(new InvocationResult
						{
							InvocationContext = iinvocationContext.InvocationCreateNew(),
							DelegateWasCalled = delegateWasCalled,
							Result = result.Build(),
							IsUnhandledException = isUnhandledException,
							ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
							DelegateMethodName = string.IsNullOrWhiteSpace(delegateName)
								? iinvocationContext.TraceFrameStack.LastFrame ?? new TraceFrame().Frame
								: delegateName!,
						});
					}
					catch (Exception profilerEx)
					{
						var error = new ErrorMessageBuilder(
							iinvocationContext.InvocationCreateNew(),
							Exceptions.Internal.ErrorCodes.ResultException.InvocationResult)
							.ExceptionInfo(profilerEx)
							.Build();

						result.WithError(error);

						try
						{
							iinvocationContext.Logger?.LogErrorMessage(error);
						}
						catch (Exception loggerEx)
						{
							result.WithError(
								iinvocationContext.InvocationCreateNew(),
								Exceptions.Internal.ErrorCodes.ResultException.InvocationResult,
								x => x.ExceptionInfo(loggerEx));
						}
					}
				}
			}

			//disposableInvocationContext?.Dispose();
		}
	}

	public static IResult Call<TInvocationContext, T1, T2, T3, T4>(
		Action<TInvocationContext, T1, T2, T3, T4> @delegate,
		TInvocationContext invocationContext,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		[CallerArgumentExpression(nameof(@delegate))] string? delegateName = null)
		where TInvocationContext : IInvocationContext
	{
		var result = new ResultBuilder();

		var delegateWasCalled = false;
		var isUnhandledException = false;
		Stopwatch? stopwatch = null;
		IInvocationContext iinvocationContext = invocationContext;
		InvocationContext? disposableInvocationContext = null;

		try
		{
			Throw.IfNull(iinvocationContext);

			if (iinvocationContext.InvocationResultSyncCallback != null)
				stopwatch = Stopwatch.StartNew();

			if (result.IsArgumentNull(iinvocationContext.InvocationCreateNew(), @delegate))
			{
				var logResult = iinvocationContext.LogResultErrorMessages(result.Build());
				result.MergeErrors(logResult);

				return result.Build();
			}

			delegateWasCalled = true;

			@delegate(invocationContext, arg1, arg2, arg3, arg4);

			return result.Build();
		}
		catch (Exception ex)
		{
			isUnhandledException = true;
			stopwatch?.Stop();

			if (iinvocationContext == null)
			{
				disposableInvocationContext = new InvocationContextBuilder(ScopeContext.Create($"{nameof(Legion)} > {nameof(Result)}")).Build();
				iinvocationContext = disposableInvocationContext;
			}

			result.WithError(
				iinvocationContext.InvocationCreateNew(),
				iinvocationContext.UnhandledErrorCode ?? Exceptions.Internal.ErrorCodes.ResultException.Unhandled,
				x => x.ExceptionInfo(ex));

			var res = result.Build();

			var logResult = iinvocationContext.LogResultErrorMessages(res);
			result.MergeErrors(logResult);

			if (iinvocationContext.InvocationResultSyncCallback != null)
			{
				try
				{
					iinvocationContext.InvocationResultSyncCallback(new InvocationResult
					{
						InvocationContext = iinvocationContext.InvocationCreateNew(),
						DelegateWasCalled = delegateWasCalled,
						Result = result.Build(),
						IsUnhandledException = isUnhandledException,
						ElapsedMilliseconds = stopwatch?.ElapsedMilliseconds ?? -1,
						DelegateMethodName = string.IsNullOrWhiteSpace(delegateName)
							? iinvocationContext.TraceFrameStack.LastFrame ?? new TraceFrame().Frame
							: delegateName!,
					});
				}
				catch (Exception profilerEx)
				{
					var error = new ErrorMessageBuilder(
						iinvocationContext.InvocationCreateNew(),
						Exceptions.Internal.ErrorCodes.ResultException.InvocationResult)
						.ExceptionInfo(profilerEx)
						.Build();

					result.WithError(error);

					try
					{
						iinvocationContext.Logger?.LogErrorMessage(error);
					}
					catch (Exception loggerEx)
					{
						result.WithError(
							iinvocationContext.InvocationCreateNew(),
							Exceptions.Internal.ErrorCodes.ResultException.InvocationResult,
							x => x.ExceptionInfo(loggerEx));
					}
				}
			}

			return res;
		}
		finally
		{
			if (stopwatch != null && !isUnhandledException)
			{
				stopwatch.Stop();
				if (iinvocationContext.InvocationResultSyncCallback != null)
				{
					try
					{
						iinvocationContext.InvocationResultSyncCallback(new InvocationResult
						{
							InvocationContext = iinvocationContext.InvocationCreateNew(),
							DelegateWasCalled = delegateWasCalled,
							Result = result.Build(),
							IsUnhandledException = isUnhandledException,
							ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
							DelegateMethodName = string.IsNullOrWhiteSpace(delegateName)
								? iinvocationContext.TraceFrameStack.LastFrame ?? new TraceFrame().Frame
								: delegateName!,
						});
					}
					catch (Exception profilerEx)
					{
						var error = new ErrorMessageBuilder(
							iinvocationContext.InvocationCreateNew(),
							Exceptions.Internal.ErrorCodes.ResultException.InvocationResult)
							.ExceptionInfo(profilerEx)
							.Build();

						result.WithError(error);

						try
						{
							iinvocationContext.Logger?.LogErrorMessage(error);
						}
						catch (Exception loggerEx)
						{
							result.WithError(
								iinvocationContext.InvocationCreateNew(),
								Exceptions.Internal.ErrorCodes.ResultException.InvocationResult,
								x => x.ExceptionInfo(loggerEx));
						}
					}
				}
			}

			//disposableInvocationContext?.Dispose();
		}
	}

	public static IResult Call<TInvocationContext, T1, T2, T3, T4, T5>(
		Action<TInvocationContext, T1, T2, T3, T4, T5> @delegate,
		TInvocationContext invocationContext,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		T5 arg5,
		[CallerArgumentExpression(nameof(@delegate))] string? delegateName = null)
		where TInvocationContext : IInvocationContext
	{
		var result = new ResultBuilder();

		var delegateWasCalled = false;
		var isUnhandledException = false;
		Stopwatch? stopwatch = null;
		IInvocationContext iinvocationContext = invocationContext;
		InvocationContext? disposableInvocationContext = null;

		try
		{
			Throw.IfNull(iinvocationContext);

			if (iinvocationContext.InvocationResultSyncCallback != null)
				stopwatch = Stopwatch.StartNew();

			if (result.IsArgumentNull(iinvocationContext.InvocationCreateNew(), @delegate))
			{
				var logResult = iinvocationContext.LogResultErrorMessages(result.Build());
				result.MergeErrors(logResult);

				return result.Build();
			}

			delegateWasCalled = true;

			@delegate(invocationContext, arg1, arg2, arg3, arg4, arg5);

			return result.Build();
		}
		catch (Exception ex)
		{
			isUnhandledException = true;
			stopwatch?.Stop();

			if (iinvocationContext == null)
			{
				disposableInvocationContext = new InvocationContextBuilder(ScopeContext.Create($"{nameof(Legion)} > {nameof(Result)}")).Build();
				iinvocationContext = disposableInvocationContext;
			}

			result.WithError(
				iinvocationContext.InvocationCreateNew(),
				iinvocationContext.UnhandledErrorCode ?? Exceptions.Internal.ErrorCodes.ResultException.Unhandled,
				x => x.ExceptionInfo(ex));

			var res = result.Build();

			var logResult = iinvocationContext.LogResultErrorMessages(res);
			result.MergeErrors(logResult);

			if (iinvocationContext.InvocationResultSyncCallback != null)
			{
				try
				{
					iinvocationContext.InvocationResultSyncCallback(new InvocationResult
					{
						InvocationContext = iinvocationContext.InvocationCreateNew(),
						DelegateWasCalled = delegateWasCalled,
						Result = result.Build(),
						IsUnhandledException = isUnhandledException,
						ElapsedMilliseconds = stopwatch?.ElapsedMilliseconds ?? -1,
						DelegateMethodName = string.IsNullOrWhiteSpace(delegateName)
							? iinvocationContext.TraceFrameStack.LastFrame ?? new TraceFrame().Frame
							: delegateName!,
					});
				}
				catch (Exception profilerEx)
				{
					var error = new ErrorMessageBuilder(
						iinvocationContext.InvocationCreateNew(),
						Exceptions.Internal.ErrorCodes.ResultException.InvocationResult)
						.ExceptionInfo(profilerEx)
						.Build();

					result.WithError(error);

					try
					{
						iinvocationContext.Logger?.LogErrorMessage(error);
					}
					catch (Exception loggerEx)
					{
						result.WithError(
							iinvocationContext.InvocationCreateNew(),
							Exceptions.Internal.ErrorCodes.ResultException.InvocationResult,
							x => x.ExceptionInfo(loggerEx));
					}
				}
			}

			return res;
		}
		finally
		{
			if (stopwatch != null && !isUnhandledException)
			{
				stopwatch.Stop();
				if (iinvocationContext.InvocationResultSyncCallback != null)
				{
					try
					{
						iinvocationContext.InvocationResultSyncCallback(new InvocationResult
						{
							InvocationContext = iinvocationContext.InvocationCreateNew(),
							DelegateWasCalled = delegateWasCalled,
							Result = result.Build(),
							IsUnhandledException = isUnhandledException,
							ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
							DelegateMethodName = string.IsNullOrWhiteSpace(delegateName)
								? iinvocationContext.TraceFrameStack.LastFrame ?? new TraceFrame().Frame
								: delegateName!,
						});
					}
					catch (Exception profilerEx)
					{
						var error = new ErrorMessageBuilder(
							iinvocationContext.InvocationCreateNew(),
							Exceptions.Internal.ErrorCodes.ResultException.InvocationResult)
							.ExceptionInfo(profilerEx)
							.Build();

						result.WithError(error);

						try
						{
							iinvocationContext.Logger?.LogErrorMessage(error);
						}
						catch (Exception loggerEx)
						{
							result.WithError(
								iinvocationContext.InvocationCreateNew(),
								Exceptions.Internal.ErrorCodes.ResultException.InvocationResult,
								x => x.ExceptionInfo(loggerEx));
						}
					}
				}
			}

			//disposableInvocationContext?.Dispose();
		}
	}

	public static IResult Call<TInvocationContext, T1, T2, T3, T4, T5, T6>(
		Action<TInvocationContext, T1, T2, T3, T4, T5, T6> @delegate,
		TInvocationContext invocationContext,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		T5 arg5,
		T6 arg6,
		[CallerArgumentExpression(nameof(@delegate))] string? delegateName = null)
		where TInvocationContext : IInvocationContext
	{
		var result = new ResultBuilder();

		var delegateWasCalled = false;
		var isUnhandledException = false;
		Stopwatch? stopwatch = null;
		IInvocationContext iinvocationContext = invocationContext;
		InvocationContext? disposableInvocationContext = null;

		try
		{
			Throw.IfNull(iinvocationContext);

			if (iinvocationContext.InvocationResultSyncCallback != null)
				stopwatch = Stopwatch.StartNew();

			if (result.IsArgumentNull(iinvocationContext.InvocationCreateNew(), @delegate))
			{
				var logResult = iinvocationContext.LogResultErrorMessages(result.Build());
				result.MergeErrors(logResult);

				return result.Build();
			}

			delegateWasCalled = true;

			@delegate(invocationContext, arg1, arg2, arg3, arg4, arg5, arg6);

			return result.Build();
		}
		catch (Exception ex)
		{
			isUnhandledException = true;
			stopwatch?.Stop();

			if (iinvocationContext == null)
			{
				disposableInvocationContext = new InvocationContextBuilder(ScopeContext.Create($"{nameof(Legion)} > {nameof(Result)}")).Build();
				iinvocationContext = disposableInvocationContext;
			}

			result.WithError(
				iinvocationContext.InvocationCreateNew(),
				iinvocationContext.UnhandledErrorCode ?? Exceptions.Internal.ErrorCodes.ResultException.Unhandled,
				x => x.ExceptionInfo(ex));

			var res = result.Build();

			var logResult = iinvocationContext.LogResultErrorMessages(res);
			result.MergeErrors(logResult);

			if (iinvocationContext.InvocationResultSyncCallback != null)
			{
				try
				{
					iinvocationContext.InvocationResultSyncCallback(new InvocationResult
					{
						InvocationContext = iinvocationContext.InvocationCreateNew(),
						DelegateWasCalled = delegateWasCalled,
						Result = result.Build(),
						IsUnhandledException = isUnhandledException,
						ElapsedMilliseconds = stopwatch?.ElapsedMilliseconds ?? -1,
						DelegateMethodName = string.IsNullOrWhiteSpace(delegateName)
							? iinvocationContext.TraceFrameStack.LastFrame ?? new TraceFrame().Frame
							: delegateName!,
					});
				}
				catch (Exception profilerEx)
				{
					var error = new ErrorMessageBuilder(
						iinvocationContext.InvocationCreateNew(),
						Exceptions.Internal.ErrorCodes.ResultException.InvocationResult)
						.ExceptionInfo(profilerEx)
						.Build();

					result.WithError(error);

					try
					{
						iinvocationContext.Logger?.LogErrorMessage(error);
					}
					catch (Exception loggerEx)
					{
						result.WithError(
							iinvocationContext.InvocationCreateNew(),
							Exceptions.Internal.ErrorCodes.ResultException.InvocationResult,
							x => x.ExceptionInfo(loggerEx));
					}
				}
			}

			return res;
		}
		finally
		{
			if (stopwatch != null && !isUnhandledException)
			{
				stopwatch.Stop();
				if (iinvocationContext.InvocationResultSyncCallback != null)
				{
					try
					{
						iinvocationContext.InvocationResultSyncCallback(new InvocationResult
						{
							InvocationContext = iinvocationContext.InvocationCreateNew(),
							DelegateWasCalled = delegateWasCalled,
							Result = result.Build(),
							IsUnhandledException = isUnhandledException,
							ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
							DelegateMethodName = string.IsNullOrWhiteSpace(delegateName)
								? iinvocationContext.TraceFrameStack.LastFrame ?? new TraceFrame().Frame
								: delegateName!,
						});
					}
					catch (Exception profilerEx)
					{
						var error = new ErrorMessageBuilder(
							iinvocationContext.InvocationCreateNew(),
							Exceptions.Internal.ErrorCodes.ResultException.InvocationResult)
							.ExceptionInfo(profilerEx)
							.Build();

						result.WithError(error);

						try
						{
							iinvocationContext.Logger?.LogErrorMessage(error);
						}
						catch (Exception loggerEx)
						{
							result.WithError(
								iinvocationContext.InvocationCreateNew(),
								Exceptions.Internal.ErrorCodes.ResultException.InvocationResult,
								x => x.ExceptionInfo(loggerEx));
						}
					}
				}
			}

			//disposableInvocationContext?.Dispose();
		}
	}

	public static IResult Call<TInvocationContext, T1, T2, T3, T4, T5, T6, T7>(
		Action<TInvocationContext, T1, T2, T3, T4, T5, T6, T7> @delegate,
		TInvocationContext invocationContext,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		T5 arg5,
		T6 arg6,
		T7 arg7,
		[CallerArgumentExpression(nameof(@delegate))] string? delegateName = null)
		where TInvocationContext : IInvocationContext
	{
		var result = new ResultBuilder();

		var delegateWasCalled = false;
		var isUnhandledException = false;
		Stopwatch? stopwatch = null;
		IInvocationContext iinvocationContext = invocationContext;
		InvocationContext? disposableInvocationContext = null;

		try
		{
			Throw.IfNull(iinvocationContext);

			if (iinvocationContext.InvocationResultSyncCallback != null)
				stopwatch = Stopwatch.StartNew();

			if (result.IsArgumentNull(iinvocationContext.InvocationCreateNew(), @delegate))
			{
				var logResult = iinvocationContext.LogResultErrorMessages(result.Build());
				result.MergeErrors(logResult);

				return result.Build();
			}

			delegateWasCalled = true;

			@delegate(invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, arg7);

			return result.Build();
		}
		catch (Exception ex)
		{
			isUnhandledException = true;
			stopwatch?.Stop();

			if (iinvocationContext == null)
			{
				disposableInvocationContext = new InvocationContextBuilder(ScopeContext.Create($"{nameof(Legion)} > {nameof(Result)}")).Build();
				iinvocationContext = disposableInvocationContext;
			}

			result.WithError(
				iinvocationContext.InvocationCreateNew(),
				iinvocationContext.UnhandledErrorCode ?? Exceptions.Internal.ErrorCodes.ResultException.Unhandled,
				x => x.ExceptionInfo(ex));

			var res = result.Build();

			var logResult = iinvocationContext.LogResultErrorMessages(res);
			result.MergeErrors(logResult);

			if (iinvocationContext.InvocationResultSyncCallback != null)
			{
				try
				{
					iinvocationContext.InvocationResultSyncCallback(new InvocationResult
					{
						InvocationContext = iinvocationContext.InvocationCreateNew(),
						DelegateWasCalled = delegateWasCalled,
						Result = result.Build(),
						IsUnhandledException = isUnhandledException,
						ElapsedMilliseconds = stopwatch?.ElapsedMilliseconds ?? -1,
						DelegateMethodName = string.IsNullOrWhiteSpace(delegateName)
							? iinvocationContext.TraceFrameStack.LastFrame ?? new TraceFrame().Frame
							: delegateName!,
					});
				}
				catch (Exception profilerEx)
				{
					var error = new ErrorMessageBuilder(
						iinvocationContext.InvocationCreateNew(),
						Exceptions.Internal.ErrorCodes.ResultException.InvocationResult)
						.ExceptionInfo(profilerEx)
						.Build();

					result.WithError(error);

					try
					{
						iinvocationContext.Logger?.LogErrorMessage(error);
					}
					catch (Exception loggerEx)
					{
						result.WithError(
							iinvocationContext.InvocationCreateNew(),
							Exceptions.Internal.ErrorCodes.ResultException.InvocationResult,
							x => x.ExceptionInfo(loggerEx));
					}
				}
			}

			return res;
		}
		finally
		{
			if (stopwatch != null && !isUnhandledException)
			{
				stopwatch.Stop();
				if (iinvocationContext.InvocationResultSyncCallback != null)
				{
					try
					{
						iinvocationContext.InvocationResultSyncCallback(new InvocationResult
						{
							InvocationContext = iinvocationContext.InvocationCreateNew(),
							DelegateWasCalled = delegateWasCalled,
							Result = result.Build(),
							IsUnhandledException = isUnhandledException,
							ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
							DelegateMethodName = string.IsNullOrWhiteSpace(delegateName)
								? iinvocationContext.TraceFrameStack.LastFrame ?? new TraceFrame().Frame
								: delegateName!,
						});
					}
					catch (Exception profilerEx)
					{
						var error = new ErrorMessageBuilder(
							iinvocationContext.InvocationCreateNew(),
							Exceptions.Internal.ErrorCodes.ResultException.InvocationResult)
							.ExceptionInfo(profilerEx)
							.Build();

						result.WithError(error);

						try
						{
							iinvocationContext.Logger?.LogErrorMessage(error);
						}
						catch (Exception loggerEx)
						{
							result.WithError(
								iinvocationContext.InvocationCreateNew(),
								Exceptions.Internal.ErrorCodes.ResultException.InvocationResult,
								x => x.ExceptionInfo(loggerEx));
						}
					}
				}
			}

			//disposableInvocationContext?.Dispose();
		}
	}

	public static IResult Call<TInvocationContext, T1, T2, T3, T4, T5, T6, T7, T8>(
		Action<TInvocationContext, T1, T2, T3, T4, T5, T6, T7, T8> @delegate,
		TInvocationContext invocationContext,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		T5 arg5,
		T6 arg6,
		T7 arg7,
		T8 arg8,
		[CallerArgumentExpression(nameof(@delegate))] string? delegateName = null)
		where TInvocationContext : IInvocationContext
	{
		var result = new ResultBuilder();

		var delegateWasCalled = false;
		var isUnhandledException = false;
		Stopwatch? stopwatch = null;
		IInvocationContext iinvocationContext = invocationContext;
		InvocationContext? disposableInvocationContext = null;

		try
		{
			Throw.IfNull(iinvocationContext);

			if (iinvocationContext.InvocationResultSyncCallback != null)
				stopwatch = Stopwatch.StartNew();

			if (result.IsArgumentNull(iinvocationContext.InvocationCreateNew(), @delegate))
			{
				var logResult = iinvocationContext.LogResultErrorMessages(result.Build());
				result.MergeErrors(logResult);

				return result.Build();
			}

			delegateWasCalled = true;

			@delegate(invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);

			return result.Build();
		}
		catch (Exception ex)
		{
			isUnhandledException = true;
			stopwatch?.Stop();

			if (iinvocationContext == null)
			{
				disposableInvocationContext = new InvocationContextBuilder(ScopeContext.Create($"{nameof(Legion)} > {nameof(Result)}")).Build();
				iinvocationContext = disposableInvocationContext;
			}

			result.WithError(
				iinvocationContext.InvocationCreateNew(),
				iinvocationContext.UnhandledErrorCode ?? Exceptions.Internal.ErrorCodes.ResultException.Unhandled,
				x => x.ExceptionInfo(ex));

			var res = result.Build();

			var logResult = iinvocationContext.LogResultErrorMessages(res);
			result.MergeErrors(logResult);

			if (iinvocationContext.InvocationResultSyncCallback != null)
			{
				try
				{
					iinvocationContext.InvocationResultSyncCallback(new InvocationResult
					{
						InvocationContext = iinvocationContext.InvocationCreateNew(),
						DelegateWasCalled = delegateWasCalled,
						Result = result.Build(),
						IsUnhandledException = isUnhandledException,
						ElapsedMilliseconds = stopwatch?.ElapsedMilliseconds ?? -1,
						DelegateMethodName = string.IsNullOrWhiteSpace(delegateName)
							? iinvocationContext.TraceFrameStack.LastFrame ?? new TraceFrame().Frame
							: delegateName!,
					});
				}
				catch (Exception profilerEx)
				{
					var error = new ErrorMessageBuilder(
						iinvocationContext.InvocationCreateNew(),
						Exceptions.Internal.ErrorCodes.ResultException.InvocationResult)
						.ExceptionInfo(profilerEx)
						.Build();

					result.WithError(error);

					try
					{
						iinvocationContext.Logger?.LogErrorMessage(error);
					}
					catch (Exception loggerEx)
					{
						result.WithError(
							iinvocationContext.InvocationCreateNew(),
							Exceptions.Internal.ErrorCodes.ResultException.InvocationResult,
							x => x.ExceptionInfo(loggerEx));
					}
				}
			}

			return res;
		}
		finally
		{
			if (stopwatch != null && !isUnhandledException)
			{
				stopwatch.Stop();
				if (iinvocationContext.InvocationResultSyncCallback != null)
				{
					try
					{
						iinvocationContext.InvocationResultSyncCallback(new InvocationResult
						{
							InvocationContext = iinvocationContext.InvocationCreateNew(),
							DelegateWasCalled = delegateWasCalled,
							Result = result.Build(),
							IsUnhandledException = isUnhandledException,
							ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
							DelegateMethodName = string.IsNullOrWhiteSpace(delegateName)
								? iinvocationContext.TraceFrameStack.LastFrame ?? new TraceFrame().Frame
								: delegateName!,
						});
					}
					catch (Exception profilerEx)
					{
						var error = new ErrorMessageBuilder(
							iinvocationContext.InvocationCreateNew(),
							Exceptions.Internal.ErrorCodes.ResultException.InvocationResult)
							.ExceptionInfo(profilerEx)
							.Build();

						result.WithError(error);

						try
						{
							iinvocationContext.Logger?.LogErrorMessage(error);
						}
						catch (Exception loggerEx)
						{
							result.WithError(
								iinvocationContext.InvocationCreateNew(),
								Exceptions.Internal.ErrorCodes.ResultException.InvocationResult,
								x => x.ExceptionInfo(loggerEx));
						}
					}
				}
			}

			//disposableInvocationContext?.Dispose();
		}
	}

	public static IResult Call<TInvocationContext, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
		Action<TInvocationContext, T1, T2, T3, T4, T5, T6, T7, T8, T9> @delegate,
		TInvocationContext invocationContext,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		T5 arg5,
		T6 arg6,
		T7 arg7,
		T8 arg8,
		T9 arg9,
		[CallerArgumentExpression(nameof(@delegate))] string? delegateName = null)
		where TInvocationContext : IInvocationContext
	{
		var result = new ResultBuilder();

		var delegateWasCalled = false;
		var isUnhandledException = false;
		Stopwatch? stopwatch = null;
		IInvocationContext iinvocationContext = invocationContext;
		InvocationContext? disposableInvocationContext = null;

		try
		{
			Throw.IfNull(iinvocationContext);

			if (iinvocationContext.InvocationResultSyncCallback != null)
				stopwatch = Stopwatch.StartNew();

			if (result.IsArgumentNull(iinvocationContext.InvocationCreateNew(), @delegate))
			{
				var logResult = iinvocationContext.LogResultErrorMessages(result.Build());
				result.MergeErrors(logResult);

				return result.Build();
			}

			delegateWasCalled = true;

			@delegate(invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);

			return result.Build();
		}
		catch (Exception ex)
		{
			isUnhandledException = true;
			stopwatch?.Stop();

			if (iinvocationContext == null)
			{
				disposableInvocationContext = new InvocationContextBuilder(ScopeContext.Create($"{nameof(Legion)} > {nameof(Result)}")).Build();
				iinvocationContext = disposableInvocationContext;
			}

			result.WithError(
				iinvocationContext.InvocationCreateNew(),
				iinvocationContext.UnhandledErrorCode ?? Exceptions.Internal.ErrorCodes.ResultException.Unhandled,
				x => x.ExceptionInfo(ex));

			var res = result.Build();

			var logResult = iinvocationContext.LogResultErrorMessages(res);
			result.MergeErrors(logResult);

			if (iinvocationContext.InvocationResultSyncCallback != null)
			{
				try
				{
					iinvocationContext.InvocationResultSyncCallback(new InvocationResult
					{
						InvocationContext = iinvocationContext.InvocationCreateNew(),
						DelegateWasCalled = delegateWasCalled,
						Result = result.Build(),
						IsUnhandledException = isUnhandledException,
						ElapsedMilliseconds = stopwatch?.ElapsedMilliseconds ?? -1,
						DelegateMethodName = string.IsNullOrWhiteSpace(delegateName)
							? iinvocationContext.TraceFrameStack.LastFrame ?? new TraceFrame().Frame
							: delegateName!,
					});
				}
				catch (Exception profilerEx)
				{
					var error = new ErrorMessageBuilder(
						iinvocationContext.InvocationCreateNew(),
						Exceptions.Internal.ErrorCodes.ResultException.InvocationResult)
						.ExceptionInfo(profilerEx)
						.Build();

					result.WithError(error);

					try
					{
						iinvocationContext.Logger?.LogErrorMessage(error);
					}
					catch (Exception loggerEx)
					{
						result.WithError(
							iinvocationContext.InvocationCreateNew(),
							Exceptions.Internal.ErrorCodes.ResultException.InvocationResult,
							x => x.ExceptionInfo(loggerEx));
					}
				}
			}

			return res;
		}
		finally
		{
			if (stopwatch != null && !isUnhandledException)
			{
				stopwatch.Stop();
				if (iinvocationContext.InvocationResultSyncCallback != null)
				{
					try
					{
						iinvocationContext.InvocationResultSyncCallback(new InvocationResult
						{
							InvocationContext = iinvocationContext.InvocationCreateNew(),
							DelegateWasCalled = delegateWasCalled,
							Result = result.Build(),
							IsUnhandledException = isUnhandledException,
							ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
							DelegateMethodName = string.IsNullOrWhiteSpace(delegateName)
								? iinvocationContext.TraceFrameStack.LastFrame ?? new TraceFrame().Frame
								: delegateName!,
						});
					}
					catch (Exception profilerEx)
					{
						var error = new ErrorMessageBuilder(
							iinvocationContext.InvocationCreateNew(),
							Exceptions.Internal.ErrorCodes.ResultException.InvocationResult)
							.ExceptionInfo(profilerEx)
							.Build();

						result.WithError(error);

						try
						{
							iinvocationContext.Logger?.LogErrorMessage(error);
						}
						catch (Exception loggerEx)
						{
							result.WithError(
								iinvocationContext.InvocationCreateNew(),
								Exceptions.Internal.ErrorCodes.ResultException.InvocationResult,
								x => x.ExceptionInfo(loggerEx));
						}
					}
				}
			}

			//disposableInvocationContext?.Dispose();
		}
	}

	public static IResult Call<TInvocationContext, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
		Action<TInvocationContext, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> @delegate,
		TInvocationContext invocationContext,
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
		[CallerArgumentExpression(nameof(@delegate))] string? delegateName = null)
		where TInvocationContext : IInvocationContext
	{
		var result = new ResultBuilder();

		var delegateWasCalled = false;
		var isUnhandledException = false;
		Stopwatch? stopwatch = null;
		IInvocationContext iinvocationContext = invocationContext;
		InvocationContext? disposableInvocationContext = null;

		try
		{
			Throw.IfNull(iinvocationContext);

			if (iinvocationContext.InvocationResultSyncCallback != null)
				stopwatch = Stopwatch.StartNew();

			if (result.IsArgumentNull(iinvocationContext.InvocationCreateNew(), @delegate))
			{
				var logResult = iinvocationContext.LogResultErrorMessages(result.Build());
				result.MergeErrors(logResult);

				return result.Build();
			}

			delegateWasCalled = true;

			@delegate(invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);

			return result.Build();
		}
		catch (Exception ex)
		{
			isUnhandledException = true;
			stopwatch?.Stop();

			if (iinvocationContext == null)
			{
				disposableInvocationContext = new InvocationContextBuilder(ScopeContext.Create($"{nameof(Legion)} > {nameof(Result)}")).Build();
				iinvocationContext = disposableInvocationContext;
			}

			result.WithError(
				iinvocationContext.InvocationCreateNew(),
				iinvocationContext.UnhandledErrorCode ?? Exceptions.Internal.ErrorCodes.ResultException.Unhandled,
				x => x.ExceptionInfo(ex));

			var res = result.Build();

			var logResult = iinvocationContext.LogResultErrorMessages(res);
			result.MergeErrors(logResult);

			if (iinvocationContext.InvocationResultSyncCallback != null)
			{
				try
				{
					iinvocationContext.InvocationResultSyncCallback(new InvocationResult
					{
						InvocationContext = iinvocationContext.InvocationCreateNew(),
						DelegateWasCalled = delegateWasCalled,
						Result = result.Build(),
						IsUnhandledException = isUnhandledException,
						ElapsedMilliseconds = stopwatch?.ElapsedMilliseconds ?? -1,
						DelegateMethodName = string.IsNullOrWhiteSpace(delegateName)
							? iinvocationContext.TraceFrameStack.LastFrame ?? new TraceFrame().Frame
							: delegateName!,
					});
				}
				catch (Exception profilerEx)
				{
					var error = new ErrorMessageBuilder(
						iinvocationContext.InvocationCreateNew(),
						Exceptions.Internal.ErrorCodes.ResultException.InvocationResult)
						.ExceptionInfo(profilerEx)
						.Build();

					result.WithError(error);

					try
					{
						iinvocationContext.Logger?.LogErrorMessage(error);
					}
					catch (Exception loggerEx)
					{
						result.WithError(
							iinvocationContext.InvocationCreateNew(),
							Exceptions.Internal.ErrorCodes.ResultException.InvocationResult,
							x => x.ExceptionInfo(loggerEx));
					}
				}
			}

			return res;
		}
		finally
		{
			if (stopwatch != null && !isUnhandledException)
			{
				stopwatch.Stop();
				if (iinvocationContext.InvocationResultSyncCallback != null)
				{
					try
					{
						iinvocationContext.InvocationResultSyncCallback(new InvocationResult
						{
							InvocationContext = iinvocationContext.InvocationCreateNew(),
							DelegateWasCalled = delegateWasCalled,
							Result = result.Build(),
							IsUnhandledException = isUnhandledException,
							ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
							DelegateMethodName = string.IsNullOrWhiteSpace(delegateName)
								? iinvocationContext.TraceFrameStack.LastFrame ?? new TraceFrame().Frame
								: delegateName!,
						});
					}
					catch (Exception profilerEx)
					{
						var error = new ErrorMessageBuilder(
							iinvocationContext.InvocationCreateNew(),
							Exceptions.Internal.ErrorCodes.ResultException.InvocationResult)
							.ExceptionInfo(profilerEx)
							.Build();

						result.WithError(error);

						try
						{
							iinvocationContext.Logger?.LogErrorMessage(error);
						}
						catch (Exception loggerEx)
						{
							result.WithError(
								iinvocationContext.InvocationCreateNew(),
								Exceptions.Internal.ErrorCodes.ResultException.InvocationResult,
								x => x.ExceptionInfo(loggerEx));
						}
					}
				}
			}

			//disposableInvocationContext?.Dispose();
		}
	}

	public static IResult Call<TInvocationContext, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
		Action<TInvocationContext, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> @delegate,
		TInvocationContext invocationContext,
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
		[CallerArgumentExpression(nameof(@delegate))] string? delegateName = null)
		where TInvocationContext : IInvocationContext
	{
		var result = new ResultBuilder();

		var delegateWasCalled = false;
		var isUnhandledException = false;
		Stopwatch? stopwatch = null;
		IInvocationContext iinvocationContext = invocationContext;
		InvocationContext? disposableInvocationContext = null;

		try
		{
			Throw.IfNull(iinvocationContext);

			if (iinvocationContext.InvocationResultSyncCallback != null)
				stopwatch = Stopwatch.StartNew();

			if (result.IsArgumentNull(iinvocationContext.InvocationCreateNew(), @delegate))
			{
				var logResult = iinvocationContext.LogResultErrorMessages(result.Build());
				result.MergeErrors(logResult);

				return result.Build();
			}

			delegateWasCalled = true;

			@delegate(invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);

			return result.Build();
		}
		catch (Exception ex)
		{
			isUnhandledException = true;
			stopwatch?.Stop();

			if (iinvocationContext == null)
			{
				disposableInvocationContext = new InvocationContextBuilder(ScopeContext.Create($"{nameof(Legion)} > {nameof(Result)}")).Build();
				iinvocationContext = disposableInvocationContext;
			}

			result.WithError(
				iinvocationContext.InvocationCreateNew(),
				iinvocationContext.UnhandledErrorCode ?? Exceptions.Internal.ErrorCodes.ResultException.Unhandled,
				x => x.ExceptionInfo(ex));

			var res = result.Build();

			var logResult = iinvocationContext.LogResultErrorMessages(res);
			result.MergeErrors(logResult);

			if (iinvocationContext.InvocationResultSyncCallback != null)
			{
				try
				{
					iinvocationContext.InvocationResultSyncCallback(new InvocationResult
					{
						InvocationContext = iinvocationContext.InvocationCreateNew(),
						DelegateWasCalled = delegateWasCalled,
						Result = result.Build(),
						IsUnhandledException = isUnhandledException,
						ElapsedMilliseconds = stopwatch?.ElapsedMilliseconds ?? -1,
						DelegateMethodName = string.IsNullOrWhiteSpace(delegateName)
							? iinvocationContext.TraceFrameStack.LastFrame ?? new TraceFrame().Frame
							: delegateName!,
					});
				}
				catch (Exception profilerEx)
				{
					var error = new ErrorMessageBuilder(
						iinvocationContext.InvocationCreateNew(),
						Exceptions.Internal.ErrorCodes.ResultException.InvocationResult)
						.ExceptionInfo(profilerEx)
						.Build();

					result.WithError(error);

					try
					{
						iinvocationContext.Logger?.LogErrorMessage(error);
					}
					catch (Exception loggerEx)
					{
						result.WithError(
							iinvocationContext.InvocationCreateNew(),
							Exceptions.Internal.ErrorCodes.ResultException.InvocationResult,
							x => x.ExceptionInfo(loggerEx));
					}
				}
			}

			return res;
		}
		finally
		{
			if (stopwatch != null && !isUnhandledException)
			{
				stopwatch.Stop();
				if (iinvocationContext.InvocationResultSyncCallback != null)
				{
					try
					{
						iinvocationContext.InvocationResultSyncCallback(new InvocationResult
						{
							InvocationContext = iinvocationContext.InvocationCreateNew(),
							DelegateWasCalled = delegateWasCalled,
							Result = result.Build(),
							IsUnhandledException = isUnhandledException,
							ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
							DelegateMethodName = string.IsNullOrWhiteSpace(delegateName)
								? iinvocationContext.TraceFrameStack.LastFrame ?? new TraceFrame().Frame
								: delegateName!,
						});
					}
					catch (Exception profilerEx)
					{
						var error = new ErrorMessageBuilder(
							iinvocationContext.InvocationCreateNew(),
							Exceptions.Internal.ErrorCodes.ResultException.InvocationResult)
							.ExceptionInfo(profilerEx)
							.Build();

						result.WithError(error);

						try
						{
							iinvocationContext.Logger?.LogErrorMessage(error);
						}
						catch (Exception loggerEx)
						{
							result.WithError(
								iinvocationContext.InvocationCreateNew(),
								Exceptions.Internal.ErrorCodes.ResultException.InvocationResult,
								x => x.ExceptionInfo(loggerEx));
						}
					}
				}
			}

			//disposableInvocationContext?.Dispose();
		}
	}

	public static IResult Call<TInvocationContext, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
		Action<TInvocationContext, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> @delegate,
		TInvocationContext invocationContext,
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
		[CallerArgumentExpression(nameof(@delegate))] string? delegateName = null)
		where TInvocationContext : IInvocationContext
	{
		var result = new ResultBuilder();

		var delegateWasCalled = false;
		var isUnhandledException = false;
		Stopwatch? stopwatch = null;
		IInvocationContext iinvocationContext = invocationContext;
		InvocationContext? disposableInvocationContext = null;

		try
		{
			Throw.IfNull(iinvocationContext);

			if (iinvocationContext.InvocationResultSyncCallback != null)
				stopwatch = Stopwatch.StartNew();

			if (result.IsArgumentNull(iinvocationContext.InvocationCreateNew(), @delegate))
			{
				var logResult = iinvocationContext.LogResultErrorMessages(result.Build());
				result.MergeErrors(logResult);

				return result.Build();
			}

			delegateWasCalled = true;

			@delegate(invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);

			return result.Build();
		}
		catch (Exception ex)
		{
			isUnhandledException = true;
			stopwatch?.Stop();

			if (iinvocationContext == null)
			{
				disposableInvocationContext = new InvocationContextBuilder(ScopeContext.Create($"{nameof(Legion)} > {nameof(Result)}")).Build();
				iinvocationContext = disposableInvocationContext;
			}

			result.WithError(
				iinvocationContext.InvocationCreateNew(),
				iinvocationContext.UnhandledErrorCode ?? Exceptions.Internal.ErrorCodes.ResultException.Unhandled,
				x => x.ExceptionInfo(ex));

			var res = result.Build();

			var logResult = iinvocationContext.LogResultErrorMessages(res);
			result.MergeErrors(logResult);

			if (iinvocationContext.InvocationResultSyncCallback != null)
			{
				try
				{
					iinvocationContext.InvocationResultSyncCallback(new InvocationResult
					{
						InvocationContext = iinvocationContext.InvocationCreateNew(),
						DelegateWasCalled = delegateWasCalled,
						Result = result.Build(),
						IsUnhandledException = isUnhandledException,
						ElapsedMilliseconds = stopwatch?.ElapsedMilliseconds ?? -1,
						DelegateMethodName = string.IsNullOrWhiteSpace(delegateName)
							? iinvocationContext.TraceFrameStack.LastFrame ?? new TraceFrame().Frame
							: delegateName!,
					});
				}
				catch (Exception profilerEx)
				{
					var error = new ErrorMessageBuilder(
						iinvocationContext.InvocationCreateNew(),
						Exceptions.Internal.ErrorCodes.ResultException.InvocationResult)
						.ExceptionInfo(profilerEx)
						.Build();

					result.WithError(error);

					try
					{
						iinvocationContext.Logger?.LogErrorMessage(error);
					}
					catch (Exception loggerEx)
					{
						result.WithError(
							iinvocationContext.InvocationCreateNew(),
							Exceptions.Internal.ErrorCodes.ResultException.InvocationResult,
							x => x.ExceptionInfo(loggerEx));
					}
				}
			}

			return res;
		}
		finally
		{
			if (stopwatch != null && !isUnhandledException)
			{
				stopwatch.Stop();
				if (iinvocationContext.InvocationResultSyncCallback != null)
				{
					try
					{
						iinvocationContext.InvocationResultSyncCallback(new InvocationResult
						{
							InvocationContext = iinvocationContext.InvocationCreateNew(),
							DelegateWasCalled = delegateWasCalled,
							Result = result.Build(),
							IsUnhandledException = isUnhandledException,
							ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
							DelegateMethodName = string.IsNullOrWhiteSpace(delegateName)
								? iinvocationContext.TraceFrameStack.LastFrame ?? new TraceFrame().Frame
								: delegateName!,
						});
					}
					catch (Exception profilerEx)
					{
						var error = new ErrorMessageBuilder(
							iinvocationContext.InvocationCreateNew(),
							Exceptions.Internal.ErrorCodes.ResultException.InvocationResult)
							.ExceptionInfo(profilerEx)
							.Build();

						result.WithError(error);

						try
						{
							iinvocationContext.Logger?.LogErrorMessage(error);
						}
						catch (Exception loggerEx)
						{
							result.WithError(
								iinvocationContext.InvocationCreateNew(),
								Exceptions.Internal.ErrorCodes.ResultException.InvocationResult,
								x => x.ExceptionInfo(loggerEx));
						}
					}
				}
			}

			//disposableInvocationContext?.Dispose();
		}
	}

	public static IResult Call<TInvocationContext, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
		Action<TInvocationContext, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> @delegate,
		TInvocationContext invocationContext,
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
		[CallerArgumentExpression(nameof(@delegate))] string? delegateName = null)
		where TInvocationContext : IInvocationContext
	{
		var result = new ResultBuilder();

		var delegateWasCalled = false;
		var isUnhandledException = false;
		Stopwatch? stopwatch = null;
		IInvocationContext iinvocationContext = invocationContext;
		InvocationContext? disposableInvocationContext = null;

		try
		{
			Throw.IfNull(iinvocationContext);

			if (iinvocationContext.InvocationResultSyncCallback != null)
				stopwatch = Stopwatch.StartNew();

			if (result.IsArgumentNull(iinvocationContext.InvocationCreateNew(), @delegate))
			{
				var logResult = iinvocationContext.LogResultErrorMessages(result.Build());
				result.MergeErrors(logResult);

				return result.Build();
			}

			delegateWasCalled = true;

			@delegate(invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);

			return result.Build();
		}
		catch (Exception ex)
		{
			isUnhandledException = true;
			stopwatch?.Stop();

			if (iinvocationContext == null)
			{
				disposableInvocationContext = new InvocationContextBuilder(ScopeContext.Create($"{nameof(Legion)} > {nameof(Result)}")).Build();
				iinvocationContext = disposableInvocationContext;
			}

			result.WithError(
				iinvocationContext.InvocationCreateNew(),
				iinvocationContext.UnhandledErrorCode ?? Exceptions.Internal.ErrorCodes.ResultException.Unhandled,
				x => x.ExceptionInfo(ex));

			var res = result.Build();

			var logResult = iinvocationContext.LogResultErrorMessages(res);
			result.MergeErrors(logResult);

			if (iinvocationContext.InvocationResultSyncCallback != null)
			{
				try
				{
					iinvocationContext.InvocationResultSyncCallback(new InvocationResult
					{
						InvocationContext = iinvocationContext.InvocationCreateNew(),
						DelegateWasCalled = delegateWasCalled,
						Result = result.Build(),
						IsUnhandledException = isUnhandledException,
						ElapsedMilliseconds = stopwatch?.ElapsedMilliseconds ?? -1,
						DelegateMethodName = string.IsNullOrWhiteSpace(delegateName)
							? iinvocationContext.TraceFrameStack.LastFrame ?? new TraceFrame().Frame
							: delegateName!,
					});
				}
				catch (Exception profilerEx)
				{
					var error = new ErrorMessageBuilder(
						iinvocationContext.InvocationCreateNew(),
						Exceptions.Internal.ErrorCodes.ResultException.InvocationResult)
						.ExceptionInfo(profilerEx)
						.Build();

					result.WithError(error);

					try
					{
						iinvocationContext.Logger?.LogErrorMessage(error);
					}
					catch (Exception loggerEx)
					{
						result.WithError(
							iinvocationContext.InvocationCreateNew(),
							Exceptions.Internal.ErrorCodes.ResultException.InvocationResult,
							x => x.ExceptionInfo(loggerEx));
					}
				}
			}

			return res;
		}
		finally
		{
			if (stopwatch != null && !isUnhandledException)
			{
				stopwatch.Stop();
				if (iinvocationContext.InvocationResultSyncCallback != null)
				{
					try
					{
						iinvocationContext.InvocationResultSyncCallback(new InvocationResult
						{
							InvocationContext = iinvocationContext.InvocationCreateNew(),
							DelegateWasCalled = delegateWasCalled,
							Result = result.Build(),
							IsUnhandledException = isUnhandledException,
							ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
							DelegateMethodName = string.IsNullOrWhiteSpace(delegateName)
								? iinvocationContext.TraceFrameStack.LastFrame ?? new TraceFrame().Frame
								: delegateName!,
						});
					}
					catch (Exception profilerEx)
					{
						var error = new ErrorMessageBuilder(
							iinvocationContext.InvocationCreateNew(),
							Exceptions.Internal.ErrorCodes.ResultException.InvocationResult)
							.ExceptionInfo(profilerEx)
							.Build();

						result.WithError(error);

						try
						{
							iinvocationContext.Logger?.LogErrorMessage(error);
						}
						catch (Exception loggerEx)
						{
							result.WithError(
								iinvocationContext.InvocationCreateNew(),
								Exceptions.Internal.ErrorCodes.ResultException.InvocationResult,
								x => x.ExceptionInfo(loggerEx));
						}
					}
				}
			}

			//disposableInvocationContext?.Dispose();
		}
	}

	public static IResult Call<TInvocationContext, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
		Action<TInvocationContext, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> @delegate,
		TInvocationContext invocationContext,
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
		[CallerArgumentExpression(nameof(@delegate))] string? delegateName = null)
		where TInvocationContext : IInvocationContext
	{
		var result = new ResultBuilder();

		var delegateWasCalled = false;
		var isUnhandledException = false;
		Stopwatch? stopwatch = null;
		IInvocationContext iinvocationContext = invocationContext;
		InvocationContext? disposableInvocationContext = null;

		try
		{
			Throw.IfNull(iinvocationContext);

			if (iinvocationContext.InvocationResultSyncCallback != null)
				stopwatch = Stopwatch.StartNew();

			if (result.IsArgumentNull(iinvocationContext.InvocationCreateNew(), @delegate))
			{
				var logResult = iinvocationContext.LogResultErrorMessages(result.Build());
				result.MergeErrors(logResult);

				return result.Build();
			}

			delegateWasCalled = true;

			@delegate(invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);

			return result.Build();
		}
		catch (Exception ex)
		{
			isUnhandledException = true;
			stopwatch?.Stop();

			if (iinvocationContext == null)
			{
				disposableInvocationContext = new InvocationContextBuilder(ScopeContext.Create($"{nameof(Legion)} > {nameof(Result)}")).Build();
				iinvocationContext = disposableInvocationContext;
			}

			result.WithError(
				iinvocationContext.InvocationCreateNew(),
				iinvocationContext.UnhandledErrorCode ?? Exceptions.Internal.ErrorCodes.ResultException.Unhandled,
				x => x.ExceptionInfo(ex));

			var res = result.Build();

			var logResult = iinvocationContext.LogResultErrorMessages(res);
			result.MergeErrors(logResult);

			if (iinvocationContext.InvocationResultSyncCallback != null)
			{
				try
				{
					iinvocationContext.InvocationResultSyncCallback(new InvocationResult
					{
						InvocationContext = iinvocationContext.InvocationCreateNew(),
						DelegateWasCalled = delegateWasCalled,
						Result = result.Build(),
						IsUnhandledException = isUnhandledException,
						ElapsedMilliseconds = stopwatch?.ElapsedMilliseconds ?? -1,
						DelegateMethodName = string.IsNullOrWhiteSpace(delegateName)
							? iinvocationContext.TraceFrameStack.LastFrame ?? new TraceFrame().Frame
							: delegateName!,
					});
				}
				catch (Exception profilerEx)
				{
					var error = new ErrorMessageBuilder(
						iinvocationContext.InvocationCreateNew(),
						Exceptions.Internal.ErrorCodes.ResultException.InvocationResult)
						.ExceptionInfo(profilerEx)
						.Build();

					result.WithError(error);

					try
					{
						iinvocationContext.Logger?.LogErrorMessage(error);
					}
					catch (Exception loggerEx)
					{
						result.WithError(
							iinvocationContext.InvocationCreateNew(),
							Exceptions.Internal.ErrorCodes.ResultException.InvocationResult,
							x => x.ExceptionInfo(loggerEx));
					}
				}
			}

			return res;
		}
		finally
		{
			if (stopwatch != null && !isUnhandledException)
			{
				stopwatch.Stop();
				if (iinvocationContext.InvocationResultSyncCallback != null)
				{
					try
					{
						iinvocationContext.InvocationResultSyncCallback(new InvocationResult
						{
							InvocationContext = iinvocationContext.InvocationCreateNew(),
							DelegateWasCalled = delegateWasCalled,
							Result = result.Build(),
							IsUnhandledException = isUnhandledException,
							ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
							DelegateMethodName = string.IsNullOrWhiteSpace(delegateName)
								? iinvocationContext.TraceFrameStack.LastFrame ?? new TraceFrame().Frame
								: delegateName!,
						});
					}
					catch (Exception profilerEx)
					{
						var error = new ErrorMessageBuilder(
							iinvocationContext.InvocationCreateNew(),
							Exceptions.Internal.ErrorCodes.ResultException.InvocationResult)
							.ExceptionInfo(profilerEx)
							.Build();

						result.WithError(error);

						try
						{
							iinvocationContext.Logger?.LogErrorMessage(error);
						}
						catch (Exception loggerEx)
						{
							result.WithError(
								iinvocationContext.InvocationCreateNew(),
								Exceptions.Internal.ErrorCodes.ResultException.InvocationResult,
								x => x.ExceptionInfo(loggerEx));
						}
					}
				}
			}

			//disposableInvocationContext?.Dispose();
		}
	}
}
