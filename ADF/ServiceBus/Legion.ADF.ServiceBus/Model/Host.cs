using Legion.Validation;

namespace Legion.ADF.ServiceBus.Model;

public sealed partial class Host : ServiceBus.ServiceBusBaseEntity, Legion.Model.Concurrence.IConcurrent, Legion.Model.IEntity
{
	private List<ServiceBus.Model.HostLog> _hostLogs;

	public static IValidator<Host> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdHost { get; private set; }

	/// <summary>
	/// Database DataType: varchar(255) NOT NULL
	/// </summary>
	public string Name { get; private set; }

	/// <summary>
	/// Database DataType: varchar(511) NOT NULL
	/// </summary>
	public string Description { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NOT NULL
	/// </summary>
	public DateTime CreatedUtc { get; private set; }

	/// <summary>
	/// Database DataType: boolean NOT NULL
	/// </summary>
	public bool IsEnabled { get; private set; }

	/// <summary>
	/// Database DataType: jsonb NOT NULL
	/// </summary>
	public string Configuration { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid RowVersion { get; set; }


	/// <summary>
	/// 1:_1 HostActivity.IdHost | FK_HostActivity_IdHost
	/// </summary>
	public ServiceBus.Model.HostActivity HostActivity { get; private set; }

	/// <summary>
	/// N:_1 ServiceBus.Model.HostLog.IdHost | FK_HostLog_IdHost
	/// </summary>
	public IReadOnlyList<ServiceBus.Model.HostLog> HostLogs => _hostLogs;

	private Host()
	{
		_hostLogs = [];
	}

	[System.ComponentModel.DataAnnotations.Schema.NotMapped]
	string Legion.Model.Concurrence.IConcurrent.ConcurrencyTokenPropertyName => nameof(RowVersion);

	public void SetNewConcurrencyToken()
	{
		RowVersion = Legion.GlobalContext.Instance.NewGuid();
	}

	static Host()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<Host>()).Build();
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdHost), IdHost },
			{ nameof(Name), Name },
			{ nameof(Description), Description },
			{ nameof(CreatedUtc), CreatedUtc },
			{ nameof(IsEnabled), IsEnabled },
			{ nameof(Configuration), Configuration },
			{ nameof(RowVersion), RowVersion },
		};

	public void TrimStringValuesToFitDatabaseMaxLengths(string? postfix = null)
	{
		Name = Legion.Text.StringHelper.TrimToFitMaxLength(Name, 255, postfix);
		Description = Legion.Text.StringHelper.TrimToFitMaxLength(Description, 511, postfix);
	}

	public override string? GetPrimaryKeyValue()
	{
		return IdHost.ToString();
	}

	public override string? ToString()
	{
		return IdHost.ToString();
	}

	public static ValidatorBuilder<Host> SetDBValidatorRules(ValidatorBuilder<Host> builder)
		=> builder
			.ForProperty(x => x.IdHost, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.Name, v => v.NotDefaultOrEmpty().MaxLength(255))
			.ForProperty(x => x.Description, v => v.NotDefaultOrEmpty().MaxLength(511))
			//.ForProperty(x => x.CreatedUtc, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.Configuration, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.RowVersion, v => v.NotDefaultOrEmpty())
		;
}
