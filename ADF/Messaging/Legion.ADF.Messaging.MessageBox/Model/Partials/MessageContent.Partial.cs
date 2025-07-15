namespace Legion.ADF.Messaging.MessageBox.Model;

public sealed partial class MessageContent : MessageBox.MessageBoxBaseEntity, Legion.Model.IEntity
{
	internal static IResult<MessageContent?> CreateByteArray(
		IScopeContext scopeContext,
		DTOs.ByteArrayContent content)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<MessageContent?>();

		if (result.IsArgumentNull(scopeContext, content))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, content.MimeType))
			return result.Build();

		if (result.IsArgumentNullOrEmpty(scopeContext, content.ByteArray))
			return result.Build();

		var id = GlobalContext.Instance.NewGuid();
		var messageContent = new MessageContent
		{
			__IsNewObject = true,
			IdMessageContent = id,
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
				.Validate(messageContent);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(messageContent).Build();
	}

	internal static IResult<MessageContent?> CreateJson(
		IScopeContext scopeContext,
		DTOs.JsonContent content)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<MessageContent?>();

		if (result.IsArgumentNull(scopeContext, content))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, content.MimeType))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, content.Json))
			return result.Build();

		var id = GlobalContext.Instance.NewGuid();
		var messageContent = new MessageContent
		{
			__IsNewObject = true,
			IdMessageContent = id,
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
				.Validate(messageContent);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(messageContent).Build();
	}

	internal static IResult<MessageContent?> CreateString(
		IScopeContext scopeContext,
		DTOs.StringContent content)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<MessageContent?>();

		if (result.IsArgumentNull(scopeContext, content))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, content.MimeType))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, content.String))
			return result.Build();

		var id = GlobalContext.Instance.NewGuid();
		var messageContent = new MessageContent
		{
			__IsNewObject = true,
			IdMessageContent = id,
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
				.Validate(messageContent);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(messageContent).Build();
	}

	internal static IResult<MessageContent?> CreateDbOid(
		IScopeContext scopeContext,
		DTOs.DbOidContent content)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<MessageContent?>();

		if (result.IsArgumentNull(scopeContext, content))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, content.MimeType))
			return result.Build();

		var id = GlobalContext.Instance.NewGuid();
		var messageContent = new MessageContent
		{
			__IsNewObject = true,
			IdMessageContent = id,
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
				.Validate(messageContent);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(messageContent).Build();
	}

	internal static IResult<MessageContent?> CreateFileRelativePath(
		IScopeContext scopeContext,
		DTOs.FileRelativePath content)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<MessageContent?>();

		if (result.IsArgumentNull(scopeContext, content))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, content.MimeType))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, content.RelativePath))
			return result.Build();

		var id = GlobalContext.Instance.NewGuid();
		var messageContent = new MessageContent
		{
			__IsNewObject = true,
			IdMessageContent = id,
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
				.Validate(messageContent);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(messageContent).Build();
	}
}
