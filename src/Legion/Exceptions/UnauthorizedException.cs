using Legion.Exceptions.Internal;
using System.Diagnostics.CodeAnalysis;

namespace Legion.Exceptions;

public class UnauthorizedException : LegionException, ILegionException
{
	public UnauthorizedException(IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null)
		: base(errorCode ?? ErrorCodes.UnauthorizedException.Default, detail, scopeContext)
	{
	}

	public UnauthorizedException(IErrorCode? errorCode, string? detail, Exception? innerException, IScopeContext? scopeContext = null)
		: base(errorCode ?? ErrorCodes.UnauthorizedException.Default, detail, innerException, scopeContext)
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
		=> throw new UnauthorizedException(errorCode, detail, innerException, scopeContext);
}
