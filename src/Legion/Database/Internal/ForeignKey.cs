using Legion.Database.Metamodel;
using Legion.Validation;

namespace Legion.Database.Internal;

public class ForeignKey : IForeignKey, IValidable
{
	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	private Table _table;

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	private Column _fromColumn;

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	private Column _toColumn;

	public string Name { get; set; }
	public string Column { get; set; }
	public string ForeignSchemaAlias { get; set; }
	public string ForeignTableName { get; set; }
	public string ForeignColumnName { get; set; }
	public ReferentialAction? OnUpdateAction { get; set; }
	public ReferentialAction? OnDeleteAction { get; set; }
	public MatchOprions? MatchOption { get; set; }

	public ITable Table => _table;
	public IColumn FromColumn => _fromColumn;
	public IColumn ToColumn => _toColumn;

	private bool built;
	public bool Build(Table table)
	{
		if (built)
			return false;

		Throw.IfArgumentNull(table);

		built = true;

		_table = table;

		return true;
	}

	internal ForeignKey SetFromColumn(Column column)
	{
		Throw.IfArgumentNull(column);

		_fromColumn = column;
		return this;
	}

	internal ForeignKey SetToColumn(Column column)
	{
		Throw.IfArgumentNull(column);

		_toColumn = column;
		return this;
	}

	public static void SetValidatorRules(
		ValidatorBuilder<ForeignKey> builder,
		Dictionary<string, object>? globalValidationState = null,
		Dictionary<string, object>? localValidationState = null)
	{
		builder
			.ForProperty(x => x.Name, v => v.NotDefaultOrWhiteSpace())
			.ForProperty(x => x.Column, v => v.NotDefaultOrWhiteSpace())
			.ForProperty(x => x.ForeignSchemaAlias, v => v.NotDefaultOrWhiteSpace())
			.ForProperty(x => x.ForeignTableName, v => v.NotDefaultOrWhiteSpace())
			.ForProperty(x => x.ForeignColumnName, v => v.NotDefaultOrWhiteSpace())
		;
	}

	public IValidationResult Validate(Dictionary<string, object>? globalValidationState = null)
	{
		var builder = new ValidatorBuilder<ForeignKey>();
		SetValidatorRules(builder, globalValidationState, localValidationState: null);
		var validator = builder.Build();

		return validator.Validate(this);
	}

	public ForeignKey Clone()
		=> new()
		{
			Name = Name,
			Column = Column,
			ForeignSchemaAlias = ForeignSchemaAlias,
			ForeignTableName = ForeignTableName,
			ForeignColumnName = ForeignColumnName,
			OnUpdateAction = OnUpdateAction,
			OnDeleteAction = OnDeleteAction
		};

	public override string? ToString()
	{
		return Name;
	}
}
