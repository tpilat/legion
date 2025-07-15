using Legion.Exceptions.Internal;
using System.Diagnostics.CodeAnalysis;

namespace Legion.Exceptions;

public class ResultException : LegionException, ILegionException
{
	public IResult Result { get; }

	public ResultException(IResult result, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null)
		: base(errorCode ?? ErrorCodes.ResultException.Default, detail, scopeContext)
	{
		Result = result;
	}

	public ResultException(IResult result, IErrorCode? errorCode, string? detail, Exception? innerException, IScopeContext? scopeContext = null)
		: base(errorCode ?? ErrorCodes.ResultException.Default, detail, innerException, scopeContext)
	{
		Result = result;
	}

	[System.Diagnostics.StackTraceHidden]
	public static void ThrowIf(bool condition, IResult result, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null)
	{
		if (condition)
			Throw(result, errorCode, detail, scopeContext);
	}

	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static void Throw(IResult result, IErrorCode? errorCode, string? detail = null, IScopeContext? scopeContext = null, Exception? innerException = null)
		=> throw new ResultException(result, errorCode, detail, innerException, scopeContext);
}
