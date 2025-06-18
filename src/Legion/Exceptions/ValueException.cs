namespace Legion.Exceptions;

public abstract class ValueException : LegionException, ILegionException
{
	private readonly string? _valueName;

	public virtual string? ValueName => _valueName;

	public override string Message
	{
		get
		{
			string message = base.Message;
			if (!string.IsNullOrEmpty(_valueName))
				message = MessageWithValueName(message, _valueName);

			return message;
		}
	}

	private static string MessageWithValueName(string message, string? paramName)
		=> $"{message} (Value: '{paramName}')";

	public ValueException(IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null)
		: base(errorCode, detail, scopeContext)
	{
	}

	public ValueException(IErrorCode? errorCode, string? valueName, string? detail = null, IScopeContext? scopeContext = null)
		: base(errorCode, detail, scopeContext)
	{
		_valueName = valueName;
	}

	public ValueException(IErrorCode? errorCode, string? detail, Exception? innerException, IScopeContext? scopeContext = null)
		: base(errorCode, detail, innerException, scopeContext)
	{
	}
}
