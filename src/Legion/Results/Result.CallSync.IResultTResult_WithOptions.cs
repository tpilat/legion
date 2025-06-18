using Legion.Logging;
using Legion.Policy;
using System.Runtime.CompilerServices;

namespace Legion;

public partial class Result : IResult
{
	public static IResult<TResult> Call<TInvocationContext, TResult>(
		Func<TInvocationContext, IResult<TResult>> @delegate,
		TInvocationContext invocationContext,
		CallOptions callOptions,
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
				InvocationHelper.FireAndForget(Call, @delegate, invocationContext, delegateName);
				return result.Build();
			}
			else if (callOptions.RetryOptions != null)
			{
				var retryResult = result.Build();
				var retry = new RetryState(callOptions.RetryOptions);
				while (retry.RetryWithDelay())
				{
					try
					{
						if (callOptions.Timeout.HasValue)
						{
							retryResult = InvocationHelper.FuncWithTimeout(Call, @delegate, invocationContext, delegateName, callOptions.Timeout.Value);
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
							retryResult = Call(@delegate, invocationContext, delegateName);
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
				return InvocationHelper.FuncWithTimeout(Call, @delegate, invocationContext, delegateName, callOptions.Timeout.Value);
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
						ElapsedMilliseconds = -1,
						DelegateMethodName = string.IsNullOrWhiteSpace(delegateName)
							? iinvocationContext.TraceFrameStack.LastFrame ?? new TraceFrame().Frame
							: delegateName!
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
		//finally
		//{
		//	disposableInvocationContext?.Dispose();
		//}
	}

	public static IResult<TResult> Call<TInvocationContext, T, TResult>(
		Func<TInvocationContext, T, IResult<TResult>> @delegate,
		TInvocationContext invocationContext,
		T arg,
		CallOptions callOptions,
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
				InvocationHelper.FireAndForget(Call, @delegate, invocationContext, arg, delegateName);
				return result.Build();
			}
			else if (callOptions.RetryOptions != null)
			{
				var retryResult = result.Build();
				var retry = new RetryState(callOptions.RetryOptions);
				while (retry.RetryWithDelay())
				{
					try
					{
						if (callOptions.Timeout.HasValue)
						{
							retryResult = InvocationHelper.FuncWithTimeout(Call, @delegate, invocationContext, arg, delegateName, callOptions.Timeout.Value);
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
							retryResult = Call(@delegate, invocationContext, arg, delegateName);
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
				return InvocationHelper.FuncWithTimeout(Call, @delegate, invocationContext, arg, delegateName, callOptions.Timeout.Value);
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
						ElapsedMilliseconds = -1,
						DelegateMethodName = string.IsNullOrWhiteSpace(delegateName)
							? iinvocationContext.TraceFrameStack.LastFrame ?? new TraceFrame().Frame
							: delegateName!
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
		//finally
		//{
		//	disposableInvocationContext?.Dispose();
		//}
	}

	public static IResult<TResult> Call<TInvocationContext, T1, T2, TResult>(
		Func<TInvocationContext, T1, T2, IResult<TResult>> @delegate,
		TInvocationContext invocationContext,
		T1 arg1,
		T2 arg2,
		CallOptions callOptions,
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
				InvocationHelper.FireAndForget(Call, @delegate, invocationContext, arg1, arg2, delegateName);
				return result.Build();
			}
			else if (callOptions.RetryOptions != null)
			{
				var retryResult = result.Build();
				var retry = new RetryState(callOptions.RetryOptions);
				while (retry.RetryWithDelay())
				{
					try
					{
						if (callOptions.Timeout.HasValue)
						{
							retryResult = InvocationHelper.FuncWithTimeout(Call, @delegate, invocationContext, arg1, arg2, delegateName, callOptions.Timeout.Value);
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
							retryResult = Call(@delegate, invocationContext, arg1, arg2, delegateName);
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
				return InvocationHelper.FuncWithTimeout(Call, @delegate, invocationContext, arg1, arg2, delegateName, callOptions.Timeout.Value);
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
						ElapsedMilliseconds = -1,
						DelegateMethodName = string.IsNullOrWhiteSpace(delegateName)
							? iinvocationContext.TraceFrameStack.LastFrame ?? new TraceFrame().Frame
							: delegateName!
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
		//finally
		//{
		//	disposableInvocationContext?.Dispose();
		//}
	}

	public static IResult<TResult> Call<TInvocationContext, T1, T2, T3, TResult>(
		Func<TInvocationContext, T1, T2, T3, IResult<TResult>> @delegate,
		TInvocationContext invocationContext,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		CallOptions callOptions,
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
				InvocationHelper.FireAndForget(Call, @delegate, invocationContext, arg1, arg2, arg3, delegateName);
				return result.Build();
			}
			else if (callOptions.RetryOptions != null)
			{
				var retryResult = result.Build();
				var retry = new RetryState(callOptions.RetryOptions);
				while (retry.RetryWithDelay())
				{
					try
					{
						if (callOptions.Timeout.HasValue)
						{
							retryResult = InvocationHelper.FuncWithTimeout(Call, @delegate, invocationContext, arg1, arg2, arg3, delegateName, callOptions.Timeout.Value);
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
							retryResult = Call(@delegate, invocationContext, arg1, arg2, arg3, delegateName);
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
				return InvocationHelper.FuncWithTimeout(Call, @delegate, invocationContext, arg1, arg2, arg3, delegateName, callOptions.Timeout.Value);
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
						ElapsedMilliseconds = -1,
						DelegateMethodName = string.IsNullOrWhiteSpace(delegateName)
							? iinvocationContext.TraceFrameStack.LastFrame ?? new TraceFrame().Frame
							: delegateName!
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
		//finally
		//{
		//	disposableInvocationContext?.Dispose();
		//}
	}

	public static IResult<TResult> Call<TInvocationContext, T1, T2, T3, T4, TResult>(
		Func<TInvocationContext, T1, T2, T3, T4, IResult<TResult>> @delegate,
		TInvocationContext invocationContext,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		CallOptions callOptions,
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
				InvocationHelper.FireAndForget(Call, @delegate, invocationContext, arg1, arg2, arg3, arg4, delegateName);
				return result.Build();
			}
			else if (callOptions.RetryOptions != null)
			{
				var retryResult = result.Build();
				var retry = new RetryState(callOptions.RetryOptions);
				while (retry.RetryWithDelay())
				{
					try
					{
						if (callOptions.Timeout.HasValue)
						{
							retryResult = InvocationHelper.FuncWithTimeout(Call, @delegate, invocationContext, arg1, arg2, arg3, arg4, delegateName, callOptions.Timeout.Value);
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
							retryResult = Call(@delegate, invocationContext, arg1, arg2, arg3, arg4, delegateName);
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
				return InvocationHelper.FuncWithTimeout(Call, @delegate, invocationContext, arg1, arg2, arg3, arg4, delegateName, callOptions.Timeout.Value);
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
						ElapsedMilliseconds = -1,
						DelegateMethodName = string.IsNullOrWhiteSpace(delegateName)
							? iinvocationContext.TraceFrameStack.LastFrame ?? new TraceFrame().Frame
							: delegateName!
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
		//finally
		//{
		//	disposableInvocationContext?.Dispose();
		//}
	}

	public static IResult<TResult> Call<TInvocationContext, T1, T2, T3, T4, T5, TResult>(
		Func<TInvocationContext, T1, T2, T3, T4, T5, IResult<TResult>> @delegate,
		TInvocationContext invocationContext,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		T5 arg5,
		CallOptions callOptions,
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
				InvocationHelper.FireAndForget(Call, @delegate, invocationContext, arg1, arg2, arg3, arg4, arg5, delegateName);
				return result.Build();
			}
			else if (callOptions.RetryOptions != null)
			{
				var retryResult = result.Build();
				var retry = new RetryState(callOptions.RetryOptions);
				while (retry.RetryWithDelay())
				{
					try
					{
						if (callOptions.Timeout.HasValue)
						{
							retryResult = InvocationHelper.FuncWithTimeout(Call, @delegate, invocationContext, arg1, arg2, arg3, arg4, arg5, delegateName, callOptions.Timeout.Value);
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
							retryResult = Call(@delegate, invocationContext, arg1, arg2, arg3, arg4, arg5, delegateName);
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
				return InvocationHelper.FuncWithTimeout(Call, @delegate, invocationContext, arg1, arg2, arg3, arg4, arg5, delegateName, callOptions.Timeout.Value);
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
						ElapsedMilliseconds = -1,
						DelegateMethodName = string.IsNullOrWhiteSpace(delegateName)
							? iinvocationContext.TraceFrameStack.LastFrame ?? new TraceFrame().Frame
							: delegateName!
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
		//finally
		//{
		//	disposableInvocationContext?.Dispose();
		//}
	}

	public static IResult<TResult> Call<TInvocationContext, T1, T2, T3, T4, T5, T6, TResult>(
		Func<TInvocationContext, T1, T2, T3, T4, T5, T6, IResult<TResult>> @delegate,
		TInvocationContext invocationContext,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		T5 arg5,
		T6 arg6,
		CallOptions callOptions,
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
				InvocationHelper.FireAndForget(Call, @delegate, invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, delegateName);
				return result.Build();
			}
			else if (callOptions.RetryOptions != null)
			{
				var retryResult = result.Build();
				var retry = new RetryState(callOptions.RetryOptions);
				while (retry.RetryWithDelay())
				{
					try
					{
						if (callOptions.Timeout.HasValue)
						{
							retryResult = InvocationHelper.FuncWithTimeout(Call, @delegate, invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, delegateName, callOptions.Timeout.Value);
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
							retryResult = Call(@delegate, invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, delegateName);
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
				return InvocationHelper.FuncWithTimeout(Call, @delegate, invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, delegateName, callOptions.Timeout.Value);
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
						ElapsedMilliseconds = -1,
						DelegateMethodName = string.IsNullOrWhiteSpace(delegateName)
							? iinvocationContext.TraceFrameStack.LastFrame ?? new TraceFrame().Frame
							: delegateName!
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
		//finally
		//{
		//	disposableInvocationContext?.Dispose();
		//}
	}

	public static IResult<TResult> Call<TInvocationContext, T1, T2, T3, T4, T5, T6, T7, TResult>(
		Func<TInvocationContext, T1, T2, T3, T4, T5, T6, T7, IResult<TResult>> @delegate,
		TInvocationContext invocationContext,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		T5 arg5,
		T6 arg6,
		T7 arg7,
		CallOptions callOptions,
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
				InvocationHelper.FireAndForget(Call, @delegate, invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, arg7, delegateName);
				return result.Build();
			}
			else if (callOptions.RetryOptions != null)
			{
				var retryResult = result.Build();
				var retry = new RetryState(callOptions.RetryOptions);
				while (retry.RetryWithDelay())
				{
					try
					{
						if (callOptions.Timeout.HasValue)
						{
							retryResult = InvocationHelper.FuncWithTimeout(Call, @delegate, invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, arg7, delegateName, callOptions.Timeout.Value);
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
							retryResult = Call(@delegate, invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, arg7, delegateName);
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
				return InvocationHelper.FuncWithTimeout(Call, @delegate, invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, arg7, delegateName, callOptions.Timeout.Value);
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
						ElapsedMilliseconds = -1,
						DelegateMethodName = string.IsNullOrWhiteSpace(delegateName)
							? iinvocationContext.TraceFrameStack.LastFrame ?? new TraceFrame().Frame
							: delegateName!
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
		//finally
		//{
		//	disposableInvocationContext?.Dispose();
		//}
	}

	public static IResult<TResult> Call<TInvocationContext, T1, T2, T3, T4, T5, T6, T7, T8, TResult>(
		Func<TInvocationContext, T1, T2, T3, T4, T5, T6, T7, T8, IResult<TResult>> @delegate,
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
				InvocationHelper.FireAndForget(Call, @delegate, invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, delegateName);
				return result.Build();
			}
			else if (callOptions.RetryOptions != null)
			{
				var retryResult = result.Build();
				var retry = new RetryState(callOptions.RetryOptions);
				while (retry.RetryWithDelay())
				{
					try
					{
						if (callOptions.Timeout.HasValue)
						{
							retryResult = InvocationHelper.FuncWithTimeout(Call, @delegate, invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, delegateName, callOptions.Timeout.Value);
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
							retryResult = Call(@delegate, invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, delegateName);
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
				return InvocationHelper.FuncWithTimeout(Call, @delegate, invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, delegateName, callOptions.Timeout.Value);
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
						ElapsedMilliseconds = -1,
						DelegateMethodName = string.IsNullOrWhiteSpace(delegateName)
							? iinvocationContext.TraceFrameStack.LastFrame ?? new TraceFrame().Frame
							: delegateName!
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
		//finally
		//{
		//	disposableInvocationContext?.Dispose();
		//}
	}

	public static IResult<TResult> Call<TInvocationContext, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(
		Func<TInvocationContext, T1, T2, T3, T4, T5, T6, T7, T8, T9, IResult<TResult>> @delegate,
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
				InvocationHelper.FireAndForget(Call, @delegate, invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, delegateName);
				return result.Build();
			}
			else if (callOptions.RetryOptions != null)
			{
				var retryResult = result.Build();
				var retry = new RetryState(callOptions.RetryOptions);
				while (retry.RetryWithDelay())
				{
					try
					{
						if (callOptions.Timeout.HasValue)
						{
							retryResult = InvocationHelper.FuncWithTimeout(Call, @delegate, invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, delegateName, callOptions.Timeout.Value);
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
							retryResult = Call(@delegate, invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, delegateName);
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
				return InvocationHelper.FuncWithTimeout(Call, @delegate, invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, delegateName, callOptions.Timeout.Value);
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
						ElapsedMilliseconds = -1,
						DelegateMethodName = string.IsNullOrWhiteSpace(delegateName)
							? iinvocationContext.TraceFrameStack.LastFrame ?? new TraceFrame().Frame
							: delegateName!
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
		//finally
		//{
		//	disposableInvocationContext?.Dispose();
		//}
	}

	public static IResult<TResult> Call<TInvocationContext, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(
		Func<TInvocationContext, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, IResult<TResult>> @delegate,
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
				InvocationHelper.FireAndForget(Call, @delegate, invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, delegateName);
				return result.Build();
			}
			else if (callOptions.RetryOptions != null)
			{
				var retryResult = result.Build();
				var retry = new RetryState(callOptions.RetryOptions);
				while (retry.RetryWithDelay())
				{
					try
					{
						if (callOptions.Timeout.HasValue)
						{
							retryResult = InvocationHelper.FuncWithTimeout(Call, @delegate, invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, delegateName, callOptions.Timeout.Value);
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
							retryResult = Call(@delegate, invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, delegateName);
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
				return InvocationHelper.FuncWithTimeout(Call, @delegate, invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, delegateName, callOptions.Timeout.Value);
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
						ElapsedMilliseconds = -1,
						DelegateMethodName = string.IsNullOrWhiteSpace(delegateName)
							? iinvocationContext.TraceFrameStack.LastFrame ?? new TraceFrame().Frame
							: delegateName!
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
		//finally
		//{
		//	disposableInvocationContext?.Dispose();
		//}
	}

	public static IResult<TResult> Call<TInvocationContext, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(
		Func<TInvocationContext, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, IResult<TResult>> @delegate,
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
				InvocationHelper.FireAndForget(Call, @delegate, invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, delegateName);
				return result.Build();
			}
			else if (callOptions.RetryOptions != null)
			{
				var retryResult = result.Build();
				var retry = new RetryState(callOptions.RetryOptions);
				while (retry.RetryWithDelay())
				{
					try
					{
						if (callOptions.Timeout.HasValue)
						{
							retryResult = InvocationHelper.FuncWithTimeout(Call, @delegate, invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, delegateName, callOptions.Timeout.Value);
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
							retryResult = Call(@delegate, invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, delegateName);
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
				return InvocationHelper.FuncWithTimeout(Call, @delegate, invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, delegateName, callOptions.Timeout.Value);
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
						ElapsedMilliseconds = -1,
						DelegateMethodName = string.IsNullOrWhiteSpace(delegateName)
							? iinvocationContext.TraceFrameStack.LastFrame ?? new TraceFrame().Frame
							: delegateName!
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
		//finally
		//{
		//	disposableInvocationContext?.Dispose();
		//}
	}

	public static IResult<TResult> Call<TInvocationContext, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(
		Func<TInvocationContext, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, IResult<TResult>> @delegate,
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
				InvocationHelper.FireAndForget(Call, @delegate, invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, delegateName);
				return result.Build();
			}
			else if (callOptions.RetryOptions != null)
			{
				var retryResult = result.Build();
				var retry = new RetryState(callOptions.RetryOptions);
				while (retry.RetryWithDelay())
				{
					try
					{
						if (callOptions.Timeout.HasValue)
						{
							retryResult = InvocationHelper.FuncWithTimeout(Call, @delegate, invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, delegateName, callOptions.Timeout.Value);
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
							retryResult = Call(@delegate, invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, delegateName);
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
				return InvocationHelper.FuncWithTimeout(Call, @delegate, invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, delegateName, callOptions.Timeout.Value);
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
						ElapsedMilliseconds = -1,
						DelegateMethodName = string.IsNullOrWhiteSpace(delegateName)
							? iinvocationContext.TraceFrameStack.LastFrame ?? new TraceFrame().Frame
							: delegateName!
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
		//finally
		//{
		//	disposableInvocationContext?.Dispose();
		//}
	}

	public static IResult<TResult> Call<TInvocationContext, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(
		Func<TInvocationContext, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, IResult<TResult>> @delegate,
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
				InvocationHelper.FireAndForget(Call, @delegate, invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, delegateName);
				return result.Build();
			}
			else if (callOptions.RetryOptions != null)
			{
				var retryResult = result.Build();
				var retry = new RetryState(callOptions.RetryOptions);
				while (retry.RetryWithDelay())
				{
					try
					{
						if (callOptions.Timeout.HasValue)
						{
							retryResult = InvocationHelper.FuncWithTimeout(Call, @delegate, invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, delegateName, callOptions.Timeout.Value);
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
							retryResult = Call(@delegate, invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, delegateName);
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
				return InvocationHelper.FuncWithTimeout(Call, @delegate, invocationContext, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, delegateName, callOptions.Timeout.Value);
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
						ElapsedMilliseconds = -1,
						DelegateMethodName = string.IsNullOrWhiteSpace(delegateName)
							? iinvocationContext.TraceFrameStack.LastFrame ?? new TraceFrame().Frame
							: delegateName!
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
		//finally
		//{
		//	disposableInvocationContext?.Dispose();
		//}
	}
}
