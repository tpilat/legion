namespace Legion.ADF.Messaging.Inbox.Model;

public sealed partial class InboxMessageContent : Inbox.InboxBaseEntity, Legion.Model.IEntity
{
	internal static IResult<InboxMessageContent?> CreateByteArray(
		IScopeContext scopeContext,
		DTOs.ByteArrayContent content)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<InboxMessageContent?>();

		if (result.IsArgumentNull(scopeContext, content))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, content.MimeType))
			return result.Build();

		if (result.IsArgumentNullOrEmpty(scopeContext, content.ByteArray))
			return result.Build();

		var id = Guid.NewGuid();
		var inboxMessageContent = new InboxMessageContent
		{
			__IsNewObject = true,
			IdInboxMessageContent = id,
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
				.Validate(inboxMessageContent);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(inboxMessageContent).Build();
	}

	internal static IResult<InboxMessageContent?> CreateJson(
		IScopeContext scopeContext,
		DTOs.JsonContent content)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<InboxMessageContent?>();

		if (result.IsArgumentNull(scopeContext, content))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, content.MimeType))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, content.Json))
			return result.Build();

		var id = Guid.NewGuid();
		var inboxMessageContent = new InboxMessageContent
		{
			__IsNewObject = true,
			IdInboxMessageContent = id,
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
				.Validate(inboxMessageContent);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(inboxMessageContent).Build();
	}

	internal static IResult<InboxMessageContent?> CreateString(
		IScopeContext scopeContext,
		DTOs.StringContent content)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<InboxMessageContent?>();

		if (result.IsArgumentNull(scopeContext, content))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, content.MimeType))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, content.String))
			return result.Build();

		var id = Guid.NewGuid();
		var inboxMessageContent = new InboxMessageContent
		{
			__IsNewObject = true,
			IdInboxMessageContent = id,
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
				.Validate(inboxMessageContent);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(inboxMessageContent).Build();
	}

	internal static IResult<InboxMessageContent?> CreateDbOid(
		IScopeContext scopeContext,
		DTOs.DbOidContent content)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<InboxMessageContent?>();

		if (result.IsArgumentNull(scopeContext, content))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, content.MimeType))
			return result.Build();

		var id = Guid.NewGuid();
		var inboxMessageContent = new InboxMessageContent
		{
			__IsNewObject = true,
			IdInboxMessageContent = id,
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
				.Validate(inboxMessageContent);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(inboxMessageContent).Build();
	}

	internal static IResult<InboxMessageContent?> CreateFileRelativePath(
		IScopeContext scopeContext,
		DTOs.FileRelativePath content)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<InboxMessageContent?>();

		if (result.IsArgumentNull(scopeContext, content))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, content.MimeType))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, content.RelativePath))
			return result.Build();

		var id = Guid.NewGuid();
		var inboxMessageContent = new InboxMessageContent
		{
			__IsNewObject = true,
			IdInboxMessageContent = id,
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
				.Validate(inboxMessageContent);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(inboxMessageContent).Build();
	}
}
