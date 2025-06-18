using Legion.Validation;

namespace Legion.ADF.Config.Model;

public sealed partial class ConfigurationKeyValue : Config.ConfigBaseEntity, Legion.Model.Audit.ISelfAuditableEntity, Legion.Model.Concurrence.IConcurrent, Legion.Model.Audit.IAuditableEntity, Legion.Model.IEntity
{
	public static IValidator<ConfigurationKeyValue> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdConfigurationKeyValue { get; private set; }

	/// <summary>
	/// Database DataType: text NOT NULL
	/// </summary>
	public string Key { get; private set; }

	/// <summary>
	/// Database DataType: text NULL
	/// </summary>
	public string? Value { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NOT NULL
	/// </summary>
	public DateTime AuditCreatedUtc { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NULL
	/// </summary>
	public DateTime? AuditModifiedUtc { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL
	/// </summary>
	public Guid? IdAuditCreatedBy { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL
	/// </summary>
	public Guid? IdAuditModifiedBy { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid ConcurrencyToken { get; set; }

	private ConfigurationKeyValue()
	{
	}

	void Legion.Model.Audit.ISelfAuditableEntity.SetAuditCreated(DateTime auditCreatedUtc, Guid? idAuditCreatedBy)
	{
		AuditCreatedUtc = auditCreatedUtc;
		IdAuditCreatedBy = idAuditCreatedBy;
	}

	void Legion.Model.Audit.ISelfAuditableEntity.SetAuditModified(DateTime auditModifiedUtc, Guid? idAuditModifiedBy)
	{
		AuditModifiedUtc = auditModifiedUtc;
		IdAuditModifiedBy = idAuditModifiedBy;
	}

	[System.ComponentModel.DataAnnotations.Schema.NotMapped]
	string Legion.Model.Concurrence.IConcurrent.ConcurrencyTokenPropertyName => nameof(ConcurrencyToken);

	public void SetNewConcurrencyToken()
	{
		ConcurrencyToken = Guid.NewGuid();
	}

	static ConfigurationKeyValue()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<ConfigurationKeyValue>()).Build();
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdConfigurationKeyValue), IdConfigurationKeyValue },
			{ nameof(Key), Key },
			{ nameof(Value), Value },
			{ nameof(AuditCreatedUtc), AuditCreatedUtc },
			{ nameof(AuditModifiedUtc), AuditModifiedUtc },
			{ nameof(IdAuditCreatedBy), IdAuditCreatedBy },
			{ nameof(IdAuditModifiedBy), IdAuditModifiedBy },
			{ nameof(ConcurrencyToken), ConcurrencyToken },
		};

	public void TrimStringValuesToFitDatabaseMaxLengths(string? postfix = null)
	{
	}

	public override string? GetPrimaryKeyValue()
	{
		return IdConfigurationKeyValue.ToString();
	}

	public override string? ToString()
	{
		return Key;
	}

	public static ValidatorBuilder<ConfigurationKeyValue> SetDBValidatorRules(ValidatorBuilder<ConfigurationKeyValue> builder)
		=> builder
			.ForProperty(x => x.IdConfigurationKeyValue, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.Key, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.AuditCreatedUtc, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.ConcurrencyToken, v => v.NotDefaultOrEmpty())
		;
}
