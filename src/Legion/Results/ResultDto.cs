using Legion.Exceptions;
using Legion.Logging;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace Legion.Results;

public class ResultDto : IResult
{
	public string Name { get; set; }

	public List<LogMessageDto> SuccessMessages { get; set; }

	public List<LogMessageDto> WarningMessages { get; set; }

	public List<ErrorMessageDto> ErrorMessages { get; set; }

	public bool HasSuccessMessage => 0 < SuccessMessages.Count;

	public bool HasWarning => 0 < WarningMessages.Count;

	public bool HasError => 0 < ErrorMessages.Count;

	public bool HasErrorOrNullData => HasError || (CanStoreData && (!DataWasSet || GetData() == null));

	public virtual bool CanStoreData => false;

	public bool DataWasSet { get; set; }

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	public bool HasAnyTransactionRollbackError => ErrorMessages.Where(x => !x.DisableTransactionRollback).Any();

	public bool HasAnyMessage => HasSuccessMessage || HasWarning || HasError;

	public long? AffectedEntities { get; set; }

	List<ILogMessage> IResult.SuccessMessages => SuccessMessages?.Select(x => (ILogMessage)x.ToLogMessage()).ToList() ?? [];

	List<ILogMessage> IResult.WarningMessages => SuccessMessages?.Select(x => (ILogMessage)x.ToLogMessage()).ToList() ?? [];

	List<IErrorMessage> IResult.ErrorMessages => SuccessMessages?.Select(x => (IErrorMessage)x.ToLogMessage()).ToList() ?? [];

	public ResultDto()
	{
		SuccessMessages = [];
		WarningMessages = [];
		ErrorMessages = [];
	}

	internal ResultDto(Result result)
	{
		SuccessMessages = result.SuccessMessages?.Select(x => x.ToClientDto()).ToList() ?? [];
		WarningMessages = result.WarningMessages?.Select(x => x.ToClientDto()).ToList() ?? [];
		ErrorMessages = result.ErrorMessages?.Select(x => x.ToClientDto()).ToList() ?? [];
		DataWasSet = result.DataWasSet;
		AffectedEntities = result.AffectedEntities;
	}

	public void Log(
		IScopeContext scopeContext,
		ILogger logger,
		bool dataMustBeNotNull,
		IErrorCode? errorCode = null,
		bool skipIfAlreadyLogged = true,
		bool logWarnings = true)
	{
		Throw.IfArgumentNull(logger);

		logger.LogResultErrorMessages(
			scopeContext,
			errorCode ?? Legion.Exceptions.Internal.ErrorCodes.ResultException.Default,
			this,
			dataMustBeNotNull,
			skipIfAlreadyLogged,
			logWarnings);
	}

	public bool LogHasError(
		IScopeContext scopeContext,
		ILogger logger,
		bool dataMustBeNotNull,
		IErrorCode? errorCode = null,
		bool skipIfAlreadyLogged = true,
		bool logWarnings = true)
	{
		Log(
			scopeContext,
			logger,
			dataMustBeNotNull,
			errorCode,
			skipIfAlreadyLogged,
			logWarnings);

		return dataMustBeNotNull ? HasErrorOrNullData : HasError;
	}

	public ResultException? ToException(IScopeContext scopeContext, IErrorCode? errorCode, bool dataMustBeNotNull, bool withErrorMessageDetails)
		=> ExceptionHelper.ToException(scopeContext, errorCode, this, dataMustBeNotNull, withErrorMessageDetails);

	public void ThrowIfError(
		IScopeContext scopeContext,
		IErrorCode? errorCode,
		bool withErrorMessageDetails,
		ILogger? logger = null,
		bool skipIfAlreadyLogged = true,
		bool logWarnings = true)
	{
		if (logger != null)
		{
			try
			{
				logger.LogResultErrorMessages(scopeContext, errorCode ?? Legion.Exceptions.Internal.ErrorCodes.ResultException.Default, this, dataMustBeNotNull: false, skipIfAlreadyLogged, logWarnings);
			}
			catch { }
		}

		var exception = ToException(scopeContext, errorCode, dataMustBeNotNull: false, withErrorMessageDetails);

		if (exception != null)
			throw exception;
	}

	public void ThrowIfErrorOrNullData(
		IScopeContext scopeContext,
		IErrorCode? errorCode,
		bool withErrorMessageDetails,
		ILogger? logger = null,
		bool skipIfAlreadyLogged = true,
		bool logWarnings = true)
	{
		if (logger != null)
		{
			try
			{
				logger.LogResultErrorMessages(scopeContext, errorCode ?? Legion.Exceptions.Internal.ErrorCodes.ResultException.Default, this, dataMustBeNotNull: true, skipIfAlreadyLogged, logWarnings);
			}
			catch { }
		}

		var exception = ToException(scopeContext, errorCode, dataMustBeNotNull: true, withErrorMessageDetails);

		if (exception != null)
			throw exception;
	}

	public virtual object? GetData()
		=> default;

	public T? GetData<T>()
		=> GetDataInternal<T>();

	protected internal virtual T? GetDataInternal<T>()
		=> default;

	public bool TryGetData<T>([MaybeNullWhen(false)] out T data)
		=> TryGetDataInternal(out data);

	protected internal virtual bool TryGetDataInternal<T>([MaybeNullWhen(false)] out T data)
	{
		data = default;
		return false;
	}

	public ResultDto ToDto()
		=> this;
}

public class ResultDto<TData> : ResultDto, IResult<TData>, IResult
{
	public override bool CanStoreData => true;

	public TData? Data { get; set; }

	public ResultDto()
		: base()
	{
	}

	internal ResultDto(Result<TData> result)
	{
		SuccessMessages = result.SuccessMessages?.Select(x => x.ToClientDto()).ToList() ?? [];
		WarningMessages = result.WarningMessages?.Select(x => x.ToClientDto()).ToList() ?? [];
		ErrorMessages = result.ErrorMessages?.Select(x => x.ToClientDto()).ToList() ?? [];
		DataWasSet = result.DataWasSet;
		AffectedEntities = result.AffectedEntities;
		Data = result.Data;
	}

	public void ClearData()
	{
		Data = default;
		DataWasSet = false;
	}

	public override object? GetData()
		=> Data;

	protected internal override T GetDataInternal<T>()
	{
		if (Data is T data)
			return data;

		return default!;
	}

	protected internal override bool TryGetDataInternal<T>([MaybeNullWhen(false)] out T data)
	{
		if (Data is T d)
		{
			data = d;
			return true;
		}

		data = default!;
		return false;
	}

	public ResultDto<TData> ToDto()
		=> this;
}
