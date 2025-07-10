using Legion.Validation;

namespace Legion.ADF.ServiceBus.Model;

public sealed partial class HostLog : ServiceBus.ServiceBusBaseEntity, Legion.Model.IEntity
{
	public static IValidator<HostLog> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdHostLog { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | ServiceBus.Model.Host.Host | FK_HostLog_IdHost
	/// </summary>
	public Guid IdHost { get; private set; }

	/// <summary>
	/// Database DataType: integer NOT NULL
	/// </summary>
	public int IdLogLevel { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NOT NULL
	/// </summary>
	public DateTime CreatedUtc { get; private set; }

	/// <summary>
	/// Database DataType: boolean NOT NULL
	/// </summary>
	public bool IsRunning { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid TraceCorrelationId { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL
	/// </summary>
	public Guid? IdLogMessage { get; private set; }

	/// <summary>
	/// Database DataType: varchar(127) NOT NULL
	/// </summary>
	public string Code { get; private set; }

	/// <summary>
	/// Database DataType: text NULL
	/// </summary>
	public string? Detail { get; private set; }


	/// <summary>
	/// _1:N Guid IdHost | FK_HostLog_IdHost
	/// </summary>
	public ServiceBus.Model.Host Host { get; private set; }

	private HostLog()
	{
	}

	static HostLog()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<HostLog>()).Build();
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdHostLog), IdHostLog },
			{ nameof(IdHost), IdHost },
			{ nameof(IdLogLevel), IdLogLevel },
			{ nameof(CreatedUtc), CreatedUtc },
			{ nameof(IsRunning), IsRunning },
			{ nameof(TraceCorrelationId), TraceCorrelationId },
			{ nameof(IdLogMessage), IdLogMessage },
			{ nameof(Code), Code },
			{ nameof(Detail), Detail },
		};

	public void TrimStringValuesToFitDatabaseMaxLengths(string? postfix = null)
	{
		Code = Legion.Text.StringHelper.TrimToFitMaxLength(Code, 127, postfix);
	}

	public override string? GetPrimaryKeyValue()
	{
		return IdHostLog.ToString();
	}

	public override string? ToString()
	{
		return IdHostLog.ToString();
	}

	public static ValidatorBuilder<HostLog> SetDBValidatorRules(ValidatorBuilder<HostLog> builder)
		=> builder
			.ForProperty(x => x.IdHostLog, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdHost, v => v.NotDefaultOrEmpty(), (x, parent) => x.Host == null)
			//.ForProperty(x => x.IdLogLevel, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.CreatedUtc, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.TraceCorrelationId, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.Code, v => v.NotDefaultOrEmpty().MaxLength(127))
		;
}
