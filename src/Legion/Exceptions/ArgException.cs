using Legion.Exceptions.Internal;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Legion.Exceptions;

public class ArgException : LegionException, ILegionException
{
	private readonly string? _paramName;

	public virtual string? ParamName => _paramName;

	public override string Message
	{
		get
		{
			string message = base.Message;
			if (!string.IsNullOrEmpty(_paramName))
				message = MessageWithParamName(message, _paramName);

			return message;
		}
	}

	private static string MessageWithParamName(string message, string? paramName)
		=> $"{message} (Parameter: '{paramName}')";

	public ArgException(IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null)
		: base(errorCode ?? ErrorCodes.ArgException.Default, detail, scopeContext)
	{
	}

	public ArgException(IErrorCode? errorCode, string? paramName, string? detail = null, IScopeContext? scopeContext = null)
		: base(errorCode ?? ErrorCodes.ArgException.Default, detail, scopeContext)
	{
		_paramName = paramName;
	}

	public ArgException(IErrorCode? errorCode, string? detail, Exception? innerException, IScopeContext? scopeContext = null)
		: base(errorCode ?? ErrorCodes.ArgException.Default, detail, innerException, scopeContext)
	{
	}

	[System.Diagnostics.StackTraceHidden]
	public static void ThrowIf<T>(bool condition, T? argument, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
	{
		if (condition)
			Throw(paramName, errorCode, detail, scopeContext);
	}

	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static void Throw(string? paramName, IErrorCode? errorCode, string? detail, IScopeContext? scopeContext)
		=> throw new ArgException(errorCode, paramName, detail, scopeContext);
}
