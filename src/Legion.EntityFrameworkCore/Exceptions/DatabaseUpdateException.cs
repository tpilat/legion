//using System.Diagnostics.CodeAnalysis;
//using System.Runtime.CompilerServices;

//namespace Legion.EntityFrameworkCore.Exceptions;

//public class DatabaseUpdateException : LegionException, ILegionException
//{
//	public DatabaseUpdateException(IErrorCode? errorCode = null, IScopeContext? scopeContext = null)
//		: base(errorCode ?? Legion.EntityFrameworkCore.Exceptions.Internal.ErrorCodes.DatabaseUpdateException.Default, scopeContext)
//	{
//	}

//	public DatabaseUpdateException(string? message, IScopeContext ? scopeContext = null)
//		: base(message ?? Legion.EntityFrameworkCore.Exceptions.Internal.ErrorCodes.DatabaseUpdateException.Default.Message, scopeContext)
//	{
//	}

//	public DatabaseUpdateException(IErrorCode? errorCode, Exception? innerException, IScopeContext? scopeContext = null)
//		: base(errorCode ?? Legion.EntityFrameworkCore.Exceptions.Internal.ErrorCodes.DatabaseUpdateException.Default, innerException, scopeContext)
//	{
//	}

//	public DatabaseUpdateException(string? message, Exception? innerException, IScopeContext? scopeContext = null)
//		: base(message ?? Legion.EntityFrameworkCore.Exceptions.Internal.ErrorCodes.DatabaseUpdateException.Default.Message, innerException, scopeContext)
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
//		=> throw new DatabaseUpdateException(errorCode, innerException, scopeContext);

//	[System.Diagnostics.StackTraceHidden]
//	[DoesNotReturn]
//	public static new void Throw(string message, IScopeContext? scopeContext, Exception? innerException = null)
//		=> throw new DatabaseUpdateException(message, innerException, scopeContext);
//}
