using Legion.Extensions;
using Legion.Http;
using Legion.Serializer;
using Legion.Validation;
using Legion.Web.Logging;

namespace Legion.ADF.ESB.Components.Model;

public sealed partial class AdapterRequestPayload : Components.ComponentsBaseEntity, Legion.Model.IEntity
{
	public static async Task<IResult<List<AdapterRequestPayload>>> CreateAdapterRequestPayloadChunksAsync(
		IScopeContext scopeContext,
		AdapterRequest adapterRequest,
		RequestDto request,
		HttpContentDto requestContent,
		string clientName)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<List<AdapterRequestPayload>>();

		if (result.IsArgumentNull(scopeContext, adapterRequest))
			return result.Build();

		if (result.IsArgumentNull(scopeContext, request))
			return result.Build();

		if (result.IsArgumentNull(scopeContext, requestContent))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, clientName))
			return result.Build();

		var payloads = new List<AdapterRequestPayload>();

		try
		{
			await requestContent.ReadContentAsync();
		}
		catch (Exception ex)
		{
			//TODO: LOG ERROR
		}

		if (0 < requestContent.StringContents?.Count)
		{
			foreach (var stringContent in requestContent.StringContents)
			{
				var adapterRequestPayload = new AdapterRequestPayload
				{
					IdAdapterRequestPayload = Guid.NewGuid(),
					AdapterRequest = adapterRequest,
					RequestContentType = nameof(Legion.Http.StringContent),
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
					SetDBValidatorRules(new ValidatorBuilder<AdapterRequestPayload>())
						.Build()
						.Validate(adapterRequestPayload);

				if (result.MergeHasError(scopeContext, validationResult, true))
					return result.Build();

				payloads.Add(adapterRequestPayload);
				var addResult = adapterRequest.AddPaylod(scopeContext, adapterRequestPayload);
				if (result.MergeHasError(addResult))
					return result.Build();
			}
		}

		if (0 < requestContent.JsonContents?.Count)
		{
			foreach (var jsonContent in requestContent.JsonContents)
			{
				var adapterRequestPayload = new AdapterRequestPayload
				{
					IdAdapterRequestPayload = Guid.NewGuid(),
					AdapterRequest = adapterRequest,
					RequestContentType = nameof(Legion.Http.JsonContent),
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
					SetDBValidatorRules(new ValidatorBuilder<AdapterRequestPayload>())
						.Build()
						.Validate(adapterRequestPayload);

				if (result.MergeHasError(scopeContext, validationResult, true))
					return result.Build();

				payloads.Add(adapterRequestPayload);
				var addResult = adapterRequest.AddPaylod(scopeContext, adapterRequestPayload);
				if (result.MergeHasError(addResult))
					return result.Build();
			}
		}

		if (0 < requestContent.StreamContents?.Count)
		{
			foreach (var streamContent in requestContent.StreamContents)
			{
				var adapterRequestPayload = new AdapterRequestPayload
				{
					IdAdapterRequestPayload = Guid.NewGuid(),
					AdapterRequest = adapterRequest,
					RequestContentType = nameof(Legion.Http.StreamContent),
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
					SetDBValidatorRules(new ValidatorBuilder<AdapterRequestPayload>())
						.Build()
						.Validate(adapterRequestPayload);

				if (result.MergeHasError(scopeContext, validationResult, true))
					return result.Build();

				payloads.Add(adapterRequestPayload);
				var addResult = adapterRequest.AddPaylod(scopeContext, adapterRequestPayload);
				if (result.MergeHasError(addResult))
					return result.Build();
			}
		}

		if (0 < requestContent.ByteArrayContents?.Count)
		{
			foreach (var byteArrayContent in requestContent.ByteArrayContents)
			{
				var adapterRequestPayload = new AdapterRequestPayload
				{
					IdAdapterRequestPayload = Guid.NewGuid(),
					AdapterRequest = adapterRequest,
					RequestContentType = nameof(Legion.Http.ByteArrayContent),
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
					SetDBValidatorRules(new ValidatorBuilder<AdapterRequestPayload>())
						.Build()
						.Validate(adapterRequestPayload);

				if (result.MergeHasError(scopeContext, validationResult, true))
					return result.Build();

				payloads.Add(adapterRequestPayload);
				var addResult = adapterRequest.AddPaylod(scopeContext, adapterRequestPayload);
				if (result.MergeHasError(addResult))
					return result.Build();
			}
		}

		if (0 < requestContent.HttpContents?.Count)
		{
			foreach (var httpContent in requestContent.HttpContents)
			{
				var adapterRequestPayload = new AdapterRequestPayload
				{
					IdAdapterRequestPayload = Guid.NewGuid(),
					AdapterRequest = adapterRequest,
					RequestContentType = nameof(Legion.Http.HttpContent),
					ByteArrayContent = httpContent.Stream?.ToArray(seek: true),
					JsonContent = null,
					StringContent = null,
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
					SetDBValidatorRules(new ValidatorBuilder<AdapterRequestPayload>())
						.Build()
						.Validate(adapterRequestPayload);

				if (result.MergeHasError(scopeContext, validationResult, true))
					return result.Build();

				payloads.Add(adapterRequestPayload);
				var addResult = adapterRequest.AddPaylod(scopeContext, adapterRequestPayload);
				if (result.MergeHasError(addResult))
					return result.Build();
			}
		}

		return result.WithData(payloads).Build();
	}
}
