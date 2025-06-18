using Legion.Exceptions.Internal;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Legion.Exceptions;

public class ArgNullException : ArgException, ILegionException
{
	public ArgNullException(IErrorCode? errorCode = null, string? detail = null, IScopeContext ? scopeContext = null)
		: base(errorCode ?? ErrorCodes.ArgNullException.Default, detail, scopeContext)
	{
	}

	public ArgNullException(string? paramName, IErrorCode? errorCode, string? detail = null, IScopeContext? scopeContext = null)
		: base(errorCode ?? ErrorCodes.ArgNullException.Default, paramName, detail, scopeContext)
	{
	}

	public ArgNullException(IErrorCode? errorCode, string? detail, Exception? innerException, IScopeContext? scopeContext = null)
		: base(errorCode ?? ErrorCodes.ArgNullException.Default, detail, innerException, scopeContext)
	{
	}

	[System.Diagnostics.StackTraceHidden]
	public static new void ThrowIf<T>(bool condition, T? argument, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
	{
		if (condition)
			Throw(paramName, errorCode, detail, scopeContext);
	}

	[System.Diagnostics.StackTraceHidden]
	[Obsolete("Use NotEmpty or NotWhitespace instead. Do not use with nameof()"
#if NET6_0_OR_GREATER
	, DiagnosticId = "L_ArgNullEx_Str")]
#else
	)]
#endif
	public static string? ThrowIfNull([NotNull] string? argument, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
	{
		if (argument is null)
			Throw(paramName, errorCode, detail, scopeContext);

		return argument;
	}

	[System.Diagnostics.StackTraceHidden]
	public static T ThrowIfNull<T>([NotNull] T? argument, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
	{
		if (argument is null)
			Throw(paramName, errorCode, detail, scopeContext);

		return argument;
	}

	[System.Diagnostics.StackTraceHidden]
	public static object? ThrowIfNull([NotNull] object? argument, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
	{
		if (argument is null)
			Throw(paramName, errorCode, detail, scopeContext);

		return argument;
	}

	[System.Diagnostics.StackTraceHidden]
	public static unsafe void ThrowIfNull([NotNull] void* argument, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
	{
		if (argument is null)
			Throw(paramName, errorCode, detail, scopeContext);
	}

	public static unsafe void ThrowIfNull(IntPtr argument, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
	{
		if (argument == IntPtr.Zero)
			Throw(paramName, errorCode, detail, scopeContext);
	}

	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static new void Throw(string? paramName, IErrorCode? errorCode, string? detail = null, IScopeContext? scopeContext = null)
		=> throw new ArgNullException(paramName, errorCode, detail, scopeContext);
}
