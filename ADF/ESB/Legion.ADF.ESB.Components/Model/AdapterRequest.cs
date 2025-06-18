using Legion.Validation;

namespace Legion.ADF.ESB.Components.Model;

public sealed partial class AdapterRequest : Components.ComponentsBaseEntity, Legion.Model.IEntity
{
	private List<Components.Model.AdapterRequestPayload> _adapterRequestPayloads;
	private List<Components.Model.AdapterResponse> _adapterResponses;

	public static IValidator<AdapterRequest> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdAdapterRequest { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | Components.Model.Adapter.Adapter | FK_AdapterRequest_IdAdapter
	/// </summary>
	public Guid IdAdapter { get; private set; }

	/// <summary>
	/// Database DataType: timestamp without time zone NOT NULL
	/// </summary>
	public DateTime CreatedUtc { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid LogCorrelationId { get; private set; }

	/// <summary>
	/// Database DataType: jsonb NULL
	/// </summary>
	public string? Properties { get; private set; }

	/// <summary>
	/// Database DataType: varchar(127) NOT NULL
	/// </summary>
	public string Identifier { get; private set; }

	/// <summary>
	/// Database DataType: varchar(2047) NOT NULL
	/// </summary>
	public string Url { get; private set; }

	/// <summary>
	/// Database DataType: varchar(15) NULL
	/// </summary>
	public string? Method { get; private set; }

	/// <summary>
	/// Database DataType: jsonb NULL
	/// </summary>
	public string? Headers { get; private set; }

	/// <summary>
	/// Database DataType: varchar(255) NULL
	/// </summary>
	public string? ContentType { get; private set; }


	/// <summary>
	/// _1:N Guid IdAdapter | FK_AdapterRequest_IdAdapter
	/// </summary>
	public Components.Model.Adapter Adapter { get; private set; }


	/// <summary>
	/// N:_1 Components.Model.AdapterRequestPayload.IdAdapterRequest | FK_AdapterRequestPayload_IdAdapterRequest
	/// </summary>
	public IReadOnlyList<Components.Model.AdapterRequestPayload> AdapterRequestPayloads => _adapterRequestPayloads;

	/// <summary>
	/// N:_1 Components.Model.AdapterResponse.IdAdapterRequest | FK_AdapterResponse_IdAdapterRequest
	/// </summary>
	public IReadOnlyList<Components.Model.AdapterResponse> AdapterResponses => _adapterResponses;

	private AdapterRequest()
	{
		_adapterRequestPayloads = [];
		_adapterResponses = [];
	}

	static AdapterRequest()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<AdapterRequest>()).Build();
	}

	public override string? ToString()
	{
		return IdAdapterRequest.ToString();
	}

	public static ValidatorBuilder<AdapterRequest> SetDBValidatorRules(ValidatorBuilder<AdapterRequest> builder)
		=> builder
			.ForProperty(x => x.IdAdapterRequest, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdAdapter, v => v.NotDefaultOrEmpty(), x => x.Adapter == null)
			//.ForProperty(x => x.CreatedUtc, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.LogCorrelationId, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.Identifier, v => v.NotDefaultOrEmpty().MaxLength(127))
			.ForProperty(x => x.Url, v => v.NotDefaultOrEmpty().MaxLength(2047))
			.ForProperty(x => x.Method, v => v.MaxLength(15))
			.ForProperty(x => x.ContentType, v => v.MaxLength(255))
		;
}
