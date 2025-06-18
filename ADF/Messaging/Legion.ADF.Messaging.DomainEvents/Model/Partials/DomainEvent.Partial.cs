using Legion.Logging;
using Legion.Model;

namespace Legion.ADF.Messaging.DomainEvents.Model;

public sealed partial class DomainEvent : DomainEvents.DomainEventsBaseEntity, Legion.Model.IEntity
{
	internal static IResult<DomainEvent?> Create(
		IScopeContext scopeContext,
		IDomainEvent domainEventContent,
		string? propertiesJson,
		string? publisher,
		string? publisherId)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<DomainEvent?>();

		if (result.IsArgumentNull(scopeContext, domainEventContent))
			return result.Build();

		var dbDomainEventContentResult = DomainEventContent.Create(scopeContext, domainEventContent);
		if (result.MergeHasError(dbDomainEventContentResult))
			return result.Build();

		var dbDomainEventContent = dbDomainEventContentResult.Data;

		var nowUtc = GlobalContext.Instance.UtcNow;
		var domainEvent = new DomainEvent
		{
			__IsNewObject = true,
			IdDomainEvent = domainEventContent.Id,
			Content = dbDomainEventContent!,
			IdDomainEventProcessingStatus = DomainEventProcessingStatus.Created,
			Namespace = domainEventContent.Namespace,
			TraceCorrelationId = scopeContext.TraceCorrelationId,
			Properties = propertiesJson,
			Publisher = publisher,
			PublisherId = publisherId,
			CreatedUtc = nowUtc,
			ProcessedUtc = null,
			SuspendedUtc = null,
			LastProcessingUtc = null,
			LastProcessingTimeoutUtc = null,
			NextProcessingUtc = nowUtc,
			RetryCount = 0,
			Priority = 50 //50%
		};

		var validationResult =
			DefaultDBValidator
				.Validate(domainEvent);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(domainEvent).Build();
	}

	internal static IResult<List<DomainEvent>> CreateRange(
		IScopeContext scopeContext,
		IEnumerable<Legion.Model.IDomainEvent> idomainEvents,
		string? publisher,
		string? publisherId)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<List<DomainEvent>>();

		if (result.IsArgumentNullOrEmpty(scopeContext, idomainEvents))
			return result.Build();

		var domainEvents = new List<DomainEvent>();

		foreach (var idomainEvent in idomainEvents)
		{
			var createResult = Create(scopeContext, idomainEvent, null, publisher, publisherId);
			if (result.MergeHasError(createResult))
				return result.Build();

			domainEvents.Add(createResult.Data!);
		}

		return result.WithData(domainEvents).Build();
	}

	internal IResult<DomainEventProcessingLog> SetProcessing(
		IScopeContext scopeContext,
		TimeSpan processingTimeout,
		bool disableProcessingLogFirstTime,
		bool disableProcessingLogNextTime,
		ILogMessage? logMessage = null,
		string logCode = nameof(Model.DomainEventProcessingStatus.Processing),
		string? logDetail = null)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<DomainEventProcessingLog>();

		if (result.IsArgumentLessThanOrEqual(scopeContext, processingTimeout, TimeSpan.Zero))
			return result.Build();

		var nowUtc = GlobalContext.Instance.UtcNow;

		bool wasProcessed = LastProcessingUtc.HasValue;

		IdDomainEventProcessingStatus = Model.DomainEventProcessingStatus.Processing;
		ProcessedUtc = null;
		SuspendedUtc = null;
		LastProcessingUtc = nowUtc;
		LastProcessingTimeoutUtc = nowUtc.Add(processingTimeout);
		NextProcessingUtc = LastProcessingTimeoutUtc.Value;

		var validationResult =
			DefaultDBValidator
				.Validate(this);

		if (result.MergeHasError(scopeContext.CreateNew(), validationResult, true))
			return result.Build();

		if ((!wasProcessed && !disableProcessingLogFirstTime)
			|| (wasProcessed && !disableProcessingLogNextTime))
		{
			var createProcessingLogResult = DomainEventProcessingLog.Create(
				scopeContext,
				IdDomainEvent,
				IdDomainEventProcessingStatus,
				logMessage?.IdLogMessage,
				logCode,
				logDetail ?? logMessage?.ToMessage());

			if (!result.MergeHasError(createProcessingLogResult))
				result.WithData(createProcessingLogResult.Data!); // _domainEventProcessingLogs.Add(createProcessingLogResult.Data!);
		}

		return result.Build();
	}

	internal IResult<DomainEventProcessingLog> SetProcessed(
		IScopeContext scopeContext,
		bool disableProcessingLog,
		ILogMessage? logMessage = null,
		string logCode = nameof(Model.DomainEventProcessingStatus.Processed),
		string? logDetail = null)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<DomainEventProcessingLog>();

		IdDomainEventProcessingStatus = Model.DomainEventProcessingStatus.Processed;
		ProcessedUtc = GlobalContext.Instance.UtcNow;
		SuspendedUtc = null;

		var validationResult =
			DefaultDBValidator
				.Validate(this);

		if (result.MergeHasError(scopeContext.CreateNew(), validationResult, true))
			return result.Build();

		if (!disableProcessingLog)
		{
			var createProcessingLogResult = DomainEventProcessingLog.Create(
				scopeContext,
				IdDomainEvent,
				IdDomainEventProcessingStatus,
				logMessage?.IdLogMessage,
				logCode,
				logDetail ?? logMessage?.ToMessage());

			if (!result.MergeHasError(createProcessingLogResult))
				result.WithData(createProcessingLogResult.Data!); // _domainEventProcessingLogs.Add(createProcessingLogResult.Data!);
		}

		return result.Build();
	}

	internal IResult<DomainEventProcessingLog> SetFailed(
		IScopeContext scopeContext,
		int maxRetryCount,
		TimeSpan nextProcessingDelay,
		bool disableProcessingLog,
		IErrorMessage errorMessage,
		string logCode = nameof(Model.DomainEventProcessingStatus.Failed),
		string? logDetail = null)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<DomainEventProcessingLog>();

		if (result.IsArgumentLessThan(scopeContext, maxRetryCount, 0))
			return result.Build();

		if (result.IsArgumentLessThanOrEqual(scopeContext, nextProcessingDelay, TimeSpan.Zero))
			return result.Build();

		var nowUtc = GlobalContext.Instance.UtcNow;

		ProcessedUtc = null;
		RetryCount++;

		if (maxRetryCount <= RetryCount)
		{
			SuspendedUtc = nowUtc;
			IdDomainEventProcessingStatus = Model.DomainEventProcessingStatus.Suspended;
			if (logCode == nameof(Model.DomainEventProcessingStatus.Failed))
				logCode = nameof(Model.DomainEventProcessingStatus.Suspended);
		}
		else
		{
			SuspendedUtc = null;
			NextProcessingUtc = nowUtc.Add(nextProcessingDelay);
			IdDomainEventProcessingStatus = Model.DomainEventProcessingStatus.Failed;
		}

		var validationResult =
			DefaultDBValidator
				.Validate(this);

		if (result.MergeHasError(scopeContext.CreateNew(), validationResult, true))
			return result.Build();

		if (!disableProcessingLog)
		{
			var createProcessingLogResult = DomainEventProcessingLog.Create(
				scopeContext,
				IdDomainEvent,
				IdDomainEventProcessingStatus,
				errorMessage?.IdLogMessage,
				logCode,
				logDetail ?? errorMessage?.ToMessage());

			if (!result.MergeHasError(createProcessingLogResult))
				result.WithData(createProcessingLogResult.Data!); // _domainEventProcessingLogs.Add(createProcessingLogResult.Data!);
		}

		return result.Build();
	}

	internal IResult<DomainEventProcessingLog> SetNoHandler(
		IScopeContext scopeContext,
		bool disableProcessingLog,
		ILogMessage? logMessage = null,
		string logCode = nameof(Model.DomainEventProcessingStatus.NoHandler),
		string? logDetail = null)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<DomainEventProcessingLog>();

		ProcessedUtc = null;
		SuspendedUtc = GlobalContext.Instance.UtcNow;
		IdDomainEventProcessingStatus = Model.DomainEventProcessingStatus.NoHandler;
		//RetryCount++;

		var validationResult =
			DefaultDBValidator
				.Validate(this);

		if (result.MergeHasError(scopeContext.CreateNew(), validationResult, true))
			return result.Build();

		if (!disableProcessingLog)
		{
			var createProcessingLogResult = DomainEventProcessingLog.Create(
				scopeContext,
				IdDomainEvent,
				IdDomainEventProcessingStatus,
				logMessage?.IdLogMessage,
				logCode,
				logDetail ?? logMessage?.ToMessage());

			if (!result.MergeHasError(createProcessingLogResult))
				result.WithData(createProcessingLogResult.Data!); // _domainEventProcessingLogs.Add(createProcessingLogResult.Data!);
		}

		return result.Build();
	}

	internal IResult<DomainEventProcessingLog> SetBlocked(
		IScopeContext scopeContext,
		bool disableProcessingLog,
		ILogMessage? logMessage = null,
		string logCode = nameof(Model.DomainEventProcessingStatus.Blocked),
		string? logDetail = null)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<DomainEventProcessingLog>();

		ProcessedUtc = null;
		SuspendedUtc = GlobalContext.Instance.UtcNow;
		IdDomainEventProcessingStatus = Model.DomainEventProcessingStatus.Blocked;
		//RetryCount++;

		var validationResult =
			DefaultDBValidator
				.Validate(this);

		if (result.MergeHasError(scopeContext.CreateNew(), validationResult, true))
			return result.Build();

		if (!disableProcessingLog)
		{
			var createProcessingLogResult = DomainEventProcessingLog.Create(
				scopeContext,
				IdDomainEvent,
				IdDomainEventProcessingStatus,
				logMessage?.IdLogMessage,
				logCode,
				logDetail ?? logMessage?.ToMessage());

			if (!result.MergeHasError(createProcessingLogResult))
				result.WithData(createProcessingLogResult.Data!); // _domainEventProcessingLogs.Add(createProcessingLogResult.Data!);
		}

		return result.Build();
	}

	internal IResult<DomainEventProcessingLog> SetSuspended(
		IScopeContext scopeContext,
		bool disableProcessingLog,
		IErrorMessage errorMessage,
		string logCode = nameof(Model.DomainEventProcessingStatus.Suspended),
		string? logDetail = null)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<DomainEventProcessingLog>();

		var nowUtc = GlobalContext.Instance.UtcNow;

		ProcessedUtc = null;
		RetryCount++;
		SuspendedUtc = nowUtc;
		IdDomainEventProcessingStatus = Model.DomainEventProcessingStatus.Suspended;

		var validationResult =
			DefaultDBValidator
				.Validate(this);

		if (result.MergeHasError(scopeContext.CreateNew(), validationResult, true))
			return result.Build();

		if (!disableProcessingLog)
		{
			var createProcessingLogResult = DomainEventProcessingLog.Create(
				scopeContext,
				IdDomainEvent,
				IdDomainEventProcessingStatus,
				errorMessage?.IdLogMessage,
				logCode,
				logDetail ?? errorMessage?.ToMessage());

			if (!result.MergeHasError(createProcessingLogResult))
				result.WithData(createProcessingLogResult.Data!); // _domainEventProcessingLogs.Add(createProcessingLogResult.Data!);
		}

		return result.Build();
	}

	//internal IResult<DomainEventProcessingLog> SetRetryCount(
	//	IScopeContext scopeContext,
	//	int retryCount,
	//	bool disableProcessingLog,
	//	ILogMessage? logMessage = null,
	//	string logCode = "SetRetryCount",
	//	string? logDetail = null)
	//{
	//	scopeContext = scopeContext.CreateNew();

	//	var result = new ResultBuilder<DomainEventProcessingLog>();

	//	if (result.IsArgumentLessThan(scopeContext, retryCount, 0))
	//		return result.Build();

	//	RetryCount = retryCount;

	//	var validationResult =
	//		DefaultDBValidator
	//			.Validate(this);

	//	if (result.MergeHasError(scopeContext.CreateNew(), validationResult, true))
	//		return result.Build();

	//	if (!disableProcessingLog)
	//	{
	//		var createProcessingLogResult = DomainEventProcessingLog.Create(
	//			scopeContext,
	//			IdDomainEvent,
	//			IdDomainEventProcessingStatus,
	//			logMessage?.IdLogMessage,
	//			logCode,
	//			logDetail ?? logMessage?.ToMessage());

	//		if (!result.MergeHasError(createProcessingLogResult))
	//			result.WithData(createProcessingLogResult.Data!); // _domainEventProcessingLogs.Add(createProcessingLogResult.Data!);
	//	}

	//	return result.Build();
	//}

	//internal IResult<DomainEventProcessingLog> SetSuspended(
	//	IScopeContext scopeContext,
	//	bool disableProcessingLog,
	//	ILogMessage? logMessage,
	//	string logCode = nameof(Model.DomainEventProcessingStatus.Suspended),
	//	string? logDetail = null)
	//{
	//	scopeContext = scopeContext.CreateNew();

	//	var result = new ResultBuilder<DomainEventProcessingLog>();

	//	ProcessedUtc = null;
	//	SuspendedUtc = GlobalContext.Instance.UtcNow;
	//	IdDomainEventProcessingStatus = Model.DomainEventProcessingStatus.Suspended;
	//	RetryCount++;

	//	var validationResult =
	//		DefaultDBValidator
	//			.Validate(this);

	//	if (result.MergeHasError(scopeContext.CreateNew(), validationResult, true))
	//		return result.Build();

	//	if (!disableProcessingLog)
	//	{
	//		var createProcessingLogResult = DomainEventProcessingLog.Create(
	//			scopeContext,
	//			IdDomainEvent,
	//			IdDomainEventProcessingStatus,
	//			logMessage?.IdLogMessage,
	//			logCode,
	//			logDetail ?? logMessage?.ToMessage());

	//		if (!result.MergeHasError(createProcessingLogResult))
	//			result.WithData(createProcessingLogResult.Data!); // _domainEventProcessingLogs.Add(createProcessingLogResult.Data!);
	//	}

	//	return result.Build();
	//}

	//internal IResult<DomainEventProcessingLog> SetSuspended(
	//	IScopeContext scopeContext,
	//	ILogMessage? logMessage,
	//	int? retryCount,
	//	bool disableProcessingLog,
	//	string logCode = nameof(Model.DomainEventProcessingStatus.Suspended),
	//	string? logDetail = null)
	//{
	//	scopeContext = scopeContext.CreateNew();

	//	var result = new ResultBuilder<DomainEventProcessingLog>();

	//	if (retryCount.HasValue && result.IsArgumentLessThan(scopeContext, retryCount.Value, 0))
	//		return result.Build();

	//	ProcessedUtc = null;
	//	SuspendedUtc = GlobalContext.Instance.UtcNow;
	//	IdDomainEventProcessingStatus = Model.DomainEventProcessingStatus.Suspended;

	//	if (retryCount.HasValue)
	//		RetryCount = retryCount.Value;

	//	var validationResult =
	//		DefaultDBValidator
	//			.Validate(this);

	//	if (result.MergeHasError(scopeContext.CreateNew(), validationResult, true))
	//		return result.Build();

	//	if (!disableProcessingLog)
	//	{
	//		var createProcessingLogResult = DomainEventProcessingLog.Create(
	//			scopeContext,
	//			IdDomainEvent,
	//			IdDomainEventProcessingStatus,
	//			logMessage?.IdLogMessage,
	//			logCode,
	//			logDetail ?? logMessage?.ToMessage());

	//		if (!result.MergeHasError(createProcessingLogResult))
	//			result.WithData(createProcessingLogResult.Data!); // _domainEventProcessingLogs.Add(createProcessingLogResult.Data!);
	//	}

	//	return result.Build();
	//}
}
