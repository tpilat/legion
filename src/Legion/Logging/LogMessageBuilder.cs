using Legion.Diagnostics;
using Legion.Extensions;
using Microsoft.Extensions.Logging;

namespace Legion.Logging;

public interface ILogMessageBuilder<TBuilder, TObject>
	where TBuilder : ILogMessageBuilder<TBuilder, TObject>
	where TObject : ILogMessage
{
	TBuilder Object(TObject logMessage);

	TObject Build();

	TBuilder LogLevel(LogLevel logLevel, bool force = false);

	TBuilder Created(DateTime created, bool force = false);

	TBuilder ScopeContext(IScopeContext scopeContext, bool force = false);
	
	TBuilder OperationNameFORCED(string? operationName);

	TBuilder OperationName(string? operationName, bool force = false);

	TBuilder AggregateName(string? aggregateName, bool force = false);

	TBuilder AggregateIdentifier(string? aggregateIdentifier, bool force = false);

	TBuilder ClientMessage(string? clientMessage, bool force = false);

	TBuilder InternalMessage(string? internalMessage, bool force = false);

	TBuilder ClientAndInternalMessage(string? message, bool force = false);

	TBuilder StackTrace(bool force = false);

	TBuilder StackTrace(int skipFrames, bool force = false);

	TBuilder StackTrace(string? callerMethodFullName, bool force = false);

	TBuilder Detail(string? detail, bool force = false);

	TBuilder AppendDetail(string? detail);

	TBuilder AppendDetail(Exception? exception);

	TBuilder IsLogged(bool isLogged, bool force = false);

	TBuilder PropertyName(string? propertyName, bool force = false);

	TBuilder DisplayPropertyName(string? displayPropertyName, bool force = false);

	TBuilder IsValidationError(bool isValidationError);

	TBuilder ExceptionInfo(Exception? exception, bool force = false, bool addErrorMessageToExceptionData = false);

	TBuilder AddCustomData(string key, string? value, bool force = false);
	TBuilder SourceContext(string? sourceContext, bool force = false);
}

public abstract class LogMessageBuilderBase<TBuilder, TObject> : ILogMessageBuilder<TBuilder, TObject>
	where TBuilder : LogMessageBuilderBase<TBuilder, TObject>
	where TObject : ILogMessage
{
	private const string DETAIL_DELIMITER = "**************************";

	protected readonly TBuilder _builder;
	protected TObject _logMessage;

	protected LogMessageBuilderBase(TObject logMessage)
	{
		_logMessage = logMessage;
		_builder = (TBuilder)this;
	}

	public virtual TBuilder Object(TObject logMessage)
	{
		_logMessage = logMessage;
		return _builder;
	}

	public TObject Build()
	{
		return _logMessage;
	}

	public virtual TBuilder LogLevel(LogLevel logLevel, bool force = false)
	{
		if (force || _logMessage.LogLevel == default)
			_logMessage.LogLevel = logLevel;

		return _builder;
	}

	public TBuilder Created(DateTime created, bool force = false)
	{
		if (force || _logMessage.CreatedUtc == default)
			_logMessage.CreatedUtc = created;

		return _builder;
	}

	public TBuilder ScopeContext(IScopeContext scopeContext, bool force = false)
	{
		if (force || _logMessage.ScopeContext == null)
			_logMessage.ScopeContext = scopeContext;

		return _builder;
	}

	public TBuilder OperationNameFORCED(string? operationName)
		=> OperationName(operationName, true);

	public TBuilder OperationName(string? operationName, bool force = false)
	{
		if (force || string.IsNullOrWhiteSpace(_logMessage.OperationName))
			_logMessage.OperationName = operationName;

		return _builder;
	}

	public TBuilder AggregateName(string? aggregateName, bool force = false)
	{
		if (force || string.IsNullOrWhiteSpace(_logMessage.AggregateName))
			_logMessage.AggregateName = aggregateName;

		return _builder;
	}

	public TBuilder AggregateIdentifier(string? aggregateIdentifier, bool force = false)
	{
		if (force || string.IsNullOrWhiteSpace(_logMessage.AggregateIdentifier))
			_logMessage.AggregateIdentifier = aggregateIdentifier;

		return _builder;
	}

	public TBuilder ClientMessage(string? clientMessage, bool force = false)
	{
		if (force || string.IsNullOrWhiteSpace(_logMessage.ClientMessage))
			_logMessage.ClientMessage = clientMessage;

		return _builder;
	}

	public TBuilder InternalMessage(string? internalMessage, bool force = false)
	{
		if (force || string.IsNullOrWhiteSpace(_logMessage.InternalMessage))
			_logMessage.InternalMessage = internalMessage;

		return _builder;
	}

	public TBuilder ClientAndInternalMessage(string? message, bool force = false)
	{
		if (force || string.IsNullOrWhiteSpace(_logMessage.ClientMessage))
			_logMessage.ClientMessage = message;

		if (force || string.IsNullOrWhiteSpace(_logMessage.InternalMessage))
			_logMessage.InternalMessage = message;

		return _builder;
	}

	public TBuilder StackTrace(bool force = false)
	{
		if (force || string.IsNullOrWhiteSpace(_logMessage.StackTrace))
			_logMessage.StackTrace = StackTraceHelper.GetStackTrace(2, true);

		return _builder;
	}

	public TBuilder StackTrace(int skipFrames, bool force = false)
	{
		if (force || string.IsNullOrWhiteSpace(_logMessage.StackTrace))
			_logMessage.StackTrace = StackTraceHelper.GetStackTrace(skipFrames + 2, true);

		return _builder;
	}

	public TBuilder StackTrace(string? callerMethodFullName, bool force = false)
	{
		if (force || string.IsNullOrWhiteSpace(_logMessage.StackTrace))
			_logMessage.StackTrace = callerMethodFullName;

		return _builder;
	}

	public TBuilder Detail(string? detail, bool force = false)
	{
		if (force || string.IsNullOrWhiteSpace(_logMessage.Detail))
			_logMessage.Detail = detail;

		return _builder;
	}

	public TBuilder AppendDetail(string? detail)
	{
		if (string.IsNullOrWhiteSpace(detail))
			return _builder;

		if (string.IsNullOrWhiteSpace(_logMessage.Detail))
			_logMessage.Detail = detail;
		else
			_logMessage.Detail = $"{_logMessage.Detail}{Environment.NewLine}{DETAIL_DELIMITER}{Environment.NewLine}{detail}";

		return _builder;
	}

	public TBuilder AppendDetail(Exception? exception)
	{
		if (exception == null)
			return _builder;

		if (string.IsNullOrWhiteSpace(_logMessage.Detail))
			_logMessage.Detail = exception.ToStringTrace();
		else
			_logMessage.Detail = $"{_logMessage.Detail}{Environment.NewLine}{DETAIL_DELIMITER}{Environment.NewLine}{exception.ToStringTrace()}";

		return _builder;
	}

	public TBuilder IsLogged(bool isLogged, bool force = false)
	{
		if (force || !_logMessage.IsLogged)
			_logMessage.IsLogged = isLogged;

		return _builder;
	}

	public TBuilder ValidationFailure(object? validationFailure, bool force = false)
	{
		if (force || _logMessage.ValidationFailure == null)
		{
			_logMessage.ValidationFailure = validationFailure;
			_logMessage.IsValidationError = validationFailure != null;
		}

		return _builder;
	}

	public TBuilder PropertyName(string? propertyName, bool force = false)
	{
		if (force || string.IsNullOrWhiteSpace(_logMessage.PropertyName))
		{
			_logMessage.PropertyName = propertyName;
			if (string.IsNullOrWhiteSpace(_logMessage.DisplayPropertyName))
				DisplayPropertyName(propertyName, true);
		}

		return _builder;
	}

	public TBuilder DisplayPropertyName(string? displayPropertyName, bool force = false)
	{
		if (force || string.IsNullOrWhiteSpace(_logMessage.DisplayPropertyName))
			_logMessage.DisplayPropertyName = displayPropertyName;

		return _builder;
	}

	public TBuilder IsValidationError(bool isValidationError)
	{
		_logMessage.IsValidationError = isValidationError;
		return _builder;
	}

	public TBuilder SourceContext(string? sourceContext, bool force = false)
	{
		if (force || string.IsNullOrWhiteSpace(_logMessage.SourceContext))
			_logMessage.SourceContext = sourceContext;

		return _builder;
	}

	public TBuilder ExceptionInfo(Exception? exception, bool force = false, bool addErrorMessageToExceptionData = false)
	{
		if (exception != null)
		{
			if (force || _logMessage.Exception == null)
			{
				if (addErrorMessageToExceptionData)
					exception.AddErrorMessage(_logMessage);

				_logMessage.Exception = exception;
			}

			if (force || string.IsNullOrWhiteSpace(_logMessage.InternalMessage))
				_logMessage.InternalMessage = exception.Message ?? string.Empty;

			if (string.IsNullOrWhiteSpace(_logMessage.StackTrace))
				_logMessage.StackTrace = exception.ToStringTrace();
			else
				_logMessage.StackTrace = $"{_logMessage.StackTrace}{Environment.NewLine}{exception.ToStringTrace()}";

			_logMessage.IsValidationError = false; //TODO TOM: exception is LegionValidationException;
		}
		return _builder;
	}

	public TBuilder AddCustomData(string key, string? value, bool force = false)
	{
		_logMessage.ScopeContext.AddContextProperty(key, value, force);

		return _builder;
	}
}

public class LogMessageBuilder : LogMessageBuilderBase<LogMessageBuilder, ILogMessage>
{
	public LogMessageBuilder(IScopeContext scopeContext, IErrorCode? errorCode)
		: this(new LogMessage(scopeContext, errorCode))
	{
	}

	public LogMessageBuilder(ILogMessage logMessage)
		: base(logMessage)
	{
	}

	public static implicit operator LogMessage?(LogMessageBuilder builder)
	{
		if (builder == null)
			return null;

		return builder._logMessage as LogMessage;
	}

	public static implicit operator LogMessageBuilder?(LogMessage logMessage)
	{
		if (logMessage == null)
			return null;

		return new LogMessageBuilder(logMessage);
	}
}