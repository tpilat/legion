using Legion.Logging;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Legion;

public partial class Result : IResult
{
	public static async Task<IResult<TResult>> CallAsync<TInvocationContext, TResult>(
		Func<TInvocationContext, CancellationToken, Task<IResult<TResult>>> @delegate,
		TInvocationContext invocationContext,
		CancellationToken cancellationToken = default,
		[CallerArgumentExpression(nameof(@delegate))] string? delegateName = null)
		where TInvocationContext : IInvocationContext
	{
		var result = new ResultBuilder<TResult>();

		var delegateWasCalled = false;
		var isUnhandledException = false;
		Stopwatch? stopwatch = null;
		IInvocationContext iinvocationContext = invocationContext;
		InvocationContext? disposableInvocationContext = null;

		try
		{
			Throw.IfNull(iinvocationContext);

			if (iinvocationContext.InvocationResultAsyncCallback != null)
				stopwatch = Stopwatch.StartNew();

			if (result.IsArgumentNull(iinvocationContext.InvocationCreateNew(), @delegate))
			{
				var logResult = iinvocationContext.LogResultErrorMessages(result.Build());
				result.MergeErrors(logResult);

				return result.Build();
			}

			delegateWasCalled = true;

			var res = await @delegate(invocationContext, invocationContext.CancellationToken.HasValue ? (cancellationToken == default ? invocationContext.CancellationToken.Value : cancellationToken) : cancellationToken);

			var logRes = iinvocationContext.LogResultErrorMessages(res);
			new ResultBuilder<TResult>(res).MergeErrors(logRes);
			return res;
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

			if (iinvocationContext.InvocationResultAsyncCallback != null)
			{
				try
				{
					await iinvocationContext.InvocationResultAsyncCallback(new InvocationResult
					{
						InvocationContext = iinvocationContext.InvocationCreateNew(),
						DelegateWasCalled = delegateWasCalled,
						Result = result.Build(),
						IsUnhandledException = isUnhandledException,
						ElapsedMilliseconds = stopwatch?.ElapsedMilliseconds ?? -1,
						DelegateMethodName = string.IsNullOrWhiteSpace(delegateName)
							? iinvocationContext.TraceFrameStack.LastFrame ?? new TraceFrame().Frame
							: delegateName!,
					},
					invocationContext.CancellationToken.HasValue ? (cancellationToken == default ? invocationContext.CancellationToken.Value : cancellationToken) : cancellationToken);
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
				if (iinvocationContext.InvocationResultAsyncCallback != null)
				{
					try
					{
						await iinvocationContext.InvocationResultAsyncCallback(new InvocationResult
						{
							InvocationContext = iinvocationContext.InvocationCreateNew(),
							DelegateWasCalled = delegateWasCalled,
							Result = result.Build(),
							IsUnhandledException = isUnhandledException,
							ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
							DelegateMethodName = string.IsNullOrWhiteSpace(delegateName)
								? iinvocationContext.TraceFrameStack.LastFrame ?? new TraceFrame().Frame
								: delegateName!,
						},
						invocationContext.CancellationToken.HasValue ? (cancellationToken == default ? invocationContext.CancellationToken.Value : cancellationToken) : cancellationToken);
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

			//if (disposableInvocationContext != null)
			//	await disposableInvocationContext.DisposeAsync();
		}
	}

	public static async Task<IResult<TResult>> CallAsync<TInvocationContext, T, TResult>(
		Func<TInvocationContext, T, CancellationToken, Task<IResult<TResult>>> @delegate,
		TInvocationContext invocationContext,
		T arg,
		CancellationToken cancellationToken = default,
		[CallerArgumentExpression(nameof(@delegate))] string? delegateName = null)
		where TInvocationContext : IInvocationContext
	{
		var result = new ResultBuilder<TResult>();

		var delegateWasCalled = false;
		var isUnhandledException = false;
		Stopwatch? stopwatch = null;
		IInvocationContext iinvocationContext = invocationContext;
		InvocationContext? disposableInvocationContext = null;

		try
		{
			Throw.IfNull(iinvocationContext);

			if (iinvocationContext.InvocationResultAsyncCallback != null)
				stopwatch = Stopwatch.StartNew();

			if (result.IsArgumentNull(iinvocationContext.InvocationCreateNew(), @delegate))
			{
				var logResult = iinvocationContext.LogResultErrorMessages(result.Build());
				result.MergeErrors(logResult);

				return result.Build();
			}

			delegateWasCalled = true;

			var res = await @delegate(invocationContext, arg, invocationContext.CancellationToken.HasValue ? (cancellationToken == default ? invocationContext.CancellationToken.Value : cancellationToken) : cancellationToken);

			var logRes = iinvocationContext.LogResultErrorMessages(res);
			new ResultBuilder<TResult>(res).MergeErrors(logRes);
			return res;
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

			if (iinvocationContext.InvocationResultAsyncCallback != null)
			{
				try
				{
					await iinvocationContext.InvocationResultAsyncCallback(new InvocationResult
					{
						InvocationContext = iinvocationContext.InvocationCreateNew(),
						DelegateWasCalled = delegateWasCalled,
						Result = result.Build(),
						IsUnhandledException = isUnhandledException,
						ElapsedMilliseconds = stopwatch?.ElapsedMilliseconds ?? -1,
						DelegateMethodName = string.IsNullOrWhiteSpace(delegateName)
							? iinvocationContext.TraceFrameStack.LastFrame ?? new TraceFrame().Frame
							: delegateName!,
					},
					invocationContext.CancellationToken.HasValue ? (cancellationToken == default ? invocationContext.CancellationToken.Value : cancellationToken) : cancellationToken);
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
				if (iinvocationContext.InvocationResultAsyncCallback != null)
				{
					try
					{
						await iinvocationContext.InvocationResultAsyncCallback(new InvocationResult
						{
							InvocationContext = iinvocationContext.InvocationCreateNew(),
							DelegateWasCalled = delegateWasCalled,
							Result = result.Build(),
							IsUnhandledException = isUnhandledException,
							ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
							DelegateMethodName = string.IsNullOrWhiteSpace(delegateName)
								? iinvocationContext.TraceFrameStack.LastFrame ?? new TraceFrame().Frame
								: delegateName!,
						},
						invocationContext.CancellationToken.HasValue ? (cancellationToken == default ? invocationContext.CancellationToken.Value : cancellationToken) : cancellationToken);
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

			//if (disposableInvocationContext != null)
			//	await disposableInvocationContext.DisposeAsync();
		}
	}

	public static async Task<IResult<TResult>> CallAsync<TInvocationContext, T1, T2, TResult>(
		Func<TInvocationContext, T1, T2, CancellationToken, Task<IResult<TResult>>> @delegate,
		TInvocationContext invocationContext,
		T1 arg1,
		T2 arg2,
		CancellationToken cancellationToken = default,
		[CallerArgumentExpression(nameof(@delegate))] string? delegateName = null)
		where TInvocationContext : IInvocationContext
	{
		var result = new ResultBuilder<TResult>();

		var delegateWasCalled = false;
		var isUnhandledException = false;
		Stopwatch? stopwatch = null;
		IInvocationContext iinvocationContext = invocationContext;
		InvocationContext? disposableInvocationContext = null;

		try
		{
			Throw.IfNull(iinvocationContext);

			if (iinvocationContext.InvocationResultAsyncCallback != null)
				stopwatch = Stopwatch.StartNew();

			if (result.IsArgumentNull(iinvocationContext.InvocationCreateNew(), @delegate))
			{
				var logResult = iinvocationContext.LogResultErrorMessages(result.Build());
				result.MergeErrors(logResult);

				return result.Build();
			}

			delegateWasCalled = true;

			var res = await @delegate(invocationContext, arg1, arg2, invocationContext.CancellationToken.HasValue ? (cancellationToken == default ? invocationContext.CancellationToken.Value : cancellationToken) : cancellationToken);

			var logRes = iinvocationContext.LogResultErrorMessages(res);
			new ResultBuilder<TResult>(res).MergeErrors(logRes);
			return res;
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

			if (iinvocationContext.InvocationResultAsyncCallback != null)
			{
				try
				{
					await iinvocationContext.InvocationResultAsyncCallback(new InvocationResult
					{
						InvocationContext = iinvocationContext.InvocationCreateNew(),
						DelegateWasCalled = delegateWasCalled,
						Result = result.Build(),
						IsUnhandledException = isUnhandledException,
						ElapsedMilliseconds = stopwatch?.ElapsedMilliseconds ?? -1,
						DelegateMethodName = string.IsNullOrWhiteSpace(delegateName)
							? iinvocationContext.TraceFrameStack.LastFrame ?? new TraceFrame().Frame
							: delegateName!,
					},
					invocationContext.CancellationToken.HasValue ? (cancellationToken == default ? invocationContext.CancellationToken.Value : cancellationToken) : cancellationToken);
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
				if (iinvocationContext.InvocationResultAsyncCallback != null)
				{
					try
					{
						await iinvocationContext.InvocationResultAsyncCallback(new InvocationResult
						{
							InvocationContext = iinvocationContext.InvocationCreateNew(),
							DelegateWasCalled = delegateWasCalled,
							Result = result.Build(),
							IsUnhandledException = isUnhandledException,
							ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
							DelegateMethodName = string.IsNullOrWhiteSpace(delegateName)
								? iinvocationContext.TraceFrameStack.LastFrame ?? new TraceFrame().Frame
								: delegateName!,
						},
						invocationContext.CancellationToken.HasValue ? (cancellationToken == default ? invocationContext.CancellationToken.Value : cancellationToken) : cancellationToken);
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

			//if (disposableInvocationContext != null)
			//	await disposableInvocationContext.DisposeAsync();
		}
	}

	public static async Task<IResult<TResult>> CallAsync<TInvocationContext, T1, T2, T3, TResult>(
		Func<TInvocationContext, T1, T2, T3, CancellationToken, Task<IResult<TResult>>> @delegate,
		TInvocationContext invocationContext,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		CancellationToken cancellationToken = default,
		[CallerArgumentExpression(nameof(@delegate))] string? delegateName = null)
		where TInvocationContext : IInvocationContext
	{
		var result = new ResultBuilder<TResult>();

		var delegateWasCalled = false;
		var isUnhandledException = false;
		Stopwatch? stopwatch = null;
		IInvocationContext iinvocationContext = invocationContext;
		InvocationContext? disposableInvocationContext = null;

		try
		{
			Throw.IfNull(iinvocationContext);

			if (iinvocationContext.InvocationResultAsyncCallback != null)
				stopwatch = Stopwatch.StartNew();

			if (result.IsArgumentNull(iinvocationContext.InvocationCreateNew(), @delegate))
			{
				var logResult = iinvocationContext.LogResultErrorMessages(result.Build());
				result.MergeErrors(logResult);

				return result.Build();
			}

			delegateWasCalled = true;

			var res = await @delegate(invocationContext, arg1, arg2, arg3, invocationContext.CancellationToken.HasValue ? (cancellationToken == default ? invocationContext.CancellationToken.Value : cancellationToken) : cancellationToken);

			var logRes = iinvocationContext.LogResultErrorMessages(res);
			new ResultBuilder<TResult>(res).MergeErrors(logRes);
			return res;
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

			if (iinvocationContext.InvocationResultAsyncCallback != null)
			{
				try
				{
					await iinvocationContext.InvocationResultAsyncCallback(new InvocationResult
					{
						InvocationContext = iinvocationContext.InvocationCreateNew(),
						DelegateWasCalled = delegateWasCalled,
						Result = result.Build(),
						IsUnhandledException = isUnhandledException,
						ElapsedMilliseconds = stopwatch?.ElapsedMilliseconds ?? -1,
						DelegateMethodName = string.IsNullOrWhiteSpace(delegateName)
							? iinvocationContext.TraceFrameStack.LastFrame ?? new TraceFrame().Frame
							: delegateName!,
					},
					invocationContext.CancellationToken.HasValue ? (cancellationToken == default ? invocationContext.CancellationToken.Value : cancellationToken) : cancellationToken);
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
				if (iinvocationContext.InvocationResultAsyncCallback != null)
				{
					try
					{
						await iinvocationContext.InvocationResultAsyncCallback(new InvocationResult
						{
							InvocationContext = iinvocationContext.InvocationCreateNew(),
							DelegateWasCalled = delegateWasCalled,
							Result = result.Build(),
							IsUnhandledException = isUnhandledException,
							ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
							DelegateMethodName = string.IsNullOrWhiteSpace(delegateName)
								? iinvocationContext.TraceFrameStack.LastFrame ?? new TraceFrame().Frame
								: delegateName!,
						},
						invocationContext.CancellationToken.HasValue ? (cancellationToken == default ? invocationContext.CancellationToken.Value : cancellationToken) : cancellationToken);
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

			//if (disposableInvocationContext != null)
			//	await disposableInvocationContext.DisposeAsync();
		}
	}

	public static async Task<IResult<TResult>> CallAsync<TInvocationContext, T1, T2, T3, T4, TResult>(
		Func<TInvocationContext, T1, T2, T3, T4, CancellationToken, Task<IResult<TResult>>> @delegate,
		TInvocationContext invocationContext,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		CancellationToken cancellationToken = default,
		[CallerArgumentExpression(nameof(@delegate))] string? delegateName = null)
		where TInvocationContext : IInvocationContext
	{
		var result = new ResultBuilder<TResult>();

		var delegateWasCalled = false;
		var isUnhandledException = false;
		Stopwatch? stopwatch = null;
		IInvocationContext iinvocationContext = invocationContext;
		InvocationContext? disposableInvocationContext = null;

		try
		{
			Throw.IfNull(iinvocationContext);

			if (iinvocationContext.InvocationResultAsyncCallback != null)
				stopwatch = Stopwatch.StartNew();

			if (result.IsArgumentNull(iinvocationContext.InvocationCreateNew(), @delegate))
			{
				var logResult = iinvocationContext.LogResultErrorMessages(result.Build());
				result.MergeErrors(logResult);

				return result.Build();
			}

			delegateWasCalled = true;

			var res = await @delegate(invocationContext, arg1, arg2, arg3, arg4, invocationContext.CancellationToken.HasValue ? (cancellationToken == default ? invocationContext.CancellationToken.Value : cancellationToken) : cancellationToken);

			var logRes = iinvocationContext.LogResultErrorMessages(res);
			new ResultBuilder<TResult>(res).MergeErrors(logRes);
			return res;
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

			if (iinvocationContext.InvocationResultAsyncCallback != null)
			{
				try
				{
					await iinvocationContext.InvocationResultAsyncCallback(new InvocationResult
					{
						InvocationContext = iinvocationContext.InvocationCreateNew(),
						DelegateWasCalled = delegateWasCalled,
						Result = result.Build(),
						IsUnhandledException = isUnhandledException,
						ElapsedMilliseconds = stopwatch?.ElapsedMilliseconds ?? -1,
						DelegateMethodName = string.IsNullOrWhiteSpace(delegateName)
							? iinvocationContext.TraceFrameStack.LastFrame ?? new TraceFrame().Frame
							: delegateName!,
					},
					invocationContext.CancellationToken.HasValue ? (cancellationToken == default ? invocationContext.CancellationToken.Value : cancellationToken) : cancellationToken);
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
				if (iinvocationContext.InvocationResultAsyncCallback != null)
				{
					try
					{
						await iinvocationContext.InvocationResultAsyncCallback(new InvocationResult
						{
							InvocationContext = iinvocationContext.InvocationCreateNew(),
							DelegateWasCalled = delegateWasCalled,
							Result = result.Build(),
							IsUnhandledException = isUnhandledException,
							ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
							DelegateMethodName = string.IsNullOrWhiteSpace(delegateName)
								? iinvocationContext.TraceFrameStack.LastFrame ?? new TraceFrame().Frame
								: delegateName!,
						},
						invocationContext.CancellationToken.HasValue ? (cancellationToken == default ? invocationContext.CancellationToken.Value : cancellationToken) : cancellationToken);
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

			//if (disposableInvocationContext != null)
			//	await disposableInvocationContext.DisposeAsync();
		}
	}

	public static async Task<IResult<TResult>> CallAsync<TInvocationContext, T1, T2, T3, T4, T5, TResult>(
		Func<TInvocationContext, T1, T2, T3, T4, T5, CancellationToken, Task<IResult<TResult>>> @delegate,
		TInvocationContext invocationContext,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		T5 arg5,
		CancellationToken cancellationToken = default,
		[CallerArgumentExpression(nameof(@delegate))] string? delegateName = null)
		where TInvocationContext : IInvocationContext
	{
		var result = new ResultBuilder<TResult>();

		var delegateWasCalled = false;
		var isUnhandledException = false;
		Stopwatch? stopwatch = null;
		IInvocationContext iinvocationContext = invocationContext;
		InvocationContext? disposableInvocationContext = null;

		try
		{
			Throw.IfNull(iinvocationContext);

			if (iinvocationContext.InvocationResultAsyncCallback != null)
				stopwatch = Stopwatch.StartNew();

			if (result.IsArgumentNull(iinvocationContext.InvocationCreateNew(), @delegate))
			{
				var logResult = iinvocationContext.LogResultErrorMessages(result.Build());
				result.MergeErrors(logResult);

				return result.Build();
			}

			delegateWasCalled = true;

			var res = await @delegate(invocationContext, arg1, arg2, arg3, arg4, arg5, invocationContext.CancellationToken.HasValue ? (cancellationToken == default ? invocationContext.CancellationToken.Value : cancellationToken) : cancellationToken);

			var logRes = iinvocationContext.LogResultErrorMessages(res);
			new ResultBuilder<TResult>(res).MergeErrors(logRes);
			return res;
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

			if (iinvocationContext.InvocationResultAsyncCallback != null)
			{
				try
				{
					await iinvocationContext.InvocationResultAsyncCallback(new InvocationResult
					{
						InvocationContext = iinvocationContext.InvocationCreateNew(),
						DelegateWasCalled = delegateWasCalled,
						Result = result.Build(),
						IsUnhandledException = isUnhandledException,
						ElapsedMilliseconds = stopwatch?.ElapsedMilliseconds ?? -1,
						DelegateMethodName = string.IsNullOrWhiteSpace(delegateName)
							? iinvocationContext.TraceFrameStack.LastFrame ?? new TraceFrame().Frame
							: delegateName!,
					},
					invocationContext.CancellationToken.HasValue ? (cancellationToken == default ? invocationContext.CancellationToken.Value : cancellationToken) : cancellationToken);
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
				if (iinvocationContext.InvocationResultAsyncCallback != null)
				{
					try
					{
						await iinvocationContext.InvocationResultAsyncCallback(new InvocationResult
						{
							InvocationContext = iinvocationContext.InvocationCreateNew(),
							DelegateWasCalled = delegateWasCalled,
							Result = result.Build(),
							IsUnhandledException = isUnhandledException,
							ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
							DelegateMethodName = string.IsNullOrWhiteSpace(delegateName)
								? iinvocationContext.TraceFrameStack.LastFrame ?? new TraceFrame().Frame
								: delegateName!,
						},
						invocationContext.CancellationToken.HasValue ? (cancellationToken == default ? invocationContext.CancellationToken.Value : cancellationToken) : cancellationToken);
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

			//if (disposableInvocationContext != null)
			//	await disposableInvocationContext.DisposeAsync();
		}
	}

	public static async Task<IResult<TResult>> CallAsync<TInvocationContext, T1, T2, T3, T4, T5, T6, TResult>(
		Func<TInvocationContext, T1, T2, T3, T4, T5, T6, CancellationToken, Task<IResult<TResult>>> @delegate,
		TInvocationContext invocationContext,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		T5 arg5,
		T6 arg6,
		CancellationToken cancellationToken = default,
		[CallerArgumentExpression(nameof(@delegate))] string? delegateName = null)
		where TInvocationContext : IInvocationContext
	{
		var result = new ResultBuilder<TResult>();

		var delegateWasCalled = false;
		var isUnhandledException = false;
		Stopwatch? stopwatch = null;
		IInvocationContext iinvocationContext = invocationContext;
		InvocationContext? disposableInvocationContext = null;

		try
		{
			Throw.IfNull(iinvocationContext);

			if (iinvocationContext.InvocationResultAsyncCallback != null)
				stopwatch = Stopwatch.StartNew();

			if (result.IsArgumentNull(iinvocationContext.InvocationCreateNew(), @delegate))
			{
				var logResult = iinvocationContext.LogResultErrorMessages(result.Build());
				result.MergeErrors(logResult);

				return result.Build();
			}

			delegateWasCalled = true;

			var res = await @delegate(invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, invocationContext.CancellationToken.HasValue ? (cancellationToken == default ? invocationContext.CancellationToken.Value : cancellationToken) : cancellationToken);

			var logRes = iinvocationContext.LogResultErrorMessages(res);
			new ResultBuilder<TResult>(res).MergeErrors(logRes);
			return res;
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

			if (iinvocationContext.InvocationResultAsyncCallback != null)
			{
				try
				{
					await iinvocationContext.InvocationResultAsyncCallback(new InvocationResult
					{
						InvocationContext = iinvocationContext.InvocationCreateNew(),
						DelegateWasCalled = delegateWasCalled,
						Result = result.Build(),
						IsUnhandledException = isUnhandledException,
						ElapsedMilliseconds = stopwatch?.ElapsedMilliseconds ?? -1,
						DelegateMethodName = string.IsNullOrWhiteSpace(delegateName)
							? iinvocationContext.TraceFrameStack.LastFrame ?? new TraceFrame().Frame
							: delegateName!,
					},
					invocationContext.CancellationToken.HasValue ? (cancellationToken == default ? invocationContext.CancellationToken.Value : cancellationToken) : cancellationToken);
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
				if (iinvocationContext.InvocationResultAsyncCallback != null)
				{
					try
					{
						await iinvocationContext.InvocationResultAsyncCallback(new InvocationResult
						{
							InvocationContext = iinvocationContext.InvocationCreateNew(),
							DelegateWasCalled = delegateWasCalled,
							Result = result.Build(),
							IsUnhandledException = isUnhandledException,
							ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
							DelegateMethodName = string.IsNullOrWhiteSpace(delegateName)
								? iinvocationContext.TraceFrameStack.LastFrame ?? new TraceFrame().Frame
								: delegateName!,
						},
						invocationContext.CancellationToken.HasValue ? (cancellationToken == default ? invocationContext.CancellationToken.Value : cancellationToken) : cancellationToken);
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

			//if (disposableInvocationContext != null)
			//	await disposableInvocationContext.DisposeAsync();
		}
	}

	public static async Task<IResult<TResult>> CallAsync<TInvocationContext, T1, T2, T3, T4, T5, T6, T7, TResult>(
		Func<TInvocationContext, T1, T2, T3, T4, T5, T6, T7, CancellationToken, Task<IResult<TResult>>> @delegate,
		TInvocationContext invocationContext,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		T5 arg5,
		T6 arg6,
		T7 arg7,
		CancellationToken cancellationToken = default,
		[CallerArgumentExpression(nameof(@delegate))] string? delegateName = null)
		where TInvocationContext : IInvocationContext
	{
		var result = new ResultBuilder<TResult>();

		var delegateWasCalled = false;
		var isUnhandledException = false;
		Stopwatch? stopwatch = null;
		IInvocationContext iinvocationContext = invocationContext;
		InvocationContext? disposableInvocationContext = null;

		try
		{
			Throw.IfNull(iinvocationContext);

			if (iinvocationContext.InvocationResultAsyncCallback != null)
				stopwatch = Stopwatch.StartNew();

			if (result.IsArgumentNull(iinvocationContext.InvocationCreateNew(), @delegate))
			{
				var logResult = iinvocationContext.LogResultErrorMessages(result.Build());
				result.MergeErrors(logResult);

				return result.Build();
			}

			delegateWasCalled = true;

			var res = await @delegate(invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, arg7, invocationContext.CancellationToken.HasValue ? (cancellationToken == default ? invocationContext.CancellationToken.Value : cancellationToken) : cancellationToken);

			var logRes = iinvocationContext.LogResultErrorMessages(res);
			new ResultBuilder<TResult>(res).MergeErrors(logRes);
			return res;
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

			if (iinvocationContext.InvocationResultAsyncCallback != null)
			{
				try
				{
					await iinvocationContext.InvocationResultAsyncCallback(new InvocationResult
					{
						InvocationContext = iinvocationContext.InvocationCreateNew(),
						DelegateWasCalled = delegateWasCalled,
						Result = result.Build(),
						IsUnhandledException = isUnhandledException,
						ElapsedMilliseconds = stopwatch?.ElapsedMilliseconds ?? -1,
						DelegateMethodName = string.IsNullOrWhiteSpace(delegateName)
							? iinvocationContext.TraceFrameStack.LastFrame ?? new TraceFrame().Frame
							: delegateName!,
					},
					invocationContext.CancellationToken.HasValue ? (cancellationToken == default ? invocationContext.CancellationToken.Value : cancellationToken) : cancellationToken);
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
				if (iinvocationContext.InvocationResultAsyncCallback != null)
				{
					try
					{
						await iinvocationContext.InvocationResultAsyncCallback(new InvocationResult
						{
							InvocationContext = iinvocationContext.InvocationCreateNew(),
							DelegateWasCalled = delegateWasCalled,
							Result = result.Build(),
							IsUnhandledException = isUnhandledException,
							ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
							DelegateMethodName = string.IsNullOrWhiteSpace(delegateName)
								? iinvocationContext.TraceFrameStack.LastFrame ?? new TraceFrame().Frame
								: delegateName!,
						},
						invocationContext.CancellationToken.HasValue ? (cancellationToken == default ? invocationContext.CancellationToken.Value : cancellationToken) : cancellationToken);
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

			//if (disposableInvocationContext != null)
			//	await disposableInvocationContext.DisposeAsync();
		}
	}

	public static async Task<IResult<TResult>> CallAsync<TInvocationContext, T1, T2, T3, T4, T5, T6, T7, T8, TResult>(
		Func<TInvocationContext, T1, T2, T3, T4, T5, T6, T7, T8, CancellationToken, Task<IResult<TResult>>> @delegate,
		TInvocationContext invocationContext,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		T5 arg5,
		T6 arg6,
		T7 arg7,
		T8 arg8,
		CancellationToken cancellationToken = default,
		[CallerArgumentExpression(nameof(@delegate))] string? delegateName = null)
		where TInvocationContext : IInvocationContext
	{
		var result = new ResultBuilder<TResult>();

		var delegateWasCalled = false;
		var isUnhandledException = false;
		Stopwatch? stopwatch = null;
		IInvocationContext iinvocationContext = invocationContext;
		InvocationContext? disposableInvocationContext = null;

		try
		{
			Throw.IfNull(iinvocationContext);

			if (iinvocationContext.InvocationResultAsyncCallback != null)
				stopwatch = Stopwatch.StartNew();

			if (result.IsArgumentNull(iinvocationContext.InvocationCreateNew(), @delegate))
			{
				var logResult = iinvocationContext.LogResultErrorMessages(result.Build());
				result.MergeErrors(logResult);

				return result.Build();
			}

			delegateWasCalled = true;

			var res = await @delegate(invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, invocationContext.CancellationToken.HasValue ? (cancellationToken == default ? invocationContext.CancellationToken.Value : cancellationToken) : cancellationToken);

			var logRes = iinvocationContext.LogResultErrorMessages(res);
			new ResultBuilder<TResult>(res).MergeErrors(logRes);
			return res;
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

			if (iinvocationContext.InvocationResultAsyncCallback != null)
			{
				try
				{
					await iinvocationContext.InvocationResultAsyncCallback(new InvocationResult
					{
						InvocationContext = iinvocationContext.InvocationCreateNew(),
						DelegateWasCalled = delegateWasCalled,
						Result = result.Build(),
						IsUnhandledException = isUnhandledException,
						ElapsedMilliseconds = stopwatch?.ElapsedMilliseconds ?? -1,
						DelegateMethodName = string.IsNullOrWhiteSpace(delegateName)
							? iinvocationContext.TraceFrameStack.LastFrame ?? new TraceFrame().Frame
							: delegateName!,
					},
					invocationContext.CancellationToken.HasValue ? (cancellationToken == default ? invocationContext.CancellationToken.Value : cancellationToken) : cancellationToken);
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
				if (iinvocationContext.InvocationResultAsyncCallback != null)
				{
					try
					{
						await iinvocationContext.InvocationResultAsyncCallback(new InvocationResult
						{
							InvocationContext = iinvocationContext.InvocationCreateNew(),
							DelegateWasCalled = delegateWasCalled,
							Result = result.Build(),
							IsUnhandledException = isUnhandledException,
							ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
							DelegateMethodName = string.IsNullOrWhiteSpace(delegateName)
								? iinvocationContext.TraceFrameStack.LastFrame ?? new TraceFrame().Frame
								: delegateName!,
						},
						invocationContext.CancellationToken.HasValue ? (cancellationToken == default ? invocationContext.CancellationToken.Value : cancellationToken) : cancellationToken);
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

			//if (disposableInvocationContext != null)
			//	await disposableInvocationContext.DisposeAsync();
		}
	}

	public static async Task<IResult<TResult>> CallAsync<TInvocationContext, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(
		Func<TInvocationContext, T1, T2, T3, T4, T5, T6, T7, T8, T9, CancellationToken, Task<IResult<TResult>>> @delegate,
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
		CancellationToken cancellationToken = default,
		[CallerArgumentExpression(nameof(@delegate))] string? delegateName = null)
		where TInvocationContext : IInvocationContext
	{
		var result = new ResultBuilder<TResult>();

		var delegateWasCalled = false;
		var isUnhandledException = false;
		Stopwatch? stopwatch = null;
		IInvocationContext iinvocationContext = invocationContext;
		InvocationContext? disposableInvocationContext = null;

		try
		{
			Throw.IfNull(iinvocationContext);

			if (iinvocationContext.InvocationResultAsyncCallback != null)
				stopwatch = Stopwatch.StartNew();

			if (result.IsArgumentNull(iinvocationContext.InvocationCreateNew(), @delegate))
			{
				var logResult = iinvocationContext.LogResultErrorMessages(result.Build());
				result.MergeErrors(logResult);

				return result.Build();
			}

			delegateWasCalled = true;

			var res = await @delegate(invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, invocationContext.CancellationToken.HasValue ? (cancellationToken == default ? invocationContext.CancellationToken.Value : cancellationToken) : cancellationToken);

			var logRes = iinvocationContext.LogResultErrorMessages(res);
			new ResultBuilder<TResult>(res).MergeErrors(logRes);
			return res;
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

			if (iinvocationContext.InvocationResultAsyncCallback != null)
			{
				try
				{
					await iinvocationContext.InvocationResultAsyncCallback(new InvocationResult
					{
						InvocationContext = iinvocationContext.InvocationCreateNew(),
						DelegateWasCalled = delegateWasCalled,
						Result = result.Build(),
						IsUnhandledException = isUnhandledException,
						ElapsedMilliseconds = stopwatch?.ElapsedMilliseconds ?? -1,
						DelegateMethodName = string.IsNullOrWhiteSpace(delegateName)
							? iinvocationContext.TraceFrameStack.LastFrame ?? new TraceFrame().Frame
							: delegateName!,
					},
					invocationContext.CancellationToken.HasValue ? (cancellationToken == default ? invocationContext.CancellationToken.Value : cancellationToken) : cancellationToken);
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
				if (iinvocationContext.InvocationResultAsyncCallback != null)
				{
					try
					{
						await iinvocationContext.InvocationResultAsyncCallback(new InvocationResult
						{
							InvocationContext = iinvocationContext.InvocationCreateNew(),
							DelegateWasCalled = delegateWasCalled,
							Result = result.Build(),
							IsUnhandledException = isUnhandledException,
							ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
							DelegateMethodName = string.IsNullOrWhiteSpace(delegateName)
								? iinvocationContext.TraceFrameStack.LastFrame ?? new TraceFrame().Frame
								: delegateName!,
						},
						invocationContext.CancellationToken.HasValue ? (cancellationToken == default ? invocationContext.CancellationToken.Value : cancellationToken) : cancellationToken);
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

			//if (disposableInvocationContext != null)
			//	await disposableInvocationContext.DisposeAsync();
		}
	}

	public static async Task<IResult<TResult>> CallAsync<TInvocationContext, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(
		Func<TInvocationContext, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, CancellationToken, Task<IResult<TResult>>> @delegate,
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
		CancellationToken cancellationToken = default,
		[CallerArgumentExpression(nameof(@delegate))] string? delegateName = null)
		where TInvocationContext : IInvocationContext
	{
		var result = new ResultBuilder<TResult>();

		var delegateWasCalled = false;
		var isUnhandledException = false;
		Stopwatch? stopwatch = null;
		IInvocationContext iinvocationContext = invocationContext;
		InvocationContext? disposableInvocationContext = null;

		try
		{
			Throw.IfNull(iinvocationContext);

			if (iinvocationContext.InvocationResultAsyncCallback != null)
				stopwatch = Stopwatch.StartNew();

			if (result.IsArgumentNull(iinvocationContext.InvocationCreateNew(), @delegate))
			{
				var logResult = iinvocationContext.LogResultErrorMessages(result.Build());
				result.MergeErrors(logResult);

				return result.Build();
			}

			delegateWasCalled = true;

			var res = await @delegate(invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, invocationContext.CancellationToken.HasValue ? (cancellationToken == default ? invocationContext.CancellationToken.Value : cancellationToken) : cancellationToken);

			var logRes = iinvocationContext.LogResultErrorMessages(res);
			new ResultBuilder<TResult>(res).MergeErrors(logRes);
			return res;
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

			if (iinvocationContext.InvocationResultAsyncCallback != null)
			{
				try
				{
					await iinvocationContext.InvocationResultAsyncCallback(new InvocationResult
					{
						InvocationContext = iinvocationContext.InvocationCreateNew(),
						DelegateWasCalled = delegateWasCalled,
						Result = result.Build(),
						IsUnhandledException = isUnhandledException,
						ElapsedMilliseconds = stopwatch?.ElapsedMilliseconds ?? -1,
						DelegateMethodName = string.IsNullOrWhiteSpace(delegateName)
							? iinvocationContext.TraceFrameStack.LastFrame ?? new TraceFrame().Frame
							: delegateName!,
					},
					invocationContext.CancellationToken.HasValue ? (cancellationToken == default ? invocationContext.CancellationToken.Value : cancellationToken) : cancellationToken);
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
				if (iinvocationContext.InvocationResultAsyncCallback != null)
				{
					try
					{
						await iinvocationContext.InvocationResultAsyncCallback(new InvocationResult
						{
							InvocationContext = iinvocationContext.InvocationCreateNew(),
							DelegateWasCalled = delegateWasCalled,
							Result = result.Build(),
							IsUnhandledException = isUnhandledException,
							ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
							DelegateMethodName = string.IsNullOrWhiteSpace(delegateName)
								? iinvocationContext.TraceFrameStack.LastFrame ?? new TraceFrame().Frame
								: delegateName!,
						},
						invocationContext.CancellationToken.HasValue ? (cancellationToken == default ? invocationContext.CancellationToken.Value : cancellationToken) : cancellationToken);
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

			//if (disposableInvocationContext != null)
			//	await disposableInvocationContext.DisposeAsync();
		}
	}

	public static async Task<IResult<TResult>> CallAsync<TInvocationContext, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(
		Func<TInvocationContext, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, CancellationToken, Task<IResult<TResult>>> @delegate,
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
		CancellationToken cancellationToken = default,
		[CallerArgumentExpression(nameof(@delegate))] string? delegateName = null)
		where TInvocationContext : IInvocationContext
	{
		var result = new ResultBuilder<TResult>();

		var delegateWasCalled = false;
		var isUnhandledException = false;
		Stopwatch? stopwatch = null;
		IInvocationContext iinvocationContext = invocationContext;
		InvocationContext? disposableInvocationContext = null;

		try
		{
			Throw.IfNull(iinvocationContext);

			if (iinvocationContext.InvocationResultAsyncCallback != null)
				stopwatch = Stopwatch.StartNew();

			if (result.IsArgumentNull(iinvocationContext.InvocationCreateNew(), @delegate))
			{
				var logResult = iinvocationContext.LogResultErrorMessages(result.Build());
				result.MergeErrors(logResult);

				return result.Build();
			}

			delegateWasCalled = true;

			var res = await @delegate(invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, invocationContext.CancellationToken.HasValue ? (cancellationToken == default ? invocationContext.CancellationToken.Value : cancellationToken) : cancellationToken);

			var logRes = iinvocationContext.LogResultErrorMessages(res);
			new ResultBuilder<TResult>(res).MergeErrors(logRes);
			return res;
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

			if (iinvocationContext.InvocationResultAsyncCallback != null)
			{
				try
				{
					await iinvocationContext.InvocationResultAsyncCallback(new InvocationResult
					{
						InvocationContext = iinvocationContext.InvocationCreateNew(),
						DelegateWasCalled = delegateWasCalled,
						Result = result.Build(),
						IsUnhandledException = isUnhandledException,
						ElapsedMilliseconds = stopwatch?.ElapsedMilliseconds ?? -1,
						DelegateMethodName = string.IsNullOrWhiteSpace(delegateName)
							? iinvocationContext.TraceFrameStack.LastFrame ?? new TraceFrame().Frame
							: delegateName!,
					},
					invocationContext.CancellationToken.HasValue ? (cancellationToken == default ? invocationContext.CancellationToken.Value : cancellationToken) : cancellationToken);
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
				if (iinvocationContext.InvocationResultAsyncCallback != null)
				{
					try
					{
						await iinvocationContext.InvocationResultAsyncCallback(new InvocationResult
						{
							InvocationContext = iinvocationContext.InvocationCreateNew(),
							DelegateWasCalled = delegateWasCalled,
							Result = result.Build(),
							IsUnhandledException = isUnhandledException,
							ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
							DelegateMethodName = string.IsNullOrWhiteSpace(delegateName)
								? iinvocationContext.TraceFrameStack.LastFrame ?? new TraceFrame().Frame
								: delegateName!,
						},
						invocationContext.CancellationToken.HasValue ? (cancellationToken == default ? invocationContext.CancellationToken.Value : cancellationToken) : cancellationToken);
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

			//if (disposableInvocationContext != null)
			//	await disposableInvocationContext.DisposeAsync();
		}
	}

	public static async Task<IResult<TResult>> CallAsync<TInvocationContext, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(
		Func<TInvocationContext, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, CancellationToken, Task<IResult<TResult>>> @delegate,
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
		CancellationToken cancellationToken = default,
		[CallerArgumentExpression(nameof(@delegate))] string? delegateName = null)
		where TInvocationContext : IInvocationContext
	{
		var result = new ResultBuilder<TResult>();

		var delegateWasCalled = false;
		var isUnhandledException = false;
		Stopwatch? stopwatch = null;
		IInvocationContext iinvocationContext = invocationContext;
		InvocationContext? disposableInvocationContext = null;

		try
		{
			Throw.IfNull(iinvocationContext);

			if (iinvocationContext.InvocationResultAsyncCallback != null)
				stopwatch = Stopwatch.StartNew();

			if (result.IsArgumentNull(iinvocationContext.InvocationCreateNew(), @delegate))
			{
				var logResult = iinvocationContext.LogResultErrorMessages(result.Build());
				result.MergeErrors(logResult);

				return result.Build();
			}

			delegateWasCalled = true;

			var res = await @delegate(invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, invocationContext.CancellationToken.HasValue ? (cancellationToken == default ? invocationContext.CancellationToken.Value : cancellationToken) : cancellationToken);

			var logRes = iinvocationContext.LogResultErrorMessages(res);
			new ResultBuilder<TResult>(res).MergeErrors(logRes);
			return res;
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

			if (iinvocationContext.InvocationResultAsyncCallback != null)
			{
				try
				{
					await iinvocationContext.InvocationResultAsyncCallback(new InvocationResult
					{
						InvocationContext = iinvocationContext.InvocationCreateNew(),
						DelegateWasCalled = delegateWasCalled,
						Result = result.Build(),
						IsUnhandledException = isUnhandledException,
						ElapsedMilliseconds = stopwatch?.ElapsedMilliseconds ?? -1,
						DelegateMethodName = string.IsNullOrWhiteSpace(delegateName)
							? iinvocationContext.TraceFrameStack.LastFrame ?? new TraceFrame().Frame
							: delegateName!,
					},
					invocationContext.CancellationToken.HasValue ? (cancellationToken == default ? invocationContext.CancellationToken.Value : cancellationToken) : cancellationToken);
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
				if (iinvocationContext.InvocationResultAsyncCallback != null)
				{
					try
					{
						await iinvocationContext.InvocationResultAsyncCallback(new InvocationResult
						{
							InvocationContext = iinvocationContext.InvocationCreateNew(),
							DelegateWasCalled = delegateWasCalled,
							Result = result.Build(),
							IsUnhandledException = isUnhandledException,
							ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
							DelegateMethodName = string.IsNullOrWhiteSpace(delegateName)
								? iinvocationContext.TraceFrameStack.LastFrame ?? new TraceFrame().Frame
								: delegateName!,
						},
						invocationContext.CancellationToken.HasValue ? (cancellationToken == default ? invocationContext.CancellationToken.Value : cancellationToken) : cancellationToken);
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

			//if (disposableInvocationContext != null)
			//	await disposableInvocationContext.DisposeAsync();
		}
	}

	public static async Task<IResult<TResult>> CallAsync<TInvocationContext, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(
		Func<TInvocationContext, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, CancellationToken, Task<IResult<TResult>>> @delegate,
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
		CancellationToken cancellationToken = default,
		[CallerArgumentExpression(nameof(@delegate))] string? delegateName = null)
		where TInvocationContext : IInvocationContext
	{
		var result = new ResultBuilder<TResult>();

		var delegateWasCalled = false;
		var isUnhandledException = false;
		Stopwatch? stopwatch = null;
		IInvocationContext iinvocationContext = invocationContext;
		InvocationContext? disposableInvocationContext = null;

		try
		{
			Throw.IfNull(iinvocationContext);

			if (iinvocationContext.InvocationResultAsyncCallback != null)
				stopwatch = Stopwatch.StartNew();

			if (result.IsArgumentNull(iinvocationContext.InvocationCreateNew(), @delegate))
			{
				var logResult = iinvocationContext.LogResultErrorMessages(result.Build());
				result.MergeErrors(logResult);

				return result.Build();
			}

			delegateWasCalled = true;

			var res = await @delegate(invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, invocationContext.CancellationToken.HasValue ? (cancellationToken == default ? invocationContext.CancellationToken.Value : cancellationToken) : cancellationToken);

			var logRes = iinvocationContext.LogResultErrorMessages(res);
			new ResultBuilder<TResult>(res).MergeErrors(logRes);
			return res;
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

			if (iinvocationContext.InvocationResultAsyncCallback != null)
			{
				try
				{
					await iinvocationContext.InvocationResultAsyncCallback(new InvocationResult
					{
						InvocationContext = iinvocationContext.InvocationCreateNew(),
						DelegateWasCalled = delegateWasCalled,
						Result = result.Build(),
						IsUnhandledException = isUnhandledException,
						ElapsedMilliseconds = stopwatch?.ElapsedMilliseconds ?? -1,
						DelegateMethodName = string.IsNullOrWhiteSpace(delegateName)
							? iinvocationContext.TraceFrameStack.LastFrame ?? new TraceFrame().Frame
							: delegateName!,
					},
					invocationContext.CancellationToken.HasValue ? (cancellationToken == default ? invocationContext.CancellationToken.Value : cancellationToken) : cancellationToken);
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
				if (iinvocationContext.InvocationResultAsyncCallback != null)
				{
					try
					{
						await iinvocationContext.InvocationResultAsyncCallback(new InvocationResult
						{
							InvocationContext = iinvocationContext.InvocationCreateNew(),
							DelegateWasCalled = delegateWasCalled,
							Result = result.Build(),
							IsUnhandledException = isUnhandledException,
							ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
							DelegateMethodName = string.IsNullOrWhiteSpace(delegateName)
								? iinvocationContext.TraceFrameStack.LastFrame ?? new TraceFrame().Frame
								: delegateName!,
						},
						invocationContext.CancellationToken.HasValue ? (cancellationToken == default ? invocationContext.CancellationToken.Value : cancellationToken) : cancellationToken);
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

			//if (disposableInvocationContext != null)
			//	await disposableInvocationContext.DisposeAsync();
		}
	}

	public static async Task<IResult<TResult>> CallAsync<TInvocationContext, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(
		Func<TInvocationContext, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, CancellationToken, Task<IResult<TResult>>> @delegate,
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
		CancellationToken cancellationToken = default,
		[CallerArgumentExpression(nameof(@delegate))] string? delegateName = null)
		where TInvocationContext : IInvocationContext
	{
		var result = new ResultBuilder<TResult>();

		var delegateWasCalled = false;
		var isUnhandledException = false;
		Stopwatch? stopwatch = null;
		IInvocationContext iinvocationContext = invocationContext;
		InvocationContext? disposableInvocationContext = null;

		try
		{
			Throw.IfNull(iinvocationContext);

			if (iinvocationContext.InvocationResultAsyncCallback != null)
				stopwatch = Stopwatch.StartNew();

			if (result.IsArgumentNull(iinvocationContext.InvocationCreateNew(), @delegate))
			{
				var logResult = iinvocationContext.LogResultErrorMessages(result.Build());
				result.MergeErrors(logResult);

				return result.Build();
			}

			delegateWasCalled = true;

			var res = await @delegate(invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, invocationContext.CancellationToken.HasValue ? (cancellationToken == default ? invocationContext.CancellationToken.Value : cancellationToken) : cancellationToken);

			var logRes = iinvocationContext.LogResultErrorMessages(res);
			new ResultBuilder<TResult>(res).MergeErrors(logRes);
			return res;
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

			if (iinvocationContext.InvocationResultAsyncCallback != null)
			{
				try
				{
					await iinvocationContext.InvocationResultAsyncCallback(new InvocationResult
					{
						InvocationContext = iinvocationContext.InvocationCreateNew(),
						DelegateWasCalled = delegateWasCalled,
						Result = result.Build(),
						IsUnhandledException = isUnhandledException,
						ElapsedMilliseconds = stopwatch?.ElapsedMilliseconds ?? -1,
						DelegateMethodName = string.IsNullOrWhiteSpace(delegateName)
							? iinvocationContext.TraceFrameStack.LastFrame ?? new TraceFrame().Frame
							: delegateName!,
					},
					invocationContext.CancellationToken.HasValue ? (cancellationToken == default ? invocationContext.CancellationToken.Value : cancellationToken) : cancellationToken);
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
				if (iinvocationContext.InvocationResultAsyncCallback != null)
				{
					try
					{
						await iinvocationContext.InvocationResultAsyncCallback(new InvocationResult
						{
							InvocationContext = iinvocationContext.InvocationCreateNew(),
							DelegateWasCalled = delegateWasCalled,
							Result = result.Build(),
							IsUnhandledException = isUnhandledException,
							ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
							DelegateMethodName = string.IsNullOrWhiteSpace(delegateName)
								? iinvocationContext.TraceFrameStack.LastFrame ?? new TraceFrame().Frame
								: delegateName!,
						},
						invocationContext.CancellationToken.HasValue ? (cancellationToken == default ? invocationContext.CancellationToken.Value : cancellationToken) : cancellationToken);
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

			//if (disposableInvocationContext != null)
			//	await disposableInvocationContext.DisposeAsync();
		}
	}
}
