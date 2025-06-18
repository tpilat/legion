using Legion.Validation;
using Legion.Exceptions.Internal;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Legion.Exceptions;

public class DefaultValueException : ValueException, ILegionException
{
	public DefaultValueException(IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null)
		: base(errorCode ?? ErrorCodes.DefaultValueException.Default, detail, scopeContext)
	{
	}

	public DefaultValueException(string? paramName, IErrorCode? errorCode, string? detail = null, IScopeContext? scopeContext = null)
		: base(errorCode ?? ErrorCodes.DefaultValueException.Default, paramName, detail, scopeContext)
	{
	}

	public DefaultValueException(IErrorCode? errorCode, string? detail, Exception? innerException, IScopeContext? scopeContext = null)
		: base(errorCode ?? ErrorCodes.DefaultValueException.Default, detail, innerException, scopeContext)
	{
	}

	[System.Diagnostics.StackTraceHidden]
	public static void ThrowIf<T>(bool condition, T? argument, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
	{
		if (condition)
			Throw(paramName, errorCode, detail, scopeContext);
	}

	[System.Diagnostics.StackTraceHidden]
	public static void ThrowIfDefault<T>([NotNull] T argument, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where T : struct
	{
		if (ValidationHelper.IsDefault(argument))
			Throw(paramName, errorCode, detail, scopeContext);
	}

	[System.Diagnostics.StackTraceHidden]
	public static void ThrowIfDefault<T>([NotNull] T? argument, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where T : struct
	{
		NullValueException.ThrowIfNull(argument, errorCode, detail, scopeContext, paramName);

		if (ValidationHelper.IsDefault(argument))
			Throw(paramName, errorCode, detail, scopeContext);
	}

	[System.Diagnostics.StackTraceHidden]
	public static T? ThrowIfNullOrDefault<T>([NotNull] T? argument, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where T : struct
	{
		NullValueException.ThrowIfNull(argument, errorCode, detail, scopeContext, paramName);

		if (ValidationHelper.IsDefault(argument))
			Throw(paramName, errorCode, detail, scopeContext);

		return argument;
	}

	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static void Throw(string? paramName, IErrorCode? errorCode, string? detail = null, IScopeContext? scopeContext = null)
		=> throw new DefaultValueException(paramName, errorCode, detail, scopeContext);
}
