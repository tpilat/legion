using Legion.Http;
using Legion.Validation;
using Legion.Web.Logging;

namespace Legion.ADF.ESB.Components.Model;

public sealed partial class AdapterResponse : Components.ComponentsBaseEntity, Legion.Model.IEntity
{
	public static async Task<IResult<AdapterResponse>> CreateAdapterResponseAsync(
		IScopeContext scopeContext,
		Guid idAdapterRequest,
		Guid idAdapter,
		ResponseDto response,
		HttpContentDto responseContent,
		string clientName,
		bool logPayload)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<AdapterResponse>();

		if (result.IsArgumentNull(scopeContext, response))
			return result.Build();

		if (result.IsArgumentNull(scopeContext, responseContent))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, clientName))
			return result.Build();

		var adapterResponse = new AdapterResponse
		{
			IdAdapterResponse = Guid.NewGuid(),
			IdAdapterRequest = idAdapterRequest,
			IdAdapter = idAdapter,
			CreatedUtc = GlobalContext.Instance.UtcNow,
			LogCorrelationId = scopeContext.CorrelationId ?? Guid.NewGuid(),
			Properties = scopeContext.ContextPropertiesToJson(),
			StatusCode = response.StatusCode,
			Headers = response.Headers,
			ContentType = response.ContentType,
			Error = response.Error,
			IdLogMessage = null,
			ElapsedMilliseconds = response.ElapsedMilliseconds
		};

		var validationResult =
			SetDBValidatorRules(new ValidatorBuilder<AdapterResponse>())
				.Build()
				.Validate(adapterResponse);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		if (logPayload)
		{
			var adapterResponsePayloadResult = await Components.Model.AdapterResponsePayload.CreateAdapterResponsePayloadChunksAsync(
				scopeContext.CreateNew(),
				adapterResponse,
				response!,
				responseContent,
				clientName);

			if (result.MergeHasError(adapterResponsePayloadResult))
				return result.Build();
		}
		return result.WithData(adapterResponse).Build();
	}

	public IResult AddPaylod(IScopeContext scopeContext, AdapterResponsePayload adapterResponsePayload)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<AdapterResponse>();

		if (result.IsArgumentNull(scopeContext, adapterResponsePayload))
			return result.Build();

		_adapterResponsePayloads.Add(adapterResponsePayload);

		return result.Build();
	}
}
