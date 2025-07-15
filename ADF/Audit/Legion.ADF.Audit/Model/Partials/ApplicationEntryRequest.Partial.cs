namespace Legion.ADF.Audit.Model;

public sealed partial class ApplicationEntryRequest : Audit.AuditBaseEntity, Legion.Model.IEntity
{
	internal static IResult<ApplicationEntryRequest?> Create(
		IScopeContext scopeContext,
		ApplicationEntry applicationEntry,
		DTOs.Content content)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<ApplicationEntryRequest?>();

		if (result.IsArgumentNull(scopeContext, applicationEntry))
			return result.Build();

		if (result.IsArgumentNull(scopeContext, content))
			return result.Build();

		if (content is DTOs.ByteArrayContent byteArrayContent)
			return CreateByteArray(scopeContext, applicationEntry, byteArrayContent);
		else if (content is DTOs.JsonContent jsonContent)
			return CreateJson(scopeContext, applicationEntry, jsonContent);
		else if (content is DTOs.StringContent stringContent)
			return CreateString(scopeContext, applicationEntry, stringContent);
		else if (content is DTOs.DbOidContent dbOidContent)
			return CreateDbOid(scopeContext, applicationEntry, dbOidContent);
		else if (content is DTOs.FileRelativePath fileRelativePath)
			return CreateFileRelativePath(scopeContext, applicationEntry, fileRelativePath);
		else
			return result.WithInvalidOperationException(scopeContext, errorCode: null, detail: $"Invalid {nameof(content)} type = {content.GetType().FullName}");
	}

	internal static IResult<ApplicationEntryRequest?> CreateByteArray(
		IScopeContext scopeContext,
		ApplicationEntry applicationEntry,
		DTOs.ByteArrayContent content)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<ApplicationEntryRequest?>();

		if (result.IsArgumentNull(scopeContext, applicationEntry))
			return result.Build();

		if (result.IsArgumentNull(scopeContext, content))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, content.MimeType))
			return result.Build();

		if (result.IsArgumentNullOrEmpty(scopeContext, content.ByteArray))
			return result.Build();

		var id = GlobalContext.Instance.NewGuid();
		var applicationEntryRequest = new ApplicationEntryRequest
		{
			__IsNewObject = true,
			IdApplicationEntryRequest = id,
			ApplicationEntry = applicationEntry,
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
			EncryptionKey = content.EncryptionKey,
			CreatedUtc = GlobalContext.Instance.UtcNow
		};

		var validationResult =
			DefaultDBValidator
				.Validate(applicationEntryRequest);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(applicationEntryRequest).Build();
	}

	internal static IResult<ApplicationEntryRequest?> CreateJson(
		IScopeContext scopeContext,
		ApplicationEntry applicationEntry,
		DTOs.JsonContent content)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<ApplicationEntryRequest?>();

		if (result.IsArgumentNull(scopeContext, applicationEntry))
			return result.Build();

		if (result.IsArgumentNull(scopeContext, content))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, content.MimeType))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, content.Json))
			return result.Build();

		var id = GlobalContext.Instance.NewGuid();
		var applicationEntryRequest = new ApplicationEntryRequest
		{
			__IsNewObject = true,
			IdApplicationEntryRequest = id,
			ApplicationEntry = applicationEntry,
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
			EncryptionKey = content.EncryptionKey,
			CreatedUtc = GlobalContext.Instance.UtcNow
		};

		var validationResult =
			DefaultDBValidator
				.Validate(applicationEntryRequest);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(applicationEntryRequest).Build();
	}

	internal static IResult<ApplicationEntryRequest?> CreateString(
		IScopeContext scopeContext,
		ApplicationEntry applicationEntry,
		DTOs.StringContent content)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<ApplicationEntryRequest?>();

		if (result.IsArgumentNull(scopeContext, applicationEntry))
			return result.Build();

		if (result.IsArgumentNull(scopeContext, content))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, content.MimeType))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, content.String))
			return result.Build();

		var id = GlobalContext.Instance.NewGuid();
		var applicationEntryRequest = new ApplicationEntryRequest
		{
			__IsNewObject = true,
			IdApplicationEntryRequest = id,
			ApplicationEntry = applicationEntry,
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
			EncryptionKey = content.EncryptionKey,
			CreatedUtc = GlobalContext.Instance.UtcNow
		};

		var validationResult =
			DefaultDBValidator
				.Validate(applicationEntryRequest);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(applicationEntryRequest).Build();
	}

	internal static IResult<ApplicationEntryRequest?> CreateDbOid(
		IScopeContext scopeContext,
		ApplicationEntry applicationEntry,
		DTOs.DbOidContent content)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<ApplicationEntryRequest?>();

		if (result.IsArgumentNull(scopeContext, applicationEntry))
			return result.Build();

		if (result.IsArgumentNull(scopeContext, content))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, content.MimeType))
			return result.Build();

		var id = GlobalContext.Instance.NewGuid();
		var applicationEntryRequest = new ApplicationEntryRequest
		{
			__IsNewObject = true,
			IdApplicationEntryRequest = id,
			ApplicationEntry = applicationEntry,
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
			EncryptionKey = content.EncryptionKey,
			CreatedUtc = GlobalContext.Instance.UtcNow
		};

		var validationResult =
			DefaultDBValidator
				.Validate(applicationEntryRequest);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(applicationEntryRequest).Build();
	}

	internal static IResult<ApplicationEntryRequest?> CreateFileRelativePath(
		IScopeContext scopeContext,
		ApplicationEntry applicationEntry,
		DTOs.FileRelativePath content)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<ApplicationEntryRequest?>();

		if (result.IsArgumentNull(scopeContext, applicationEntry))
			return result.Build();

		if (result.IsArgumentNull(scopeContext, content))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, content.MimeType))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, content.RelativePath))
			return result.Build();

		var id = GlobalContext.Instance.NewGuid();
		var applicationEntryRequest = new ApplicationEntryRequest
		{
			__IsNewObject = true,
			IdApplicationEntryRequest = id,
			ApplicationEntry = applicationEntry,
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
			EncryptionKey = content.EncryptionKey,
			CreatedUtc = GlobalContext.Instance.UtcNow
		};

		var validationResult =
			DefaultDBValidator
				.Validate(applicationEntryRequest);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(applicationEntryRequest).Build();
	}
}
