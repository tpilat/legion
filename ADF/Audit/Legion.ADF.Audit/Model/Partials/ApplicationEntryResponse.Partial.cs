namespace Legion.ADF.Audit.Model;

public sealed partial class ApplicationEntryResponse : Audit.AuditBaseEntity, Legion.Model.IEntity
{
	public const string EMPTY_CONTENT = "-";

	internal static IResult<ApplicationEntryResponse?> Create(
		IScopeContext scopeContext,
		Guid idApplicationEntry,
		string statusCode,
		string? error,
		decimal elapsedMilliseconds,
		DTOs.Content? content)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<ApplicationEntryResponse?>();

		if (result.IsArgumentNull(scopeContext, content))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, statusCode))
			return result.Build();

		if (content is DTOs.ByteArrayContent byteArrayContent)
			return CreateByteArray(scopeContext, idApplicationEntry, statusCode, error, elapsedMilliseconds, byteArrayContent);
		else if (content is DTOs.JsonContent jsonContent)
			return CreateJson(scopeContext, idApplicationEntry, statusCode, error, elapsedMilliseconds, jsonContent);
		else if (content is DTOs.StringContent stringContent)
			return CreateString(scopeContext, idApplicationEntry, statusCode, error, elapsedMilliseconds, stringContent);
		else if (content is DTOs.DbOidContent dbOidContent)
			return CreateDbOid(scopeContext, idApplicationEntry, statusCode, error, elapsedMilliseconds, dbOidContent);
		else if (content is DTOs.FileRelativePath fileRelativePath)
			return CreateFileRelativePath(scopeContext, idApplicationEntry, statusCode, error, elapsedMilliseconds, fileRelativePath);
		else
			return result.WithInvalidOperationException(scopeContext, errorCode: null, detail: $"Invalid {nameof(content)} type = {content.GetType().FullName}");
	}

	internal static IResult<ApplicationEntryResponse?> Create(
		IScopeContext scopeContext,
		Guid idApplicationEntry,
		string statusCode,
		string? error,
		decimal elapsedMilliseconds)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<ApplicationEntryResponse?>();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, statusCode))
			return result.Build();

		var id = GlobalContext.Instance.NewGuid();
		var applicationEntryResponse = new ApplicationEntryResponse
		{
			__IsNewObject = true,
			IdApplicationEntryResponse = id,
			IdApplicationEntry = idApplicationEntry,
			MimeType = EMPTY_CONTENT,
			ContentEncoding = null,
			ByteArrayContent = null,
			JsonContent = null,
			StringContent = null,
			DbOid = null,
			Name = null,
			RelativePath = null,
			Metadata = null,
			IsCompressed = false,
			EncryptionKey = null,
			CreatedUtc = GlobalContext.Instance.UtcNow,
			StatusCode = statusCode,
			Error = error,
			ElapsedMilliseconds = elapsedMilliseconds
		};

		var validationResult =
			DefaultDBValidator
				.Validate(applicationEntryResponse);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(applicationEntryResponse).Build();
	}

	internal static IResult<ApplicationEntryResponse?> CreateByteArray(
		IScopeContext scopeContext,
		Guid idApplicationEntry,
		string statusCode,
		string? error,
		decimal elapsedMilliseconds,
		DTOs.ByteArrayContent? content)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<ApplicationEntryResponse?>();

		if (result.IsArgumentNull(scopeContext, content))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, content.MimeType))
			return result.Build();

		if (result.IsArgumentNullOrEmpty(scopeContext, content.ByteArray))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, statusCode))
			return result.Build();

		var id = GlobalContext.Instance.NewGuid();
		var applicationEntryResponse = new ApplicationEntryResponse
		{
			__IsNewObject = true,
			IdApplicationEntryResponse = id,
			IdApplicationEntry = idApplicationEntry,
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
			CreatedUtc = GlobalContext.Instance.UtcNow,
			StatusCode = statusCode,
			Error = error,
			ElapsedMilliseconds = elapsedMilliseconds
		};

		var validationResult =
			DefaultDBValidator
				.Validate(applicationEntryResponse);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(applicationEntryResponse).Build();
	}

	internal static IResult<ApplicationEntryResponse?> CreateJson(
		IScopeContext scopeContext,
		Guid idApplicationEntry,
		string statusCode,
		string? error,
		decimal elapsedMilliseconds,
		DTOs.JsonContent content)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<ApplicationEntryResponse?>();

		if (result.IsArgumentNull(scopeContext, content))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, content.MimeType))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, content.Json))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, statusCode))
			return result.Build();

		var id = GlobalContext.Instance.NewGuid();
		var applicationEntryResponse = new ApplicationEntryResponse
		{
			__IsNewObject = true,
			IdApplicationEntryResponse = id,
			IdApplicationEntry = idApplicationEntry,
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
			CreatedUtc = GlobalContext.Instance.UtcNow,
			StatusCode = statusCode,
			Error = error,
			ElapsedMilliseconds = elapsedMilliseconds
		};

		var validationResult =
			DefaultDBValidator
				.Validate(applicationEntryResponse);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(applicationEntryResponse).Build();
	}

	internal static IResult<ApplicationEntryResponse?> CreateString(
		IScopeContext scopeContext,
		Guid idApplicationEntry,
		string statusCode,
		string? error,
		decimal elapsedMilliseconds,
		DTOs.StringContent content)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<ApplicationEntryResponse?>();

		if (result.IsArgumentNull(scopeContext, content))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, content.MimeType))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, content.String))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, statusCode))
			return result.Build();

		var id = GlobalContext.Instance.NewGuid();
		var applicationEntryResponse = new ApplicationEntryResponse
		{
			__IsNewObject = true,
			IdApplicationEntryResponse = id,
			IdApplicationEntry = idApplicationEntry,
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
			CreatedUtc = GlobalContext.Instance.UtcNow,
			StatusCode = statusCode,
			Error = error,
			ElapsedMilliseconds = elapsedMilliseconds
		};

		var validationResult =
			DefaultDBValidator
				.Validate(applicationEntryResponse);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(applicationEntryResponse).Build();
	}

	internal static IResult<ApplicationEntryResponse?> CreateDbOid(
		IScopeContext scopeContext,
		Guid idApplicationEntry,
		string statusCode,
		string? error,
		decimal elapsedMilliseconds,
		DTOs.DbOidContent content)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<ApplicationEntryResponse?>();

		if (result.IsArgumentNull(scopeContext, content))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, content.MimeType))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, statusCode))
			return result.Build();

		var id = GlobalContext.Instance.NewGuid();
		var applicationEntryResponse = new ApplicationEntryResponse
		{
			__IsNewObject = true,
			IdApplicationEntryResponse = id,
			IdApplicationEntry = idApplicationEntry,
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
			CreatedUtc = GlobalContext.Instance.UtcNow,
			StatusCode = statusCode,
			Error = error,
			ElapsedMilliseconds = elapsedMilliseconds
		};

		var validationResult =
			DefaultDBValidator
				.Validate(applicationEntryResponse);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(applicationEntryResponse).Build();
	}

	internal static IResult<ApplicationEntryResponse?> CreateFileRelativePath(
		IScopeContext scopeContext,
		Guid idApplicationEntry,
		string statusCode,
		string? error,
		decimal elapsedMilliseconds,
		DTOs.FileRelativePath content)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<ApplicationEntryResponse?>();

		if (result.IsArgumentNull(scopeContext, content))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, content.MimeType))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, content.RelativePath))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, statusCode))
			return result.Build();

		var id = GlobalContext.Instance.NewGuid();
		var applicationEntryResponse = new ApplicationEntryResponse
		{
			__IsNewObject = true,
			IdApplicationEntryResponse = id,
			IdApplicationEntry = idApplicationEntry,
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
			CreatedUtc = GlobalContext.Instance.UtcNow,
			StatusCode = statusCode,
			Error = error,
			ElapsedMilliseconds = elapsedMilliseconds
		};

		var validationResult =
			DefaultDBValidator
				.Validate(applicationEntryResponse);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(applicationEntryResponse).Build();
	}
}
