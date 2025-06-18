using Legion.Http;
using Legion.Validation;
using Legion.Web.Logging;

namespace Legion.ADF.ESB.Components.Model;

public sealed partial class AdapterRequest : Components.ComponentsBaseEntity, Legion.Model.IEntity
{
	public  static async Task<IResult<AdapterRequest>> CreateAdapterRequestAsync(
		IScopeContext scopeContext,
		Guid idAdapter,
		RequestDto request,
		HttpContentDto requestContent,
		string clientName,
		bool logPayload)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<AdapterRequest>();

		if (result.IsArgumentNull(scopeContext, request))
			return result.Build();

		if (result.IsArgumentNull(scopeContext, requestContent))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, clientName))
			return result.Build();

		var adapterRequest = new AdapterRequest
		{
			IdAdapterRequest = Guid.NewGuid(),
			IdAdapter = idAdapter,
			CreatedUtc = GlobalContext.Instance.UtcNow,
			LogCorrelationId = scopeContext.CorrelationId ?? Guid.NewGuid(),
			Properties = scopeContext.ContextPropertiesToJson(),
			Identifier = clientName,
			Url = request.Path ?? clientName,
			Method = request.Method,
			Headers = request.Headers,
			ContentType = request.ContentType
		};

		var validationResult =
			SetDBValidatorRules(new ValidatorBuilder<AdapterRequest>())
				.Build()
				.Validate(adapterRequest);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		if (logPayload)
		{
			var adapterRequestPayloadResult = await Components.Model.AdapterRequestPayload.CreateAdapterRequestPayloadChunksAsync(
				scopeContext.CreateNew(),
				adapterRequest,
				request!,
				requestContent,
				clientName);

			if (result.MergeHasError(adapterRequestPayloadResult))
				return result.Build();
		}
		return result.WithData(adapterRequest).Build();
	}

	public IResult AddPaylod(IScopeContext scopeContext, AdapterRequestPayload adapterRequestPayload)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<AdapterRequest>();

		if (result.IsArgumentNull(scopeContext, adapterRequestPayload))
			return result.Build();

		_adapterRequestPayloads.Add(adapterRequestPayload);

		return result.Build();
	}
}
