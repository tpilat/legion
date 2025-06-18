using Legion.Logging;

namespace Legion.ADF.Messaging.MessageBox.Model;

public sealed partial class Message : MessageBox.MessageBoxBaseEntity, Legion.Model.IEntity
{
	internal static IResult<Message?> Create(
		IScopeContext scopeContext,
		Guid idMessageType,
		Guid? idQueue,
		Guid? idTopic,
		DTOs.MessageBoxMessageDto messageDto)
	{
		scopeContext = scopeContext.CreateNew();
		var result = new ResultBuilder<Message?>();

		if (result.IsArgumentNull(scopeContext, messageDto))
			return result.Build();

		if (messageDto.Content == null)
		{
			var res = CreateWithNoContent(
				scopeContext,
				idMessageType,
				idQueue,
				idTopic,
				messageDto);

			result.MergeAllWithDataHasError(res);
			return result.Build();
		}
		else if (messageDto.Content is DTOs.ByteArrayContent byteArrayContent)
		{
			var res = CreateWithByteArrayContent(
				scopeContext,
				idMessageType,
				idQueue,
				idTopic,
				messageDto,
				byteArrayContent);

			result.MergeAllWithDataHasError(res);
			return result.Build();
		}
		else if (messageDto.Content is DTOs.JsonContent jsonContent)
		{
			var res = CreateWithJsonContent(
				scopeContext,
				idMessageType,
				idQueue,
				idTopic,
				messageDto,
				jsonContent);

			result.MergeAllWithDataHasError(res);
			return result.Build();
		}
		else if (messageDto.Content is DTOs.StringContent stringContent)
		{
			var res = CreateWithStringContent(
				scopeContext,
				idMessageType,
				idQueue,
				idTopic,
				messageDto,
				stringContent);

			result.MergeAllWithDataHasError(res);
			return result.Build();
		}
		else if (messageDto.Content is DTOs.DbOidContent dbOidContent)
		{
			var res = CreateWithDbOidContent(
				scopeContext,
				idMessageType,
				idQueue,
				idTopic,
				messageDto,
				dbOidContent);

			result.MergeAllWithDataHasError(res);
			return result.Build();
		}
		else if (messageDto.Content is DTOs.FileRelativePath fileRelativePath)
		{
			var res = CreateWithFileRelativePathContent(
				scopeContext,
				idMessageType,
				idQueue,
				idTopic,
				messageDto,
				fileRelativePath);

			result.MergeAllWithDataHasError(res);
			return result.Build();
		}
		else
		{
			return result.WithNotSupportedException(scopeContext, Messaging.Exceptions.Internal.ErrorCodes.MessageBoxMessageContent.InvalidContent(messageDto.Content.GetType()));
		}
	}

	private static IResult<Message?> Create(
		IScopeContext scopeContext,
		Guid idMessageType,
		Guid? idQueue,
		Guid? idTopic,
		DTOs.MessageBoxMessageDto messageDto,
		MessageContent? messageContent)
	{
		scopeContext = scopeContext.CreateNew()
			.AddContextProperty(nameof(idQueue), idQueue?.ToString())
			.AddContextProperty(nameof(idTopic), idTopic?.ToString());

		var result = new ResultBuilder<Message?>();

		if (result.IsArgumentNull(scopeContext, messageDto))
			return result.Build();

		if (!idQueue.HasValue && !idTopic.HasValue && result.IsArgumentNull(scopeContext, idQueue, errorCode: null, detail: null, paramName: $"{nameof(idQueue)} && {nameof(idTopic)}"))
			return result.Build();

		if (idQueue.HasValue && idTopic.HasValue)
			return result.WithArgumentException(scopeContext, idQueue, errorCode: null, detail: "Both are set", paramName: $"{nameof(idQueue)} != null && {nameof(idTopic)} != null");

		var nowUtc = GlobalContext.Instance.UtcNow;
		var id = messageContent?.IdMessageContent ?? Guid.NewGuid();
		var message = new Message
		{
			__IsNewObject = true,
			IdMessage = id,
			IdMessageType = idMessageType,
			IdMessageStatus = Model.MessageStatus.Created,
			MessageContent = messageContent!,
			IdQueue = idQueue,
			IdTopic = idTopic,
			MessageId = messageDto.MessageId,
			BusinessId = messageDto.BusinessId,
			CorrelationId = messageDto.CorrelationId,
			SessionId = messageDto.SessionId,
			SessionMessagePartId = messageDto.SessionMessagePartId,
			TraceCorrelationId = scopeContext.TraceCorrelationId,
			Properties = messageDto.Properties,
			Publisher = messageDto.Publisher,
			PublisherId = messageDto.PublisherId,
			CreatedUtc = nowUtc,
			ValidToUtc = messageDto.ValidToUtc,
			Priority = messageDto.Priority ?? 0,
			IdMessageBoxInstance = Infrastructure.EnvironmentInfo.RUNTIME_UNIQUE_KEY
		};

		var validationResult =
			DefaultDBValidator
				.Validate(message);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(message).Build();
	}

	internal static IResult<Message?> CreateWithNoContent(
		IScopeContext scopeContext,
		Guid idMessageType,
		Guid? idQueue,
		Guid? idTopic,
		DTOs.MessageBoxMessageDto messageDto)
		=> Create(
			scopeContext,
			idMessageType,
			idQueue,
			idTopic,
			messageDto,
			(MessageContent?)null);

	internal static IResult<Message?> CreateWithByteArrayContent(
		IScopeContext scopeContext,
		Guid idMessageType,
		Guid? idQueue,
		Guid? idTopic,
		DTOs.MessageBoxMessageDto messageDto,
		DTOs.ByteArrayContent content)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<Message?>();

		var contentResult = MessageContent.CreateByteArray(scopeContext, content);

		if (result.MergeHasError(contentResult))
			return result.Build();

		return Create(
			scopeContext,
			idMessageType,
			idQueue,
			idTopic,
			messageDto,
			contentResult.Data);
	}

	internal static IResult<Message?> CreateWithJsonContent(
		IScopeContext scopeContext,
		Guid idMessageType,
		Guid? idQueue,
		Guid? idTopic,
		DTOs.MessageBoxMessageDto messageDto,
		DTOs.JsonContent content)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<Message?>();

		var contentResult = MessageContent.CreateJson(scopeContext, content);

		if (result.MergeHasError(contentResult))
			return result.Build();

		return Create(
			scopeContext,
			idMessageType,
			idQueue,
			idTopic,
			messageDto,
			contentResult.Data);
	}

	internal static IResult<Message?> CreateWithStringContent(
		IScopeContext scopeContext,
		Guid idMessageType,
		Guid? idQueue,
		Guid? idTopic,
		DTOs.MessageBoxMessageDto messageDto,
		DTOs.StringContent content)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<Message?>();

		var contentResult = MessageContent.CreateString(scopeContext, content);

		if (result.MergeHasError(contentResult))
			return result.Build();

		return Create(
			scopeContext,
			idMessageType,
			idQueue,
			idTopic,
			messageDto,
			contentResult.Data);
	}

	internal static IResult<Message?> CreateWithDbOidContent(
		IScopeContext scopeContext,
		Guid idMessageType,
		Guid? idQueue,
		Guid? idTopic,
		DTOs.MessageBoxMessageDto messageDto,
		DTOs.DbOidContent content)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<Message?>();

		var contentResult = MessageContent.CreateDbOid(scopeContext, content);

		if (result.MergeHasError(contentResult))
			return result.Build();

		return Create(
			scopeContext,
			idMessageType,
			idQueue,
			idTopic,
			messageDto,
			contentResult.Data);
	}

	internal static IResult<Message?> CreateWithFileRelativePathContent(
		IScopeContext scopeContext,
		Guid idMessageType,
		Guid? idQueue,
		Guid? idTopic,
		DTOs.MessageBoxMessageDto messageDto,
		DTOs.FileRelativePath content)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<Message?>();

		var contentResult = MessageContent.CreateFileRelativePath(scopeContext, content);

		if (result.MergeHasError(contentResult))
			return result.Build();

		return Create(
			scopeContext,
			idMessageType,
			idQueue,
			idTopic,
			messageDto,
			contentResult.Data);
	}
}
