using Legion.Validation;

namespace Legion.ADF.Audit.Model;

public sealed partial class ApplicationEntryToken : Audit.AuditBaseEntity, Legion.Model.IEntity
{
	private List<Audit.Model.ApplicationEntry> _applicationEntries;

	public static IValidator<ApplicationEntryToken> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdApplicationEntryToken { get; private set; }

	/// <summary>
	/// Database DataType: varchar(255) NOT NULL
	/// </summary>
	public string Token { get; private set; }

	/// <summary>
	/// Database DataType: varchar(511) NOT NULL
	/// </summary>
	public string SourceFilePath { get; private set; }

	/// <summary>
	/// Database DataType: varchar(511) NULL
	/// </summary>
	public string? MethodInfo { get; private set; }

	/// <summary>
	/// Database DataType: varchar(255) NULL
	/// </summary>
	public string? AggregateName { get; private set; }

	/// <summary>
	/// Database DataType: varchar(511) NULL
	/// </summary>
	public string? Description { get; private set; }


	/// <summary>
	/// N:_1 Audit.Model.ApplicationEntry.IdApplicationEntryToken | FK_ApplicationEntry_IdApplicationEntryToken
	/// </summary>
	public IReadOnlyList<Audit.Model.ApplicationEntry> ApplicationEntries => _applicationEntries;

	private ApplicationEntryToken()
	{
		_applicationEntries = [];
	}

	static ApplicationEntryToken()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<ApplicationEntryToken>()).Build();
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdApplicationEntryToken), IdApplicationEntryToken },
			{ nameof(Token), Token },
			{ nameof(SourceFilePath), SourceFilePath },
			{ nameof(MethodInfo), MethodInfo },
			{ nameof(AggregateName), AggregateName },
			{ nameof(Description), Description },
		};

	public void TrimStringValuesToFitDatabaseMaxLengths(string? postfix = null)
	{
		Token = Legion.Text.StringHelper.TrimToFitMaxLength(Token, 255, postfix);
		SourceFilePath = Legion.Text.StringHelper.TrimToFitMaxLength(SourceFilePath, 511, postfix);
		MethodInfo = Legion.Text.StringHelper.TrimToFitMaxLength(MethodInfo, 511, postfix);
		AggregateName = Legion.Text.StringHelper.TrimToFitMaxLength(AggregateName, 255, postfix);
		Description = Legion.Text.StringHelper.TrimToFitMaxLength(Description, 511, postfix);
	}

	public override string? GetPrimaryKeyValue()
	{
		return IdApplicationEntryToken.ToString();
	}

	public override string? ToString()
	{
		return IdApplicationEntryToken.ToString();
	}

	public static ValidatorBuilder<ApplicationEntryToken> SetDBValidatorRules(ValidatorBuilder<ApplicationEntryToken> builder)
		=> builder
			.ForProperty(x => x.IdApplicationEntryToken, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.Token, v => v.NotDefaultOrEmpty().MaxLength(255))
			.ForProperty(x => x.SourceFilePath, v => v.NotDefaultOrEmpty().MaxLength(511))
			.ForProperty(x => x.MethodInfo, v => v.MaxLength(511))
			.ForProperty(x => x.AggregateName, v => v.MaxLength(255))
			.ForProperty(x => x.Description, v => v.MaxLength(511))
		;
}
