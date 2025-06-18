using Legion.Validation;

namespace Legion.ADF.Config.Model;

public sealed partial class ConfigurationClass : Config.ConfigBaseEntity, Legion.Model.IEntity
{
	public static IValidator<ConfigurationClass> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdConfigurationClass { get; private set; }

	/// <summary>
	/// Database DataType: text NOT NULL
	/// </summary>
	public string RootPath { get; private set; }

	/// <summary>
	/// Database DataType: varchar(255) NOT NULL
	/// </summary>
	public string DisplayName { get; private set; }

	/// <summary>
	/// Database DataType: text NULL
	/// </summary>
	public string? Class { get; private set; }

	private ConfigurationClass()
	{
	}

	static ConfigurationClass()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<ConfigurationClass>()).Build();
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdConfigurationClass), IdConfigurationClass },
			{ nameof(RootPath), RootPath },
			{ nameof(DisplayName), DisplayName },
			{ nameof(Class), Class },
		};

	public void TrimStringValuesToFitDatabaseMaxLengths(string? postfix = null)
	{
		DisplayName = Legion.Text.StringHelper.TrimToFitMaxLength(DisplayName, 255, postfix);
	}

	public override string? GetPrimaryKeyValue()
	{
		return IdConfigurationClass.ToString();
	}

	public override string? ToString()
	{
		return IdConfigurationClass.ToString();
	}

	public static ValidatorBuilder<ConfigurationClass> SetDBValidatorRules(ValidatorBuilder<ConfigurationClass> builder)
		=> builder
			.ForProperty(x => x.IdConfigurationClass, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.RootPath, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.DisplayName, v => v.NotDefaultOrEmpty().MaxLength(255))
		;
}
