using Legion.Validation;

namespace Legion.ADF.ServiceBus.Model;

public sealed partial class HostActivity : ServiceBus.ServiceBusBaseEntity, Legion.Model.Concurrence.IConcurrent, Legion.Model.IEntity
{
	public static IValidator<HostActivity> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdHostActivity { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | ServiceBus.Model.Host.Host | FK_HostActivity_IdHost
	/// </summary>
	public Guid IdHost { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NOT NULL
	/// </summary>
	public DateTime StartedUtc { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NOT NULL
	/// </summary>
	public DateTime LastActivityUtc { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NULL
	/// </summary>
	public DateTime? StoppedUtc { get; private set; }

	/// <summary>
	/// Database DataType: boolean NOT NULL
	/// </summary>
	public bool IsDistributedManagerAvailable { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid RowVersion { get; set; }


	/// <summary>
	/// UNIQUE INDEX: UQ_HostActivity_IdHost
	/// _1:1 Guid IdHost | FK_HostActivity_IdHost
	/// </summary>
	public ServiceBus.Model.Host Host { get; private set; }

	private HostActivity()
	{
	}

	[System.ComponentModel.DataAnnotations.Schema.NotMapped]
	string Legion.Model.Concurrence.IConcurrent.ConcurrencyTokenPropertyName => nameof(RowVersion);

	public void SetNewConcurrencyToken()
	{
		RowVersion = Legion.GlobalContext.Instance.NewGuid();
	}

	static HostActivity()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<HostActivity>()).Build();
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdHostActivity), IdHostActivity },
			{ nameof(IdHost), IdHost },
			{ nameof(StartedUtc), StartedUtc },
			{ nameof(LastActivityUtc), LastActivityUtc },
			{ nameof(StoppedUtc), StoppedUtc },
			{ nameof(IsDistributedManagerAvailable), IsDistributedManagerAvailable },
			{ nameof(RowVersion), RowVersion },
		};

	public void TrimStringValuesToFitDatabaseMaxLengths(string? postfix = null)
	{
	}

	public override string? GetPrimaryKeyValue()
	{
		return IdHostActivity.ToString();
	}

	public override string? ToString()
	{
		return IdHostActivity.ToString();
	}

	public static ValidatorBuilder<HostActivity> SetDBValidatorRules(ValidatorBuilder<HostActivity> builder)
		=> builder
			.ForProperty(x => x.IdHostActivity, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdHost, v => v.NotDefaultOrEmpty(), (x, parent) => x.Host == null)
			//.ForProperty(x => x.StartedUtc, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.LastActivityUtc, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.RowVersion, v => v.NotDefaultOrEmpty())
		;
}
