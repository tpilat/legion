using Legion.Validation;

namespace Legion.ADF.ESB.Components.Model;

public sealed partial class AdapterStatus : Components.ComponentsBaseEntity, Legion.Model.IEntity
{
	private List<Components.Model.AdapterLog> _adapterLogs;
	private List<Components.Model.Adapter> _adapters;

	public static IValidator<AdapterStatus> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdAdapterStatus { get; private set; }

	/// <summary>
	/// Database DataType: varchar(63) NOT NULL
	/// </summary>
	public string Code { get; private set; }

	/// <summary>
	/// Database DataType: varchar(127) NOT NULL
	/// </summary>
	public string Name { get; private set; }


	/// <summary>
	/// N:_1 Components.Model.AdapterLog.IdAdapterStatus | FK_AdapterLog_IdAdapterStatus
	/// </summary>
	public IReadOnlyList<Components.Model.AdapterLog> AdapterLogs => _adapterLogs;

	/// <summary>
	/// N:_1 Components.Model.Adapter.IdAdapterStatus | FK_Adapter_IdAdapterStatus
	/// </summary>
	public IReadOnlyList<Components.Model.Adapter> Adapters => _adapters;

	private AdapterStatus()
	{
		_adapterLogs = [];
		_adapters = [];
	}

	public override string? ToString()
	{
		return Code;
	}

	public static ValidatorBuilder<AdapterStatus> SetDBValidatorRules(ValidatorBuilder<AdapterStatus> builder)
		=> builder
			.ForProperty(x => x.IdAdapterStatus, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.Code, v => v.NotDefaultOrEmpty().MaxLength(63))
			.ForProperty(x => x.Name, v => v.NotDefaultOrEmpty().MaxLength(127))
		;
}
