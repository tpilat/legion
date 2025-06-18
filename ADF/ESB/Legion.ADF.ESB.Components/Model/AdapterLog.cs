using Legion.Validation;

namespace Legion.ADF.ESB.Components.Model;

public sealed partial class AdapterLog : Components.ComponentsBaseEntity, Legion.Model.IEntity
{
	public static IValidator<AdapterLog> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdAdapterLog { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | Components.Model.Adapter.Adapter | FK_AdapterLog_IdAdapter
	/// </summary>
	public Guid IdAdapter { get; private set; }

	/// <summary>
	/// Database DataType: timestamp without time zone NOT NULL
	/// </summary>
	public DateTime CreatedUtc { get; private set; }

	/// <summary>
	/// Database DataType: integer NOT NULL
	/// </summary>
	public int IdLogLevel { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid LogCorrelationId { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | Components.Model.AdapterStatus.AdapterStatus | FK_AdapterLog_IdAdapterStatus
	/// </summary>
	public Guid IdAdapterStatus { get; private set; }

	/// <summary>
	/// Database DataType: text NOT NULL
	/// </summary>
	public string Detail { get; private set; }

	/// <summary>
	/// Database DataType: jsonb NULL
	/// </summary>
	public string? Data { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL
	/// </summary>
	public Guid? IdLogMessage { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL | --NO TARGET-- | FK_AdapterLog_IdMessageProcessingLog
	/// </summary>
	public Guid? IdMessageProcessingLog { get; private set; }


	/// <summary>
	/// _1:N Guid IdAdapter | FK_AdapterLog_IdAdapter
	/// </summary>
	public Components.Model.Adapter Adapter { get; private set; }

	/// <summary>
	/// _1:N Guid IdAdapterStatus | FK_AdapterLog_IdAdapterStatus
	/// </summary>
	public Components.Model.AdapterStatus AdapterStatus { get; private set; }

	private AdapterLog()
	{
	}

	static AdapterLog()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<AdapterLog>()).Build();
	}

	public override string? ToString()
	{
		return IdAdapterLog.ToString();
	}

	public static ValidatorBuilder<AdapterLog> SetDBValidatorRules(ValidatorBuilder<AdapterLog> builder)
		=> builder
			.ForProperty(x => x.IdAdapterLog, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdAdapter, v => v.NotDefaultOrEmpty(), x => x.Adapter == null)
			//.ForProperty(x => x.CreatedUtc, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.IdLogLevel, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.LogCorrelationId, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdAdapterStatus, v => v.NotDefaultOrEmpty(), x => x.AdapterStatus == null)
			.ForProperty(x => x.Detail, v => v.NotDefaultOrEmpty())
		;
}
