namespace Legion.Database.Metamodel;

public interface IColumn
{
	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	ITable Table { get; }

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	IView View { get; }

	string Name { get; }
	string DatabaseType { get; }
	bool IsNotNull { get; }
	string? DefaultValue { get; }
	int CharacterMaximumLength { get; }
	int? Precision { get; }
	int? Scale { get; }
	bool IsIdentity { get; }
	long? IdentityStart { get; }
	long? IdentityIncrement { get; }
	long? LastIdentity { get; }
	string? ComputedColumnSql { get; }
	bool IsSingleUniqueConstraint { get; }
	bool IsMultiUniqueConstraint { get; }

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	IPrimaryKey? PrimaryKey { get; }

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	IEnumerable<IUniqueConstraint> UniqueConstraints { get; }

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	IEnumerable<IIndex> Indexes { get; }

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	IForeignKey? TargetForeignKey { get; }

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	IEnumerable<IForeignKey>? SourceForeignKeys { get; }
}
