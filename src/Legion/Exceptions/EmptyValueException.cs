using Legion.Exceptions.Internal;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Legion.Exceptions;

public class EmptyValueException : ValueException, ILegionException
{
	public EmptyValueException(IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null)
		: base(errorCode ?? ErrorCodes.EmptyValueException.Default, detail, scopeContext)
	{
	}

	public EmptyValueException(string? paramName, IErrorCode? errorCode, string? detail = null, IScopeContext? scopeContext = null)
		: base(errorCode ?? ErrorCodes.EmptyValueException.Default, paramName, detail, scopeContext)
	{
	}

	public EmptyValueException(IErrorCode? errorCode, string? detail, Exception? innerException, IScopeContext? scopeContext = null)
		: base(errorCode ?? ErrorCodes.EmptyValueException.Default, detail, innerException, scopeContext)
	{
	}

	[System.Diagnostics.StackTraceHidden]
	public static void ThrowIf<T>(bool condition, T? argument, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
	{
		if (condition)
			Throw(paramName, errorCode, detail, scopeContext);
	}

	[System.Diagnostics.StackTraceHidden]
	public static string? ThrowIfNullOrEmpty([NotNull] string? argument, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
	{
#pragma warning disable L_NullEx_Str // Type or member is obsolete
		NullValueException.ThrowIfNull(argument, errorCode, detail, scopeContext, paramName);
#pragma warning restore L_NullEx_Str // Type or member is obsolete

		if (string.IsNullOrEmpty(argument))
			Throw(paramName, errorCode ?? ErrorCodes.EmptyValueException.EmptyString, detail, scopeContext);

		return argument;
	}

	[System.Diagnostics.StackTraceHidden]
	public static string? ThrowIfNullOrWhiteSpace([NotNull] string? argument, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
	{
#pragma warning disable L_NullEx_Str // Type or member is obsolete
		NullValueException.ThrowIfNull(argument, errorCode, detail, scopeContext, paramName);
#pragma warning restore L_NullEx_Str // Type or member is obsolete

		if (string.IsNullOrWhiteSpace(argument))
			Throw(paramName, errorCode ?? ErrorCodes.EmptyValueException.WhiteSpace, detail, scopeContext);

		return argument;
	}

	[System.Diagnostics.StackTraceHidden]
	public static ICollection? ThrowIfNullOrEmpty([NotNull] ICollection? argument, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
	{
		NullValueException.ThrowIfNull(argument, errorCode, detail, scopeContext, paramName);

		if (argument.Count == 0)
			Throw(paramName, errorCode ?? ErrorCodes.EmptyValueException.Collection, detail, scopeContext);

		return argument;
	}

	[System.Diagnostics.StackTraceHidden]
	public static Array? ThrowIfNullOrEmpty([NotNull] Array? argument, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
	{
		NullValueException.ThrowIfNull(argument, errorCode, detail, scopeContext, paramName);

		if (argument.Length == 0)
			Throw(paramName, errorCode ?? ErrorCodes.EmptyValueException.Array, detail, scopeContext);

		return argument;
	}

	[System.Diagnostics.StackTraceHidden]
	public static IEnumerable? ThrowIfNullOrEmpty([NotNull] IEnumerable? argument, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
	{
		NullValueException.ThrowIfNull(argument, errorCode, detail, scopeContext, paramName);

		if (!argument.Cast<object>().Any())
			Throw(paramName, errorCode ?? ErrorCodes.EmptyValueException.Enumerable, detail, scopeContext);

		return argument;
	}

	[System.Diagnostics.StackTraceHidden]
	public static T? ThrowIfNullOrEmpty<T>([NotNull] T? argument, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where T: IEnumerable?
	{
		NullValueException.ThrowIfNull(argument, errorCode, detail, scopeContext, paramName);

		if (!argument.Cast<object>().Any())
			Throw(paramName, errorCode ?? ErrorCodes.EmptyValueException.Enumerable, detail, scopeContext);

		return argument;
	}

	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static void Throw(string? paramName, IErrorCode? errorCode, string? detail = null, IScopeContext? scopeContext = null)
		=> throw new EmptyValueException(paramName, errorCode, detail, scopeContext);
}
