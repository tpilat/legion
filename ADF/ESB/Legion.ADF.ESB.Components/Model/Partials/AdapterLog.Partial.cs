using Legion.ADF.ESB.Components;
using Legion.Extensions;
using Legion.Logging;
using Legion.Validation;
using Microsoft.Extensions.Logging;

namespace Legion.ADF.ESB.Components.Model;

public sealed partial class AdapterLog : Components.ComponentsBaseEntity, Legion.Model.IEntity
{
	private const string EMPTY_TEXT = "_";

	public static IResult<AdapterLog?> CreateDebugAdapterLog(
		IScopeContext scopeContext,
		IESBAdapter esbAdapter,
		ILogMessage logMessage,
		string? jsonData = null)
		=> CreateAdapterLog(
			scopeContext,
			esbAdapter,
			LogLevel.Debug,
			detail: null!,
			logMessage,
			jsonData,
			idMessageProcessingLog: null);

	public static IResult<AdapterLog?> CreateDebugAdapterLog(
		IScopeContext scopeContext,
		IESBAdapter esbAdapter,
		string detail,
		ILogMessage? logMessage = null,
		string? jsonData = null)
		=> CreateAdapterLog(
			scopeContext,
			esbAdapter,
			LogLevel.Debug,
			detail,
			logMessage,
			jsonData,
			idMessageProcessingLog: null);

	public static IResult<AdapterLog?> CreateTraceAdapterLog(
		IScopeContext scopeContext,
		IESBAdapter esbAdapter,
		ILogMessage logMessage,
		string? jsonData = null)
		=> CreateAdapterLog(
			scopeContext,
			esbAdapter,
			LogLevel.Trace,
			detail: null!,
			logMessage,
			jsonData,
			idMessageProcessingLog: null);

	public static IResult<AdapterLog?> CreateTraceAdapterLog(
		IScopeContext scopeContext,
		IESBAdapter esbAdapter,
		string detail,
		ILogMessage? logMessage = null,
		string? jsonData = null)
		=> CreateAdapterLog(
			scopeContext,
			esbAdapter,
			LogLevel.Trace,
			detail,
			logMessage,
			jsonData,
			idMessageProcessingLog: null);

	public static IResult<AdapterLog?> CreateInformationAdapterLog(
		IScopeContext scopeContext,
		IESBAdapter esbAdapter,
		ILogMessage logMessage,
		string? jsonData = null)
		=> CreateAdapterLog(
			scopeContext,
			esbAdapter,
			LogLevel.Information,
			detail: null!,
			logMessage,
			jsonData,
			idMessageProcessingLog: null);

	public static IResult<AdapterLog?> CreateInformationAdapterLog(
		IScopeContext scopeContext,
		IESBAdapter esbAdapter,
		string detail,
		ILogMessage? logMessage = null,
		string? jsonData = null)
		=> CreateAdapterLog(
			scopeContext,
			esbAdapter,
			LogLevel.Information,
			detail,
			logMessage,
			jsonData,
			idMessageProcessingLog: null);

	public static IResult<AdapterLog?> CreateWarningAdapterLog(
		IScopeContext scopeContext,
		IESBAdapter esbAdapter,
		ILogMessage logMessage,
		string? jsonData = null)
		=> CreateAdapterLog(
			scopeContext,
			esbAdapter,
			LogLevel.Warning,
			detail: null!,
			logMessage,
			jsonData,
			idMessageProcessingLog: null);

	public static IResult<AdapterLog?> CreateWarningAdapterLog(
		IScopeContext scopeContext,
		IESBAdapter esbAdapter,
		string detail,
		ILogMessage? logMessage = null,
		string? jsonData = null)
		=> CreateAdapterLog(
			scopeContext,
			esbAdapter,
			LogLevel.Warning,
			detail,
			logMessage,
			jsonData,
			idMessageProcessingLog: null);

	public static IResult<AdapterLog?> CreateErrorAdapterLog(
		IScopeContext scopeContext,
		IESBAdapter esbAdapter,
		ILogMessage errorMessage,
		string? jsonData = null)
		=> CreateAdapterLog(
			scopeContext,
			esbAdapter,
			LogLevel.Error,
			detail: null!,
			errorMessage,
			jsonData,
			idMessageProcessingLog: null);

	public static IResult<AdapterLog?> CreateErrorAdapterLog(
		IScopeContext scopeContext,
		IESBAdapter esbAdapter,
		string detail,
		ILogMessage? errorMessage = null,
		string? jsonData = null)
		=> CreateAdapterLog(
			scopeContext,
			esbAdapter,
			LogLevel.Error,
			detail,
			errorMessage,
			jsonData,
			idMessageProcessingLog: null);

	public static IResult<AdapterLog?> CreateCriticalAdapterLog(
		IScopeContext scopeContext,
		IESBAdapter esbAdapter,
		ILogMessage errorMessage,
		string? jsonData = null)
		=> CreateAdapterLog(
			scopeContext,
			esbAdapter,
			LogLevel.Critical,
			detail: null!,
			errorMessage,
			jsonData,
			idMessageProcessingLog: null);

	public static IResult<AdapterLog?> CreateCriticalAdapterLog(
		IScopeContext scopeContext,
		IESBAdapter esbAdapter,
		string detail,
		ILogMessage? errorMessage = null,
		string? jsonData = null)
		=> CreateAdapterLog(
			scopeContext,
			esbAdapter,
			LogLevel.Critical,
			detail,
			errorMessage,
			jsonData,
			idMessageProcessingLog: null);

	private static IResult<AdapterLog?> CreateAdapterLog(
		IScopeContext scopeContext,
		IESBAdapter esbAdapter,
		LogLevel logLevel,
		string detail,
		ILogMessage? logMessage = null,
		string? jsonData = null,
		Guid? idMessageProcessingLog = null)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<AdapterLog?>();

		if (result.IsArgumentNull(scopeContext, esbAdapter))
			return result.Build();

		if (esbAdapter.MinLogLevel < logLevel)
			return result.WithData(null).Build();

		var adapterLog = new AdapterLog
		{
			IdAdapterLog = Guid.NewGuid(),
			IdAdapter = esbAdapter.IdAdapter,
			CreatedUtc = GlobalContext.Instance.UtcNow,
			IdLogLevel = (int)logLevel,
			LogCorrelationId = scopeContext.CorrelationId ?? Guid.NewGuid(),
			IdAdapterStatus = esbAdapter.IdAdapterStatus,
			Detail = detail.DefaultIfNullOrWhiteSpace(logMessage?.InternalMessage?.DefaultIfNullOrWhiteSpace(logMessage?.Detail) ?? EMPTY_TEXT),
			Data = jsonData,
			IdLogMessage = logMessage?.IdLogMessage,
			IdMessageProcessingLog = idMessageProcessingLog
		};

		var validationResult =
			SetDBValidatorRules(new ValidatorBuilder<AdapterLog>())
				.Build()
				.Validate(adapterLog);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(adapterLog).Build();
	}
}
