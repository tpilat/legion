using Legion.Extensions;
using Legion.Http;
using Legion.Serializer;
using Legion.Validation;
using Legion.Web.Logging;

namespace Legion.ADF.ESB.Components.Model;

public sealed partial class AdapterResponsePayload : Components.ComponentsBaseEntity, Legion.Model.IEntity
{
	private const string MimeType_JSON = "json";
	private const string MimeType_HTML = "html";
	private const string MimeType_TEXT = "text";
	private const string MimeType_XML = "xml";
	private const string MimeType_CSS = "css";
	private const string MimeType_JAVASCRIPT = "javascript";

	public static async Task<IResult<List<AdapterResponsePayload>>> CreateAdapterResponsePayloadChunksAsync(
		IScopeContext scopeContext,
		AdapterResponse adapterResponse,
		ResponseDto response,
		HttpContentDto responseContent,
		string clientName)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<List<AdapterResponsePayload>>();

		if (result.IsArgumentNull(scopeContext, adapterResponse))
			return result.Build();

		if (result.IsArgumentNull(scopeContext, response))
			return result.Build();

		if (result.IsArgumentNull(scopeContext, responseContent))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, clientName))
			return result.Build();

		var payloads = new List<AdapterResponsePayload>();

		try
		{
			await responseContent.ReadContentAsync();
		}
		catch (Exception ex)
		{
			//TODO: LOG ERROR
		}

		if (0 < responseContent.StringContents?.Count)
		{
			foreach (var stringContent in responseContent.StringContents)
			{
				var adapterResponsePayload = new AdapterResponsePayload
				{
					IdAdapterResponsePayload = Guid.NewGuid(),
					AdapterResponse = adapterResponse,
					ResponseContentType = nameof(Legion.Http.StringContent),
					ByteArrayContent = null,
					JsonContent = null,
					StringContent = stringContent.Content,
					ContentHeaders = JsonSerializerHelper.Serialize(stringContent.Headers),
					DbOid = null,
					Name = null,
					RelativePath = null,
					Metadata = null,
					IsCompressed = false,
					EncryptionKey = null,
					ContentEncoding = stringContent.Encoding?.WebName,
					MediaType = stringContent.MediaType,
					MultipartFormDataContentName = stringContent.HttpContentNameFormDataMultipartPurpose,
					MultipartFormDataFileName = null,
					JsonInputCSharpType = null
				};

				var validationResult =
					SetDBValidatorRules(new ValidatorBuilder<AdapterResponsePayload>())
						.Build()
						.Validate(adapterResponsePayload);

				if (result.MergeHasError(scopeContext, validationResult, true))
					return result.Build();

				payloads.Add(adapterResponsePayload);
				var addResult = adapterResponse.AddPaylod(scopeContext, adapterResponsePayload);
				if (result.MergeHasError(addResult))
					return result.Build();
			}
		}

		if (0 < responseContent.JsonContents?.Count)
		{
			foreach (var jsonContent in responseContent.JsonContents)
			{
				var adapterResponsePayload = new AdapterResponsePayload
				{
					IdAdapterResponsePayload = Guid.NewGuid(),
					AdapterResponse = adapterResponse,
					ResponseContentType = nameof(Legion.Http.JsonContent),
					ByteArrayContent = null,
					JsonContent = await jsonContent.ToStringAsync(),
					StringContent = null,
					ContentHeaders = JsonSerializerHelper.Serialize(jsonContent.Headers),
					DbOid = null,
					Name = null,
					RelativePath = null,
					Metadata = null,
					IsCompressed = false,
					EncryptionKey = null,
					ContentEncoding = null,
					MediaType = jsonContent.MediaType?.ToString(),
					MultipartFormDataContentName = jsonContent.HttpContentNameFormDataMultipartPurpose,
					MultipartFormDataFileName = null,
					JsonInputCSharpType = jsonContent.InputType?.ToFriendlyFullName()
				};

				var validationResult =
					SetDBValidatorRules(new ValidatorBuilder<AdapterResponsePayload>())
						.Build()
						.Validate(adapterResponsePayload);

				if (result.MergeHasError(scopeContext, validationResult, true))
					return result.Build();

				payloads.Add(adapterResponsePayload);
				var addResult = adapterResponse.AddPaylod(scopeContext, adapterResponsePayload);
				if (result.MergeHasError(addResult))
					return result.Build();
			}
		}

		if (0 < responseContent.StreamContents?.Count)
		{
			foreach (var streamContent in responseContent.StreamContents)
			{
				var adapterResponsePayload = new AdapterResponsePayload
				{
					IdAdapterResponsePayload = Guid.NewGuid(),
					AdapterResponse = adapterResponse,
					ResponseContentType = nameof(Legion.Http.StreamContent),
					ByteArrayContent = streamContent.Stream?.ToArray(seek: true),
					JsonContent = null,
					StringContent = null,
					ContentHeaders = JsonSerializerHelper.Serialize(streamContent.Headers),
					DbOid = null,
					Name = null,
					RelativePath = null,
					Metadata = null,
					IsCompressed = false,
					EncryptionKey = null,
					ContentEncoding = null,
					MediaType = null,
					MultipartFormDataContentName = streamContent.HttpContentNameFormDataMultipartPurposeMultipartPurpose,
					MultipartFormDataFileName = streamContent.HttpContentFileNameFormDataMultipartPurposeMultipartPurpose,
					JsonInputCSharpType = null
				};

				var validationResult =
					SetDBValidatorRules(new ValidatorBuilder<AdapterResponsePayload>())
						.Build()
						.Validate(adapterResponsePayload);

				if (result.MergeHasError(scopeContext, validationResult, true))
					return result.Build();

				payloads.Add(adapterResponsePayload);
				var addResult = adapterResponse.AddPaylod(scopeContext, adapterResponsePayload);
				if (result.MergeHasError(addResult))
					return result.Build();
			}
		}

		if (0 < responseContent.ByteArrayContents?.Count)
		{
			foreach (var byteArrayContent in responseContent.ByteArrayContents)
			{
				var adapterResponsePayload = new AdapterResponsePayload
				{
					IdAdapterResponsePayload = Guid.NewGuid(),
					AdapterResponse = adapterResponse,
					ResponseContentType = nameof(Legion.Http.ByteArrayContent),
					ByteArrayContent = byteArrayContent.ByteArray,
					JsonContent = null,
					StringContent = null,
					ContentHeaders = JsonSerializerHelper.Serialize(byteArrayContent.Headers),
					DbOid = null,
					Name = null,
					RelativePath = null,
					Metadata = null,
					IsCompressed = false,
					EncryptionKey = null,
					ContentEncoding = null,
					MediaType = null,
					MultipartFormDataContentName = byteArrayContent.HttpContentNameFormDataMultipartPurposeMultipartPurpose,
					MultipartFormDataFileName = byteArrayContent.HttpContentFileNameFormDataMultipartPurposeMultipartPurpose,
					JsonInputCSharpType = null
				};

				var validationResult =
					SetDBValidatorRules(new ValidatorBuilder<AdapterResponsePayload>())
						.Build()
						.Validate(adapterResponsePayload);

				if (result.MergeHasError(scopeContext, validationResult, true))
					return result.Build();

				payloads.Add(adapterResponsePayload);
				var addResult = adapterResponse.AddPaylod(scopeContext, adapterResponsePayload);
				if (result.MergeHasError(addResult))
					return result.Build();
			}
		}

		if (0 < responseContent.HttpContents?.Count)
		{
			foreach (var httpContent in responseContent.HttpContents)
			{
				byte[]? byteArrayContent = null;
				string? jsonContent = null;
				string? stringContent = null;

				if (response.ContentType?.Contains(MimeType_JSON, StringComparison.InvariantCultureIgnoreCase) == true)
				{
					jsonContent = await httpContent.ToStringAsync();
				}
				else if (response.ContentType?.Contains(MimeType_HTML, StringComparison.InvariantCultureIgnoreCase) == true
					|| response.ContentType?.Contains(MimeType_XML, StringComparison.InvariantCultureIgnoreCase) == true
					|| response.ContentType?.Contains(MimeType_TEXT, StringComparison.InvariantCultureIgnoreCase) == true
					|| response.ContentType?.Contains(MimeType_CSS, StringComparison.InvariantCultureIgnoreCase) == true
					|| response.ContentType?.Contains(MimeType_JAVASCRIPT, StringComparison.InvariantCultureIgnoreCase) == true)
				{
					stringContent = await httpContent.ToStringAsync();
				}
				else
				{
					byteArrayContent = httpContent.Stream?.ToArray(seek: true);
				}

				var adapterResponsePayload = new AdapterResponsePayload
				{
					IdAdapterResponsePayload = Guid.NewGuid(),
					AdapterResponse = adapterResponse,
					ResponseContentType = nameof(Legion.Http.HttpContent),
					ByteArrayContent = byteArrayContent,
					JsonContent = jsonContent,
					StringContent = stringContent,
					ContentHeaders = JsonSerializerHelper.Serialize(httpContent.Headers),
					DbOid = null,
					Name = null,
					RelativePath = null,
					Metadata = null,
					IsCompressed = false,
					EncryptionKey = null,
					ContentEncoding = null,
					MediaType = null,
					MultipartFormDataContentName = null,
					MultipartFormDataFileName = null,
					JsonInputCSharpType = null
				};

				var validationResult =
					SetDBValidatorRules(new ValidatorBuilder<AdapterResponsePayload>())
						.Build()
						.Validate(adapterResponsePayload);

				if (result.MergeHasError(scopeContext, validationResult, true))
					return result.Build();

				payloads.Add(adapterResponsePayload);
				var addResult = adapterResponse.AddPaylod(scopeContext, adapterResponsePayload);
				if (result.MergeHasError(addResult))
					return result.Build();
			}
		}

		return result.WithData(payloads).Build();
	}
}
