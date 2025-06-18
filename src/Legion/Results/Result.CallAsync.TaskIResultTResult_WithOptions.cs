using Legion.Extensions;
using Legion.Logging;
using Legion.Policy;
using System.Runtime.CompilerServices;

namespace Legion;

public partial class Result : IResult
{
	public static async Task<IResult<TResult>> CallAsync<TInvocationContext, TResult>(
		Func<TInvocationContext, CancellationToken, Task<IResult<TResult>>> @delegate,
		TInvocationContext invocationContext,
		CallOptions callOptions,
		CancellationToken cancellationToken = default,
		[CallerArgumentExpression(nameof(@delegate))] string? delegateName = null)
		where TInvocationContext : IInvocationContext
	{
		var result = new ResultBuilder<TResult>();

		var delegateWasCalled = false;
		var isUnhandledException = false;
		IInvocationContext iinvocationContext = invocationContext;
		InvocationContext? disposableInvocationContext = null;

		try
		{
			Throw.IfNull(iinvocationContext);
			Throw.IfNull(callOptions);

			if (result.IsArgumentNull(iinvocationContext.InvocationCreateNew(), @delegate))
			{
				var logResult = iinvocationContext.LogResultErrorMessages(result.Build());
				result.MergeErrors(logResult);

				return result.Build();
			}

			delegateWasCalled = true;

			if (callOptions.FireAndForget)
			{
				var task = CallAsync(@delegate, invocationContext, cancellationToken, delegateName);
				task.FireAndForget();
				return result.Build();
			}
			else if (callOptions.RetryOptions != null)
			{
				var retryResult = result.Build();
				var retry = new RetryState(callOptions.RetryOptions);
				while (await retry.RetryWithDelayAsync())
				{
					try
					{
						if (callOptions.Timeout.HasValue)
						{
							var task = CallAsync(@delegate, invocationContext, cancellationToken, delegateName);
							retryResult = await task.OrTimeoutAsync(callOptions.Timeout.Value);
							if (!retryResult.HasError)
							{
								if (retry.CallBreak(retryResult.GetData()))
									break;
								else
									retry.CallNoBreakMatch(retryResult);
							}
						}
						else
						{
							retryResult = await CallAsync(@delegate, invocationContext, cancellationToken, delegateName);
							if (!retryResult.HasError)
							{
								if (retry.CallBreak(retryResult.GetData()))
									break;
								else
									retry.CallNoBreakMatch(retryResult);
							}
						}
					}
					catch (Exception retryEx)
					{
						retryResult = new ResultBuilder<TResult>()
							.WithInvalidOperationException(invocationContext, null, x => x.ExceptionInfo(retryEx));
					}
				}

				return retryResult;
			}
			else if (callOptions.Timeout.HasValue)
			{
				var task = CallAsync(@delegate, invocationContext, cancellationToken, delegateName);
				return await task.OrTimeoutAsync(callOptions.Timeout.Value);
			}
			else
			{
				return result.WithNotSupportedException(invocationContext, null, x => x.InternalMessage($"Invalid CallOptions"));
			}
		}
		catch (Exception ex)
		{
			isUnhandledException = true;

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
						ElapsedMilliseconds = -1,
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
		//finally
		//{
		//	if (disposableInvocationContext != null)
		//		await disposableInvocationContext.DisposeAsync();
		//}
	}

	public static async Task<IResult<TResult>> CallAsync<TInvocationContext, T, TResult>(
		Func<TInvocationContext, T, CancellationToken, Task<IResult<TResult>>> @delegate,
		TInvocationContext invocationContext,
		T arg,
		CallOptions callOptions,
		CancellationToken cancellationToken = default,
		[CallerArgumentExpression(nameof(@delegate))] string? delegateName = null)
		where TInvocationContext : IInvocationContext
	{
		var result = new ResultBuilder<TResult>();

		var delegateWasCalled = false;
		var isUnhandledException = false;
		IInvocationContext iinvocationContext = invocationContext;
		InvocationContext? disposableInvocationContext = null;

		try
		{
			Throw.IfNull(iinvocationContext);
			Throw.IfNull(callOptions);

			if (result.IsArgumentNull(iinvocationContext.InvocationCreateNew(), @delegate))
			{
				var logResult = iinvocationContext.LogResultErrorMessages(result.Build());
				result.MergeErrors(logResult);

				return result.Build();
			}

			delegateWasCalled = true;

			if (callOptions.FireAndForget)
			{
				var task = CallAsync(@delegate, invocationContext, arg, cancellationToken, delegateName);
				task.FireAndForget();
				return result.Build();
			}
			else if (callOptions.RetryOptions != null)
			{
				var retryResult = result.Build();
				var retry = new RetryState(callOptions.RetryOptions);
				while (await retry.RetryWithDelayAsync())
				{
					try
					{
						if (callOptions.Timeout.HasValue)
						{
							var task = CallAsync(@delegate, invocationContext, arg, cancellationToken, delegateName);
							retryResult = await task.OrTimeoutAsync(callOptions.Timeout.Value);
							if (!retryResult.HasError)
							{
								if (retry.CallBreak(retryResult.GetData()))
									break;
								else
									retry.CallNoBreakMatch(retryResult);
							}
						}
						else
						{
							retryResult = await CallAsync(@delegate, invocationContext, arg, cancellationToken, delegateName);
							if (!retryResult.HasError)
							{
								if (retry.CallBreak(retryResult.GetData()))
									break;
								else
									retry.CallNoBreakMatch(retryResult);
							}
						}
					}
					catch (Exception retryEx)
					{
						retryResult = new ResultBuilder<TResult>()
							.WithInvalidOperationException(invocationContext, null, x => x.ExceptionInfo(retryEx));
					}
				}

				return retryResult;
			}
			else if (callOptions.Timeout.HasValue)
			{
				var task = CallAsync(@delegate, invocationContext, arg, cancellationToken, delegateName);
				return await task.OrTimeoutAsync(callOptions.Timeout.Value);
			}
			else
			{
				return result.WithNotSupportedException(invocationContext, null, x => x.InternalMessage($"Invalid CallOptions"));
			}
		}
		catch (Exception ex)
		{
			isUnhandledException = true;

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
						ElapsedMilliseconds = -1,
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
		//finally
		//{
		//	if (disposableInvocationContext != null)
		//		await disposableInvocationContext.DisposeAsync();
		//}
	}

	public static async Task<IResult<TResult>> CallAsync<TInvocationContext, T1, T2, TResult>(
		Func<TInvocationContext, T1, T2, CancellationToken, Task<IResult<TResult>>> @delegate,
		TInvocationContext invocationContext,
		T1 arg1,
		T2 arg2,
		CallOptions callOptions,
		CancellationToken cancellationToken = default,
		[CallerArgumentExpression(nameof(@delegate))] string? delegateName = null)
		where TInvocationContext : IInvocationContext
	{
		var result = new ResultBuilder<TResult>();

		var delegateWasCalled = false;
		var isUnhandledException = false;
		IInvocationContext iinvocationContext = invocationContext;
		InvocationContext? disposableInvocationContext = null;

		try
		{
			Throw.IfNull(iinvocationContext);
			Throw.IfNull(callOptions);

			if (result.IsArgumentNull(iinvocationContext.InvocationCreateNew(), @delegate))
			{
				var logResult = iinvocationContext.LogResultErrorMessages(result.Build());
				result.MergeErrors(logResult);

				return result.Build();
			}

			delegateWasCalled = true;

			if (callOptions.FireAndForget)
			{
				var task = CallAsync(@delegate, invocationContext, arg1, arg2, cancellationToken, delegateName);
				task.FireAndForget();
				return result.Build();
			}
			else if (callOptions.RetryOptions != null)
			{
				var retryResult = result.Build();
				var retry = new RetryState(callOptions.RetryOptions);
				while (await retry.RetryWithDelayAsync())
				{
					try
					{
						if (callOptions.Timeout.HasValue)
						{
							var task = CallAsync(@delegate, invocationContext, arg1, arg2, cancellationToken, delegateName);
							retryResult = await task.OrTimeoutAsync(callOptions.Timeout.Value);
							if (!retryResult.HasError)
							{
								if (retry.CallBreak(retryResult.GetData()))
									break;
								else
									retry.CallNoBreakMatch(retryResult);
							}
						}
						else
						{
							retryResult = await CallAsync(@delegate, invocationContext, arg1, arg2, cancellationToken, delegateName);
							if (!retryResult.HasError)
							{
								if (retry.CallBreak(retryResult.GetData()))
									break;
								else
									retry.CallNoBreakMatch(retryResult);
							}
						}
					}
					catch (Exception retryEx)
					{
						retryResult = new ResultBuilder<TResult>()
							.WithInvalidOperationException(invocationContext, null, x => x.ExceptionInfo(retryEx));
					}
				}

				return retryResult;
			}
			else if (callOptions.Timeout.HasValue)
			{
				var task = CallAsync(@delegate, invocationContext, arg1, arg2, cancellationToken, delegateName);
				return await task.OrTimeoutAsync(callOptions.Timeout.Value);
			}
			else
			{
				return result.WithNotSupportedException(invocationContext, null, x => x.InternalMessage($"Invalid CallOptions"));
			}
		}
		catch (Exception ex)
		{
			isUnhandledException = true;

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
						ElapsedMilliseconds = -1,
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
		//finally
		//{
		//	if (disposableInvocationContext != null)
		//		await disposableInvocationContext.DisposeAsync();
		//}
	}

	public static async Task<IResult<TResult>> CallAsync<TInvocationContext, T1, T2, T3, TResult>(
		Func<TInvocationContext, T1, T2, T3, CancellationToken, Task<IResult<TResult>>> @delegate,
		TInvocationContext invocationContext,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		CallOptions callOptions,
		CancellationToken cancellationToken = default,
		[CallerArgumentExpression(nameof(@delegate))] string? delegateName = null)
		where TInvocationContext : IInvocationContext
	{
		var result = new ResultBuilder<TResult>();

		var delegateWasCalled = false;
		var isUnhandledException = false;
		IInvocationContext iinvocationContext = invocationContext;
		InvocationContext? disposableInvocationContext = null;

		try
		{
			Throw.IfNull(iinvocationContext);
			Throw.IfNull(callOptions);

			if (result.IsArgumentNull(iinvocationContext.InvocationCreateNew(), @delegate))
			{
				var logResult = iinvocationContext.LogResultErrorMessages(result.Build());
				result.MergeErrors(logResult);

				return result.Build();
			}

			delegateWasCalled = true;

			if (callOptions.FireAndForget)
			{
				var task = CallAsync(@delegate, invocationContext, arg1, arg2, arg3, cancellationToken, delegateName);
				task.FireAndForget();
				return result.Build();
			}
			else if (callOptions.RetryOptions != null)
			{
				var retryResult = result.Build();
				var retry = new RetryState(callOptions.RetryOptions);
				while (await retry.RetryWithDelayAsync())
				{
					try
					{
						if (callOptions.Timeout.HasValue)
						{
							var task = CallAsync(@delegate, invocationContext, arg1, arg2, arg3, cancellationToken, delegateName);
							retryResult = await task.OrTimeoutAsync(callOptions.Timeout.Value);
							if (!retryResult.HasError)
							{
								if (retry.CallBreak(retryResult.GetData()))
									break;
								else
									retry.CallNoBreakMatch(retryResult);
							}
						}
						else
						{
							retryResult = await CallAsync(@delegate, invocationContext, arg1, arg2, arg3, cancellationToken, delegateName);
							if (!retryResult.HasError)
							{
								if (retry.CallBreak(retryResult.GetData()))
									break;
								else
									retry.CallNoBreakMatch(retryResult);
							}
						}
					}
					catch (Exception retryEx)
					{
						retryResult = new ResultBuilder<TResult>()
							.WithInvalidOperationException(invocationContext, null, x => x.ExceptionInfo(retryEx));
					}
				}

				return retryResult;
			}
			else if (callOptions.Timeout.HasValue)
			{
				var task = CallAsync(@delegate, invocationContext, arg1, arg2, arg3, cancellationToken, delegateName);
				return await task.OrTimeoutAsync(callOptions.Timeout.Value);
			}
			else
			{
				return result.WithNotSupportedException(invocationContext, null, x => x.InternalMessage($"Invalid CallOptions"));
			}
		}
		catch (Exception ex)
		{
			isUnhandledException = true;

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
						ElapsedMilliseconds = -1,
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
		//finally
		//{
		//	if (disposableInvocationContext != null)
		//		await disposableInvocationContext.DisposeAsync();
		//}
	}

	public static async Task<IResult<TResult>> CallAsync<TInvocationContext, T1, T2, T3, T4, TResult>(
		Func<TInvocationContext, T1, T2, T3, T4, CancellationToken, Task<IResult<TResult>>> @delegate,
		TInvocationContext invocationContext,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		CallOptions callOptions,
		CancellationToken cancellationToken = default,
		[CallerArgumentExpression(nameof(@delegate))] string? delegateName = null)
		where TInvocationContext : IInvocationContext
	{
		var result = new ResultBuilder<TResult>();

		var delegateWasCalled = false;
		var isUnhandledException = false;
		IInvocationContext iinvocationContext = invocationContext;
		InvocationContext? disposableInvocationContext = null;

		try
		{
			Throw.IfNull(iinvocationContext);
			Throw.IfNull(callOptions);

			if (result.IsArgumentNull(iinvocationContext.InvocationCreateNew(), @delegate))
			{
				var logResult = iinvocationContext.LogResultErrorMessages(result.Build());
				result.MergeErrors(logResult);

				return result.Build();
			}

			delegateWasCalled = true;

			if (callOptions.FireAndForget)
			{
				var task = CallAsync(@delegate, invocationContext, arg1, arg2, arg3, arg4, cancellationToken, delegateName);
				task.FireAndForget();
				return result.Build();
			}
			else if (callOptions.RetryOptions != null)
			{
				var retryResult = result.Build();
				var retry = new RetryState(callOptions.RetryOptions);
				while (await retry.RetryWithDelayAsync())
				{
					try
					{
						if (callOptions.Timeout.HasValue)
						{
							var task = CallAsync(@delegate, invocationContext, arg1, arg2, arg3, arg4, cancellationToken, delegateName);
							retryResult = await task.OrTimeoutAsync(callOptions.Timeout.Value);
							if (!retryResult.HasError)
							{
								if (retry.CallBreak(retryResult.GetData()))
									break;
								else
									retry.CallNoBreakMatch(retryResult);
							}
						}
						else
						{
							retryResult = await CallAsync(@delegate, invocationContext, arg1, arg2, arg3, arg4, cancellationToken, delegateName);
							if (!retryResult.HasError)
							{
								if (retry.CallBreak(retryResult.GetData()))
									break;
								else
									retry.CallNoBreakMatch(retryResult);
							}
						}
					}
					catch (Exception retryEx)
					{
						retryResult = new ResultBuilder<TResult>()
							.WithInvalidOperationException(invocationContext, null, x => x.ExceptionInfo(retryEx));
					}
				}

				return retryResult;
			}
			else if (callOptions.Timeout.HasValue)
			{
				var task = CallAsync(@delegate, invocationContext, arg1, arg2, arg3, arg4, cancellationToken, delegateName);
				return await task.OrTimeoutAsync(callOptions.Timeout.Value);
			}
			else
			{
				return result.WithNotSupportedException(invocationContext, null, x => x.InternalMessage($"Invalid CallOptions"));
			}
		}
		catch (Exception ex)
		{
			isUnhandledException = true;

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
						ElapsedMilliseconds = -1,
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
		//finally
		//{
		//	if (disposableInvocationContext != null)
		//		await disposableInvocationContext.DisposeAsync();
		//}
	}

	public static async Task<IResult<TResult>> CallAsync<TInvocationContext, T1, T2, T3, T4, T5, TResult>(
		Func<TInvocationContext, T1, T2, T3, T4, T5, CancellationToken, Task<IResult<TResult>>> @delegate,
		TInvocationContext invocationContext,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		T5 arg5,
		CallOptions callOptions,
		CancellationToken cancellationToken = default,
		[CallerArgumentExpression(nameof(@delegate))] string? delegateName = null)
		where TInvocationContext : IInvocationContext
	{
		var result = new ResultBuilder<TResult>();

		var delegateWasCalled = false;
		var isUnhandledException = false;
		IInvocationContext iinvocationContext = invocationContext;
		InvocationContext? disposableInvocationContext = null;

		try
		{
			Throw.IfNull(iinvocationContext);
			Throw.IfNull(callOptions);

			if (result.IsArgumentNull(iinvocationContext.InvocationCreateNew(), @delegate))
			{
				var logResult = iinvocationContext.LogResultErrorMessages(result.Build());
				result.MergeErrors(logResult);

				return result.Build();
			}

			delegateWasCalled = true;

			if (callOptions.FireAndForget)
			{
				var task = CallAsync(@delegate, invocationContext, arg1, arg2, arg3, arg4, arg5, cancellationToken, delegateName);
				task.FireAndForget();
				return result.Build();
			}
			else if (callOptions.RetryOptions != null)
			{
				var retryResult = result.Build();
				var retry = new RetryState(callOptions.RetryOptions);
				while (await retry.RetryWithDelayAsync())
				{
					try
					{
						if (callOptions.Timeout.HasValue)
						{
							var task = CallAsync(@delegate, invocationContext, arg1, arg2, arg3, arg4, arg5, cancellationToken, delegateName);
							retryResult = await task.OrTimeoutAsync(callOptions.Timeout.Value);
							if (!retryResult.HasError)
							{
								if (retry.CallBreak(retryResult.GetData()))
									break;
								else
									retry.CallNoBreakMatch(retryResult);
							}
						}
						else
						{
							retryResult = await CallAsync(@delegate, invocationContext, arg1, arg2, arg3, arg4, arg5, cancellationToken, delegateName);
							if (!retryResult.HasError)
							{
								if (retry.CallBreak(retryResult.GetData()))
									break;
								else
									retry.CallNoBreakMatch(retryResult);
							}
						}
					}
					catch (Exception retryEx)
					{
						retryResult = new ResultBuilder<TResult>()
							.WithInvalidOperationException(invocationContext, null, x => x.ExceptionInfo(retryEx));
					}
				}

				return retryResult;
			}
			else if (callOptions.Timeout.HasValue)
			{
				var task = CallAsync(@delegate, invocationContext, arg1, arg2, arg3, arg4, arg5, cancellationToken, delegateName);
				return await task.OrTimeoutAsync(callOptions.Timeout.Value);
			}
			else
			{
				return result.WithNotSupportedException(invocationContext, null, x => x.InternalMessage($"Invalid CallOptions"));
			}
		}
		catch (Exception ex)
		{
			isUnhandledException = true;

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
						ElapsedMilliseconds = -1,
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
		//finally
		//{
		//	if (disposableInvocationContext != null)
		//		await disposableInvocationContext.DisposeAsync();
		//}
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
		CallOptions callOptions,
		CancellationToken cancellationToken = default,
		[CallerArgumentExpression(nameof(@delegate))] string? delegateName = null)
		where TInvocationContext : IInvocationContext
	{
		var result = new ResultBuilder<TResult>();

		var delegateWasCalled = false;
		var isUnhandledException = false;
		IInvocationContext iinvocationContext = invocationContext;
		InvocationContext? disposableInvocationContext = null;

		try
		{
			Throw.IfNull(iinvocationContext);
			Throw.IfNull(callOptions);

			if (result.IsArgumentNull(iinvocationContext.InvocationCreateNew(), @delegate))
			{
				var logResult = iinvocationContext.LogResultErrorMessages(result.Build());
				result.MergeErrors(logResult);

				return result.Build();
			}

			delegateWasCalled = true;

			if (callOptions.FireAndForget)
			{
				var task = CallAsync(@delegate, invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, cancellationToken, delegateName);
				task.FireAndForget();
				return result.Build();
			}
			else if (callOptions.RetryOptions != null)
			{
				var retryResult = result.Build();
				var retry = new RetryState(callOptions.RetryOptions);
				while (await retry.RetryWithDelayAsync())
				{
					try
					{
						if (callOptions.Timeout.HasValue)
						{
							var task = CallAsync(@delegate, invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, cancellationToken, delegateName);
							retryResult = await task.OrTimeoutAsync(callOptions.Timeout.Value);
							if (!retryResult.HasError)
							{
								if (retry.CallBreak(retryResult.GetData()))
									break;
								else
									retry.CallNoBreakMatch(retryResult);
							}
						}
						else
						{
							retryResult = await CallAsync(@delegate, invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, cancellationToken, delegateName);
							if (!retryResult.HasError)
							{
								if (retry.CallBreak(retryResult.GetData()))
									break;
								else
									retry.CallNoBreakMatch(retryResult);
							}
						}
					}
					catch (Exception retryEx)
					{
						retryResult = new ResultBuilder<TResult>()
							.WithInvalidOperationException(invocationContext, null, x => x.ExceptionInfo(retryEx));
					}
				}

				return retryResult;
			}
			else if (callOptions.Timeout.HasValue)
			{
				var task = CallAsync(@delegate, invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, cancellationToken, delegateName);
				return await task.OrTimeoutAsync(callOptions.Timeout.Value);
			}
			else
			{
				return result.WithNotSupportedException(invocationContext, null, x => x.InternalMessage($"Invalid CallOptions"));
			}
		}
		catch (Exception ex)
		{
			isUnhandledException = true;

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
						ElapsedMilliseconds = -1,
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
		//finally
		//{
		//	if (disposableInvocationContext != null)
		//		await disposableInvocationContext.DisposeAsync();
		//}
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
		CallOptions callOptions,
		CancellationToken cancellationToken = default,
		[CallerArgumentExpression(nameof(@delegate))] string? delegateName = null)
		where TInvocationContext : IInvocationContext
	{
		var result = new ResultBuilder<TResult>();

		var delegateWasCalled = false;
		var isUnhandledException = false;
		IInvocationContext iinvocationContext = invocationContext;
		InvocationContext? disposableInvocationContext = null;

		try
		{
			Throw.IfNull(iinvocationContext);
			Throw.IfNull(callOptions);

			if (result.IsArgumentNull(iinvocationContext.InvocationCreateNew(), @delegate))
			{
				var logResult = iinvocationContext.LogResultErrorMessages(result.Build());
				result.MergeErrors(logResult);

				return result.Build();
			}

			delegateWasCalled = true;

			if (callOptions.FireAndForget)
			{
				var task = CallAsync(@delegate, invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, arg7, cancellationToken, delegateName);
				task.FireAndForget();
				return result.Build();
			}
			else if (callOptions.RetryOptions != null)
			{
				var retryResult = result.Build();
				var retry = new RetryState(callOptions.RetryOptions);
				while (await retry.RetryWithDelayAsync())
				{
					try
					{
						if (callOptions.Timeout.HasValue)
						{
							var task = CallAsync(@delegate, invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, arg7, cancellationToken, delegateName);
							retryResult = await task.OrTimeoutAsync(callOptions.Timeout.Value);
							if (!retryResult.HasError)
							{
								if (retry.CallBreak(retryResult.GetData()))
									break;
								else
									retry.CallNoBreakMatch(retryResult);
							}
						}
						else
						{
							retryResult = await CallAsync(@delegate, invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, arg7, cancellationToken, delegateName);
							if (!retryResult.HasError)
							{
								if (retry.CallBreak(retryResult.GetData()))
									break;
								else
									retry.CallNoBreakMatch(retryResult);
							}
						}
					}
					catch (Exception retryEx)
					{
						retryResult = new ResultBuilder<TResult>()
							.WithInvalidOperationException(invocationContext, null, x => x.ExceptionInfo(retryEx));
					}
				}

				return retryResult;
			}
			else if (callOptions.Timeout.HasValue)
			{
				var task = CallAsync(@delegate, invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, arg7, cancellationToken, delegateName);
				return await task.OrTimeoutAsync(callOptions.Timeout.Value);
			}
			else
			{
				return result.WithNotSupportedException(invocationContext, null, x => x.InternalMessage($"Invalid CallOptions"));
			}
		}
		catch (Exception ex)
		{
			isUnhandledException = true;

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
						ElapsedMilliseconds = -1,
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
		//finally
		//{
		//	if (disposableInvocationContext != null)
		//		await disposableInvocationContext.DisposeAsync();
		//}
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
		CallOptions callOptions,
		CancellationToken cancellationToken = default,
		[CallerArgumentExpression(nameof(@delegate))] string? delegateName = null)
		where TInvocationContext : IInvocationContext
	{
		var result = new ResultBuilder<TResult>();

		var delegateWasCalled = false;
		var isUnhandledException = false;
		IInvocationContext iinvocationContext = invocationContext;
		InvocationContext? disposableInvocationContext = null;

		try
		{
			Throw.IfNull(iinvocationContext);
			Throw.IfNull(callOptions);

			if (result.IsArgumentNull(iinvocationContext.InvocationCreateNew(), @delegate))
			{
				var logResult = iinvocationContext.LogResultErrorMessages(result.Build());
				result.MergeErrors(logResult);

				return result.Build();
			}

			delegateWasCalled = true;

			if (callOptions.FireAndForget)
			{
				var task = CallAsync(@delegate, invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, cancellationToken, delegateName);
				task.FireAndForget();
				return result.Build();
			}
			else if (callOptions.RetryOptions != null)
			{
				var retryResult = result.Build();
				var retry = new RetryState(callOptions.RetryOptions);
				while (await retry.RetryWithDelayAsync())
				{
					try
					{
						if (callOptions.Timeout.HasValue)
						{
							var task = CallAsync(@delegate, invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, cancellationToken, delegateName);
							retryResult = await task.OrTimeoutAsync(callOptions.Timeout.Value);
							if (!retryResult.HasError)
							{
								if (retry.CallBreak(retryResult.GetData()))
									break;
								else
									retry.CallNoBreakMatch(retryResult);
							}
						}
						else
						{
							retryResult = await CallAsync(@delegate, invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, cancellationToken, delegateName);
							if (!retryResult.HasError)
							{
								if (retry.CallBreak(retryResult.GetData()))
									break;
								else
									retry.CallNoBreakMatch(retryResult);
							}
						}
					}
					catch (Exception retryEx)
					{
						retryResult = new ResultBuilder<TResult>()
							.WithInvalidOperationException(invocationContext, null, x => x.ExceptionInfo(retryEx));
					}
				}

				return retryResult;
			}
			else if (callOptions.Timeout.HasValue)
			{
				var task = CallAsync(@delegate, invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, cancellationToken, delegateName);
				return await task.OrTimeoutAsync(callOptions.Timeout.Value);
			}
			else
			{
				return result.WithNotSupportedException(invocationContext, null, x => x.InternalMessage($"Invalid CallOptions"));
			}
		}
		catch (Exception ex)
		{
			isUnhandledException = true;

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
						ElapsedMilliseconds = -1,
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
		//finally
		//{
		//	if (disposableInvocationContext != null)
		//		await disposableInvocationContext.DisposeAsync();
		//}
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
		CallOptions callOptions,
		CancellationToken cancellationToken = default,
		[CallerArgumentExpression(nameof(@delegate))] string? delegateName = null)
		where TInvocationContext : IInvocationContext
	{
		var result = new ResultBuilder<TResult>();

		var delegateWasCalled = false;
		var isUnhandledException = false;
		IInvocationContext iinvocationContext = invocationContext;
		InvocationContext? disposableInvocationContext = null;

		try
		{
			Throw.IfNull(iinvocationContext);
			Throw.IfNull(callOptions);

			if (result.IsArgumentNull(iinvocationContext.InvocationCreateNew(), @delegate))
			{
				var logResult = iinvocationContext.LogResultErrorMessages(result.Build());
				result.MergeErrors(logResult);

				return result.Build();
			}

			delegateWasCalled = true;

			if (callOptions.FireAndForget)
			{
				var task = CallAsync(@delegate, invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, cancellationToken, delegateName);
				task.FireAndForget();
				return result.Build();
			}
			else if (callOptions.RetryOptions != null)
			{
				var retryResult = result.Build();
				var retry = new RetryState(callOptions.RetryOptions);
				while (await retry.RetryWithDelayAsync())
				{
					try
					{
						if (callOptions.Timeout.HasValue)
						{
							var task = CallAsync(@delegate, invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, cancellationToken, delegateName);
							retryResult = await task.OrTimeoutAsync(callOptions.Timeout.Value);
							if (!retryResult.HasError)
							{
								if (retry.CallBreak(retryResult.GetData()))
									break;
								else
									retry.CallNoBreakMatch(retryResult);
							}
						}
						else
						{
							retryResult = await CallAsync(@delegate, invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, cancellationToken, delegateName);
							if (!retryResult.HasError)
							{
								if (retry.CallBreak(retryResult.GetData()))
									break;
								else
									retry.CallNoBreakMatch(retryResult);
							}
						}
					}
					catch (Exception retryEx)
					{
						retryResult = new ResultBuilder<TResult>()
							.WithInvalidOperationException(invocationContext, null, x => x.ExceptionInfo(retryEx));
					}
				}

				return retryResult;
			}
			else if (callOptions.Timeout.HasValue)
			{
				var task = CallAsync(@delegate, invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, cancellationToken, delegateName);
				return await task.OrTimeoutAsync(callOptions.Timeout.Value);
			}
			else
			{
				return result.WithNotSupportedException(invocationContext, null, x => x.InternalMessage($"Invalid CallOptions"));
			}
		}
		catch (Exception ex)
		{
			isUnhandledException = true;

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
						ElapsedMilliseconds = -1,
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
		//finally
		//{
		//	if (disposableInvocationContext != null)
		//		await disposableInvocationContext.DisposeAsync();
		//}
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
		CallOptions callOptions,
		CancellationToken cancellationToken = default,
		[CallerArgumentExpression(nameof(@delegate))] string? delegateName = null)
		where TInvocationContext : IInvocationContext
	{
		var result = new ResultBuilder<TResult>();

		var delegateWasCalled = false;
		var isUnhandledException = false;
		IInvocationContext iinvocationContext = invocationContext;
		InvocationContext? disposableInvocationContext = null;

		try
		{
			Throw.IfNull(iinvocationContext);
			Throw.IfNull(callOptions);

			if (result.IsArgumentNull(iinvocationContext.InvocationCreateNew(), @delegate))
			{
				var logResult = iinvocationContext.LogResultErrorMessages(result.Build());
				result.MergeErrors(logResult);

				return result.Build();
			}

			delegateWasCalled = true;

			if (callOptions.FireAndForget)
			{
				var task = CallAsync(@delegate, invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, cancellationToken, delegateName);
				task.FireAndForget();
				return result.Build();
			}
			else if (callOptions.RetryOptions != null)
			{
				var retryResult = result.Build();
				var retry = new RetryState(callOptions.RetryOptions);
				while (await retry.RetryWithDelayAsync())
				{
					try
					{
						if (callOptions.Timeout.HasValue)
						{
							var task = CallAsync(@delegate, invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, cancellationToken, delegateName);
							retryResult = await task.OrTimeoutAsync(callOptions.Timeout.Value);
							if (!retryResult.HasError)
							{
								if (retry.CallBreak(retryResult.GetData()))
									break;
								else
									retry.CallNoBreakMatch(retryResult);
							}
						}
						else
						{
							retryResult = await CallAsync(@delegate, invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, cancellationToken, delegateName);
							if (!retryResult.HasError)
							{
								if (retry.CallBreak(retryResult.GetData()))
									break;
								else
									retry.CallNoBreakMatch(retryResult);
							}
						}
					}
					catch (Exception retryEx)
					{
						retryResult = new ResultBuilder<TResult>()
							.WithInvalidOperationException(invocationContext, null, x => x.ExceptionInfo(retryEx));
					}
				}

				return retryResult;
			}
			else if (callOptions.Timeout.HasValue)
			{
				var task = CallAsync(@delegate, invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, cancellationToken, delegateName);
				return await task.OrTimeoutAsync(callOptions.Timeout.Value);
			}
			else
			{
				return result.WithNotSupportedException(invocationContext, null, x => x.InternalMessage($"Invalid CallOptions"));
			}
		}
		catch (Exception ex)
		{
			isUnhandledException = true;

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
						ElapsedMilliseconds = -1,
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
		//finally
		//{
		//	if (disposableInvocationContext != null)
		//		await disposableInvocationContext.DisposeAsync();
		//}
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
		CallOptions callOptions,
		CancellationToken cancellationToken = default,
		[CallerArgumentExpression(nameof(@delegate))] string? delegateName = null)
		where TInvocationContext : IInvocationContext
	{
		var result = new ResultBuilder<TResult>();

		var delegateWasCalled = false;
		var isUnhandledException = false;
		IInvocationContext iinvocationContext = invocationContext;
		InvocationContext? disposableInvocationContext = null;

		try
		{
			Throw.IfNull(iinvocationContext);
			Throw.IfNull(callOptions);

			if (result.IsArgumentNull(iinvocationContext.InvocationCreateNew(), @delegate))
			{
				var logResult = iinvocationContext.LogResultErrorMessages(result.Build());
				result.MergeErrors(logResult);

				return result.Build();
			}

			delegateWasCalled = true;

			if (callOptions.FireAndForget)
			{
				var task = CallAsync(@delegate, invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, cancellationToken, delegateName);
				task.FireAndForget();
				return result.Build();
			}
			else if (callOptions.RetryOptions != null)
			{
				var retryResult = result.Build();
				var retry = new RetryState(callOptions.RetryOptions);
				while (await retry.RetryWithDelayAsync())
				{
					try
					{
						if (callOptions.Timeout.HasValue)
						{
							var task = CallAsync(@delegate, invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, cancellationToken, delegateName);
							retryResult = await task.OrTimeoutAsync(callOptions.Timeout.Value);
							if (!retryResult.HasError)
							{
								if (retry.CallBreak(retryResult.GetData()))
									break;
								else
									retry.CallNoBreakMatch(retryResult);
							}
						}
						else
						{
							retryResult = await CallAsync(@delegate, invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, cancellationToken, delegateName);
							if (!retryResult.HasError)
							{
								if (retry.CallBreak(retryResult.GetData()))
									break;
								else
									retry.CallNoBreakMatch(retryResult);
							}
						}
					}
					catch (Exception retryEx)
					{
						retryResult = new ResultBuilder<TResult>()
							.WithInvalidOperationException(invocationContext, null, x => x.ExceptionInfo(retryEx));
					}
				}

				return retryResult;
			}
			else if (callOptions.Timeout.HasValue)
			{
				var task = CallAsync(@delegate, invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, cancellationToken, delegateName);
				return await task.OrTimeoutAsync(callOptions.Timeout.Value);
			}
			else
			{
				return result.WithNotSupportedException(invocationContext, null, x => x.InternalMessage($"Invalid CallOptions"));
			}
		}
		catch (Exception ex)
		{
			isUnhandledException = true;

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
						ElapsedMilliseconds = -1,
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
		//finally
		//{
		//	if (disposableInvocationContext != null)
		//		await disposableInvocationContext.DisposeAsync();
		//}
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
		CallOptions callOptions,
		CancellationToken cancellationToken = default,
		[CallerArgumentExpression(nameof(@delegate))] string? delegateName = null)
		where TInvocationContext : IInvocationContext
	{
		var result = new ResultBuilder<TResult>();

		var delegateWasCalled = false;
		var isUnhandledException = false;
		IInvocationContext iinvocationContext = invocationContext;
		InvocationContext? disposableInvocationContext = null;

		try
		{
			Throw.IfNull(iinvocationContext);
			Throw.IfNull(callOptions);

			if (result.IsArgumentNull(iinvocationContext.InvocationCreateNew(), @delegate))
			{
				var logResult = iinvocationContext.LogResultErrorMessages(result.Build());
				result.MergeErrors(logResult);

				return result.Build();
			}

			delegateWasCalled = true;

			if (callOptions.FireAndForget)
			{
				var task = CallAsync(@delegate, invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, cancellationToken, delegateName);
				task.FireAndForget();
				return result.Build();
			}
			else if (callOptions.RetryOptions != null)
			{
				var retryResult = result.Build();
				var retry = new RetryState(callOptions.RetryOptions);
				while (await retry.RetryWithDelayAsync())
				{
					try
					{
						if (callOptions.Timeout.HasValue)
						{
							var task = CallAsync(@delegate, invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, cancellationToken, delegateName);
							retryResult = await task.OrTimeoutAsync(callOptions.Timeout.Value);
							if (!retryResult.HasError)
							{
								if (retry.CallBreak(retryResult.GetData()))
									break;
								else
									retry.CallNoBreakMatch(retryResult);
							}
						}
						else
						{
							retryResult = await CallAsync(@delegate, invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, cancellationToken, delegateName);
							if (!retryResult.HasError)
							{
								if (retry.CallBreak(retryResult.GetData()))
									break;
								else
									retry.CallNoBreakMatch(retryResult);
							}
						}
					}
					catch (Exception retryEx)
					{
						retryResult = new ResultBuilder<TResult>()
							.WithInvalidOperationException(invocationContext, null, x => x.ExceptionInfo(retryEx));
					}
				}

				return retryResult;
			}
			else if (callOptions.Timeout.HasValue)
			{
				var task = CallAsync(@delegate, invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, cancellationToken, delegateName);
				return await task.OrTimeoutAsync(callOptions.Timeout.Value);
			}
			else
			{
				return result.WithNotSupportedException(invocationContext, null, x => x.InternalMessage($"Invalid CallOptions"));
			}
		}
		catch (Exception ex)
		{
			isUnhandledException = true;

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
						ElapsedMilliseconds = -1,
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
		//finally
		//{
		//	if (disposableInvocationContext != null)
		//		await disposableInvocationContext.DisposeAsync();
		//}
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
		CallOptions callOptions,
		CancellationToken cancellationToken = default,
		[CallerArgumentExpression(nameof(@delegate))] string? delegateName = null)
		where TInvocationContext : IInvocationContext
	{
		var result = new ResultBuilder<TResult>();

		var delegateWasCalled = false;
		var isUnhandledException = false;
		IInvocationContext iinvocationContext = invocationContext;
		InvocationContext? disposableInvocationContext = null;

		try
		{
			Throw.IfNull(iinvocationContext);
			Throw.IfNull(callOptions);

			if (result.IsArgumentNull(iinvocationContext.InvocationCreateNew(), @delegate))
			{
				var logResult = iinvocationContext.LogResultErrorMessages(result.Build());
				result.MergeErrors(logResult);

				return result.Build();
			}

			delegateWasCalled = true;

			if (callOptions.FireAndForget)
			{
				var task = CallAsync(@delegate, invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, cancellationToken, delegateName);
				task.FireAndForget();
				return result.Build();
			}
			else if (callOptions.RetryOptions != null)
			{
				var retryResult = result.Build();
				var retry = new RetryState(callOptions.RetryOptions);
				while (await retry.RetryWithDelayAsync())
				{
					try
					{
						if (callOptions.Timeout.HasValue)
						{
							var task = CallAsync(@delegate, invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, cancellationToken, delegateName);
							retryResult = await task.OrTimeoutAsync(callOptions.Timeout.Value);
							if (!retryResult.HasError)
							{
								if (retry.CallBreak(retryResult.GetData()))
									break;
								else
									retry.CallNoBreakMatch(retryResult);
							}
						}
						else
						{
							retryResult = await CallAsync(@delegate, invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, cancellationToken, delegateName);
							if (!retryResult.HasError)
							{
								if (retry.CallBreak(retryResult.GetData()))
									break;
								else
									retry.CallNoBreakMatch(retryResult);
							}
						}
					}
					catch (Exception retryEx)
					{
						retryResult = new ResultBuilder<TResult>()
							.WithInvalidOperationException(invocationContext, null, x => x.ExceptionInfo(retryEx));
					}
				}

				return retryResult;
			}
			else if (callOptions.Timeout.HasValue)
			{
				var task = CallAsync(@delegate, invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, cancellationToken, delegateName);
				return await task.OrTimeoutAsync(callOptions.Timeout.Value);
			}
			else
			{
				return result.WithNotSupportedException(invocationContext, null, x => x.InternalMessage($"Invalid CallOptions"));
			}
		}
		catch (Exception ex)
		{
			isUnhandledException = true;

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
						ElapsedMilliseconds = -1,
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
		//finally
		//{
		//	if (disposableInvocationContext != null)
		//		await disposableInvocationContext.DisposeAsync();
		//}
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
		CallOptions callOptions,
		CancellationToken cancellationToken = default,
		[CallerArgumentExpression(nameof(@delegate))] string? delegateName = null)
		where TInvocationContext : IInvocationContext
	{
		var result = new ResultBuilder<TResult>();

		var delegateWasCalled = false;
		var isUnhandledException = false;
		IInvocationContext iinvocationContext = invocationContext;
		InvocationContext? disposableInvocationContext = null;

		try
		{
			Throw.IfNull(iinvocationContext);
			Throw.IfNull(callOptions);

			if (result.IsArgumentNull(iinvocationContext.InvocationCreateNew(), @delegate))
			{
				var logResult = iinvocationContext.LogResultErrorMessages(result.Build());
				result.MergeErrors(logResult);

				return result.Build();
			}

			delegateWasCalled = true;

			if (callOptions.FireAndForget)
			{
				var task = CallAsync(@delegate, invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, cancellationToken, delegateName);
				task.FireAndForget();
				return result.Build();
			}
			else if (callOptions.RetryOptions != null)
			{
				var retryResult = result.Build();
				var retry = new RetryState(callOptions.RetryOptions);
				while (await retry.RetryWithDelayAsync())
				{
					try
					{
						if (callOptions.Timeout.HasValue)
						{
							var task = CallAsync(@delegate, invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, cancellationToken, delegateName);
							retryResult = await task.OrTimeoutAsync(callOptions.Timeout.Value);
							if (!retryResult.HasError)
							{
								if (retry.CallBreak(retryResult.GetData()))
									break;
								else
									retry.CallNoBreakMatch(retryResult);
							}
						}
						else
						{
							retryResult = await CallAsync(@delegate, invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, cancellationToken, delegateName);
							if (!retryResult.HasError)
							{
								if (retry.CallBreak(retryResult.GetData()))
									break;
								else
									retry.CallNoBreakMatch(retryResult);
							}
						}
					}
					catch (Exception retryEx)
					{
						retryResult = new ResultBuilder<TResult>()
							.WithInvalidOperationException(invocationContext, null, x => x.ExceptionInfo(retryEx));
					}
				}

				return retryResult;
			}
			else if (callOptions.Timeout.HasValue)
			{
				var task = CallAsync(@delegate, invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, cancellationToken, delegateName);
				return await task.OrTimeoutAsync(callOptions.Timeout.Value);
			}
			else
			{
				return result.WithNotSupportedException(invocationContext, null, x => x.InternalMessage($"Invalid CallOptions"));
			}
		}
		catch (Exception ex)
		{
			isUnhandledException = true;

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
						ElapsedMilliseconds = -1,
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
		//finally
		//{
		//	if (disposableInvocationContext != null)
		//		await disposableInvocationContext.DisposeAsync();
		//}
	}
}
