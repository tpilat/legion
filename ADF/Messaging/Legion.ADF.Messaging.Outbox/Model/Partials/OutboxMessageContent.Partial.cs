namespace Legion.ADF.Messaging.Outbox.Model;

public sealed partial class OutboxMessageContent : Outbox.OutboxBaseEntity, Legion.Model.IEntity
{
	internal static IResult<OutboxMessageContent?> CreateByteArray(
		IScopeContext scopeContext,
		DTOs.ByteArrayContent content)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<OutboxMessageContent?>();

		if (result.IsArgumentNull(scopeContext, content))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, content.MimeType))
			return result.Build();

		if (result.IsArgumentNullOrEmpty(scopeContext, content.ByteArray))
			return result.Build();

		var id = Guid.NewGuid();
		var outboxMessageContent = new OutboxMessageContent
		{
			__IsNewObject = true,
			IdOutboxMessageContent = id,
			MimeType = content.MimeType,
			ContentEncoding = content.ContentEncoding,
			ByteArrayContent = content.ByteArray,
			JsonContent = null,
			StringContent = null,
			DbOid = null,
			Name = content.Name,
			RelativePath = null,
			Metadata = content.Metadata,
			IsCompressed = content.IsCompressed,
			EncryptionKey = content.EncryptionKey
		};

		var validationResult =
			DefaultDBValidator
				.Validate(outboxMessageContent);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(outboxMessageContent).Build();
	}

	internal static IResult<OutboxMessageContent?> CreateJson(
		IScopeContext scopeContext,
		DTOs.JsonContent content)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<OutboxMessageContent?>();

		if (result.IsArgumentNull(scopeContext, content))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, content.MimeType))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, content.Json))
			return result.Build();

		var id = Guid.NewGuid();
		var outboxMessageContent = new OutboxMessageContent
		{
			__IsNewObject = true,
			IdOutboxMessageContent = id,
			MimeType = content.MimeType,
			ContentEncoding = content.ContentEncoding,
			ByteArrayContent = null,
			JsonContent = content.Json,
			StringContent = null,
			DbOid = null,
			Name = content.Name,
			RelativePath = null,
			Metadata = content.Metadata,
			IsCompressed = content.IsCompressed,
			EncryptionKey = content.EncryptionKey
		};

		var validationResult =
			DefaultDBValidator
				.Validate(outboxMessageContent);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(outboxMessageContent).Build();
	}

	internal static IResult<OutboxMessageContent?> CreateString(
		IScopeContext scopeContext,
		DTOs.StringContent content)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<OutboxMessageContent?>();

		if (result.IsArgumentNull(scopeContext, content))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, content.MimeType))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, content.String))
			return result.Build();

		var id = Guid.NewGuid();
		var outboxMessageContent = new OutboxMessageContent
		{
			__IsNewObject = true,
			IdOutboxMessageContent = id,
			MimeType = content.MimeType,
			ContentEncoding = content.ContentEncoding,
			ByteArrayContent = null,
			JsonContent = null,
			StringContent = content.String,
			DbOid = null,
			Name = content.Name,
			RelativePath = null,
			Metadata = content.Metadata,
			IsCompressed = content.IsCompressed,
			EncryptionKey = content.EncryptionKey
		};

		var validationResult =
			DefaultDBValidator
				.Validate(outboxMessageContent);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(outboxMessageContent).Build();
	}

	internal static IResult<OutboxMessageContent?> CreateDbOid(
		IScopeContext scopeContext,
		DTOs.DbOidContent content)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<OutboxMessageContent?>();

		if (result.IsArgumentNull(scopeContext, content))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, content.MimeType))
			return result.Build();

		var id = Guid.NewGuid();
		var outboxMessageContent = new OutboxMessageContent
		{
			__IsNewObject = true,
			IdOutboxMessageContent = id,
			MimeType = content.MimeType,
			ContentEncoding = content.ContentEncoding,
			ByteArrayContent = null,
			JsonContent = null,
			StringContent = null,
			DbOid = content.DbOid,
			Name = content.Name,
			RelativePath = null,
			Metadata = content.Metadata,
			IsCompressed = content.IsCompressed,
			EncryptionKey = content.EncryptionKey
		};

		var validationResult =
			DefaultDBValidator
				.Validate(outboxMessageContent);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(outboxMessageContent).Build();
	}

	internal static IResult<OutboxMessageContent?> CreateFileRelativePath(
		IScopeContext scopeContext,
		DTOs.FileRelativePath content)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<OutboxMessageContent?>();

		if (result.IsArgumentNull(scopeContext, content))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, content.MimeType))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, content.RelativePath))
			return result.Build();

		var id = Guid.NewGuid();
		var outboxMessageContent = new OutboxMessageContent
		{
			__IsNewObject = true,
			IdOutboxMessageContent = id,
			MimeType = content.MimeType,
			ContentEncoding = content.ContentEncoding,
			ByteArrayContent = null,
			JsonContent = null,
			StringContent = null,
			DbOid = null,
			Name = content.Name,
			RelativePath = content.RelativePath,
			Metadata = content.Metadata,
			IsCompressed = content.IsCompressed,
			EncryptionKey = content.EncryptionKey
		};

		var validationResult =
			DefaultDBValidator
				.Validate(outboxMessageContent);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(outboxMessageContent).Build();
	}
}
