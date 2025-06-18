using Legion.Validation;

namespace Legion.ADF.ESB.Components.Model;

public sealed partial class AdapterResponse : Components.ComponentsBaseEntity, Legion.Model.IEntity
{
	private List<Components.Model.AdapterResponsePayload> _adapterResponsePayloads;

	public static IValidator<AdapterResponse> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdAdapterResponse { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | Components.Model.AdapterRequest.AdapterRequest | FK_AdapterResponse_IdAdapterRequest
	/// </summary>
	public Guid IdAdapterRequest { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | Components.Model.Adapter.Adapter | FK_AdapterResponse_IdAdapter
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
	/// Database DataType: integer NULL
	/// </summary>
	public int? StatusCode { get; private set; }

	/// <summary>
	/// Database DataType: jsonb NULL
	/// </summary>
	public string? Headers { get; private set; }

	/// <summary>
	/// Database DataType: varchar(255) NULL
	/// </summary>
	public string? ContentType { get; private set; }

	/// <summary>
	/// Database DataType: text NULL
	/// </summary>
	public string? Error { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL
	/// </summary>
	public Guid? IdLogMessage { get; private set; }

	/// <summary>
	/// Database DataType: numeric NULL
	/// </summary>
	public decimal? ElapsedMilliseconds { get; private set; }


	/// <summary>
	/// _1:N Guid IdAdapter | FK_AdapterResponse_IdAdapter
	/// </summary>
	public Components.Model.Adapter Adapter { get; private set; }

	/// <summary>
	/// _1:N Guid IdAdapterRequest | FK_AdapterResponse_IdAdapterRequest
	/// </summary>
	public Components.Model.AdapterRequest AdapterRequest { get; private set; }


	/// <summary>
	/// N:_1 Components.Model.AdapterResponsePayload.IdAdapterResponse | FK_AdapterResponsePayload_IdAdapterResponse
	/// </summary>
	public IReadOnlyList<Components.Model.AdapterResponsePayload> AdapterResponsePayloads => _adapterResponsePayloads;

	private AdapterResponse()
	{
		_adapterResponsePayloads = [];
	}

	static AdapterResponse()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<AdapterResponse>()).Build();
	}

	public override string? ToString()
	{
		return IdAdapterResponse.ToString();
	}

	public static ValidatorBuilder<AdapterResponse> SetDBValidatorRules(ValidatorBuilder<AdapterResponse> builder)
		=> builder
			.ForProperty(x => x.IdAdapterResponse, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdAdapterRequest, v => v.NotDefaultOrEmpty(), x => x.AdapterRequest == null)
			.ForProperty(x => x.IdAdapter, v => v.NotDefaultOrEmpty(), x => x.Adapter == null)
			//.ForProperty(x => x.CreatedUtc, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.LogCorrelationId, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.ContentType, v => v.MaxLength(255))
		;
}
