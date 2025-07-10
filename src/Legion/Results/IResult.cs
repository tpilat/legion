using Legion.Exceptions;
using Legion.Logging;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace Legion;

#if NET6_0_OR_GREATER
[Serializer.JsonPolymorphicConverter]
#endif
public interface IResult
{
	List<ILogMessage> SuccessMessages { get; }

	List<ILogMessage> WarningMessages { get; }

	List<IErrorMessage> ErrorMessages { get; }

	bool HasSuccessMessage { get; }

	bool HasWarning { get; }

	bool HasError { get; }

	bool HasErrorOrNullData { get; }

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	bool HasAnyTransactionRollbackError { get; }

	bool HasAnyMessage { get; }

	void Log(
		IScopeContext scopeContext,
		ILogger logger,
		bool dataMustBeNotNull,
		IErrorCode? errorCode = null,
		bool skipIfAlreadyLogged = true,
		bool logWarnings = true);

	bool LogHasError(
		IScopeContext scopeContext,
		ILogger logger,
		bool dataMustBeNotNull,
		IErrorCode? errorCode = null,
		bool skipIfAlreadyLogged = true,
		bool logWarnings = true);

	/// <summary>
	/// Returns null if no Error message found
	/// </summary>
	ResultException? ToException(IScopeContext scopeContext, IErrorCode? errorCode, bool dataMustBeNotNull, bool withErrorMessageDetails);

	void ThrowIfError(
		IScopeContext scopeContext,
		IErrorCode? errorCode,
		bool withErrorMessageDetails,
		ILogger? logger = null,
		bool skipIfAlreadyLogged = true,
		bool logWarnings = true);

	void ThrowIfErrorOrNullData(
		IScopeContext scopeContext,
		IErrorCode? errorCode,
		bool withErrorMessageDetails,
		ILogger? logger = null,
		bool skipIfAlreadyLogged = true,
		bool logWarnings = true);

	bool CanStoreData { get; }

	bool DataWasSet { get; }

	object? GetData();

	T? GetData<T>();

	bool TryGetData<T>([MaybeNullWhen(false)] out T data);

	Results.ResultDto ToDto();
}

#if NET6_0_OR_GREATER
[Serializer.JsonPolymorphicConverter]
#endif
public interface IResult<TData> : IResult
{
	TData? Data { get; set; }

	void ClearData();

	new Results.ResultDto<TData> ToDto();
}
