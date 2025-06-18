using Legion.Exceptions.Internal;
using System.Diagnostics.CodeAnalysis;

namespace Legion.Exceptions;

public class ValidationException : LegionException, ILegionException
{
	public ValidationException(IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null)
		: base(errorCode ?? ErrorCodes.ValidationException.Default, detail, scopeContext)
	{
	}

	public ValidationException(IErrorCode? errorCode, string? detail, Exception? innerException, IScopeContext? scopeContext = null)
		: base(errorCode ?? ErrorCodes.ValidationException.Default, detail, innerException, scopeContext)
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
		=> throw new ValidationException(errorCode, detail, innerException, scopeContext);
}
