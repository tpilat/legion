using Legion.Validation;

namespace Legion.ADF.ESB.Components.Model;

public sealed partial class Adapter : Components.ComponentsBaseEntity, Legion.Model.IEntity
{
	private List<Components.Model.AdapterLog> _adapterLogs;
	private List<Components.Model.AdapterRequest> _adapterRequests;
	private List<Components.Model.AdapterResponse> _adapterResponses;

	public static IValidator<Adapter> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdAdapter { get; private set; }

	/// <summary>
	/// Database DataType: varchar(127) NOT NULL
	/// </summary>
	public string Code { get; private set; }

	/// <summary>
	/// Database DataType: varchar(127) NOT NULL
	/// </summary>
	public string Name { get; private set; }

	/// <summary>
	/// Database DataType: varchar(1023) NULL
	/// </summary>
	public string? Description { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | Components.Model.AdapterStatus.AdapterStatus | FK_Adapter_IdAdapterStatus
	/// </summary>
	public Guid IdAdapterStatus { get; private set; }

	/// <summary>
	/// Database DataType: varchar(2047) NOT NULL
	/// </summary>
	public string Class { get; private set; }

	/// <summary>
	/// Database DataType: jsonb NULL
	/// </summary>
	public string? Properties { get; private set; }

	/// <summary>
	/// Database DataType: boolean NOT NULL
	/// </summary>
	public bool IsInbound { get; private set; }

	/// <summary>
	/// Database DataType: boolean NOT NULL
	/// </summary>
	public bool IsOutbound { get; private set; }


	/// <summary>
	/// _1:N Guid IdAdapterStatus | FK_Adapter_IdAdapterStatus
	/// </summary>
	public Components.Model.AdapterStatus AdapterStatus { get; private set; }


	/// <summary>
	/// N:_1 Components.Model.AdapterLog.IdAdapter | FK_AdapterLog_IdAdapter
	/// </summary>
	public IReadOnlyList<Components.Model.AdapterLog> AdapterLogs => _adapterLogs;

	/// <summary>
	/// N:_1 Components.Model.AdapterRequest.IdAdapter | FK_AdapterRequest_IdAdapter
	/// </summary>
	public IReadOnlyList<Components.Model.AdapterRequest> AdapterRequests => _adapterRequests;

	/// <summary>
	/// N:_1 Components.Model.AdapterResponse.IdAdapter | FK_AdapterResponse_IdAdapter
	/// </summary>
	public IReadOnlyList<Components.Model.AdapterResponse> AdapterResponses => _adapterResponses;

	private Adapter()
	{
		_adapterLogs = [];
		_adapterRequests = [];
		_adapterResponses = [];
	}

	static Adapter()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<Adapter>()).Build();
	}

	public override string? ToString()
	{
		return IdAdapter.ToString();
	}

	public static ValidatorBuilder<Adapter> SetDBValidatorRules(ValidatorBuilder<Adapter> builder)
		=> builder
			.ForProperty(x => x.IdAdapter, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.Code, v => v.NotDefaultOrEmpty().MaxLength(127))
			.ForProperty(x => x.Name, v => v.NotDefaultOrEmpty().MaxLength(127))
			.ForProperty(x => x.Description, v => v.MaxLength(1023))
			.ForProperty(x => x.IdAdapterStatus, v => v.NotDefaultOrEmpty(), x => x.AdapterStatus == null)
			.ForProperty(x => x.Class, v => v.NotDefaultOrEmpty().MaxLength(2047))
		;
}
