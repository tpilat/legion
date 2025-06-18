using Legion.Logging;

namespace Legion.ADF.Messaging.MessageBox.Model;

public sealed partial class QueuedMessage : MessageBox.MessageBoxBaseEntity, Legion.Model.IEntity
{
	internal static IResult<QueuedMessage?> Create(
		IScopeContext scopeContext,
		Guid idMessage,
		Guid idQueue,
		DTOs.MessageBoxMessageDto messageDto)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<QueuedMessage?>();

		if (result.IsArgumentNull(scopeContext, messageDto))
			return result.Build();

		var nowUtc = GlobalContext.Instance.UtcNow;
		var id = Guid.NewGuid();
		var queuedMessage = new QueuedMessage
		{
			__IsNewObject = true,
			IdQueuedMessage = id,
			IdQueue = idQueue,
			IdMessage = idMessage,
			IdMessageProcessingStatus = Model.MessageProcessingStatus.Created,
			AssignedUtc = nowUtc,
			ProcessedUtc = null,
			SuspendedUtc = null,
			LastProcessingUtc = null,
			LastProcessingTimeoutUtc = null,
			NextProcessingUtc = nowUtc,
			RetryCount = 0,
			IdMessageBoxInstance = Infrastructure.EnvironmentInfo.RUNTIME_UNIQUE_KEY
		};

		var validationResult =
			DefaultDBValidator
				.Validate(queuedMessage);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(queuedMessage).Build();
	}

	internal IResult<MessageProcessingLog> SetProcessing(
		IScopeContext scopeContext,
		TimeSpan processingTimeout,
		bool disableProcessingLogNextTime,
		ILogMessage? logMessage = null,
		string logCode = nameof(Model.MessageProcessingStatus.Processing),
		string? logDetail = null)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<MessageProcessingLog>();

		if (result.IsArgumentLessThanOrEqual(scopeContext, processingTimeout, TimeSpan.Zero))
			return result.Build();

		var nowUtc = GlobalContext.Instance.UtcNow;

		bool wasProcessed = LastProcessingUtc.HasValue;

		IdMessageProcessingStatus = Model.MessageProcessingStatus.Processing;
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

		if (!wasProcessed || !disableProcessingLogNextTime)
		{
			var createProcessingLogResult = MessageProcessingLog.Create(
				scopeContext,
				IdMessage,
				IdQueuedMessage,
				null,
				IdMessageProcessingStatus,
				logMessage?.IdLogMessage,
				logCode,
				logDetail ?? logMessage?.ToMessage());

			if (!result.MergeHasError(createProcessingLogResult))
				result.WithData(createProcessingLogResult.Data!); // _messageProcessingLogs.Add(createProcessingLogResult.Data!);
		}

		return result.Build();
	}

	internal IResult<MessageProcessingLog> SetProcessed(
		IScopeContext scopeContext,
		bool disableProcessingLog,
		ILogMessage? logMessage = null,
		string logCode = nameof(Model.MessageProcessingStatus.Processed),
		string? logDetail = null)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<MessageProcessingLog>();

		IdMessageProcessingStatus = Model.MessageProcessingStatus.Processed;
		ProcessedUtc = GlobalContext.Instance.UtcNow;
		SuspendedUtc = null;

		var validationResult =
			DefaultDBValidator
				.Validate(this);

		if (result.MergeHasError(scopeContext.CreateNew(), validationResult, true))
			return result.Build();

		if (!disableProcessingLog)
		{
			var createProcessingLogResult = MessageProcessingLog.Create(
				scopeContext,
				IdMessage,
				IdQueuedMessage,
				null,
				IdMessageProcessingStatus,
				logMessage?.IdLogMessage,
				logCode,
				logDetail ?? logMessage?.ToMessage());

			if (!result.MergeHasError(createProcessingLogResult))
				result.WithData(createProcessingLogResult.Data!); // _messageProcessingLogs.Add(createProcessingLogResult.Data!);
		}

		return result.Build();
	}

	internal IResult<MessageProcessingLog> SetFailed(
		IScopeContext scopeContext,
		int maxRetryCount,
		TimeSpan nextProcessingDelay,
		bool disableProcessingLog,
		IErrorMessage errorMessage,
		string logCode = nameof(Model.MessageProcessingStatus.Failed),
		string? logDetail = null)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<MessageProcessingLog>();

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
			IdMessageProcessingStatus = Model.MessageProcessingStatus.Suspended;
			if (logCode == nameof(Model.MessageProcessingStatus.Failed))
				logCode = nameof(Model.MessageProcessingStatus.Suspended);
		}
		else
		{
			SuspendedUtc = null;
			NextProcessingUtc = nowUtc.Add(nextProcessingDelay);
			IdMessageProcessingStatus = Model.MessageProcessingStatus.Failed;
		}

		var validationResult =
			DefaultDBValidator
				.Validate(this);

		if (result.MergeHasError(scopeContext.CreateNew(), validationResult, true))
			return result.Build();

		if (!disableProcessingLog)
		{
			var createProcessingLogResult = MessageProcessingLog.Create(
				scopeContext,
				IdMessage,
				IdQueuedMessage,
				null,
				IdMessageProcessingStatus,
				errorMessage?.IdLogMessage,
				logCode,
				logDetail ?? errorMessage?.ToMessage());

			if (!result.MergeHasError(createProcessingLogResult))
				result.WithData(createProcessingLogResult.Data!); // _messageProcessingLogs.Add(createProcessingLogResult.Data!);
		}

		return result.Build();
	}

	internal IResult<MessageProcessingLog> SetUnknownType(
		IScopeContext scopeContext,
		bool disableProcessingLog,
		ILogMessage? logMessage = null,
		string logCode = nameof(Model.MessageProcessingStatus.UnknownType),
		string? logDetail = null)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<MessageProcessingLog>();

		ProcessedUtc = null;
		SuspendedUtc = GlobalContext.Instance.UtcNow;
		IdMessageProcessingStatus = Model.MessageProcessingStatus.UnknownType;
		//RetryCount++;

		var validationResult =
			DefaultDBValidator
				.Validate(this);

		if (result.MergeHasError(scopeContext.CreateNew(), validationResult, true))
			return result.Build();

		if (!disableProcessingLog)
		{
			var createProcessingLogResult = MessageProcessingLog.Create(
				scopeContext,
				IdMessage,
				IdQueuedMessage,
				null,
				IdMessageProcessingStatus,
				logMessage?.IdLogMessage,
				logCode,
				logDetail ?? logMessage?.ToMessage());

			if (!result.MergeHasError(createProcessingLogResult))
				result.WithData(createProcessingLogResult.Data!); // _messageProcessingLogs.Add(createProcessingLogResult.Data!);
		}

		return result.Build();
	}

	internal IResult<MessageProcessingLog> SetNoHandler(
		IScopeContext scopeContext,
		bool disableProcessingLog,
		ILogMessage? logMessage = null,
		string logCode = nameof(Model.MessageProcessingStatus.NoHandler),
		string? logDetail = null)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<MessageProcessingLog>();

		ProcessedUtc = null;
		SuspendedUtc = GlobalContext.Instance.UtcNow;
		IdMessageProcessingStatus = Model.MessageProcessingStatus.NoHandler;
		//RetryCount++;

		var validationResult =
			DefaultDBValidator
				.Validate(this);

		if (result.MergeHasError(scopeContext.CreateNew(), validationResult, true))
			return result.Build();

		if (!disableProcessingLog)
		{
			var createProcessingLogResult = MessageProcessingLog.Create(
				scopeContext,
				IdMessage,
				IdQueuedMessage,
				null,
				IdMessageProcessingStatus,
				logMessage?.IdLogMessage,
				logCode,
				logDetail ?? logMessage?.ToMessage());

			if (!result.MergeHasError(createProcessingLogResult))
				result.WithData(createProcessingLogResult.Data!); // _messageProcessingLogs.Add(createProcessingLogResult.Data!);
		}

		return result.Build();
	}

	internal IResult<MessageProcessingLog> SetBlocked(
		IScopeContext scopeContext,
		bool disableProcessingLog,
		ILogMessage? logMessage = null,
		string logCode = nameof(Model.MessageProcessingStatus.Blocked),
		string? logDetail = null)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<MessageProcessingLog>();

		ProcessedUtc = null;
		SuspendedUtc = GlobalContext.Instance.UtcNow;
		IdMessageProcessingStatus = Model.MessageProcessingStatus.Blocked;
		//RetryCount++;

		var validationResult =
			DefaultDBValidator
				.Validate(this);

		if (result.MergeHasError(scopeContext.CreateNew(), validationResult, true))
			return result.Build();

		if (!disableProcessingLog)
		{
			var createProcessingLogResult = MessageProcessingLog.Create(
				scopeContext,
				IdMessage,
				IdQueuedMessage,
				null,
				IdMessageProcessingStatus,
				logMessage?.IdLogMessage,
				logCode,
				logDetail ?? logMessage?.ToMessage());

			if (!result.MergeHasError(createProcessingLogResult))
				result.WithData(createProcessingLogResult.Data!); // _messageProcessingLogs.Add(createProcessingLogResult.Data!);
		}

		return result.Build();
	}

	internal IResult<MessageProcessingLog> SetSuspended(
		IScopeContext scopeContext,
		bool disableProcessingLog,
		IErrorMessage errorMessage,
		string logCode = nameof(Model.MessageProcessingStatus.Suspended),
		string? logDetail = null)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<MessageProcessingLog>();

		var nowUtc = GlobalContext.Instance.UtcNow;

		ProcessedUtc = null;
		RetryCount++;
		SuspendedUtc = nowUtc;
		IdMessageProcessingStatus = Model.MessageProcessingStatus.Suspended;

		var validationResult =
			DefaultDBValidator
				.Validate(this);

		if (result.MergeHasError(scopeContext.CreateNew(), validationResult, true))
			return result.Build();

		if (!disableProcessingLog)
		{
			var createProcessingLogResult = MessageProcessingLog.Create(
				scopeContext,
				IdMessage,
				IdQueuedMessage,
				null,
				IdMessageProcessingStatus,
				errorMessage?.IdLogMessage,
				logCode,
				logDetail ?? errorMessage?.ToMessage());

			if (!result.MergeHasError(createProcessingLogResult))
				result.WithData(createProcessingLogResult.Data!); // _messageProcessingLogs.Add(createProcessingLogResult.Data!);
		}

		return result.Build();
	}
}
