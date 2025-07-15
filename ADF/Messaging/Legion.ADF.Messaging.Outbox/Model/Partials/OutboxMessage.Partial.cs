using Legion.Logging;

namespace Legion.ADF.Messaging.Outbox.Model;

public sealed partial class OutboxMessage : Outbox.OutboxBaseEntity, Legion.Model.IEntity
{
	internal static IResult<OutboxMessage?> Create(
		IScopeContext scopeContext,
		Guid idMessageType,
		Guid idOutboxQueue,
		DTOs.OutboxMessageDto outboxMessageDto)
	{
		scopeContext = scopeContext.CreateNew();
		var result = new ResultBuilder<OutboxMessage?>();

		if (result.IsArgumentNull(scopeContext, outboxMessageDto))
			return result.Build();

		if (outboxMessageDto.Content == null)
		{
			var res = CreateWithNoContent(
				scopeContext,
				idMessageType,
				idOutboxQueue,
				outboxMessageDto);

			result.MergeAllWithDataHasError(res);
			return result.Build();
		}
		else if (outboxMessageDto.Content is DTOs.ByteArrayContent byteArrayContent)
		{
			var res = CreateWithByteArrayContent(
				scopeContext,
				idMessageType,
				idOutboxQueue,
				outboxMessageDto,
				byteArrayContent);

			result.MergeAllWithDataHasError(res);
			return result.Build();
		}
		else if (outboxMessageDto.Content is DTOs.JsonContent jsonContent)
		{
			var res = CreateWithJsonContent(
				scopeContext,
				idMessageType,
				idOutboxQueue,
				outboxMessageDto,
				jsonContent);

			result.MergeAllWithDataHasError(res);
			return result.Build();
		}
		else if (outboxMessageDto.Content is DTOs.StringContent stringContent)
		{
			var res = CreateWithStringContent(
				scopeContext,
				idMessageType,
				idOutboxQueue,
				outboxMessageDto,
				stringContent);

			result.MergeAllWithDataHasError(res);
			return result.Build();
		}
		else if (outboxMessageDto.Content is DTOs.DbOidContent dbOidContent)
		{
			var res = CreateWithDbOidContent(
				scopeContext,
				idMessageType,
				idOutboxQueue,
				outboxMessageDto,
				dbOidContent);

			result.MergeAllWithDataHasError(res);
			return result.Build();
		}
		else if (outboxMessageDto.Content is DTOs.FileRelativePath fileRelativePath)
		{
			var res = CreateWithFileRelativePathContent(
				scopeContext,
				idMessageType,
				idOutboxQueue,
				outboxMessageDto,
				fileRelativePath);

			result.MergeAllWithDataHasError(res);
			return result.Build();
		}
		else
		{
			return result.WithNotSupportedException(scopeContext, Messaging.Exceptions.Internal.ErrorCodes.OutboxMessageContent.InvalidContent(outboxMessageDto.Content.GetType()));
		}
	}

	private static IResult<OutboxMessage?> Create(
		IScopeContext scopeContext,
		Guid idMessageType,
		Guid idOutboxQueue,
		DTOs.OutboxMessageDto outboxMessageDto,
		OutboxMessageContent? messageContent)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<OutboxMessage?>();

		if (result.IsArgumentNull(scopeContext, outboxMessageDto))
			return result.Build();

		var nowUtc = GlobalContext.Instance.UtcNow;
		var id = messageContent?.IdOutboxMessageContent ?? GlobalContext.Instance.NewGuid();
		var outboxMessage = new OutboxMessage
		{
			__IsNewObject = true,
			IdOutboxMessage = id,
			IdMessageType = idMessageType,
			IdOutboxMessageStatus = Model.OutboxMessageStatus.Created,
			MessageContent = messageContent!,
			IdOutboxQueue = idOutboxQueue,
			MessageId = outboxMessageDto.MessageId,
			BusinessId = outboxMessageDto.BusinessId,
			CorrelationId = outboxMessageDto.CorrelationId,
			SessionId = outboxMessageDto.SessionId,
			SessionMessagePartId = outboxMessageDto.SessionMessagePartId,
			TraceCorrelationId = scopeContext.TraceCorrelationId,
			Properties = outboxMessageDto.Properties,
			Publisher = outboxMessageDto.Publisher,
			PublisherId = outboxMessageDto.PublisherId,
			CreatedUtc = nowUtc,
			ProcessedUtc = null,
			SuspendedUtc = null,
			LastProcessingUtc = null,
			LastProcessingTimeoutUtc = null,
			NextProcessingUtc = nowUtc,
			RetryCount = 0,
			TargetTopic = outboxMessageDto.TargetTopic,
			TargetQueueName = outboxMessageDto.TargetQueueName,
			IdOutboxInstance = Infrastructure.EnvironmentInfo.RUNTIME_UNIQUE_KEY
		};

		var validationResult =
			DefaultDBValidator
				.Validate(outboxMessage);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(outboxMessage).Build();
	}

	internal static IResult<OutboxMessage?> CreateWithNoContent(
		IScopeContext scopeContext,
		Guid idMessageType,
		Guid idOutboxQueue,
		DTOs.OutboxMessageDto outboxMessageDto)
		=> Create(
			scopeContext,
			idMessageType,
			idOutboxQueue,
			outboxMessageDto,
			(OutboxMessageContent?)null);

	internal static IResult<OutboxMessage?> CreateWithByteArrayContent(
		IScopeContext scopeContext,
		Guid idMessageType,
		Guid idOutboxQueue,
		DTOs.OutboxMessageDto outboxMessageDto,
		DTOs.ByteArrayContent content)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<OutboxMessage?>();

		var contentResult = OutboxMessageContent.CreateByteArray(scopeContext, content);

		if (result.MergeHasError(contentResult))
			return result.Build();

		return Create(
			scopeContext,
			idMessageType,
			idOutboxQueue,
			outboxMessageDto,
			contentResult.Data);
	}

	internal static IResult<OutboxMessage?> CreateWithJsonContent(
		IScopeContext scopeContext,
		Guid idMessageType,
		Guid idOutboxQueue,
		DTOs.OutboxMessageDto outboxMessageDto,
		DTOs.JsonContent content)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<OutboxMessage?>();

		var contentResult = OutboxMessageContent.CreateJson(scopeContext, content);

		if (result.MergeHasError(contentResult))
			return result.Build();

		return Create(
			scopeContext,
			idMessageType,
			idOutboxQueue,
			outboxMessageDto,
			contentResult.Data);
	}

	internal static IResult<OutboxMessage?> CreateWithStringContent(
		IScopeContext scopeContext,
		Guid idMessageType,
		Guid idOutboxQueue,
		DTOs.OutboxMessageDto outboxMessageDto,
		DTOs.StringContent content)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<OutboxMessage?>();

		var contentResult = OutboxMessageContent.CreateString(scopeContext, content);

		if (result.MergeHasError(contentResult))
			return result.Build();

		return Create(
			scopeContext,
			idMessageType,
			idOutboxQueue,
			outboxMessageDto,
			contentResult.Data);
	}

	internal static IResult<OutboxMessage?> CreateWithDbOidContent(
		IScopeContext scopeContext,
		Guid idMessageType,
		Guid idOutboxQueue,
		DTOs.OutboxMessageDto outboxMessageDto,
		DTOs.DbOidContent content)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<OutboxMessage?>();

		var contentResult = OutboxMessageContent.CreateDbOid(scopeContext, content);

		if (result.MergeHasError(contentResult))
			return result.Build();

		return Create(
			scopeContext,
			idMessageType,
			idOutboxQueue,
			outboxMessageDto,
			contentResult.Data);
	}

	internal static IResult<OutboxMessage?> CreateWithFileRelativePathContent(
		IScopeContext scopeContext,
		Guid idMessageType,
		Guid idOutboxQueue,
		DTOs.OutboxMessageDto outboxMessageDto,
		DTOs.FileRelativePath content)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<OutboxMessage?>();

		var contentResult = OutboxMessageContent.CreateFileRelativePath(scopeContext, content);

		if (result.MergeHasError(contentResult))
			return result.Build();

		return Create(
			scopeContext,
			idMessageType,
			idOutboxQueue,
			outboxMessageDto,
			contentResult.Data);
	}

	internal IResult<OutboxMessageProcessingLog> SetProcessing(
		IScopeContext scopeContext,
		TimeSpan processingTimeout,
		bool disableProcessingLogNextTime,
		ILogMessage? logMessage = null,
		string logCode = nameof(Model.OutboxMessageStatus.Processing),
		string? logDetail = null)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<OutboxMessageProcessingLog>();

		if (result.IsArgumentLessThanOrEqual(scopeContext, processingTimeout, TimeSpan.Zero))
			return result.Build();

		var nowUtc = GlobalContext.Instance.UtcNow;

		bool wasProcessed = LastProcessingUtc.HasValue;

		IdOutboxMessageStatus = Model.OutboxMessageStatus.Processing;
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
			var createProcessingLogResult = OutboxMessageProcessingLog.Create(
				scopeContext,
				IdOutboxMessage,
				IdOutboxQueue,
				IdOutboxMessageStatus,
				logMessage?.IdLogMessage,
				logCode,
				logDetail ?? logMessage?.ToMessage());

			if (!result.MergeHasError(createProcessingLogResult))
				result.WithData(createProcessingLogResult.Data!); // _outboxMessageProcessingLogs.Add(createProcessingLogResult.Data!);
		}

		return result.Build();
	}

	internal IResult<OutboxMessageProcessingLog> SetProcessed(
		IScopeContext scopeContext,
		bool disableProcessingLog,
		ILogMessage? logMessage = null,
		string logCode = nameof(Model.OutboxMessageStatus.Processed),
		string? logDetail = null)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<OutboxMessageProcessingLog>();

		IdOutboxMessageStatus = Model.OutboxMessageStatus.Processed;
		ProcessedUtc = GlobalContext.Instance.UtcNow;
		SuspendedUtc = null;

		var validationResult =
			DefaultDBValidator
				.Validate(this);

		if (result.MergeHasError(scopeContext.CreateNew(), validationResult, true))
			return result.Build();

		if (!disableProcessingLog)
		{
			var createProcessingLogResult = OutboxMessageProcessingLog.Create(
				scopeContext,
				IdOutboxMessage,
				IdOutboxQueue,
				IdOutboxMessageStatus,
				logMessage?.IdLogMessage,
				logCode,
				logDetail ?? logMessage?.ToMessage());

			if (!result.MergeHasError(createProcessingLogResult))
				result.WithData(createProcessingLogResult.Data!); // _outboxMessageProcessingLogs.Add(createProcessingLogResult.Data!);
		}

		return result.Build();
	}

	internal IResult<OutboxMessageProcessingLog> SetFailed(
		IScopeContext scopeContext,
		int maxRetryCount,
		TimeSpan nextProcessingDelay,
		bool disableProcessingLog,
		IErrorMessage errorMessage,
		string logCode = nameof(Model.OutboxMessageStatus.Failed),
		string? logDetail = null)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<OutboxMessageProcessingLog>();

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
			IdOutboxMessageStatus = Model.OutboxMessageStatus.Suspended;
			if (logCode == nameof(Model.OutboxMessageStatus.Failed))
				logCode = nameof(Model.OutboxMessageStatus.Suspended);
		}
		else
		{
			SuspendedUtc = null;
			NextProcessingUtc = nowUtc.Add(nextProcessingDelay);
			IdOutboxMessageStatus = Model.OutboxMessageStatus.Failed;
		}

		var validationResult =
			DefaultDBValidator
				.Validate(this);

		if (result.MergeHasError(scopeContext.CreateNew(), validationResult, true))
			return result.Build();

		if (!disableProcessingLog)
		{
			var createProcessingLogResult = OutboxMessageProcessingLog.Create(
				scopeContext,
				IdOutboxMessage,
				IdOutboxQueue,
				IdOutboxMessageStatus,
				errorMessage?.IdLogMessage,
				logCode,
				logDetail ?? errorMessage?.ToMessage());

			if (!result.MergeHasError(createProcessingLogResult))
				result.WithData(createProcessingLogResult.Data!); // _outboxMessageProcessingLogs.Add(createProcessingLogResult.Data!);
		}

		return result.Build();
	}

	internal IResult<OutboxMessageProcessingLog> SetUnknownType(
		IScopeContext scopeContext,
		bool disableProcessingLog,
		ILogMessage? logMessage = null,
		string logCode = nameof(Model.OutboxMessageStatus.UnknownType),
		string? logDetail = null)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<OutboxMessageProcessingLog>();

		ProcessedUtc = null;
		SuspendedUtc = GlobalContext.Instance.UtcNow;
		IdOutboxMessageStatus = Model.OutboxMessageStatus.UnknownType;
		//RetryCount++;

		var validationResult =
			DefaultDBValidator
				.Validate(this);

		if (result.MergeHasError(scopeContext.CreateNew(), validationResult, true))
			return result.Build();

		if (!disableProcessingLog)
		{
			var createProcessingLogResult = OutboxMessageProcessingLog.Create(
				scopeContext,
				IdOutboxMessage,
				IdOutboxQueue,
				IdOutboxMessageStatus,
				logMessage?.IdLogMessage,
				logCode,
				logDetail ?? logMessage?.ToMessage());

			if (!result.MergeHasError(createProcessingLogResult))
				result.WithData(createProcessingLogResult.Data!); // _outboxMessageProcessingLogs.Add(createProcessingLogResult.Data!);
		}

		return result.Build();
	}

	internal IResult<OutboxMessageProcessingLog> SetNoHandler(
		IScopeContext scopeContext,
		bool disableProcessingLog,
		ILogMessage? logMessage = null,
		string logCode = nameof(Model.OutboxMessageStatus.NoHandler),
		string? logDetail = null)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<OutboxMessageProcessingLog>();

		ProcessedUtc = null;
		SuspendedUtc = GlobalContext.Instance.UtcNow;
		IdOutboxMessageStatus = Model.OutboxMessageStatus.NoHandler;
		//RetryCount++;

		var validationResult =
			DefaultDBValidator
				.Validate(this);

		if (result.MergeHasError(scopeContext.CreateNew(), validationResult, true))
			return result.Build();

		if (!disableProcessingLog)
		{
			var createProcessingLogResult = OutboxMessageProcessingLog.Create(
				scopeContext,
				IdOutboxMessage,
				IdOutboxQueue,
				IdOutboxMessageStatus,
				logMessage?.IdLogMessage,
				logCode,
				logDetail ?? logMessage?.ToMessage());

			if (!result.MergeHasError(createProcessingLogResult))
				result.WithData(createProcessingLogResult.Data!); // _outboxMessageProcessingLogs.Add(createProcessingLogResult.Data!);
		}

		return result.Build();
	}

	internal IResult<OutboxMessageProcessingLog> SetBlocked(
		IScopeContext scopeContext,
		bool disableProcessingLog,
		ILogMessage? logMessage = null,
		string logCode = nameof(Model.OutboxMessageStatus.Blocked),
		string? logDetail = null)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<OutboxMessageProcessingLog>();

		ProcessedUtc = null;
		SuspendedUtc = GlobalContext.Instance.UtcNow;
		IdOutboxMessageStatus = Model.OutboxMessageStatus.Blocked;
		//RetryCount++;

		var validationResult =
			DefaultDBValidator
				.Validate(this);

		if (result.MergeHasError(scopeContext.CreateNew(), validationResult, true))
			return result.Build();

		if (!disableProcessingLog)
		{
			var createProcessingLogResult = OutboxMessageProcessingLog.Create(
				scopeContext,
				IdOutboxMessage,
				IdOutboxQueue,
				IdOutboxMessageStatus,
				logMessage?.IdLogMessage,
				logCode,
				logDetail ?? logMessage?.ToMessage());

			if (!result.MergeHasError(createProcessingLogResult))
				result.WithData(createProcessingLogResult.Data!); // _outboxMessageProcessingLogs.Add(createProcessingLogResult.Data!);
		}

		return result.Build();
	}

	internal IResult<OutboxMessageProcessingLog> SetSuspended(
		IScopeContext scopeContext,
		bool disableProcessingLog,
		IErrorMessage errorMessage,
		string logCode = nameof(Model.OutboxMessageStatus.Suspended),
		string? logDetail = null)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<OutboxMessageProcessingLog>();

		var nowUtc = GlobalContext.Instance.UtcNow;

		ProcessedUtc = null;
		RetryCount++;
		SuspendedUtc = nowUtc;
		IdOutboxMessageStatus = Model.OutboxMessageStatus.Suspended;

		var validationResult =
			DefaultDBValidator
				.Validate(this);

		if (result.MergeHasError(scopeContext.CreateNew(), validationResult, true))
			return result.Build();

		if (!disableProcessingLog)
		{
			var createProcessingLogResult = OutboxMessageProcessingLog.Create(
				scopeContext,
				IdOutboxMessage,
				IdOutboxQueue,
				IdOutboxMessageStatus,
				errorMessage?.IdLogMessage,
				logCode,
				logDetail ?? errorMessage?.ToMessage());

			if (!result.MergeHasError(createProcessingLogResult))
				result.WithData(createProcessingLogResult.Data!); // _outboxMessageProcessingLogs.Add(createProcessingLogResult.Data!);
		}

		return result.Build();
	}
}
