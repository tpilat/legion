using Legion.Logging;

namespace Legion.ADF.Messaging.Inbox.Model;

public sealed partial class InboxMessage : Inbox.InboxBaseEntity, Legion.Model.IEntity
{
	internal static IResult<InboxMessage?> Create(
		IScopeContext scopeContext,
		Guid idMessageType,
		Guid idInboxQueue,
		DTOs.InboxMessageDto inboxMessageDto)
	{
		scopeContext = scopeContext.CreateNew();
		var result = new ResultBuilder<InboxMessage?>();

		if (result.IsArgumentNull(scopeContext, inboxMessageDto))
			return result.Build();

		if (inboxMessageDto.Content == null)
		{
			var res = CreateWithNoContent(
				scopeContext,
				idMessageType,
				idInboxQueue,
				inboxMessageDto);

			result.MergeAllWithDataHasError(res);
			return result.Build();
		}
		else if (inboxMessageDto.Content is DTOs.ByteArrayContent byteArrayContent)
		{
			var res = CreateWithByteArrayContent(
				scopeContext,
				idMessageType,
				idInboxQueue,
				inboxMessageDto,
				byteArrayContent);

			result.MergeAllWithDataHasError(res);
			return result.Build();
		}
		else if (inboxMessageDto.Content is DTOs.JsonContent jsonContent)
		{
			var res = CreateWithJsonContent(
				scopeContext,
				idMessageType,
				idInboxQueue,
				inboxMessageDto,
				jsonContent);

			result.MergeAllWithDataHasError(res);
			return result.Build();
		}
		else if (inboxMessageDto.Content is DTOs.StringContent stringContent)
		{
			var res = CreateWithStringContent(
				scopeContext,
				idMessageType,
				idInboxQueue,
				inboxMessageDto,
				stringContent);

			result.MergeAllWithDataHasError(res);
			return result.Build();
		}
		else if (inboxMessageDto.Content is DTOs.DbOidContent dbOidContent)
		{
			var res = CreateWithDbOidContent(
				scopeContext,
				idMessageType,
				idInboxQueue,
				inboxMessageDto,
				dbOidContent);

			result.MergeAllWithDataHasError(res);
			return result.Build();
		}
		else if (inboxMessageDto.Content is DTOs.FileRelativePath fileRelativePath)
		{
			var res = CreateWithFileRelativePathContent(
				scopeContext,
				idMessageType,
				idInboxQueue,
				inboxMessageDto,
				fileRelativePath);

			result.MergeAllWithDataHasError(res);
			return result.Build();
		}
		else
		{
			return result.WithNotSupportedException(scopeContext, Messaging.Exceptions.Internal.ErrorCodes.InboxMessageContent.InvalidContent(inboxMessageDto.Content.GetType()));
		}
	}

	private static IResult<InboxMessage?> Create(
		IScopeContext scopeContext,
		Guid idMessageType,
		Guid idInboxQueue,
		DTOs.InboxMessageDto inboxMessageDto,
		InboxMessageContent? messageContent)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<InboxMessage?>();

		if (result.IsArgumentNull(scopeContext, inboxMessageDto))
			return result.Build();

		var nowUtc = GlobalContext.Instance.UtcNow;
		var id = messageContent?.IdInboxMessageContent ?? Guid.NewGuid();
		var inboxMessage = new InboxMessage
		{
			__IsNewObject = true,
			IdInboxMessage = id,
			IdMessageType = idMessageType,
			IdInboxMessageStatus = Model.InboxMessageStatus.Created,
			MessageContent = messageContent!,
			IdInboxQueue = idInboxQueue,
			MessageId = inboxMessageDto.MessageId,
			BusinessId = inboxMessageDto.BusinessId,
			CorrelationId = inboxMessageDto.CorrelationId,
			SessionId = inboxMessageDto.SessionId,
			SessionMessagePartId = inboxMessageDto.SessionMessagePartId,
			TraceCorrelationId = scopeContext.TraceCorrelationId,
			Properties = inboxMessageDto.Properties,
			Publisher = inboxMessageDto.Publisher,
			PublisherId = inboxMessageDto.PublisherId,
			CreatedUtc = nowUtc,
			ProcessedUtc = null,
			SuspendedUtc = null,
			LastProcessingUtc = null,
			LastProcessingTimeoutUtc = null,
			NextProcessingUtc = nowUtc,
			RetryCount = 0,
			TargetTopic = inboxMessageDto.TargetTopic,
			TargetQueueName = inboxMessageDto.TargetQueueName,
			IdInboxInstance = Infrastructure.EnvironmentInfo.RUNTIME_UNIQUE_KEY
		};

		var validationResult =
			DefaultDBValidator
				.Validate(inboxMessage);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(inboxMessage).Build();
	}

	internal static IResult<InboxMessage?> CreateWithNoContent(
		IScopeContext scopeContext,
		Guid idMessageType,
		Guid idInboxQueue,
		DTOs.InboxMessageDto inboxMessageDto)
		=> Create(
			scopeContext,
			idMessageType,
			idInboxQueue,
			inboxMessageDto,
			(InboxMessageContent?)null);

	internal static IResult<InboxMessage?> CreateWithByteArrayContent(
		IScopeContext scopeContext,
		Guid idMessageType,
		Guid idInboxQueue,
		DTOs.InboxMessageDto inboxMessageDto,
		DTOs.ByteArrayContent content)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<InboxMessage?>();

		var contentResult = InboxMessageContent.CreateByteArray(scopeContext, content);

		if (result.MergeHasError(contentResult))
			return result.Build();

		return Create(
			scopeContext,
			idMessageType,
			idInboxQueue,
			inboxMessageDto,
			contentResult.Data);
	}

	internal static IResult<InboxMessage?> CreateWithJsonContent(
		IScopeContext scopeContext,
		Guid idMessageType,
		Guid idInboxQueue,
		DTOs.InboxMessageDto inboxMessageDto,
		DTOs.JsonContent content)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<InboxMessage?>();

		var contentResult = InboxMessageContent.CreateJson(scopeContext, content);

		if (result.MergeHasError(contentResult))
			return result.Build();

		return Create(
			scopeContext,
			idMessageType,
			idInboxQueue,
			inboxMessageDto,
			contentResult.Data);
	}

	internal static IResult<InboxMessage?> CreateWithStringContent(
		IScopeContext scopeContext,
		Guid idMessageType,
		Guid idInboxQueue,
		DTOs.InboxMessageDto inboxMessageDto,
		DTOs.StringContent content)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<InboxMessage?>();

		var contentResult = InboxMessageContent.CreateString(scopeContext, content);

		if (result.MergeHasError(contentResult))
			return result.Build();

		return Create(
			scopeContext,
			idMessageType,
			idInboxQueue,
			inboxMessageDto,
			contentResult.Data);
	}

	internal static IResult<InboxMessage?> CreateWithDbOidContent(
		IScopeContext scopeContext,
		Guid idMessageType,
		Guid idInboxQueue,
		DTOs.InboxMessageDto inboxMessageDto,
		DTOs.DbOidContent content)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<InboxMessage?>();

		var contentResult = InboxMessageContent.CreateDbOid(scopeContext, content);

		if (result.MergeHasError(contentResult))
			return result.Build();

		return Create(
			scopeContext,
			idMessageType,
			idInboxQueue,
			inboxMessageDto,
			contentResult.Data);
	}

	internal static IResult<InboxMessage?> CreateWithFileRelativePathContent(
		IScopeContext scopeContext,
		Guid idMessageType,
		Guid idInboxQueue,
		DTOs.InboxMessageDto inboxMessageDto,
		DTOs.FileRelativePath content)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<InboxMessage?>();

		var contentResult = InboxMessageContent.CreateFileRelativePath(scopeContext, content);

		if (result.MergeHasError(contentResult))
			return result.Build();

		return Create(
			scopeContext,
			idMessageType,
			idInboxQueue,
			inboxMessageDto,
			contentResult.Data);
	}

	internal IResult<InboxMessageProcessingLog> SetProcessing(
		IScopeContext scopeContext,
		TimeSpan processingTimeout,
		bool disableProcessingLogNextTime,
		ILogMessage? logMessage = null,
		string logCode = nameof(Model.InboxMessageStatus.Processing),
		string? logDetail = null)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<InboxMessageProcessingLog>();

		if (result.IsArgumentLessThanOrEqual(scopeContext, processingTimeout, TimeSpan.Zero))
			return result.Build();

		var nowUtc = GlobalContext.Instance.UtcNow;

		bool wasProcessed = LastProcessingUtc.HasValue;

		IdInboxMessageStatus = Model.InboxMessageStatus.Processing;
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
			var createProcessingLogResult = InboxMessageProcessingLog.Create(
				scopeContext,
				IdInboxMessage,
				IdInboxQueue,
				IdInboxMessageStatus,
				logMessage?.IdLogMessage,
				logCode,
				logDetail ?? logMessage?.ToMessage());

			if (!result.MergeHasError(createProcessingLogResult))
				result.WithData(createProcessingLogResult.Data!); // _inboxMessageProcessingLogs.Add(createProcessingLogResult.Data!);
		}

		return result.Build();
	}

	internal IResult<InboxMessageProcessingLog> SetProcessed(
		IScopeContext scopeContext,
		bool disableProcessingLog,
		ILogMessage? logMessage = null,
		string logCode = nameof(Model.InboxMessageStatus.Processed),
		string? logDetail = null)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<InboxMessageProcessingLog>();

		IdInboxMessageStatus = Model.InboxMessageStatus.Processed;
		ProcessedUtc = GlobalContext.Instance.UtcNow;
		SuspendedUtc = null;

		var validationResult =
			DefaultDBValidator
				.Validate(this);

		if (result.MergeHasError(scopeContext.CreateNew(), validationResult, true))
			return result.Build();

		if (!disableProcessingLog)
		{
			var createProcessingLogResult = InboxMessageProcessingLog.Create(
				scopeContext,
				IdInboxMessage,
				IdInboxQueue,
				IdInboxMessageStatus,
				logMessage?.IdLogMessage,
				logCode,
				logDetail ?? logMessage?.ToMessage());

			if (!result.MergeHasError(createProcessingLogResult))
				result.WithData(createProcessingLogResult.Data!); // _inboxMessageProcessingLogs.Add(createProcessingLogResult.Data!);
		}

		return result.Build();
	}

	internal IResult<InboxMessageProcessingLog> SetFailed(
		IScopeContext scopeContext,
		int maxRetryCount,
		TimeSpan nextProcessingDelay,
		bool disableProcessingLog,
		IErrorMessage errorMessage,
		string logCode = nameof(Model.InboxMessageStatus.Failed),
		string? logDetail = null)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<InboxMessageProcessingLog>();

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
			IdInboxMessageStatus = Model.InboxMessageStatus.Suspended;
			if (logCode == nameof(Model.InboxMessageStatus.Failed))
				logCode = nameof(Model.InboxMessageStatus.Suspended);
		}
		else
		{
			SuspendedUtc = null;
			NextProcessingUtc = nowUtc.Add(nextProcessingDelay);
			IdInboxMessageStatus = Model.InboxMessageStatus.Failed;
		}

		var validationResult =
			DefaultDBValidator
				.Validate(this);

		if (result.MergeHasError(scopeContext.CreateNew(), validationResult, true))
			return result.Build();

		if (!disableProcessingLog)
		{
			var createProcessingLogResult = InboxMessageProcessingLog.Create(
				scopeContext,
				IdInboxMessage,
				IdInboxQueue,
				IdInboxMessageStatus,
				errorMessage?.IdLogMessage,
				logCode,
				logDetail ?? errorMessage?.ToMessage());

			if (!result.MergeHasError(createProcessingLogResult))
				result.WithData(createProcessingLogResult.Data!); // _inboxMessageProcessingLogs.Add(createProcessingLogResult.Data!);
		}

		return result.Build();
	}

	internal IResult<InboxMessageProcessingLog> SetUnknownType(
		IScopeContext scopeContext,
		bool disableProcessingLog,
		ILogMessage? logMessage = null,
		string logCode = nameof(Model.InboxMessageStatus.UnknownType),
		string? logDetail = null)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<InboxMessageProcessingLog>();

		ProcessedUtc = null;
		SuspendedUtc = GlobalContext.Instance.UtcNow;
		IdInboxMessageStatus = Model.InboxMessageStatus.UnknownType;
		//RetryCount++;

		var validationResult =
			DefaultDBValidator
				.Validate(this);

		if (result.MergeHasError(scopeContext.CreateNew(), validationResult, true))
			return result.Build();

		if (!disableProcessingLog)
		{
			var createProcessingLogResult = InboxMessageProcessingLog.Create(
				scopeContext,
				IdInboxMessage,
				IdInboxQueue,
				IdInboxMessageStatus,
				logMessage?.IdLogMessage,
				logCode,
				logDetail ?? logMessage?.ToMessage());

			if (!result.MergeHasError(createProcessingLogResult))
				result.WithData(createProcessingLogResult.Data!); // _inboxMessageProcessingLogs.Add(createProcessingLogResult.Data!);
		}

		return result.Build();
	}

	internal IResult<InboxMessageProcessingLog> SetNoHandler(
		IScopeContext scopeContext,
		bool disableProcessingLog,
		ILogMessage? logMessage = null,
		string logCode = nameof(Model.InboxMessageStatus.NoHandler),
		string? logDetail = null)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<InboxMessageProcessingLog>();

		ProcessedUtc = null;
		SuspendedUtc = GlobalContext.Instance.UtcNow;
		IdInboxMessageStatus = Model.InboxMessageStatus.NoHandler;
		//RetryCount++;

		var validationResult =
			DefaultDBValidator
				.Validate(this);

		if (result.MergeHasError(scopeContext.CreateNew(), validationResult, true))
			return result.Build();

		if (!disableProcessingLog)
		{
			var createProcessingLogResult = InboxMessageProcessingLog.Create(
				scopeContext,
				IdInboxMessage,
				IdInboxQueue,
				IdInboxMessageStatus,
				logMessage?.IdLogMessage,
				logCode,
				logDetail ?? logMessage?.ToMessage());

			if (!result.MergeHasError(createProcessingLogResult))
				result.WithData(createProcessingLogResult.Data!); // _inboxMessageProcessingLogs.Add(createProcessingLogResult.Data!);
		}

		return result.Build();
	}

	internal IResult<InboxMessageProcessingLog> SetBlocked(
		IScopeContext scopeContext,
		bool disableProcessingLog,
		ILogMessage? logMessage = null,
		string logCode = nameof(Model.InboxMessageStatus.Blocked),
		string? logDetail = null)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<InboxMessageProcessingLog>();

		ProcessedUtc = null;
		SuspendedUtc = GlobalContext.Instance.UtcNow;
		IdInboxMessageStatus = Model.InboxMessageStatus.Blocked;
		//RetryCount++;

		var validationResult =
			DefaultDBValidator
				.Validate(this);

		if (result.MergeHasError(scopeContext.CreateNew(), validationResult, true))
			return result.Build();

		if (!disableProcessingLog)
		{
			var createProcessingLogResult = InboxMessageProcessingLog.Create(
				scopeContext,
				IdInboxMessage,
				IdInboxQueue,
				IdInboxMessageStatus,
				logMessage?.IdLogMessage,
				logCode,
				logDetail ?? logMessage?.ToMessage());

			if (!result.MergeHasError(createProcessingLogResult))
				result.WithData(createProcessingLogResult.Data!); // _inboxMessageProcessingLogs.Add(createProcessingLogResult.Data!);
		}

		return result.Build();
	}

	internal IResult<InboxMessageProcessingLog> SetSuspended(
		IScopeContext scopeContext,
		bool disableProcessingLog,
		IErrorMessage errorMessage,
		string logCode = nameof(Model.InboxMessageStatus.Suspended),
		string? logDetail = null)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<InboxMessageProcessingLog>();

		var nowUtc = GlobalContext.Instance.UtcNow;

		ProcessedUtc = null;
		RetryCount++;
		SuspendedUtc = nowUtc;
		IdInboxMessageStatus = Model.InboxMessageStatus.Suspended;

		var validationResult =
			DefaultDBValidator
				.Validate(this);

		if (result.MergeHasError(scopeContext.CreateNew(), validationResult, true))
			return result.Build();

		if (!disableProcessingLog)
		{
			var createProcessingLogResult = InboxMessageProcessingLog.Create(
				scopeContext,
				IdInboxMessage,
				IdInboxQueue,
				IdInboxMessageStatus,
				errorMessage?.IdLogMessage,
				logCode,
				logDetail ?? errorMessage?.ToMessage());

			if (!result.MergeHasError(createProcessingLogResult))
				result.WithData(createProcessingLogResult.Data!); // _inboxMessageProcessingLogs.Add(createProcessingLogResult.Data!);
		}

		return result.Build();
	}
}
