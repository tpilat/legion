using Legion.Logging;
using Microsoft.Extensions.Logging;

namespace Legion;

public interface IResultBuilder
{
	bool HasAnyError();

	bool HasAnyTransactionRollbackError();

	IResultBuilder AddWarning(ILogMessage message);

	IResultBuilder AddError(IErrorMessage message);
}

public interface IResultBuilder<TBuilder, TObject> : IResultBuilder
	where TBuilder : IResultBuilder<TBuilder, TObject>
	where TObject : IResult
{
	TBuilder Object(TObject result);
	TObject Build();

	TBuilder MergeErrors(IResult otherResult);

	TBuilder MergeAll(IResult otherResult);

	bool MergeHasError(IResult otherResult);

	bool MergeHasTransactionRollbackError(IResult otherResult);

	bool MergeAllHasError(IResult otherResult);

	bool MergeAllHasTransactionRollbackError(IResult otherResult);

	bool MergeHasError<T>(IResult<T> otherResult, bool dataMustBeSet = true);

	bool MergeHasTransactionRollbackError<T>(IResult<T> otherResult, bool dataMustBeSet = true);

	bool MergeAllHasError<T>(IResult<T> otherResult, bool dataMustBeSet = true);

	bool MergeAllHasTransactionRollbackError<T>(IResult<T> otherResult, bool dataMustBeSet = true);


	bool HasError();

	bool HasTransactionRollbackError();

	object? GetData();

	TBuilder ClearAllSuccessMessages();

	TBuilder WithSuccess(ILogMessage message);

	TBuilder WithWarn(ILogMessage message);

	TBuilder WithError(IErrorMessage message);

	TBuilder WithSuccess(IScopeContext scopeContext, Action<LogMessageBuilder>? logMessageConfigurator);

	TBuilder ForAllSuccessMessages(Action<ILogMessage> logMessageConfigurator);

	TBuilder ForAllWarningMessages(Action<ILogMessage> logMessageConfigurator);

	TBuilder ForAllIErrorMessages(Action<ILogMessage> errorMessageConfigurator);

	TBuilder ForAllMessages(Action<ILogMessage> messageConfigurator);
}

public abstract class ResultBuilderBase<TBuilder, TObject> : IResultBuilder<TBuilder, TObject>, IResultBuilder
	where TBuilder : ResultBuilderBase<TBuilder, TObject>
	where TObject : IResult
{
	protected readonly TBuilder _builder;
	protected TObject _result;

	protected ResultBuilderBase(TObject result)
	{
		_result = result;
		_builder = (TBuilder)this;
	}

	public virtual TBuilder Object(TObject result)
	{
		_result = result ?? throw new ArgumentNullException(nameof(result));

		return _builder;
	}

	public TBuilder GetBuilder()
		=> _builder;

	public TObject Build()
		=> _result;

	#region API

	public TBuilder MergeErrors(IResult otherResult)
	{
		if (otherResult != null && otherResult.HasError)
			_result.ErrorMessages.AddRange(otherResult.ErrorMessages);

		return _builder;
	}

	public TBuilder MergeAll(IResult otherResult)
	{
		if (otherResult != null)
		{
			if (otherResult.HasSuccessMessage)
				_result.SuccessMessages.AddRange(otherResult.SuccessMessages);

			if (otherResult.HasWarning)
				_result.WarningMessages.AddRange(otherResult.WarningMessages);

			if (otherResult.HasError)
				_result.ErrorMessages.AddRange(otherResult.ErrorMessages);
		}

		return _builder;
	}

	public bool MergeHasError(IResult otherResult)
	{
		MergeErrors(otherResult);
		return _result.HasError;
	}

	public bool MergeHasTransactionRollbackError(IResult otherResult)
	{
		MergeErrors(otherResult);
		return _result.HasAnyTransactionRollbackError;
	}

	public bool MergeAllHasError(IResult otherResult)
	{
		MergeAll(otherResult);
		return _result.HasError;
	}

	public bool MergeAllHasTransactionRollbackError(IResult otherResult)
	{
		MergeAll(otherResult);
		return _result.HasAnyTransactionRollbackError;
	}

	public bool MergeHasError<T>(IResult<T> otherResult, bool dataMustBeSet = true)
	{
		MergeErrors(otherResult);
		if (_result.HasError)
			return true;

		if (dataMustBeSet)
			return otherResult?.CanStoreData == true && otherResult?.DataWasSet != true;

		return false;
	}

	public bool MergeHasTransactionRollbackError<T>(IResult<T> otherResult, bool dataMustBeSet = true)
	{
		MergeErrors(otherResult);
		if (_result.HasAnyTransactionRollbackError)
			return true;

		if (dataMustBeSet)
			return otherResult?.CanStoreData == true && otherResult?.DataWasSet != true;

		return false;
	}

	public bool MergeAllHasError<T>(IResult<T> otherResult, bool dataMustBeSet = true)
	{
		MergeAll(otherResult);
		if (_result.HasError)
			return true;

		if (dataMustBeSet)
			return otherResult?.CanStoreData == true && otherResult?.DataWasSet != true;

		return false;
	}

	public bool MergeAllHasTransactionRollbackError<T>(IResult<T> otherResult, bool dataMustBeSet = true)
	{
		MergeAll(otherResult);
		if (_result.HasAnyTransactionRollbackError)
			return true;

		if (dataMustBeSet)
			return otherResult?.CanStoreData == true && otherResult?.DataWasSet != true;

		return false;
	}

	public bool HasError()
	{
		return _result.HasError;
	}

	public bool HasTransactionRollbackError()
	{
		return _result.HasAnyTransactionRollbackError;
	}

	public virtual object? GetData()
	{
		return null;
	}

	public TBuilder ClearAllSuccessMessages()
	{
		_result.SuccessMessages.Clear();
		return _builder;
	}

	public TBuilder WithSuccess(ILogMessage message)
	{
		_result.SuccessMessages.Add(message);
		return _builder;
	}

	public TBuilder WithWarn(ILogMessage message)
	{
		_result.WarningMessages.Add(message);
		return _builder;
	}

	public TBuilder WithError(IErrorMessage message)
	{
		_result.ErrorMessages.Add(message);
		return _builder;
	}

	public TBuilder WithSuccess(IScopeContext scopeContext, Action<LogMessageBuilder>? logMessageConfigurator)
	{
		var logMessageBuilder =
			new LogMessageBuilder(scopeContext, null)
				.LogLevel(LogLevel.Information);

		logMessageConfigurator?.Invoke(logMessageBuilder);
		_result.SuccessMessages.Add(logMessageBuilder.Build());
		return _builder;
	}

	//public TBuilder WithWarnInternal(IScopeContext scopeContext, Action<LogMessageBuilder>? logMessageConfigurator, IErrorCode? errorCode = null)
	//{
	//	var logMessageBuilder =
	//		new LogMessageBuilder(scopeContext, errorCode)
	//			.LogLevel(LogLevel.Warning);

	//	logMessageConfigurator?.Invoke(logMessageBuilder);
	//	_result.WarningMessages.Add(logMessageBuilder.Build());
	//	return _builder;
	//}

	//public TBuilder WithErrorInternal(IScopeContext scopeContext, Action<ErrorMessageBuilder>? errorMessageConfigurator, IErrorCode errorCode)
	//{
	//	Throw.ArgumentNull(errorCode);

	//	var errorMessageBuilder =
	//		new ErrorMessageBuilder(scopeContext, errorCode)
	//			.LogLevel(LogLevel.Error);

	//	errorMessageConfigurator?.Invoke(errorMessageBuilder);
	//	_result.ErrorMessages.Add(errorMessageBuilder.Build());
	//	return _builder;
	//}

	public TBuilder ForAllSuccessMessages(Action<ILogMessage> logMessageConfigurator)
	{
		Throw.IfArgumentNull(logMessageConfigurator);

		foreach (var successMessage in _result.SuccessMessages)
			logMessageConfigurator.Invoke(successMessage);

		return _builder;
	}

	public TBuilder ForAllWarningMessages(Action<ILogMessage> logMessageConfigurator)
	{
		Throw.IfArgumentNull(logMessageConfigurator);

		foreach (var warningMessage in _result.WarningMessages)
			logMessageConfigurator.Invoke(warningMessage);

		return _builder;
	}

	public TBuilder ForAllIErrorMessages(Action<ILogMessage> errorMessageConfigurator)
	{
		Throw.IfArgumentNull(errorMessageConfigurator);

		foreach (var errorMessage in _result.ErrorMessages)
			errorMessageConfigurator.Invoke(errorMessage);

		return _builder;
	}

	public TBuilder ForAllMessages(Action<ILogMessage> messageConfigurator)
		=> ForAllSuccessMessages(messageConfigurator)
			.ForAllWarningMessages(messageConfigurator)
			.ForAllIErrorMessages(messageConfigurator);

	bool IResultBuilder.HasAnyError()
		=> Build().HasError;

	bool IResultBuilder.HasAnyTransactionRollbackError()
		=> Build().HasAnyTransactionRollbackError;

	IResultBuilder IResultBuilder.AddWarning(ILogMessage message)
		=> WithWarn(message);

	IResultBuilder IResultBuilder.AddError(IErrorMessage message)
		=> WithError(message);

	#endregion API
}

public class ResultBuilder : ResultBuilderBase<ResultBuilder, IResult>, IResultBuilder
{
	public ResultBuilder()
		: this(new Result())
	{
	}

	public ResultBuilder(IResult result)
		: base(result)
	{
	}

	public static implicit operator Result?(ResultBuilder builder)
	{
		if (builder == null)
			return null;

		return builder._result as Result;
	}

	public static implicit operator ResultBuilder?(Result result)
	{
		if (result == null)
			return null;

		return new ResultBuilder(result);
	}

	public static IResult Empty()
		=> new ResultBuilder().Build();
}

















public interface IResultBuilder<TBuilder, TData, TObject> : IResultBuilder<TBuilder, TObject>, IResultBuilder
	where TBuilder : IResultBuilder<TBuilder, TData, TObject>
	where TObject : IResult<TData>
{
	TBuilder WithData(TData? data);

	TBuilder ClearData();

	TBuilder MergeAllWithData(IResult<TData> otherResult);

	bool HasErrorOrNoData();

	bool MergeAllWithDataHasError(IResult<TData> otherResult);

	bool MergeAllWithDataHasTransactionRollbackError(IResult<TData> otherResult);
}

public abstract class ResultBuilderBase<TBuilder, TData, TObject> : ResultBuilderBase<TBuilder, TObject>, IResultBuilder<TBuilder, TData, TObject>, IResultBuilder
	where TBuilder : ResultBuilderBase<TBuilder, TData, TObject>
	where TObject : IResult<TData>
{
	protected ResultBuilderBase(TObject result)
		: base(result)
	{
	}

	public override object? GetData()
	{
		return _result.Data;
	}

	public TBuilder WithData(TData? data)
	{
		_result.Data = data;
		return _builder;
	}

	public TBuilder ClearData()
	{
		_result.ClearData();
		return _builder;
	}

	public TBuilder MergeAllWithData(IResult<TData> otherResult)
	{
		if (otherResult != null)
		{
			if (otherResult.HasSuccessMessage)
				_result.SuccessMessages.AddRange(otherResult.SuccessMessages);

			if (otherResult.HasWarning)
				_result.WarningMessages.AddRange(otherResult.WarningMessages);

			if (otherResult.HasError)
				_result.ErrorMessages.AddRange(otherResult.ErrorMessages);

			if (otherResult.DataWasSet)
				_result.Data = otherResult.Data;
		}

		return _builder;
	}

	public bool HasErrorOrNoData() => HasError() || (_result.CanStoreData && (!_result.DataWasSet || _result.Data == null));

	public bool MergeAllWithDataHasError(IResult<TData> otherResult)
	{
		MergeAllWithData(otherResult);
		return HasErrorOrNoData();
	}

	public bool MergeAllWithDataHasTransactionRollbackError(IResult<TData> otherResult)
	{
		MergeAllWithData(otherResult);
		return _result.HasAnyTransactionRollbackError;
	}
}

public class ResultBuilder<TData> : ResultBuilderBase<ResultBuilder<TData>, TData, IResult<TData>>, IResultBuilder
{
	public ResultBuilder()
		: this(new Result<TData>())
	{
	}

	public ResultBuilder(IResult<TData> result)
		: base(result)
	{
	}

	public static implicit operator Result<TData>?(ResultBuilder<TData> builder)
	{
		if (builder == null)
			return null;

		return builder._result as Result<TData>;
	}

	public static implicit operator ResultBuilder<TData>?(Result<TData> result)
	{
		if (result == null)
			return null;

		return new ResultBuilder<TData>(result);
	}

	public static implicit operator ResultBuilder?(ResultBuilder<TData> builder)
	{
		if (builder == null)
			return null;

		return new ResultBuilder((Result<TData>)builder._result);
	}

	public static IResult<TData> Empty()
		=> new ResultBuilder<TData>().Build();

	public static IResult<TData> FromResult(TData result)
		=> new ResultBuilder<TData>().WithData(result).Build();
}
