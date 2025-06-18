//using System.Diagnostics.CodeAnalysis;
//using System.Runtime.CompilerServices;

//namespace Legion.EntityFrameworkCore.Exceptions;

//public class UniqueConstraintException : DatabaseUpdateException, ILegionException
//{
//	public UniqueConstraintException(IErrorCode? errorCode = null, IScopeContext? scopeContext = null)
//		: base(errorCode ?? Legion.EntityFrameworkCore.Exceptions.Internal.ErrorCodes.UniqueConstraintException.Default, scopeContext)
//	{
//	}

//	public UniqueConstraintException(string? message, IScopeContext ? scopeContext = null)
//		: base(message ?? Legion.EntityFrameworkCore.Exceptions.Internal.ErrorCodes.UniqueConstraintException.Default.Message, scopeContext)
//	{
//	}

//	public UniqueConstraintException(IErrorCode? errorCode, Exception? innerException, IScopeContext? scopeContext = null)
//		: base(errorCode ?? Legion.EntityFrameworkCore.Exceptions.Internal.ErrorCodes.UniqueConstraintException.Default, innerException, scopeContext)
//	{
//	}

//	public UniqueConstraintException(string? message, Exception? innerException, IScopeContext? scopeContext = null)
//		: base(message ?? Legion.EntityFrameworkCore.Exceptions.Internal.ErrorCodes.UniqueConstraintException.Default.Message, innerException, scopeContext)
//	{
//	}

//	[System.Diagnostics.StackTraceHidden]
//	public static new void ThrowIf(bool condition, IScopeContext? scopeContext = null, IErrorCode? errorCode = null)
//	{
//		if (condition)
//			Throw(errorCode, scopeContext);
//	}

//	[System.Diagnostics.StackTraceHidden]
//	public static new void ThrowIfWithoutCode(bool condition, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(condition))] string? message = null)
//	{
//		if (condition)
//			Throw(message!, scopeContext);
//	}

//	[System.Diagnostics.StackTraceHidden]
//	[DoesNotReturn]
//	public static new void Throw(IErrorCode? errorCode, IScopeContext? scopeContext, Exception? innerException = null)
//		=> throw new UniqueConstraintException(errorCode, innerException, scopeContext);

//	[System.Diagnostics.StackTraceHidden]
//	[DoesNotReturn]
//	public static new void Throw(string message, IScopeContext? scopeContext, Exception? innerException = null)
//		=> throw new UniqueConstraintException(message, innerException, scopeContext);
//}
