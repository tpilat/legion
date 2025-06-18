using Legion.Exceptions.Internal;
using System.Diagnostics.CodeAnalysis;

namespace Legion.Exceptions;

public class AuthenticationException : LegionException, ILegionException
{
	public AuthenticationException(IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null)
		: base(errorCode ?? ErrorCodes.AuthenticationException.Default, detail, scopeContext)
	{
	}

	public AuthenticationException(IErrorCode? errorCode, string? detail, Exception? innerException, IScopeContext? scopeContext = null)
		: base(errorCode ?? ErrorCodes.AuthenticationException.Default, detail, innerException, scopeContext)
	{
	}

	[System.Diagnostics.StackTraceHidden]
	public static new void ThrowIf(bool condition, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null)
	{
		if (condition)
			Throw(errorCode, detail, scopeContext);
	}

	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static new void Throw(IErrorCode? errorCode, string? detail = null, IScopeContext? scopeContext = null, Exception? innerException = null)
		=> throw new AuthenticationException(errorCode, detail, innerException, scopeContext);
}
