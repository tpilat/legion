using Legion.Exceptions.Internal;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Legion.Exceptions;

public class ArgEmptyValueException : ArgException, ILegionException
{
	public ArgEmptyValueException(IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null)
		: base(errorCode ?? ErrorCodes.ArgEmptyValueException.Default, detail, scopeContext)
	{
	}

	public ArgEmptyValueException(string? paramName, IErrorCode? errorCode, string? detail = null, IScopeContext? scopeContext = null)
		: base(errorCode ?? ErrorCodes.ArgEmptyValueException.Default, paramName, scopeContext)
	{
	}

	public ArgEmptyValueException(IErrorCode? errorCode, string? detail, Exception? innerException, IScopeContext? scopeContext = null)
		: base(errorCode ?? ErrorCodes.ArgEmptyValueException.Default, detail, innerException, scopeContext)
	{
	}

	[System.Diagnostics.StackTraceHidden]
	public static new void ThrowIf<T>(bool condition, T? argument, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
	{
		if (condition)
			Throw(paramName, errorCode, detail, scopeContext);
	}

	[System.Diagnostics.StackTraceHidden]
	public static string? ThrowIfNullOrEmpty([NotNull] string? argument, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
	{
#pragma warning disable L_ArgNullEx_Str // Type or member is obsolete
		ArgNullException.ThrowIfNull(argument, errorCode, detail, scopeContext, paramName);
#pragma warning restore L_ArgNullEx_Str // Type or member is obsolete

		if (string.IsNullOrEmpty(argument))
			Throw(paramName, errorCode ?? ErrorCodes.ArgEmptyValueException.EmptyString, detail, scopeContext);

		return argument;
	}

	[System.Diagnostics.StackTraceHidden]
	public static string? ThrowIfNullOrWhiteSpace([NotNull] string? argument, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
	{
#pragma warning disable L_ArgNullEx_Str // Type or member is obsolete
		ArgNullException.ThrowIfNull(argument, errorCode, detail, scopeContext, paramName);
#pragma warning restore L_ArgNullEx_Str // Type or member is obsolete

		if (string.IsNullOrWhiteSpace(argument))
			Throw(paramName, errorCode ?? ErrorCodes.ArgEmptyValueException.WhiteSpace, detail, scopeContext);

		return argument;
	}

	[System.Diagnostics.StackTraceHidden]
	public static ICollection? ThrowIfNullOrEmpty([NotNull] ICollection? argument, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
	{
		ArgNullException.ThrowIfNull(argument, errorCode, detail, scopeContext, paramName);

		if (argument.Count == 0)
			Throw(paramName, errorCode ?? ErrorCodes.ArgEmptyValueException.Collection, detail, scopeContext);

		return argument;
	}

	[System.Diagnostics.StackTraceHidden]
	public static Array? ThrowIfNullOrEmpty([NotNull] Array? argument, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
	{
		ArgNullException.ThrowIfNull(argument, errorCode, detail, scopeContext, paramName);

		if (argument.Length == 0)
			Throw(paramName, errorCode ?? ErrorCodes.ArgEmptyValueException.Array, detail, scopeContext);

		return argument;
	}

	[System.Diagnostics.StackTraceHidden]
	public static IEnumerable? ThrowIfNullOrEmpty([NotNull] IEnumerable? argument, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
	{
		ArgNullException.ThrowIfNull(argument, errorCode, detail, scopeContext, paramName);

		if (!argument.Cast<object>().Any())
			Throw(paramName, errorCode ?? ErrorCodes.ArgEmptyValueException.Enumerable, detail, scopeContext);

		return argument;
	}

	[System.Diagnostics.StackTraceHidden]
	public static T? ThrowIfNullOrEmpty<T>([NotNull] T? argument, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		where T : IEnumerable?
	{
		ArgNullException.ThrowIfNull(argument, errorCode, detail, scopeContext, paramName);

		if (!argument.Cast<object>().Any())
			Throw(paramName, errorCode ?? ErrorCodes.ArgEmptyValueException.Enumerable, detail, scopeContext);

		return argument;
	}

	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static new void Throw(string? paramName, IErrorCode? errorCode, string? detail = null, IScopeContext? scopeContext = null)
		=> throw new ArgEmptyValueException(paramName, errorCode, detail, scopeContext);
}
