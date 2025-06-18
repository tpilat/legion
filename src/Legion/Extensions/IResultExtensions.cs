using Legion.Exceptions.Internal;
using Legion.Extensions;
using Legion.Logging;
using Legion.Validation;
using Microsoft.Extensions.Logging;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Legion;

public static class IResultExtensions
{
	public static TResult ToDto<TResult>(
		this TResult result,
		params string[] ignoredPropterties)
		where TResult : IResult
	{
		Throw.IfArgumentNull(result);

		for (int i = 0; i < result.SuccessMessages.Count; i++)
			result.SuccessMessages[i] = result.SuccessMessages[i].ToDto(ignoredPropterties);

		for (int i = 0; i < result.WarningMessages.Count; i++)
			result.WarningMessages[i] = result.WarningMessages[i].ToDto(ignoredPropterties);

		for (int i = 0; i < result.ErrorMessages.Count; i++)
			result.ErrorMessages[i] = result.ErrorMessages[i].ToDto(ignoredPropterties);

		return result;
	}

	public static TResult ToClientDto<TResult>(this TResult result)
		where TResult : IResult
	{
		Throw.IfArgumentNull(result);

		for (int i = 0; i < result.SuccessMessages.Count; i++)
			result.SuccessMessages[i] = result.SuccessMessages[i].ToClientDto();

		for (int i = 0; i < result.WarningMessages.Count; i++)
			result.WarningMessages[i] = result.WarningMessages[i].ToClientDto();

		for (int i = 0; i < result.ErrorMessages.Count; i++)
			result.ErrorMessages[i] = result.ErrorMessages[i].ToClientDto();

		return result;
	}

	public static TObject ToDto<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		params string[] ignoredPropterties)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
	{
		Throw.IfArgumentNull(resultBuilder);

		var result = resultBuilder.Build();
		return result.ToDto(ignoredPropterties);
	}

	public static TObject ToClientDto<TBuilder, TObject>(this ResultBuilderBase<TBuilder, TObject> resultBuilder)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
	{
		Throw.IfArgumentNull(resultBuilder);

		var result = resultBuilder.Build();
		return result.ToClientDto();
	}




	private static TBuilder WithErrorInternal<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		Action<ErrorMessageBuilder>? errorMessageConfigurator,
		IErrorCode errorCode,
		LogLevel logLevel = LogLevel.Error)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(errorCode);

		var errorMessageBuilder =
			new ErrorMessageBuilder(scopeContext, errorCode)
				.LogLevel(logLevel);

		errorMessageConfigurator?.Invoke(errorMessageBuilder);
		resultBuilder.Build().ErrorMessages.Add(errorMessageBuilder.Build());
		return resultBuilder.GetBuilder();
	}

	private static TBuilder WithWarnInternal<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		Action<LogMessageBuilder>? logMessageConfigurator,
		IErrorCode? errorCode = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
	{
		var logMessageBuilder =
			new LogMessageBuilder(scopeContext, errorCode)
				.LogLevel(LogLevel.Warning);

		logMessageConfigurator?.Invoke(logMessageBuilder);
		resultBuilder.Build().WarningMessages.Add(logMessageBuilder.Build());
		return resultBuilder.GetBuilder();
	}






	[System.Diagnostics.StackTraceHidden]
	[Obsolete("Use IsArgumentNullOrEmpty or IsArgumentNullOrWhiteSpace instead. Do not use with nameof()"
#if NET6_0_OR_GREATER
	, DiagnosticId = "L_ArgNullEx_Str")]
#else
	)]
#endif
	public static bool IsArgumentNull<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		[NotNull] string? argument,
		IErrorCode? errorCode = null,
		Action<ErrorMessageBuilder>? errorMessageConfigurator = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (argument is null)
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					(x => x.InternalMessage(paramName))
						+ errorMessageConfigurator,
					errorCode ?? ErrorCodes.ArgNullException.Default);

#pragma warning disable CS8777 // Parameter must have a non-null value when exiting.
			return true;
#pragma warning restore CS8777 // Parameter must have a non-null value when exiting.
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	[Obsolete("Use IsArgumentNullOrEmpty or IsArgumentNullOrWhiteSpace instead. Do not use with nameof()"
#if NET6_0_OR_GREATER
	, DiagnosticId = "L_ArgNullEx_Str")]
#else
	)]
#endif
	public static bool IsArgumentNull<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		[NotNull] string? argument,
		IErrorCode? errorCode,
		string? detail,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (argument is null)
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					x => x
						.InternalMessage(paramName)
						.Detail(detail),
					errorCode ?? ErrorCodes.ArgNullException.Default);

#pragma warning disable CS8777 // Parameter must have a non-null value when exiting.
			return true;
#pragma warning restore CS8777 // Parameter must have a non-null value when exiting.
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsArgumentNull<T, TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		[NotNull] T? argument,
		IErrorCode? errorCode = null,
		Action<ErrorMessageBuilder>? errorMessageConfigurator = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (argument is null)
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					(x => x.InternalMessage(paramName))
						+ errorMessageConfigurator,
					errorCode ?? ErrorCodes.ArgNullException.Default);

#pragma warning disable CS8777 // Parameter must have a non-null value when exiting.
			return true;
#pragma warning restore CS8777 // Parameter must have a non-null value when exiting.
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsArgumentNull<T, TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		[NotNull] T? argument,
		IErrorCode? errorCode,
		string? detail,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (argument is null)
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					x => x
						.InternalMessage(paramName)
						.Detail(detail),
					errorCode ?? ErrorCodes.ArgNullException.Default);

#pragma warning disable CS8777 // Parameter must have a non-null value when exiting.
			return true;
#pragma warning restore CS8777 // Parameter must have a non-null value when exiting.
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsArgumentNull<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		[NotNull] object? argument,
		IErrorCode? errorCode = null,
		Action<ErrorMessageBuilder>? errorMessageConfigurator = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (argument is null)
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					(x => x.InternalMessage(paramName))
						+ errorMessageConfigurator,
					errorCode ?? ErrorCodes.ArgNullException.Default);

#pragma warning disable CS8777 // Parameter must have a non-null value when exiting.
			return true;
#pragma warning restore CS8777 // Parameter must have a non-null value when exiting.
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsArgumentNull<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		[NotNull] object? argument,
		IErrorCode? errorCode,
		string? detail,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (argument is null)
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					x => x
						.InternalMessage(paramName)
						.Detail(detail),
					errorCode ?? ErrorCodes.ArgNullException.Default);

#pragma warning disable CS8777 // Parameter must have a non-null value when exiting.
			return true;
#pragma warning restore CS8777 // Parameter must have a non-null value when exiting.
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static unsafe bool IsArgumentNull<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		[NotNull] void* argument,
		IErrorCode? errorCode = null,
		Action<ErrorMessageBuilder>? errorMessageConfigurator = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (argument is null)
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					(x => x.InternalMessage(paramName))
						+ errorMessageConfigurator,
					errorCode ?? ErrorCodes.ArgNullException.Default);

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static unsafe bool IsArgumentNull<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		[NotNull] void* argument,
		IErrorCode? errorCode,
		string? detail,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (argument is null)
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					x => x
						.InternalMessage(paramName)
						.Detail(detail),
					errorCode ?? ErrorCodes.ArgNullException.Default);

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static unsafe bool IsArgumentNull<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		IntPtr argument,
		IErrorCode? errorCode = null,
		Action<ErrorMessageBuilder>? errorMessageConfigurator = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (argument == IntPtr.Zero)
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					(x => x.InternalMessage(paramName))
						+ errorMessageConfigurator,
					errorCode ?? ErrorCodes.ArgNullException.Default);

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static unsafe bool IsArgumentNull<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		IntPtr argument,
		IErrorCode? errorCode,
		string? detail,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (argument == IntPtr.Zero)
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					x => x
						.InternalMessage(paramName)
						.Detail(detail),
					errorCode ?? ErrorCodes.ArgNullException.Default);

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsArgumentDefault<T, TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		[NotNull] T argument,
		IErrorCode? errorCode = null,
		Action<ErrorMessageBuilder>? errorMessageConfigurator = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		where T : struct, IComparable<T>, IComparable
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (ValidationHelper.IsDefault(argument))
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					(x => x.InternalMessage(paramName))
						+ errorMessageConfigurator,
					errorCode ?? ErrorCodes.ArgDefaultValueException.Default);

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsArgumentDefault<T, TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		[NotNull] T argument,
		IErrorCode? errorCode,
		string? detail,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		where T : struct, IComparable<T>, IComparable
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (ValidationHelper.IsDefault(argument))
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					x => x
						.InternalMessage(paramName)
						.Detail(detail),
					errorCode ?? ErrorCodes.ArgDefaultValueException.Default);

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsArgumentDefault<T, TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		[NotNull] T? argument,
		IErrorCode? errorCode = null,
		Action<ErrorMessageBuilder>? errorMessageConfigurator = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		where T : struct, IComparable<T>, IComparable
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		var isNull = IsArgumentNull(
			resultBuilder,
			scopeContext,
			argument,
			errorCode,
			errorMessageConfigurator,
			memberName,
			sourceFilePath,
			sourceLineNumber,
			paramName);

		if (isNull)
			return true;

		if (ValidationHelper.IsDefault(argument))
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					(x => x.InternalMessage(paramName))
						+ errorMessageConfigurator,
					errorCode ?? ErrorCodes.ArgDefaultValueException.Default);

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsArgumentDefault<T, TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		[NotNull] T? argument,
		IErrorCode? errorCode,
		string? detail,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		where T : struct, IComparable<T>, IComparable
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		var isNull = IsArgumentNull(
			resultBuilder,
			scopeContext,
			argument,
			errorCode,
			detail,
			memberName,
			sourceFilePath,
			sourceLineNumber,
			paramName);

		if (isNull)
			return true;

		if (ValidationHelper.IsDefault(argument))
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					x => x
						.InternalMessage(paramName)
						.Detail(detail),
					errorCode ?? ErrorCodes.ArgDefaultValueException.Default);

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsArgumentNullOrDefault<T, TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		[NotNull] T? argument,
		IErrorCode? errorCode = null,
		Action<ErrorMessageBuilder>? errorMessageConfigurator = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		where T : struct, IComparable<T>, IComparable
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		var isNull = IsArgumentNull(
			resultBuilder,
			scopeContext,
			argument,
			errorCode,
			errorMessageConfigurator,
			memberName,
			sourceFilePath,
			sourceLineNumber,
			paramName);

		if (isNull)
			return true;

		if (ValidationHelper.IsDefault(argument))
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					(x => x.InternalMessage(paramName))
						+ errorMessageConfigurator,
					errorCode ?? ErrorCodes.ArgDefaultValueException.Default);

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsArgumentNullOrDefault<T, TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		[NotNull] T? argument,
		IErrorCode? errorCode,
		string? detail,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		where T : struct, IComparable<T>, IComparable
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		var isNull = IsArgumentNull(
			resultBuilder,
			scopeContext,
			argument,
			errorCode,
			detail,
			memberName,
			sourceFilePath,
			sourceLineNumber,
			paramName);

		if (isNull)
			return true;

		if (ValidationHelper.IsDefault(argument))
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					x => x
						.InternalMessage(paramName)
						.Detail(detail),
					errorCode ?? ErrorCodes.ArgDefaultValueException.Default);

			return true;
		}

		return false;
	}


	[System.Diagnostics.StackTraceHidden]
	public static bool IsArgumentNullOrEmpty<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		[NotNull] string? argument,
		IErrorCode? errorCode = null,
		Action<ErrorMessageBuilder>? errorMessageConfigurator = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

#pragma warning disable L_ArgNullEx_Str // Type or member is obsolete
		var isNull = IsArgumentNull(
			resultBuilder,
			scopeContext,
			argument,
			errorCode,
			errorMessageConfigurator,
			memberName,
			sourceFilePath,
			sourceLineNumber,
			paramName);
#pragma warning restore L_ArgNullEx_Str // Type or member is obsolete

		if (isNull)
			return true;

		if (string.IsNullOrEmpty(argument))
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					(x => x.InternalMessage(paramName))
						+ errorMessageConfigurator,
					errorCode ?? ErrorCodes.ArgEmptyValueException.EmptyString);

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsArgumentNullOrEmpty<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		[NotNull] string? argument,
		IErrorCode? errorCode,
		string? detail,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

#pragma warning disable L_ArgNullEx_Str // Type or member is obsolete
		var isNull = IsArgumentNull(
			resultBuilder,
			scopeContext,
			argument,
			errorCode,
			detail,
			memberName,
			sourceFilePath,
			sourceLineNumber,
			paramName);
#pragma warning restore L_ArgNullEx_Str // Type or member is obsolete

		if (isNull)
			return true;

		if (string.IsNullOrEmpty(argument))
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					x => x
						.InternalMessage(paramName)
						.Detail(detail),
					errorCode ?? ErrorCodes.ArgEmptyValueException.EmptyString);

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsArgumentNullOrWhiteSpace<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		[NotNull] string? argument,
		IErrorCode? errorCode = null,
		Action<ErrorMessageBuilder>? errorMessageConfigurator = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

#pragma warning disable L_ArgNullEx_Str // Type or member is obsolete
		var isNull = IsArgumentNull(
			resultBuilder,
			scopeContext,
			argument,
			errorCode,
			errorMessageConfigurator,
			memberName,
			sourceFilePath,
			sourceLineNumber,
			paramName);
#pragma warning restore L_ArgNullEx_Str // Type or member is obsolete

		if (isNull)
			return true;

		if (string.IsNullOrWhiteSpace(argument))
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					(x => x.InternalMessage(paramName))
						+ errorMessageConfigurator,
					errorCode ?? ErrorCodes.ArgEmptyValueException.WhiteSpace);

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsArgumentNullOrWhiteSpace<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		[NotNull] string? argument,
		IErrorCode? errorCode,
		string? detail,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

#pragma warning disable L_ArgNullEx_Str // Type or member is obsolete
		var isNull = IsArgumentNull(
			resultBuilder,
			scopeContext,
			argument,
			errorCode,
			detail,
			memberName,
			sourceFilePath,
			sourceLineNumber,
			paramName);
#pragma warning restore L_ArgNullEx_Str // Type or member is obsolete

		if (isNull)
			return true;

		if (string.IsNullOrWhiteSpace(argument))
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					x => x
						.InternalMessage(paramName)
						.Detail(detail),
					errorCode ?? ErrorCodes.ArgEmptyValueException.WhiteSpace);

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsArgumentNullOrEmpty<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		[NotNull] ICollection? argument,
		IErrorCode? errorCode = null,
		Action<ErrorMessageBuilder>? errorMessageConfigurator = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		var isNull = IsArgumentNull(
			resultBuilder,
			scopeContext,
			argument,
			errorCode,
			errorMessageConfigurator,
			memberName,
			sourceFilePath,
			sourceLineNumber,
			paramName);

		if (isNull)
			return true;

		if (argument.Count == 0)
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					(x => x.InternalMessage(paramName))
						+ errorMessageConfigurator,
					errorCode ?? ErrorCodes.ArgEmptyValueException.Collection);

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsArgumentNullOrEmpty<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		[NotNull] ICollection? argument,
		IErrorCode? errorCode,
		string? detail,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		var isNull = IsArgumentNull(
			resultBuilder,
			scopeContext,
			argument,
			errorCode,
			detail,
			memberName,
			sourceFilePath,
			sourceLineNumber,
			paramName);

		if (isNull)
			return true;

		if (argument.Count == 0)
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					x => x
						.InternalMessage(paramName)
						.Detail(detail),
					errorCode ?? ErrorCodes.ArgEmptyValueException.Collection);

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsArgumentNullOrEmpty<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		[NotNull] Array? argument,
		IErrorCode? errorCode = null,
		Action<ErrorMessageBuilder>? errorMessageConfigurator = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		var isNull = IsArgumentNull(
			resultBuilder,
			scopeContext,
			argument,
			errorCode,
			errorMessageConfigurator,
			memberName,
			sourceFilePath,
			sourceLineNumber,
			paramName);

		if (isNull)
			return true;

		if (argument.Length == 0)
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					(x => x.InternalMessage(paramName))
						+ errorMessageConfigurator,
					errorCode ?? ErrorCodes.ArgEmptyValueException.Array);

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsArgumentNullOrEmpty<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		[NotNull] Array? argument,
		IErrorCode? errorCode,
		string? detail,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		var isNull = IsArgumentNull(
			resultBuilder,
			scopeContext,
			argument,
			errorCode,
			detail,
			memberName,
			sourceFilePath,
			sourceLineNumber,
			paramName);

		if (isNull)
			return true;

		if (argument.Length == 0)
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					x => x
						.InternalMessage(paramName)
						.Detail(detail),
					errorCode ?? ErrorCodes.ArgEmptyValueException.Array);

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsArgumentNullOrEmpty<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		[NotNull] IEnumerable? argument,
		IErrorCode? errorCode = null,
		Action<ErrorMessageBuilder>? errorMessageConfigurator = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		var isNull = IsArgumentNull(
			resultBuilder,
			scopeContext,
			argument,
			errorCode,
			errorMessageConfigurator,
			memberName,
			sourceFilePath,
			sourceLineNumber,
			paramName);

		if (isNull)
			return true;

		if (!argument.Cast<object>().Any())
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					(x => x.InternalMessage(paramName))
						+ errorMessageConfigurator,
					errorCode ?? ErrorCodes.ArgEmptyValueException.Enumerable);

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsArgumentNullOrEmpty<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		[NotNull] IEnumerable? argument,
		IErrorCode? errorCode,
		string? detail,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		var isNull = IsArgumentNull(
			resultBuilder,
			scopeContext,
			argument,
			errorCode,
			detail,
			memberName,
			sourceFilePath,
			sourceLineNumber,
			paramName);

		if (isNull)
			return true;

		if (!argument.Cast<object>().Any())
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					x => x
						.InternalMessage(paramName)
						.Detail(detail),
					errorCode ?? ErrorCodes.ArgEmptyValueException.Enumerable);

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsArgumentNullOrEmpty<T, TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		[NotNull] T? argument,
		IErrorCode? errorCode = null,
		Action<ErrorMessageBuilder>? errorMessageConfigurator = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		where T : IEnumerable?
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		var isNull = IsArgumentNull(
			resultBuilder,
			scopeContext,
			argument,
			errorCode,
			errorMessageConfigurator,
			memberName,
			sourceFilePath,
			sourceLineNumber,
			paramName);

		if (isNull)
			return true;

		if (!argument.Cast<object>().Any())
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					(x => x.InternalMessage(paramName))
						+ errorMessageConfigurator,
					errorCode ?? ErrorCodes.ArgEmptyValueException.Enumerable);

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsArgumentNullOrEmpty<T, TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		[NotNull] T? argument,
		IErrorCode? errorCode,
		string? detail,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		where T : IEnumerable?
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		var isNull = IsArgumentNull(
			resultBuilder,
			scopeContext,
			argument,
			errorCode,
			detail,
			memberName,
			sourceFilePath,
			sourceLineNumber,
			paramName);

		if (isNull)
			return true;

		if (!argument.Cast<object>().Any())
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					x => x
						.InternalMessage(paramName)
						.Detail(detail),
					errorCode ?? ErrorCodes.ArgEmptyValueException.Enumerable);

			return true;
		}

		return false;
	}



	[System.Diagnostics.StackTraceHidden]
	public static bool IsArgumentZero<T, TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		T argument,
		IErrorCode? errorCode = null,
		Action<ErrorMessageBuilder>? errorMessageConfigurator = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
#if NET8_0_OR_GREATER
		where T : INumberBase<T>
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (T.IsZero(argument))
#else
		where T : IComparable<T>
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (ComparableHelper.IsZero(argument))
#endif
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					(x => x.InternalMessage(paramName))
						+ errorMessageConfigurator,
					errorCode ?? ErrorCodes.ArgOutOfRangeException.NonZero(argument, paramName));

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsArgumentZero<T, TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		T argument,
		IErrorCode? errorCode,
		string? detail,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
#if NET8_0_OR_GREATER
		where T : INumberBase<T>
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (T.IsZero(argument))
#else
		where T : IComparable<T>
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (ComparableHelper.IsZero(argument))
#endif
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					x => x
						.InternalMessage(paramName)
						.Detail(detail),
					errorCode ?? ErrorCodes.ArgOutOfRangeException.NonZero(argument, paramName));

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsArgumentNegative<T, TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		T argument,
		IErrorCode? errorCode = null,
		Action<ErrorMessageBuilder>? errorMessageConfigurator = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
#if NET8_0_OR_GREATER
		where T : INumberBase<T>
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (T.IsNegative(argument))
#else
		where T : IComparable<T>
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (ComparableHelper.IsNegative(argument))
#endif
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					(x => x.InternalMessage(paramName))
						+ errorMessageConfigurator,
					errorCode ?? ErrorCodes.ArgOutOfRangeException.NonNegative(argument, paramName));

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsArgumentNegative<T, TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		T argument,
		IErrorCode? errorCode,
		string? detail,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
#if NET8_0_OR_GREATER
		where T : INumberBase<T>
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (T.IsNegative(argument))
#else
		where T : IComparable<T>
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (ComparableHelper.IsNegative(argument))
#endif
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					x => x
						.InternalMessage(paramName)
						.Detail(detail),
					errorCode ?? ErrorCodes.ArgOutOfRangeException.NonNegative(argument, paramName));

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsArgumentNegativeOrZero<T, TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		T argument,
		IErrorCode? errorCode = null,
		Action<ErrorMessageBuilder>? errorMessageConfigurator = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
#if NET8_0_OR_GREATER
		where T : INumberBase<T>
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (T.IsNegative(argument) || T.IsZero(argument))
#else
		where T : IComparable<T>
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (ComparableHelper.IsNegative(argument) || ComparableHelper.IsZero(argument))
#endif
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					(x => x.InternalMessage(paramName))
						+ errorMessageConfigurator,
					errorCode ?? ErrorCodes.ArgOutOfRangeException.NonNegativeNonZero(argument, paramName));

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsArgumentNegativeOrZero<T, TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		T argument,
		IErrorCode? errorCode,
		string? detail,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
#if NET8_0_OR_GREATER
		where T : INumberBase<T>
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (T.IsNegative(argument) || T.IsZero(argument))
#else
		where T : IComparable<T>
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (ComparableHelper.IsNegative(argument) || ComparableHelper.IsZero(argument))
#endif
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					x => x
						.InternalMessage(paramName)
						.Detail(detail),
					errorCode ?? ErrorCodes.ArgOutOfRangeException.NonNegativeNonZero(argument, paramName));

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsArgumentEqual<T, TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		T argument,
		T other,
		IErrorCode? errorCode = null,
		Action<ErrorMessageBuilder>? errorMessageConfigurator = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		where T : IEquatable<T>?
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (EqualityComparer<T>.Default.Equals(argument, other))
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					(x => x.InternalMessage(paramName))
						+ errorMessageConfigurator,
					errorCode ?? ErrorCodes.ArgOutOfRangeException.NotEqual(argument, other, paramName));

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsArgumentEqual<T, TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		T argument,
		T other,
		IErrorCode? errorCode,
		string? detail,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		where T : IEquatable<T>?
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (EqualityComparer<T>.Default.Equals(argument, other))
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					x => x
						.InternalMessage(paramName)
						.Detail(detail),
					errorCode ?? ErrorCodes.ArgOutOfRangeException.NotEqual(argument, other, paramName));

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsArgumentNotEqual<T, TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		T argument,
		T other,
		IErrorCode? errorCode = null,
		Action<ErrorMessageBuilder>? errorMessageConfigurator = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		where T : IEquatable<T>?
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (!EqualityComparer<T>.Default.Equals(argument, other))
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					(x => x.InternalMessage(paramName))
						+ errorMessageConfigurator,
					errorCode ?? ErrorCodes.ArgOutOfRangeException.Equal(argument, other, paramName));

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsArgumentNotEqual<T, TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		T argument,
		T other,
		IErrorCode? errorCode,
		string? detail,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		where T : IEquatable<T>?
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (!EqualityComparer<T>.Default.Equals(argument, other))
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					x => x
						.InternalMessage(paramName)
						.Detail(detail),
					errorCode ?? ErrorCodes.ArgOutOfRangeException.Equal(argument, other, paramName));

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsArgumentContainsIn<T, TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		T argument,
		IEnumerable<T> others,
		IErrorCode? errorCode = null,
		Action<ErrorMessageBuilder>? errorMessageConfigurator = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		where T : IEquatable<T>?
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (others?.Any(x => EqualityComparer<T>.Default.Equals(argument, x)) == true)
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					(x => x.InternalMessage(paramName))
						+ errorMessageConfigurator,
					errorCode ?? ErrorCodes.ArgOutOfRangeException.NotIn(argument, others, paramName));

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsArgumentContainsIn<T, TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		T argument,
		IEnumerable<T> others,
		IErrorCode? errorCode,
		string? detail,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		where T : IEquatable<T>?
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (others?.Any(x => EqualityComparer<T>.Default.Equals(argument, x)) == true)
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					x => x
						.InternalMessage(paramName)
						.Detail(detail),
					errorCode ?? ErrorCodes.ArgOutOfRangeException.NotIn(argument, others, paramName));

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsArgumentNotContainsIn<T, TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		T argument,
		IEnumerable<T> others,
		IErrorCode? errorCode = null,
		Action<ErrorMessageBuilder>? errorMessageConfigurator = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		where T : IEquatable<T>?
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (others?.Any(x => EqualityComparer<T>.Default.Equals(argument, x)) != true)
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					(x => x.InternalMessage(paramName))
						+ errorMessageConfigurator,
					errorCode ?? ErrorCodes.ArgOutOfRangeException.In(argument, others, paramName));

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsArgumentNotContainsIn<T, TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		T argument,
		IEnumerable<T> others,
		IErrorCode? errorCode,
		string? detail,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		where T : IEquatable<T>?
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (others?.Any(x => EqualityComparer<T>.Default.Equals(argument, x)) != true)
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					x => x
						.InternalMessage(paramName)
						.Detail(detail),
					errorCode ?? ErrorCodes.ArgOutOfRangeException.In(argument, others, paramName));

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsArgumentGreaterThan<T, TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		T argument,
		T other,
		IErrorCode? errorCode = null,
		Action<ErrorMessageBuilder>? errorMessageConfigurator = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		where T : IComparable<T>
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (argument.CompareTo(other) > 0)
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					(x => x.InternalMessage(paramName))
						+ errorMessageConfigurator,
					errorCode ?? ErrorCodes.ArgOutOfRangeException.LessOrEqual(argument, other, paramName));

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsArgumentGreaterThan<T, TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		T argument,
		T other,
		IErrorCode? errorCode,
		string? detail,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		where T : IComparable<T>
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (argument.CompareTo(other) > 0)
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					x => x
						.InternalMessage(paramName)
						.Detail(detail),
					errorCode ?? ErrorCodes.ArgOutOfRangeException.LessOrEqual(argument, other, paramName));

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsArgumentGreaterThanOrEqual<T, TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		T argument,
		T other,
		IErrorCode? errorCode = null,
		Action<ErrorMessageBuilder>? errorMessageConfigurator = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		where T : IComparable<T>
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (argument.CompareTo(other) >= 0)
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					(x => x.InternalMessage(paramName))
						+ errorMessageConfigurator,
					errorCode ?? ErrorCodes.ArgOutOfRangeException.Less(argument, other, paramName));

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsArgumentGreaterThanOrEqual<T, TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		T argument,
		T other,
		IErrorCode? errorCode,
		string? detail,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		where T : IComparable<T>
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (argument.CompareTo(other) >= 0)
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					x => x
						.InternalMessage(paramName)
						.Detail(detail),
					errorCode ?? ErrorCodes.ArgOutOfRangeException.Less(argument, other, paramName));

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsArgumentLessThan<T, TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		T argument,
		T other,
		IErrorCode? errorCode = null,
		Action<ErrorMessageBuilder>? errorMessageConfigurator = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		where T : IComparable<T>
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (argument.CompareTo(other) < 0)
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					(x => x.InternalMessage(paramName))
						+ errorMessageConfigurator,
					errorCode ?? ErrorCodes.ArgOutOfRangeException.GreaterOrEqual(argument, other, paramName));

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsArgumentLessThan<T, TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		T argument,
		T other,
		IErrorCode? errorCode,
		string? detail,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		where T : IComparable<T>
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (argument.CompareTo(other) < 0)
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					x => x
						.InternalMessage(paramName)
						.Detail(detail),
					errorCode ?? ErrorCodes.ArgOutOfRangeException.GreaterOrEqual(argument, other, paramName));

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsArgumentLessThanOrEqual<T, TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		T argument,
		T other,
		IErrorCode? errorCode = null,
		Action<ErrorMessageBuilder>? errorMessageConfigurator = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		where T : IComparable<T>
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (argument.CompareTo(other) <= 0)
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					(x => x.InternalMessage(paramName))
						+ errorMessageConfigurator,
					errorCode ?? ErrorCodes.ArgOutOfRangeException.Greater(argument, other, paramName));

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsArgumentLessThanOrEqual<T, TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		T argument,
		T other,
		IErrorCode? errorCode,
		string? detail,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		where T : IComparable<T>
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (argument.CompareTo(other) <= 0)
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					x => x
						.InternalMessage(paramName)
						.Detail(detail),
					errorCode ?? ErrorCodes.ArgOutOfRangeException.Greater(argument, other, paramName));

			return true;
		}

		return false;
	}






	[System.Diagnostics.StackTraceHidden]
	public static TObject WithArgumentException<T, TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		T value,
		IErrorCode? errorCode = null,
		Action<ErrorMessageBuilder>? errorMessageConfigurator = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(value))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		=> resultBuilder
			.WithErrorInternal(
				scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
				(x => x.InternalMessage(paramName))
					+ errorMessageConfigurator,
				errorCode ?? ErrorCodes.ArgException.Default)
			.Build();

	[System.Diagnostics.StackTraceHidden]
	public static TObject WithArgumentException<T, TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		T value,
		IErrorCode? errorCode,
		string? detail,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(value))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		=> resultBuilder
			.WithErrorInternal(
				scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
				x => x
					.InternalMessage(paramName)
					.Detail(detail),
				errorCode ?? ErrorCodes.ArgException.Default)
			.Build();






	[System.Diagnostics.StackTraceHidden]
	public static TObject WithArgumentOutOfRangeException<T, TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		T value,
		IErrorCode? errorCode = null,
		Action<ErrorMessageBuilder>? errorMessageConfigurator = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(value))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		=> resultBuilder
			.WithErrorInternal(
				scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
				(x => x.InternalMessage(paramName))
					+ errorMessageConfigurator,
				errorCode ?? ErrorCodes.ArgOutOfRangeException.Default)
			.Build();

	[System.Diagnostics.StackTraceHidden]
	public static TObject WithArgumentOutOfRangeException<T, TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		T value,
		IErrorCode? errorCode,
		string? detail,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(value))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		=> resultBuilder
			.WithErrorInternal(
				scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
				x => x
					.InternalMessage(paramName)
					.Detail(detail),
				errorCode ?? ErrorCodes.ArgOutOfRangeException.Default)
			.Build();




	[System.Diagnostics.StackTraceHidden]
	[Obsolete("Use IsNullOrEmpty or IsNullOrWhiteSpace instead. Do not use with nameof()"
#if NET6_0_OR_GREATER
	, DiagnosticId = "L_NullEx_Str")]
#else
	)]
#endif
	public static bool IsNull<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		[NotNull] string? argument,
		IErrorCode? errorCode = null,
		Action<ErrorMessageBuilder>? errorMessageConfigurator = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (argument is null)
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					(x => x.InternalMessage(paramName))
						+ errorMessageConfigurator,
					errorCode ?? ErrorCodes.NullValueException.Default);

#pragma warning disable CS8777 // Parameter must have a non-null value when exiting.
			return true;
#pragma warning restore CS8777 // Parameter must have a non-null value when exiting.
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	[Obsolete("Use IsNullOrEmpty or IsNullOrWhiteSpace instead. Do not use with nameof()"
#if NET6_0_OR_GREATER
	, DiagnosticId = "L_NullEx_Str")]
#else
	)]
#endif
	public static bool IsNull<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		[NotNull] string? argument,
		IErrorCode? errorCode,
		string? detail,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (argument is null)
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					x => x
						.InternalMessage(paramName)
						.Detail(detail),
					errorCode ?? ErrorCodes.NullValueException.Default);

#pragma warning disable CS8777 // Parameter must have a non-null value when exiting.
			return true;
#pragma warning restore CS8777 // Parameter must have a non-null value when exiting.
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsNull<T, TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		[NotNull] T? argument,
		IErrorCode? errorCode = null,
		Action<ErrorMessageBuilder>? errorMessageConfigurator = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (argument is null)
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					(x => x.InternalMessage(paramName))
						+ errorMessageConfigurator,
					errorCode ?? ErrorCodes.NullValueException.Default);

#pragma warning disable CS8777 // Parameter must have a non-null value when exiting.
			return true;
#pragma warning restore CS8777 // Parameter must have a non-null value when exiting.
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsNull<T, TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		[NotNull] T? argument,
		IErrorCode? errorCode,
		string? detail,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (argument is null)
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					x => x
						.InternalMessage(paramName)
						.Detail(detail),
					errorCode ?? ErrorCodes.NullValueException.Default);

#pragma warning disable CS8777 // Parameter must have a non-null value when exiting.
			return true;
#pragma warning restore CS8777 // Parameter must have a non-null value when exiting.
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsNull<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		[NotNull] object? argument,
		IErrorCode? errorCode = null,
		Action<ErrorMessageBuilder>? errorMessageConfigurator = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (argument is null)
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					(x => x.InternalMessage(paramName))
						+ errorMessageConfigurator,
					errorCode ?? ErrorCodes.NullValueException.Default);

#pragma warning disable CS8777 // Parameter must have a non-null value when exiting.
			return true;
#pragma warning restore CS8777 // Parameter must have a non-null value when exiting.
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsNull<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		[NotNull] object? argument,
		IErrorCode? errorCode,
		string? detail,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (argument is null)
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					x => x
						.InternalMessage(paramName)
						.Detail(detail),
					errorCode ?? ErrorCodes.NullValueException.Default);

#pragma warning disable CS8777 // Parameter must have a non-null value when exiting.
			return true;
#pragma warning restore CS8777 // Parameter must have a non-null value when exiting.
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static unsafe bool IsNull<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		[NotNull] void* argument,
		IErrorCode? errorCode = null,
		Action<ErrorMessageBuilder>? errorMessageConfigurator = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (argument is null)
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					(x => x.InternalMessage(paramName))
						+ errorMessageConfigurator,
					errorCode ?? ErrorCodes.NullValueException.Default);

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static unsafe bool IsNull<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		[NotNull] void* argument,
		IErrorCode? errorCode,
		string? detail,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (argument is null)
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					x => x
						.InternalMessage(paramName)
						.Detail(detail),
					errorCode ?? ErrorCodes.NullValueException.Default);

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static unsafe bool IsNull<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		IntPtr argument,
		IErrorCode? errorCode = null,
		Action<ErrorMessageBuilder>? errorMessageConfigurator = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (argument == IntPtr.Zero)
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					(x => x.InternalMessage(paramName))
						+ errorMessageConfigurator,
					errorCode ?? ErrorCodes.NullValueException.Default);

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static unsafe bool IsNull<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		IntPtr argument,
		IErrorCode? errorCode,
		string? detail,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (argument == IntPtr.Zero)
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					x => x
						.InternalMessage(paramName)
						.Detail(detail),
					errorCode ?? ErrorCodes.NullValueException.Default);

			return true;
		}

		return false;
	}


	[System.Diagnostics.StackTraceHidden]
	public static bool IsDefault<T, TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		[NotNull] T argument,
		IErrorCode? errorCode = null,
		Action<ErrorMessageBuilder>? errorMessageConfigurator = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		where T : struct, IComparable<T>, IComparable
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (ValidationHelper.IsDefault(argument))
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					(x => x.InternalMessage(paramName))
						+ errorMessageConfigurator,
					errorCode ?? ErrorCodes.DefaultValueException.Default);

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsDefault<T, TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		[NotNull] T argument,
		IErrorCode? errorCode,
		string? detail,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		where T : struct, IComparable<T>, IComparable
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (ValidationHelper.IsDefault(argument))
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					x => x
						.InternalMessage(paramName)
						.Detail(detail),
					errorCode ?? ErrorCodes.DefaultValueException.Default);

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsDefault<T, TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		[NotNull] T? argument,
		IErrorCode? errorCode = null,
		Action<ErrorMessageBuilder>? errorMessageConfigurator = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		where T : struct, IComparable<T>, IComparable
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		var isNull = IsNull(
			resultBuilder,
			scopeContext,
			argument,
			errorCode,
			errorMessageConfigurator,
			memberName,
			sourceFilePath,
			sourceLineNumber,
			paramName);

		if (isNull)
			return true;

		if (ValidationHelper.IsDefault(argument))
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					(x => x.InternalMessage(paramName))
						+ errorMessageConfigurator,
					errorCode ?? ErrorCodes.DefaultValueException.Default);

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsDefault<T, TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		[NotNull] T? argument,
		IErrorCode? errorCode,
		string? detail,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		where T : struct, IComparable<T>, IComparable
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		var isNull = IsNull(
			resultBuilder,
			scopeContext,
			argument,
			errorCode,
			detail,
			memberName,
			sourceFilePath,
			sourceLineNumber,
			paramName);

		if (isNull)
			return true;

		if (ValidationHelper.IsDefault(argument))
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					x => x
						.InternalMessage(paramName)
						.Detail(detail),
					errorCode ?? ErrorCodes.DefaultValueException.Default);

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsNullOrDefault<T, TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		[NotNull] T? argument,
		IErrorCode? errorCode = null,
		Action<ErrorMessageBuilder>? errorMessageConfigurator = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		where T : struct, IComparable<T>, IComparable
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		var isNull = IsNull(
			resultBuilder,
			scopeContext,
			argument,
			errorCode,
			errorMessageConfigurator,
			memberName,
			sourceFilePath,
			sourceLineNumber,
			paramName);

		if (isNull)
			return true;

		if (ValidationHelper.IsDefault(argument))
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					(x => x.InternalMessage(paramName))
						+ errorMessageConfigurator,
					errorCode ?? ErrorCodes.DefaultValueException.Default);

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsNullOrDefault<T, TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		[NotNull] T? argument,
		IErrorCode? errorCode,
		string? detail,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		where T : struct, IComparable<T>, IComparable
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		var isNull = IsNull(
			resultBuilder,
			scopeContext,
			argument,
			errorCode,
			detail,
			memberName,
			sourceFilePath,
			sourceLineNumber,
			paramName);

		if (isNull)
			return true;

		if (ValidationHelper.IsDefault(argument))
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					x => x
						.InternalMessage(paramName)
						.Detail(detail),
					errorCode ?? ErrorCodes.DefaultValueException.Default);

			return true;
		}

		return false;
	}




	[System.Diagnostics.StackTraceHidden]
	public static bool IsNullOrEmpty<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		[NotNull] string? argument,
		IErrorCode? errorCode = null,
		Action<ErrorMessageBuilder>? errorMessageConfigurator = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

#pragma warning disable L_NullEx_Str // Type or member is obsolete
		var isNull = IsNull(
			resultBuilder,
			scopeContext,
			argument,
			errorCode,
			errorMessageConfigurator,
			memberName,
			sourceFilePath,
			sourceLineNumber,
			paramName);
#pragma warning restore L_NullEx_Str // Type or member is obsolete

		if (isNull)
			return true;

		if (string.IsNullOrEmpty(argument))
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					(x => x.InternalMessage(paramName))
						+ errorMessageConfigurator,
					errorCode ?? ErrorCodes.EmptyValueException.EmptyString);

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsNullOrEmpty<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		[NotNull] string? argument,
		IErrorCode? errorCode,
		string? detail,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

#pragma warning disable L_NullEx_Str // Type or member is obsolete
		var isNull = IsNull(
			resultBuilder,
			scopeContext,
			argument,
			errorCode,
			detail,
			memberName,
			sourceFilePath,
			sourceLineNumber,
			paramName);
#pragma warning restore L_NullEx_Str // Type or member is obsolete

		if (isNull)
			return true;

		if (string.IsNullOrEmpty(argument))
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					x => x
						.InternalMessage(paramName)
						.Detail(detail),
					errorCode ?? ErrorCodes.EmptyValueException.EmptyString);

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsNullOrWhiteSpace<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		[NotNull] string? argument,
		IErrorCode? errorCode = null,
		Action<ErrorMessageBuilder>? errorMessageConfigurator = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

#pragma warning disable L_NullEx_Str // Type or member is obsolete
		var isNull = IsNull(
			resultBuilder,
			scopeContext,
			argument,
			errorCode,
			errorMessageConfigurator,
			memberName,
			sourceFilePath,
			sourceLineNumber,
			paramName);
#pragma warning restore L_NullEx_Str // Type or member is obsolete

		if (isNull)
			return true;

		if (string.IsNullOrWhiteSpace(argument))
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					(x => x.InternalMessage(paramName))
						+ errorMessageConfigurator,
					errorCode ?? ErrorCodes.EmptyValueException.WhiteSpace);

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsNullOrWhiteSpace<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		[NotNull] string? argument,
		IErrorCode? errorCode,
		string? detail,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

#pragma warning disable L_NullEx_Str // Type or member is obsolete
		var isNull = IsNull(
			resultBuilder,
			scopeContext,
			argument,
			errorCode,
			detail,
			memberName,
			sourceFilePath,
			sourceLineNumber,
			paramName);
#pragma warning restore L_NullEx_Str // Type or member is obsolete

		if (isNull)
			return true;

		if (string.IsNullOrWhiteSpace(argument))
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					x => x
						.InternalMessage(paramName)
						.Detail(detail),
					errorCode ?? ErrorCodes.EmptyValueException.WhiteSpace);

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsNullOrEmpty<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		[NotNull] ICollection? argument,
		IErrorCode? errorCode = null,
		Action<ErrorMessageBuilder>? errorMessageConfigurator = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		var isNull = IsNull(
			resultBuilder,
			scopeContext,
			argument,
			errorCode,
			errorMessageConfigurator,
			memberName,
			sourceFilePath,
			sourceLineNumber,
			paramName);

		if (isNull)
			return true;

		if (argument.Count == 0)
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					(x => x.InternalMessage(paramName))
						+ errorMessageConfigurator,
					errorCode ?? ErrorCodes.EmptyValueException.Collection);

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsNullOrEmpty<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		[NotNull] ICollection? argument,
		IErrorCode? errorCode,
		string? detail,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		var isNull = IsNull(
			resultBuilder,
			scopeContext,
			argument,
			errorCode,
			detail,
			memberName,
			sourceFilePath,
			sourceLineNumber,
			paramName);

		if (isNull)
			return true;

		if (argument.Count == 0)
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					x => x
						.InternalMessage(paramName)
						.Detail(detail),
					errorCode ?? ErrorCodes.EmptyValueException.Collection);

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsNullOrEmpty<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		[NotNull] Array? argument,
		IErrorCode? errorCode = null,
		Action<ErrorMessageBuilder>? errorMessageConfigurator = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		var isNull = IsNull(
			resultBuilder,
			scopeContext,
			argument,
			errorCode,
			errorMessageConfigurator,
			memberName,
			sourceFilePath,
			sourceLineNumber,
			paramName);

		if (isNull)
			return true;

		if (argument.Length == 0)
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					(x => x.InternalMessage(paramName))
						+ errorMessageConfigurator,
					errorCode ?? ErrorCodes.EmptyValueException.Array);

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsNullOrEmpty<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		[NotNull] Array? argument,
		IErrorCode? errorCode,
		string? detail,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		var isNull = IsNull(
			resultBuilder,
			scopeContext,
			argument,
			errorCode,
			detail,
			memberName,
			sourceFilePath,
			sourceLineNumber,
			paramName);

		if (isNull)
			return true;

		if (argument.Length == 0)
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					x => x
						.InternalMessage(paramName)
						.Detail(detail),
					errorCode ?? ErrorCodes.EmptyValueException.Array);

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsNullOrEmpty<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		[NotNull] IEnumerable? argument,
		IErrorCode? errorCode = null,
		Action<ErrorMessageBuilder>? errorMessageConfigurator = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		var isNull = IsNull(
			resultBuilder,
			scopeContext,
			argument,
			errorCode,
			errorMessageConfigurator,
			memberName,
			sourceFilePath,
			sourceLineNumber,
			paramName);

		if (isNull)
			return true;

		if (!argument.Cast<object>().Any())
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					(x => x.InternalMessage(paramName))
						+ errorMessageConfigurator,
					errorCode ?? ErrorCodes.EmptyValueException.Enumerable);

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsNullOrEmpty<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		[NotNull] IEnumerable? argument,
		IErrorCode? errorCode,
		string? detail,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		var isNull = IsNull(
			resultBuilder,
			scopeContext,
			argument,
			errorCode,
			detail,
			memberName,
			sourceFilePath,
			sourceLineNumber,
			paramName);

		if (isNull)
			return true;

		if (!argument.Cast<object>().Any())
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					x => x
						.InternalMessage(paramName)
						.Detail(detail),
					errorCode ?? ErrorCodes.EmptyValueException.Enumerable);

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsNullOrEmpty<T, TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		[NotNull] T? argument,
		IErrorCode? errorCode = null,
		Action<ErrorMessageBuilder>? errorMessageConfigurator = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		where T : IEnumerable?
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		var isNull = IsNull(
			resultBuilder,
			scopeContext,
			argument,
			errorCode,
			errorMessageConfigurator,
			memberName,
			sourceFilePath,
			sourceLineNumber,
			paramName);

		if (isNull)
			return true;

		if (!argument.Cast<object>().Any())
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					(x => x.InternalMessage(paramName))
						+ errorMessageConfigurator,
					errorCode ?? ErrorCodes.EmptyValueException.Enumerable);

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsNullOrEmpty<T, TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		[NotNull] T? argument,
		IErrorCode? errorCode,
		string? detail,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		where T : IEnumerable?
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		var isNull = IsNull(
			resultBuilder,
			scopeContext,
			argument,
			errorCode,
			detail,
			memberName,
			sourceFilePath,
			sourceLineNumber,
			paramName);

		if (isNull)
			return true;

		if (!argument.Cast<object>().Any())
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					x => x
						.InternalMessage(paramName)
						.Detail(detail),
					errorCode ?? ErrorCodes.EmptyValueException.Enumerable);

			return true;
		}

		return false;
	}















	[System.Diagnostics.StackTraceHidden]
	public static bool IsZero<T, TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		T value,
		IErrorCode? errorCode = null,
		Action<ErrorMessageBuilder>? errorMessageConfigurator = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(value))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
#if NET8_0_OR_GREATER
		where T : INumberBase<T>
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (T.IsZero(value))
#else
		where T : IComparable<T>
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (ComparableHelper.IsZero(value))
#endif
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					(x => x.InternalMessage(paramName))
						+ errorMessageConfigurator,
					errorCode ?? ErrorCodes.OutOfRangeException.NonZero(value, paramName));

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsZero<T, TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		T value,
		IErrorCode? errorCode,
		string? detail,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(value))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
#if NET8_0_OR_GREATER
		where T : INumberBase<T>
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (T.IsZero(value))
#else
		where T : IComparable<T>
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (ComparableHelper.IsZero(value))
#endif
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					x => x
						.InternalMessage(paramName)
						.Detail(detail),
					errorCode ?? ErrorCodes.OutOfRangeException.NonZero(value, paramName));

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsNegative<T, TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		T value,
		IErrorCode? errorCode = null,
		Action<ErrorMessageBuilder>? errorMessageConfigurator = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(value))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
#if NET8_0_OR_GREATER
		where T : INumberBase<T>
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (T.IsNegative(value))
#else
		where T : IComparable<T>
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (ComparableHelper.IsNegative(value))
#endif
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					(x => x.InternalMessage(paramName))
						+ errorMessageConfigurator,
					errorCode ?? ErrorCodes.OutOfRangeException.NonNegative(value, paramName));

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsNegative<T, TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		T value,
		IErrorCode? errorCode,
		string? detail,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(value))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
#if NET8_0_OR_GREATER
		where T : INumberBase<T>
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (T.IsNegative(value))
#else
		where T : IComparable<T>
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (ComparableHelper.IsNegative(value))
#endif
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					x => x
						.InternalMessage(paramName)
						.Detail(detail),
					errorCode ?? ErrorCodes.OutOfRangeException.NonNegative(value, paramName));

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsNegativeOrZero<T, TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		T value,
		IErrorCode? errorCode = null,
		Action<ErrorMessageBuilder>? errorMessageConfigurator = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(value))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
#if NET8_0_OR_GREATER
		where T : INumberBase<T>
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (T.IsNegative(value) || T.IsZero(value))
#else
		where T : IComparable<T>
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (ComparableHelper.IsNegative(value) || ComparableHelper.IsZero(value))
#endif
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					(x => x.InternalMessage(paramName))
						+ errorMessageConfigurator,
					errorCode ?? ErrorCodes.OutOfRangeException.NonNegativeNonZero(value, paramName));

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsNegativeOrZero<T, TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		T value,
		IErrorCode? errorCode,
		string? detail,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(value))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
#if NET8_0_OR_GREATER
		where T : INumberBase<T>
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (T.IsNegative(value) || T.IsZero(value))
#else
		where T : IComparable<T>
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (ComparableHelper.IsNegative(value) || ComparableHelper.IsZero(value))
#endif
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					x => x
						.InternalMessage(paramName)
						.Detail(detail),
					errorCode ?? ErrorCodes.OutOfRangeException.NonNegativeNonZero(value, paramName));

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsEqual<T, TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		T value,
		T other,
		IErrorCode? errorCode = null,
		Action<ErrorMessageBuilder>? errorMessageConfigurator = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(value))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		where T : IEquatable<T>?
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (EqualityComparer<T>.Default.Equals(value, other))
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					(x => x.InternalMessage(paramName))
						+ errorMessageConfigurator,
					errorCode ?? ErrorCodes.OutOfRangeException.NotEqual(value, other, paramName));

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsEqual<T, TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		T value,
		T other,
		IErrorCode? errorCode,
		string? detail,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(value))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		where T : IEquatable<T>?
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (EqualityComparer<T>.Default.Equals(value, other))
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					x => x
						.InternalMessage(paramName)
						.Detail(detail),
					errorCode ?? ErrorCodes.OutOfRangeException.NotEqual(value, other, paramName));

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsNotEqual<T, TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		T value,
		T other,
		IErrorCode? errorCode = null,
		Action<ErrorMessageBuilder>? errorMessageConfigurator = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(value))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		where T : IEquatable<T>?
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (!EqualityComparer<T>.Default.Equals(value, other))
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					(x => x.InternalMessage(paramName))
						+ errorMessageConfigurator,
					errorCode ?? ErrorCodes.OutOfRangeException.Equal(value, other, paramName));

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsNotEqual<T, TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		T value,
		T other,
		IErrorCode? errorCode,
		string? detail,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(value))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		where T : IEquatable<T>?
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (!EqualityComparer<T>.Default.Equals(value, other))
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					x => x
						.InternalMessage(paramName)
						.Detail(detail),
					errorCode ?? ErrorCodes.OutOfRangeException.Equal(value, other, paramName));

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsContainsIn<T, TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		T value,
		IEnumerable<T> others,
		IErrorCode? errorCode = null,
		Action<ErrorMessageBuilder>? errorMessageConfigurator = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(value))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		where T : IEquatable<T>?
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (others?.Any(x => EqualityComparer<T>.Default.Equals(value, x)) == true)
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					(x => x.InternalMessage(paramName))
						+ errorMessageConfigurator,
					errorCode ?? ErrorCodes.OutOfRangeException.NotIn(value, others, paramName));

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsContainsIn<T, TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		T value,
		IEnumerable<T> others,
		IErrorCode? errorCode,
		string? detail,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(value))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		where T : IEquatable<T>?
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (others?.Any(x => EqualityComparer<T>.Default.Equals(value, x)) == true)
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					x => x
						.InternalMessage(paramName)
						.Detail(detail),
					errorCode ?? ErrorCodes.OutOfRangeException.NotIn(value, others, paramName));

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsNotContainsIn<T, TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		T value,
		IEnumerable<T> others,
		IErrorCode? errorCode = null,
		Action<ErrorMessageBuilder>? errorMessageConfigurator = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(value))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		where T : IEquatable<T>?
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (others?.Any(x => EqualityComparer<T>.Default.Equals(value, x)) != true)
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					(x => x.InternalMessage(paramName))
						+ errorMessageConfigurator,
					errorCode ?? ErrorCodes.OutOfRangeException.In(value, others, paramName));

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsNotContainsIn<T, TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		T value,
		IEnumerable<T> others,
		IErrorCode? errorCode,
		string? detail,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(value))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		where T : IEquatable<T>?
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (others?.Any(x => EqualityComparer<T>.Default.Equals(value, x)) != true)
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					x => x
						.InternalMessage(paramName)
						.Detail(detail),
					errorCode ?? ErrorCodes.OutOfRangeException.In(value, others, paramName));

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsGreaterThan<T, TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		T value,
		T other,
		IErrorCode? errorCode = null,
		Action<ErrorMessageBuilder>? errorMessageConfigurator = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(value))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		where T : IComparable<T>
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (value.CompareTo(other) > 0)
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					(x => x.InternalMessage(paramName))
						+ errorMessageConfigurator,
					errorCode ?? ErrorCodes.OutOfRangeException.LessOrEqual(value, other, paramName));

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsGreaterThan<T, TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		T value,
		T other,
		IErrorCode? errorCode,
		string? detail,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(value))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		where T : IComparable<T>
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (value.CompareTo(other) > 0)
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					x => x
						.InternalMessage(paramName)
						.Detail(detail),
					errorCode ?? ErrorCodes.OutOfRangeException.LessOrEqual(value, other, paramName));

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsGreaterThanOrEqual<T, TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		T value,
		T other,
		IErrorCode? errorCode = null,
		Action<ErrorMessageBuilder>? errorMessageConfigurator = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(value))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		where T : IComparable<T>
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (value.CompareTo(other) >= 0)
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					(x => x.InternalMessage(paramName))
						+ errorMessageConfigurator,
					errorCode ?? ErrorCodes.OutOfRangeException.Less(value, other, paramName));

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsGreaterThanOrEqual<T, TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		T value,
		T other,
		IErrorCode? errorCode,
		string? detail,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(value))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		where T : IComparable<T>
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (value.CompareTo(other) >= 0)
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					x => x
						.InternalMessage(paramName)
						.Detail(detail),
					errorCode ?? ErrorCodes.OutOfRangeException.Less(value, other, paramName));

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsLessThan<T, TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		T value,
		T other,
		IErrorCode? errorCode = null,
		Action<ErrorMessageBuilder>? errorMessageConfigurator = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(value))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		where T : IComparable<T>
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (value.CompareTo(other) < 0)
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					(x => x.InternalMessage(paramName))
						+ errorMessageConfigurator,
					errorCode ?? ErrorCodes.OutOfRangeException.GreaterOrEqual(value, other, paramName));

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsLessThan<T, TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		T value,
		T other,
		IErrorCode? errorCode,
		string? detail,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(value))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		where T : IComparable<T>
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (value.CompareTo(other) < 0)
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					x => x
						.InternalMessage(paramName)
						.Detail(detail),
					errorCode ?? ErrorCodes.OutOfRangeException.GreaterOrEqual(value, other, paramName));

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsLessThanOrEqual<T, TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		T value,
		T other,
		IErrorCode? errorCode = null,
		Action<ErrorMessageBuilder>? errorMessageConfigurator = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(value))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		where T : IComparable<T>
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (value.CompareTo(other) <= 0)
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					(x => x.InternalMessage(paramName))
						+ errorMessageConfigurator,
					errorCode ?? ErrorCodes.OutOfRangeException.Greater(value, other, paramName));

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsLessThanOrEqual<T, TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		T value,
		T other,
		IErrorCode? errorCode,
		string? detail,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(value))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		where T : IComparable<T>
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (value.CompareTo(other) <= 0)
		{
			resultBuilder
				.WithErrorInternal(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					x => x
						.InternalMessage(paramName)
						.Detail(detail),
					errorCode ?? ErrorCodes.OutOfRangeException.Greater(value, other, paramName));

			return true;
		}

		return false;
	}






	[System.Diagnostics.StackTraceHidden]
	public static TObject WithOutOfRangeException<T, TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		T value,
		IErrorCode? errorCode = null,
		Action<ErrorMessageBuilder>? errorMessageConfigurator = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(value))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		=> resultBuilder
			.WithErrorInternal(
				scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
				(x => x.InternalMessage(paramName))
					+ errorMessageConfigurator,
				errorCode ?? ErrorCodes.OutOfRangeException.Default)
			.Build();

	[System.Diagnostics.StackTraceHidden]
	public static TObject WithOutOfRangeException<T, TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		T value,
		IErrorCode? errorCode,
		string? detail,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(value))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		=> resultBuilder
			.WithErrorInternal(
				scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
				x => x
					.InternalMessage(paramName)
					.Detail(detail),
				errorCode ?? ErrorCodes.OutOfRangeException.Default)
			.Build();






	[System.Diagnostics.StackTraceHidden]
	public static TObject WithAuthenticationException<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		IErrorCode? errorCode = null,
		Action<ErrorMessageBuilder>? errorMessageConfigurator = null,
		Exception? innerException = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		=> resultBuilder
			.WithErrorInternal(
				scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
				(x => x.ExceptionInfo(innerException))
					+ errorMessageConfigurator,
				errorCode ?? ErrorCodes.AuthenticationException.Default)
			.Build();

	[System.Diagnostics.StackTraceHidden]
	public static TObject WithAuthenticationException<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		IErrorCode? errorCode,
		string? detail,
		Exception? innerException = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		=> resultBuilder
			.WithErrorInternal(
				scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
				x => x
					.ExceptionInfo(innerException)
					.Detail(detail),
				errorCode ?? ErrorCodes.AuthenticationException.Default)
			.Build();


	[System.Diagnostics.StackTraceHidden]
	public static TObject WithUnauthorizedException<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		IErrorCode? errorCode = null,
		Action<ErrorMessageBuilder>? errorMessageConfigurator = null,
		Exception? innerException = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		=> resultBuilder
			.WithErrorInternal(
				scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
				(x => x.ExceptionInfo(innerException))
					+ errorMessageConfigurator,
				errorCode ?? ErrorCodes.UnauthorizedException.Default)
			.Build();

	[System.Diagnostics.StackTraceHidden]
	public static TObject WithUnauthorizedException<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		IErrorCode? errorCode,
		string? detail,
		Exception? innerException = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		=> resultBuilder
			.WithErrorInternal(
				scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
				x => x
					.ExceptionInfo(innerException)
					.Detail(detail),
				errorCode ?? ErrorCodes.UnauthorizedException.Default)
			.Build();

	[System.Diagnostics.StackTraceHidden]
	public static TObject WithUnauthorizedException<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		Func<Identity.LegionIdentity, bool> permissonDelegate,
		IErrorCode? errorCode,
		Exception? innerException = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(permissonDelegate))] string? permissonDelegateName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		=> resultBuilder
			.WithErrorInternal(
				scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
				x => x
					.ExceptionInfo(innerException)
					.Detail(permissonDelegateName),
				errorCode ?? ErrorCodes.UnauthorizedException.Default)
			.Build();


	[System.Diagnostics.StackTraceHidden]
	public static TObject WithConfigurationException<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		IErrorCode? errorCode = null,
		Action<ErrorMessageBuilder>? errorMessageConfigurator = null,
		Exception? innerException = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		=> resultBuilder
			.WithErrorInternal(
				scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
				(x => x.ExceptionInfo(innerException))
					+ errorMessageConfigurator,
				errorCode ?? ErrorCodes.ConfigurationException.Default)
			.Build();


	[System.Diagnostics.StackTraceHidden]
	public static TObject WithConfigurationException<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		IErrorCode? errorCode,
		string? detail,
		Exception? innerException = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		=> resultBuilder
			.WithErrorInternal(
				scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
				x => x
					.ExceptionInfo(innerException)
					.Detail(detail),
				errorCode ?? ErrorCodes.ConfigurationException.Default)
			.Build();


	[System.Diagnostics.StackTraceHidden]
	public static TObject WithDecorationException<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		IErrorCode? errorCode = null,
		Action<ErrorMessageBuilder>? errorMessageConfigurator = null,
		Exception? innerException = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		=> resultBuilder
			.WithErrorInternal(
				scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
				(x => x.ExceptionInfo(innerException))
					+ errorMessageConfigurator,
				errorCode ?? ErrorCodes.DecorationException.Default)
			.Build();


	[System.Diagnostics.StackTraceHidden]
	public static TObject WithDecorationException<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		IErrorCode? errorCode,
		string? detail,
		Exception? innerException = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		=> resultBuilder
			.WithErrorInternal(
				scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
				x => x
					.ExceptionInfo(innerException)
					.Detail(detail),
				errorCode ?? ErrorCodes.DecorationException.Default)
			.Build();



	[System.Diagnostics.StackTraceHidden]
	public static TObject WithInitializationException<T, TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		T value,
		IErrorCode? errorCode = null,
		Action<ErrorMessageBuilder>? errorMessageConfigurator = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(value))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		=> resultBuilder
			.WithErrorInternal(
				scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
				(x => x.InternalMessage(paramName))
					+ errorMessageConfigurator,
				errorCode ?? ErrorCodes.InitializationException.NotInitialized(paramName!))
			.Build();

	[System.Diagnostics.StackTraceHidden]
	public static TObject WithInitializationException<T, TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		T value,
		IErrorCode? errorCode,
		string? detail,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0,
		[CallerArgumentExpression(nameof(value))] string? paramName = null)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		=> resultBuilder
			.WithErrorInternal(
				scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
				x => x
					.InternalMessage(paramName)
					.Detail(detail),
				errorCode ?? ErrorCodes.InitializationException.NotInitialized(paramName!))
			.Build();


	[System.Diagnostics.StackTraceHidden]
	public static TObject WithInitializationException<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		IErrorCode? errorCode = null,
		Action<ErrorMessageBuilder>? errorMessageConfigurator = null,
		Exception? innerException = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		=> resultBuilder
			.WithErrorInternal(
				scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
				(x => x.ExceptionInfo(innerException))
					+ errorMessageConfigurator,
				errorCode ?? ErrorCodes.InitializationException.Default)
			.Build();


	[System.Diagnostics.StackTraceHidden]
	public static TObject WithInitializationException<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		IErrorCode? errorCode,
		string? detail,
		Exception? innerException = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		=> resultBuilder
			.WithErrorInternal(
				scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
				x => x
					.ExceptionInfo(innerException)
					.Detail(detail),
				errorCode ?? ErrorCodes.InitializationException.Default)
			.Build();


	[System.Diagnostics.StackTraceHidden]
	public static TObject WithInvalidOperationException<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		IErrorCode? errorCode = null,
		Action<ErrorMessageBuilder>? errorMessageConfigurator = null,
		Exception? innerException = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		=> resultBuilder
			.WithErrorInternal(
				scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
				(x => x.ExceptionInfo(innerException))
					+ errorMessageConfigurator,
				errorCode ?? ErrorCodes.InvalidOpException.Default)
			.Build();


	[System.Diagnostics.StackTraceHidden]
	public static TObject WithInvalidOperationException<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		IErrorCode? errorCode,
		string? detail,
		Exception? innerException = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		=> resultBuilder
			.WithErrorInternal(
				scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
				x => x
					.ExceptionInfo(innerException)
					.Detail(detail),
				errorCode ?? ErrorCodes.InvalidOpException.Default)
			.Build();


	[System.Diagnostics.StackTraceHidden]
	public static TObject WithOperationCanceledException<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		IErrorCode? errorCode = null,
		Action<ErrorMessageBuilder>? errorMessageConfigurator = null,
		Exception? innerException = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		=> resultBuilder
			.WithErrorInternal(
				scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
				(x => x.ExceptionInfo(innerException))
					+ errorMessageConfigurator,
				errorCode ?? ErrorCodes.OpCanceledException.Default)
			.Build();


	[System.Diagnostics.StackTraceHidden]
	public static TObject WithOperationCanceledException<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		IErrorCode? errorCode,
		string? detail,
		Exception? innerException = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		=> resultBuilder
			.WithErrorInternal(
				scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
				x => x
					.ExceptionInfo(innerException)
					.Detail(detail),
				errorCode ?? ErrorCodes.OpCanceledException.Default)
			.Build();

	[System.Diagnostics.StackTraceHidden]
	public static bool IsCancellationRequested<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		CancellationToken cancellationToken,
		IScopeContext scopeContext,
		IErrorCode? errorCode = null,
		Action<ErrorMessageBuilder>? errorMessageConfigurator = null,
		Exception? innerException = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (cancellationToken.IsCancellationRequested)
		{
			resultBuilder
				.WithOperationCanceledException(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					errorCode ?? ErrorCodes.OpCanceledException.Default,
					(x => x.ExceptionInfo(innerException))
						+ errorMessageConfigurator);

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsCancellationRequested<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		CancellationToken cancellationToken,
		IScopeContext scopeContext,
		IErrorCode? errorCode,
		string? detail,
		Exception? innerException = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (cancellationToken.IsCancellationRequested)
		{
			resultBuilder
				.WithOperationCanceledException(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					errorCode ?? ErrorCodes.OpCanceledException.Default,
					x => x
						.ExceptionInfo(innerException)
						.Detail(detail));

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsDisposed<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		bool disposed,
		IScopeContext scopeContext,
		IErrorCode? errorCode = null,
		Action<ErrorMessageBuilder>? errorMessageConfigurator = null,
		Exception? innerException = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (disposed)
		{
			resultBuilder
				.WithObjectDisposedException(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					errorCode ?? ErrorCodes.ObjDisposedException.Default,
					(x => x.ExceptionInfo(innerException))
						+ errorMessageConfigurator);

			return true;
		}

		return false;
	}

	[System.Diagnostics.StackTraceHidden]
	public static bool IsDisposed<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		bool disposed,
		IScopeContext scopeContext,
		IErrorCode? errorCode,
		string? detail,
		Exception? innerException = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);

		if (disposed)
		{
			resultBuilder
				.WithObjectDisposedException(
					scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
					errorCode ?? ErrorCodes.ObjDisposedException.Default,
					x => x
						.ExceptionInfo(innerException)
						.Detail(detail));

			return true;
		}

		return false;
	}


	[System.Diagnostics.StackTraceHidden]
	public static TObject WithObjectDisposedException<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		IErrorCode? errorCode = null,
		Action<ErrorMessageBuilder>? errorMessageConfigurator = null,
		Exception? innerException = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		=> resultBuilder
			.WithErrorInternal(
				scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
				(x => x.ExceptionInfo(innerException))
					+ errorMessageConfigurator,
				errorCode ?? ErrorCodes.ObjDisposedException.Default)
			.Build();


	[System.Diagnostics.StackTraceHidden]
	public static TObject WithObjectDisposedException<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		IErrorCode? errorCode,
		string? detail,
		Exception? innerException = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		=> resultBuilder
			.WithErrorInternal(
				scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
				x => x
					.ExceptionInfo(innerException)
					.Detail(detail),
				errorCode ?? ErrorCodes.ObjDisposedException.Default)
			.Build();


	[System.Diagnostics.StackTraceHidden]
	public static TObject WithNotSupportedException<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		IErrorCode? errorCode = null,
		Action<ErrorMessageBuilder>? errorMessageConfigurator = null,
		Exception? innerException = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		=> resultBuilder
			.WithErrorInternal(
				scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
				(x => x.ExceptionInfo(innerException))
					+ errorMessageConfigurator,
				errorCode ?? ErrorCodes.NotSupportedException.Default)
			.Build();


	[System.Diagnostics.StackTraceHidden]
	public static TObject WithNotSupportedException<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		IErrorCode? errorCode,
		string? detail,
		Exception? innerException = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		=> resultBuilder
			.WithErrorInternal(
				scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
				x => x
					.ExceptionInfo(innerException)
					.Detail(detail),
				errorCode ?? ErrorCodes.NotSupportedException.Default)
			.Build();


	[System.Diagnostics.StackTraceHidden]
	public static TObject WithNotImplementedException<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		IErrorCode? errorCode = null,
		Action<ErrorMessageBuilder>? errorMessageConfigurator = null,
		Exception? innerException = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		=> resultBuilder
			.WithErrorInternal(
				scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
				(x => x.ExceptionInfo(innerException))
					+ errorMessageConfigurator,
				errorCode ?? ErrorCodes.NotImplementedException.Default)
			.Build();


	[System.Diagnostics.StackTraceHidden]
	public static TObject WithException<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		IErrorCode? errorCode,
		string? detail,
		Exception? innerException = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		=> resultBuilder
			.WithErrorInternal(
				scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
				x => x
					.ExceptionInfo(innerException)
					.Detail(detail),
				errorCode ?? ErrorCodes.NotImplementedException.Default)
			.Build();


	[System.Diagnostics.StackTraceHidden]
	public static TObject WithTransactionException<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		IErrorCode? errorCode = null,
		Action<ErrorMessageBuilder>? errorMessageConfigurator = null,
		Exception? innerException = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		=> resultBuilder
			.WithErrorInternal(
				scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
				(x => x.ExceptionInfo(innerException))
					+ errorMessageConfigurator,
				errorCode ?? ErrorCodes.TransactionException.Default)
			.Build();


	[System.Diagnostics.StackTraceHidden]
	public static TObject WithTransactionException<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		IErrorCode? errorCode,
		string? detail,
		Exception? innerException = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		=> resultBuilder
			.WithErrorInternal(
				scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
				x => x
					.ExceptionInfo(innerException)
					.Detail(detail),
				errorCode ?? ErrorCodes.TransactionException.Default)
			.Build();


	[System.Diagnostics.StackTraceHidden]
	public static TObject WithValidationException<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		IErrorCode? errorCode = null,
		Action<ErrorMessageBuilder>? errorMessageConfigurator = null,
		Exception? innerException = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		=> resultBuilder
			.WithErrorInternal(
				scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
				(x => x.ExceptionInfo(innerException))
					+ errorMessageConfigurator,
				errorCode ?? ErrorCodes.ValidationException.Default)
			.Build();


	[System.Diagnostics.StackTraceHidden]
	public static TObject WithValidationException<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		IErrorCode? errorCode,
		string? detail,
		Exception? innerException = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		=> resultBuilder
			.WithErrorInternal(
				scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
				x => x
					.ExceptionInfo(innerException)
					.Detail(detail),
				errorCode ?? ErrorCodes.ValidationException.Default)
			.Build();








	public static TObject WithError<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		IErrorCode? errorCode,
		string? internalMessage = null,
		Exception? innerException = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		=> resultBuilder
			.WithErrorInternal(
				scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
				x => x
					.ExceptionInfo(innerException)
					.InternalMessage(internalMessage)
					.Detail(innerException == null ? null : internalMessage),
				errorCode ?? ErrorCodes.ResultException.Default,
				LogLevel.Error)
			.Build();

	public static TObject WithError<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		IErrorCode? errorCode,
		Action<ErrorMessageBuilder>? errorMessageConfigurator,
		Exception? innerException = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		=> resultBuilder
			.WithErrorInternal(
				scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
				(x => x
					.ExceptionInfo(innerException))
				+ errorMessageConfigurator,
				errorCode ?? ErrorCodes.ResultException.Default,
				LogLevel.Error)
			.Build();

	public static TObject WithCriticalError<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		IErrorCode? errorCode,
		string? internalMessage = null,
		Exception? innerException = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		=> resultBuilder
			.WithErrorInternal(
				scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
				x => x
					.ExceptionInfo(innerException)
					.InternalMessage(internalMessage)
					.Detail(innerException == null ? null : internalMessage),
				errorCode ?? ErrorCodes.ResultException.Default,
				LogLevel.Critical)
			.Build();

	public static TObject WithCriticalError<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		IErrorCode? errorCode,
		Action<ErrorMessageBuilder>? errorMessageConfigurator,
		Exception? innerException = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		=> resultBuilder
			.WithErrorInternal(
				scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
				(x => x
					.ExceptionInfo(innerException))
				+ errorMessageConfigurator,
				errorCode ?? ErrorCodes.ResultException.Default,
				LogLevel.Critical)
			.Build();

	public static TObject WithWarning<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		IErrorCode? errorCode,
		string internalMessage,
		Exception? innerException = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		=> resultBuilder
			.WithWarnInternal(
				scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
				x => x
					.ExceptionInfo(innerException)
					.InternalMessage(internalMessage)
					.Detail(innerException == null ? null : internalMessage),
				errorCode ?? ErrorCodes.ResultException.Default)
			.Build();

	public static TObject WithWarning<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		IErrorCode? errorCode,
		Action<LogMessageBuilder>? logMessageConfigurator,
		Exception? innerException = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		=> resultBuilder
			.WithWarnInternal(
				scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
				(x => x
					.ExceptionInfo(innerException))
					+ logMessageConfigurator,
				errorCode ?? ErrorCodes.ResultException.Default)
			.Build();

	public static TObject WithClientWarning<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		IErrorCode? errorCode,
		string clientMessage,
		string? internalMessage = null,
		Exception? innerException = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		=> resultBuilder
			.WithWarnInternal(
				scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
				x => x
					.ExceptionInfo(innerException)
					.ClientMessage(clientMessage)
					.InternalMessage(internalMessage)
					.Detail(innerException == null ? null : internalMessage),
				errorCode ?? ErrorCodes.ResultException.Default)
			.Build();

	public static TObject WithClientWarning<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		IErrorCode? errorCode,
		string clientMessage,
		Action<LogMessageBuilder>? logMessageConfigurator = null,
		Exception? innerException = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		=> resultBuilder
			.WithWarnInternal(
				scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
				(x => x
					.ExceptionInfo(innerException)
					.ClientMessage(clientMessage))
					+ logMessageConfigurator,
				errorCode ?? ErrorCodes.ResultException.Default)
			.Build();

	public static TObject WithClientException<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		IErrorCode? errorCode,
		string clientMessage,
		string? internalMessage = null,
		Exception? innerException = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		=> resultBuilder
			.WithErrorInternal(
				scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
				x => x
					.ExceptionInfo(innerException)
					.ClientMessage(clientMessage)
					.InternalMessage(internalMessage)
					.Detail(innerException == null ? null : internalMessage),
				errorCode ?? ErrorCodes.ResultException.Default)
			.Build();

	public static TObject WithClientException<TBuilder, TObject>(
		this ResultBuilderBase<TBuilder, TObject> resultBuilder,
		IScopeContext scopeContext,
		IErrorCode? errorCode,
		string clientMessage,
		Action<ErrorMessageBuilder>? errorMessageConfigurator,
		Exception? innerException = null,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		where TBuilder : ResultBuilderBase<TBuilder, TObject>
		where TObject : IResult
		=> resultBuilder
			.WithErrorInternal(
				scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber),
				(x => x
					.ExceptionInfo(innerException)
					.ClientMessage(clientMessage))
				+ errorMessageConfigurator,
				errorCode ?? ErrorCodes.ResultException.Default)
			.Build();








	public static bool MergeHasError(
		this IResult result,
		IScopeContext scopeContext,
		IValidationResult validationResult,
		bool withPropertyName)
	{
		Throw.IfArgumentNull(result);
		Throw.IfArgumentNull(validationResult);

		foreach (var failure in validationResult.Failures)
		{
			if (failure.Severity == ValidationSeverity.Error)
			{
				var errorMessage = ValidationFailureToErrorMessage(scopeContext, failure, withPropertyName);
				result.ErrorMessages.Add(errorMessage);
			}
			else
			{
				var warnigMessage = ValidationFailureToWarningMessage(scopeContext, failure, withPropertyName);
				result.WarningMessages.Add(warnigMessage);
			}
		}

		return result.HasError;
	}

	public static bool MergeHasAnyTransactionRollbackError(
		this IResult result,
		IScopeContext scopeContext,
		IValidationResult validationResult,
		bool withPropertyName)
	{
		Throw.IfArgumentNull(result);
		Throw.IfArgumentNull(validationResult);

		foreach (var failure in validationResult.Failures)
		{
			if (failure.Severity == ValidationSeverity.Error)
			{
				var errorMessage = ValidationFailureToErrorMessage(scopeContext, failure, withPropertyName);
				result.ErrorMessages.Add(errorMessage);
			}
			else
			{
				var warnigMessage = ValidationFailureToWarningMessage(scopeContext, failure, withPropertyName);
				result.WarningMessages.Add(warnigMessage);
			}
		}

		return result.HasAnyTransactionRollbackError;
	}

	public static bool MergeHasError(
		this IResult result,
		IScopeContext scopeContext,
		IErrorCode errorCode,
		System.Xml.Schema.ValidationEventArgs[] xmlValidationArgs)
	{
		Throw.IfArgumentNull(result);
		Throw.IfArgumentNull(scopeContext);
		Throw.IfArgumentNull(errorCode);

		if (0 < xmlValidationArgs?.Length)
		{
			foreach (var xmlValidationArg in xmlValidationArgs)
			{
				if (xmlValidationArg.Severity == System.Xml.Schema.XmlSeverityType.Error)
					result.ErrorMessages.Add(xmlValidationArg.ToErrorMessage(scopeContext, errorCode));
				else
					result.WarningMessages.Add(xmlValidationArg.ToLogMessage(scopeContext, errorCode));
			}
		}

		return result.HasError;
	}

	public static bool MergeHasAnyTransactionRollbackError(
		this IResult result,
		IScopeContext scopeContext,
		IErrorCode errorCode,
		System.Xml.Schema.ValidationEventArgs[] xmlValidationArgs)
	{
		Throw.IfArgumentNull(result);
		Throw.IfArgumentNull(scopeContext);
		Throw.IfArgumentNull(errorCode);

		if (0 < xmlValidationArgs?.Length)
		{
			foreach (var xmlValidationArg in xmlValidationArgs)
			{
				if (xmlValidationArg.Severity == System.Xml.Schema.XmlSeverityType.Error)
					result.ErrorMessages.Add(xmlValidationArg.ToErrorMessage(scopeContext, errorCode));
				else
					result.WarningMessages.Add(xmlValidationArg.ToLogMessage(scopeContext, errorCode));
			}
		}

		return result.HasAnyTransactionRollbackError;
	}

	public static bool MergeHasError<TResultBuilder>(
		this TResultBuilder resultBuilder,
		IScopeContext scopeContext,
		IValidationResult validationResult,
		bool clientMessageWithPropertyName)
		where TResultBuilder : IResultBuilder
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(validationResult);

		foreach (var failure in validationResult.Failures)
		{
			if (failure.Severity == ValidationSeverity.Error)
			{
				var errorMessage = ValidationFailureToErrorMessage(scopeContext, failure, clientMessageWithPropertyName);
				resultBuilder.AddError(errorMessage);
			}
			else
			{
				var warnigMessage = ValidationFailureToWarningMessage(scopeContext, failure, clientMessageWithPropertyName);
				resultBuilder.AddWarning(warnigMessage);
			}
		}

		return resultBuilder.HasAnyError();
	}

	public static bool MergeHasAnyTransactionRollbackError<TResultBuilder>(
		this TResultBuilder resultBuilder,
		IScopeContext scopeContext,
		IValidationResult validationResult,
		bool clientMessageWithPropertyName)
		where TResultBuilder : IResultBuilder
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(validationResult);

		foreach (var failure in validationResult.Failures)
		{
			if (failure.Severity == ValidationSeverity.Error)
			{
				var errorMessage = ValidationFailureToErrorMessage(scopeContext, failure, clientMessageWithPropertyName);
				resultBuilder.AddError(errorMessage);
			}
			else
			{
				var warnigMessage = ValidationFailureToWarningMessage(scopeContext, failure, clientMessageWithPropertyName);
				resultBuilder.AddWarning(warnigMessage);
			}
		}

		return resultBuilder.HasAnyTransactionRollbackError();
	}

	public static bool MergeHasError<TResultBuilder>(
		this TResultBuilder resultBuilder,
		IScopeContext scopeContext,
		IErrorCode errorCode,
		System.Xml.Schema.ValidationEventArgs[] xmlValidationArgs)
		where TResultBuilder : IResultBuilder
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);
		Throw.IfArgumentNull(errorCode);

		if (0 < xmlValidationArgs?.Length)
		{
			foreach (var xmlValidationArg in xmlValidationArgs)
			{
				if (xmlValidationArg.Severity == System.Xml.Schema.XmlSeverityType.Error)
					resultBuilder.AddError(xmlValidationArg.ToErrorMessage(scopeContext, errorCode));
				else
					resultBuilder.AddWarning(xmlValidationArg.ToLogMessage(scopeContext, errorCode));
			}
		}

		return resultBuilder.HasAnyError();
	}

	public static bool MergeHasAnyTransactionRollbackError<TResultBuilder>(
		this TResultBuilder resultBuilder,
		IScopeContext scopeContext,
		IErrorCode errorCode,
		System.Xml.Schema.ValidationEventArgs[] xmlValidationArgs)
		where TResultBuilder : IResultBuilder
	{
		Throw.IfArgumentNull(resultBuilder);
		Throw.IfArgumentNull(scopeContext);
		Throw.IfArgumentNull(errorCode);

		if (0 < xmlValidationArgs?.Length)
		{
			foreach (var xmlValidationArg in xmlValidationArgs)
			{
				if (xmlValidationArg.Severity == System.Xml.Schema.XmlSeverityType.Error)
					resultBuilder.AddError(xmlValidationArg.ToErrorMessage(scopeContext, errorCode));
				else
					resultBuilder.AddWarning(xmlValidationArg.ToLogMessage(scopeContext, errorCode));
			}
		}

		return resultBuilder.HasAnyTransactionRollbackError();
	}

	private static IErrorMessage ValidationFailureToErrorMessage(
		IScopeContext scopeContext,
		IValidationFailure failure,
		bool clientMessageWithPropertyName)
	{
		Throw.IfArgumentNull(failure);
		Throw.IfArgumentNull(scopeContext);

		var errorMessageBuilder =
			new ErrorMessageBuilder(scopeContext, failure.ErrorCode)
				.LogLevel(LogLevel.Error)
				.ValidationFailure(failure, true)
				.ClientMessage(clientMessageWithPropertyName ? failure.MessageWithPropertyName : failure.ErrorCode.Message, true)
				.Detail(failure.DetailInfo)
				.PropertyName(string.IsNullOrWhiteSpace(failure.ObjectPath.PropertyName) ? null : failure.ObjectPath.ToString()?.TrimPrefix("_."), !string.IsNullOrWhiteSpace(failure.ObjectPath.PropertyName));

		return errorMessageBuilder.Build();
	}

	private static ILogMessage ValidationFailureToWarningMessage(
		IScopeContext scopeContext,
		IValidationFailure failure,
		bool clientMessageWithPropertyName)
	{
		Throw.IfArgumentNull(failure);
		Throw.IfArgumentNull(scopeContext);

		var logMessageBuilder =
			new LogMessageBuilder(scopeContext, failure.ErrorCode)
				.LogLevel(LogLevel.Warning)
				.ValidationFailure(failure, true)
				.ClientMessage(clientMessageWithPropertyName ? failure.MessageWithPropertyName : failure.ErrorCode.Message, true)
				.Detail(failure.DetailInfo)
				.PropertyName(string.IsNullOrWhiteSpace(failure.ObjectPath.PropertyName) ? null : failure.ObjectPath.ToString()?.TrimPrefix("_."), !string.IsNullOrWhiteSpace(failure.ObjectPath.PropertyName));

		return logMessageBuilder.Build();
	}
}
