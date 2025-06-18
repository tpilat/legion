using Legion.Database.Internal;
using Legion.Database.Metamodel;
using Legion.Database.PostgreSQL;
using Legion.Database.SqlServer;
using Legion.Extensions;
using Legion.Text;

namespace Legion.Generators.AppGen.Model.Internal.DB;

internal class DBEntityMapper
{
	public DBAbstractions.Metamodel.Entity Entity { get; }
	
	public Dictionary<IColumn, DBAbstractions.Metamodel.Property> Properties { get; set; }

	public Dictionary<ForeignKey, DBAbstractions.Metamodel.Navigation> Navigations { get; set; }

	public Dictionary<ForeignKey, DBAbstractions.Metamodel.ForeignKey> ForeignKeys { get; set; }

	public DBEntityMapper(Table table, DBAbstractions.Metamodel.Model model)
	{
		Entity = new DBAbstractions.Metamodel.Entity
		{
			Schema = table.Schema.Name,
			TableName = table.Name,
			IsDbView = false,
			Model = model
		};

		Properties = [];
		Navigations = [];
		ForeignKeys = [];
	}

	public DBEntityMapper(View view, DBAbstractions.Metamodel.Model model)
	{
		Entity = new DBAbstractions.Metamodel.Entity
		{
			Schema = view.Schema.Name,
			TableName = view.Name,
			IsDbView = true,
			Model = model
		};

		Properties = [];
		Navigations = [];
		ForeignKeys = [];
	}

	public DBAbstractions.Metamodel.Property AddProperty(Column column, DatabaseProviderType providerType)
	{
		Throw.IfArgumentNull(column);

		bool isNullable = !column.IsPrimaryKey && (!column.IsNotNull || !string.IsNullOrWhiteSpace(column.DefaultValue));

		var property = new DBAbstractions.Metamodel.Property
		{
			ClrType = isNullable ? column.CsharpType.ToNullable() : column.CsharpType,
			ColumnName = column.Name,
			Name = GetPropertyOrNavigationUniqueName(column.Name.ToCammelCase()),
			ColumnOrdinal = column.OrdinalPosition,
			ColumnType = column.DatabaseType,
			ComputedColumnSql = column.ComputedColumnSql,
			ConfiguredColumnType = "",
			DeclaringEntity = Entity,
			DefaultValue = null,
			DefaultValueSql = column.DefaultValue,
			HasValueGeneratedMetohdName = column.IsPrimaryKey && !column.IsIdentity && string.IsNullOrWhiteSpace(column.DefaultValue),
			IsConcurrencyToken = false, //TODO
			IsNullable = isNullable,
			IsPrimaryKey = column.IsPrimaryKey,
			IsUnicode = null,
			MaxLength = 0 < column.CharacterMaximumLength ? column.CharacterMaximumLength : (int?)null,
			Precision = column.Precision,
			Scale = column.Scale,
			IsSingleUniqueConstraint = column.IsSingleUniqueConstraint,
			IsMultiUniqueConstraint = column.IsMultiUniqueConstraint,
			IsIdentity = column.IsIdentity,
			IdentityStart = column.IdentityStart,
			IdentityIncrement = column.IdentityIncrement,
			LastIdentity = column.LastIdentity,
			ValueGenerated = column.ValueGenerated,
			Namespaces = [nameof(System)]
		};

		var storeType = column.DatabaseType.ToLower();

		if (providerType == DatabaseProviderType.PostgreSQL)
		{
			if (storeType == PostgreSQLOriginalDataTypes._timestamp_without_time_zone)
				property.ConfiguredColumnType = "timestamp";

			else if (storeType == PostgreSQLOriginalDataTypes._timestamp_with_time_zone)
				property.ConfiguredColumnType = "timestamptz";

			else if (storeType == PostgreSQLOriginalDataTypes._numeric && 0 < property.Precision)
				property.ConfiguredColumnType = $"numeric({property.Precision}, {property.Scale})";

			else if (storeType == PostgreSQLOriginalDataTypes._character_varying && 0 < property.MaxLength)
				property.ConfiguredColumnType = $"varchar({property.MaxLength})";

			else if (storeType == PostgreSQLOriginalDataTypes._character && 0 < property.MaxLength)
				property.ConfiguredColumnType = $"char({property.MaxLength})";

			else if (storeType == PostgreSQLOriginalDataTypes._text_array)
				property.ConfiguredColumnType = "text[]";

			else if (storeType == PostgreSQLOriginalDataTypes._interval)
				property.ConfiguredColumnType = "interval";

			else if (storeType == PostgreSQLOriginalDataTypes._jsonb
				|| storeType == PostgreSQLOriginalDataTypes._date
				|| storeType == PostgreSQLOriginalDataTypes._bytea
				|| storeType == PostgreSQLOriginalDataTypes._uuid)
			{
				property.ConfiguredColumnType = storeType;
			}
		}
		else if (providerType == DatabaseProviderType.SqlServer)
		{
			if (storeType == SqlServerOriginalDataTypes._datetime2 && 0 < property.Scale)
				property.ConfiguredColumnType = $"datetime2({property.Scale})";

			if (storeType == SqlServerOriginalDataTypes._time && 0 < property.Scale)
				property.ConfiguredColumnType = $"time({property.Scale})";

			if (storeType == SqlServerOriginalDataTypes._decimal && 0 < property.Precision)
				property.ConfiguredColumnType = $"decimal({property.Precision}, {property.Scale})";

			if (storeType == SqlServerOriginalDataTypes._numeric && 0 < property.Precision)
				property.ConfiguredColumnType = $"numeric({property.Precision}, {property.Scale})";

			if (storeType == SqlServerOriginalDataTypes._varchar)
			{
				if (0 < property.MaxLength)
				{
					property.ConfiguredColumnType = $"varchar({property.MaxLength})";
				}
				else
				{
					property.ConfiguredColumnType = "varchar(max)";
				}
			}

			if (storeType == SqlServerOriginalDataTypes._char && 0 < property.MaxLength)
				property.ConfiguredColumnType = $"char({property.MaxLength})";

			if (storeType == SqlServerOriginalDataTypes._text && 0 < property.MaxLength)
				property.ConfiguredColumnType = $"text({property.MaxLength})";

			if (storeType == SqlServerOriginalDataTypes._nvarchar)
			{
				if (0 < property.MaxLength)
				{
					property.ConfiguredColumnType = $"nvarchar({property.MaxLength})";
				}
				else
				{
					property.ConfiguredColumnType = "nvarchar(max)";
				}
			}

			if (storeType == SqlServerOriginalDataTypes._nchar && 0 < property.MaxLength)
				property.ConfiguredColumnType = $"nchar({property.MaxLength})";

			if (storeType == SqlServerOriginalDataTypes._ntext && 0 < property.MaxLength)
				property.ConfiguredColumnType = $"ntext({property.MaxLength})";

			if (storeType == SqlServerOriginalDataTypes._varbinary)
			{
				if (0 < property.MaxLength)
				{
					property.ConfiguredColumnType = $"varbinary({property.MaxLength})";
				}
				else
				{
					property.ConfiguredColumnType = "varbinary(max)";
				}
			}

			else if (storeType == SqlServerOriginalDataTypes._datetime
				|| storeType == SqlServerOriginalDataTypes._date
				|| storeType == SqlServerOriginalDataTypes._image
				|| storeType == SqlServerOriginalDataTypes._uniqueidentifier
				|| storeType == SqlServerOriginalDataTypes._bit
				|| storeType == SqlServerOriginalDataTypes._xml)
			{
				property.ConfiguredColumnType = storeType;
			}
		}

		var valueGenerated = property.ValueGenerated;
		var isRowVersion = false;
		if (property.HasValueGeneratedMetohdName)
		{
			switch (valueGenerated)
			{
				case ValueGenerated.OnAdd:
					property.ValueGeneratedMetohdName = "ValueGeneratedOnAdd";
					break;

				case ValueGenerated.OnAddOrUpdate:
					isRowVersion = property.IsConcurrencyToken;
					property.ValueGeneratedMetohdName = isRowVersion
						? "IsRowVersion"
						: "ValueGeneratedOnAddOrUpdate";
					break;

				case ValueGenerated.Never:
					property.ValueGeneratedMetohdName = "ValueGeneratedNever";
					break;

				default:
					property.ValueGeneratedMetohdName = "";
					break;
			}
		}

		property.IsConcurrencyToken = property.IsConcurrencyToken && !isRowVersion;

		Properties.Add(column, property);
		Entity.Properties.Add(property);

		return property;
	}

	public DBAbstractions.Metamodel.Navigation AddNavigation(ForeignKey foreignKey)
	{
		if (foreignKey == null)
			throw new ArgumentNullException(nameof(foreignKey));

		var column = foreignKey.FromColumn;

		var navigation = new DBAbstractions.Metamodel.Navigation
		{
			Name = GetPropertyOrNavigationUniqueName(foreignKey.FromColumn.Name.TrimPrefix("id", true).ToCammelCase()),
			ClrType = typeof(object),
			IsCollection = false,
			IsDependentToPrincipal = true,
			DeclaringEntity = Entity
		};

		navigation.ForeignKey = new DBAbstractions.Metamodel.ForeignKey
		{
			DeclaringEntity = Entity,
			DeleteBehavior = ConvertReferentialActionToDeleteBehavior(foreignKey.OnDeleteAction ?? ReferentialAction.NoAction),
			DependentToPrincipal = navigation,
			IsRequired = column.IsNotNull,
			IsUnique = column.IsSingleUniqueConstraint,
			Schema = foreignKey.Table.Schema.Name,
			TableName = foreignKey.Table.Name,
			Name = foreignKey.Name,
			Properties = new List<DBAbstractions.Metamodel.Property>()
		};

		ForeignKeys.Add(foreignKey, navigation.ForeignKey);
		Entity.ForeignKeys.Add(navigation.ForeignKey);

		navigation.ForeignKey.Properties.Add(Properties[foreignKey.FromColumn]);
		//navigation.ForeignKey.Properties.Add(Properties[foreignKey.ToColumn]);

		Navigations.Add(foreignKey, navigation);
		Entity.Navigations.Add(navigation);

		return navigation;
	}

	public DBAbstractions.Metamodel.Navigation AddBackNavigation(ForeignKey fk, DBAbstractions.Metamodel.ForeignKey foreignKey)
	{
		foreignKey.PrincipalEntity = Entity;
		foreignKey.PrincipalKey = Entity.PrimaryKey;

		var isOneToOne = foreignKey.IsUnique;

		string name = "_";
		if (isOneToOne)
		{
			name = foreignKey.DeclaringEntity.TableName.ToCammelCase();
		}
		else
		{
			//name = foreignKey.DeclaringEntity.TableName.ToCammelCase().Pluralize() + "_" + foreignKey.Properties[0].ColumnName.TrimPrefix("id", true).ToCammelCase().Pluralize();

			//self
			if (foreignKey.DeclaringEntity == foreignKey.PrincipalEntity)
			{
				name = foreignKey.Properties[0].ColumnName.TrimPrefix("id", true).ToCammelCase().Pluralize();
			}
			else //remote
			{
				name = foreignKey.DeclaringEntity.TableName.ToCammelCase().Pluralize();
				if (IsPropertyOrNavigationNameUsed(name))
				{
					var prefix = foreignKey.Properties[0].ColumnName.TrimPrefix("id", true).ToCammelCase();
					name = $"{prefix}{name}";
				}
			}
		}

		var backNavigation = new DBAbstractions.Metamodel.Navigation
		{
			Name = GetPropertyOrNavigationUniqueName(name),
			ClrType = typeof(object),
			IsCollection = !isOneToOne,
			IsDependentToPrincipal = false,
			DeclaringEntity = Entity,
			ForeignKey = foreignKey,
			TargetType = foreignKey.DeclaringEntity
		};

		Entity.BackNavigations.Add(backNavigation);
		foreignKey.PrincipalToDependent = backNavigation;
		return backNavigation;
	}

	private string GetPropertyOrNavigationUniqueName(string name)
	{
		if (IsPropertyOrNavigationNameUsed(name))
		{
			var idx = 0;
			while (true)
			{
				idx++;
				var newName = $"{name}{idx}";
				if (!IsPropertyOrNavigationNameUsed(newName))
					return newName;
			}
		}

		return name;
	}

	private bool IsPropertyOrNavigationNameUsed(string name)
		=> Entity.Properties.Any(p => p.Name == name)
			|| Entity.Navigations.Any(n => n.Name == name)
			|| Entity.BackNavigations.Any(n => n.Name == name);


	public DBAbstractions.Metamodel.Key AddPrimaryKey(Table table)
	{
		if (table == null)
			throw new ArgumentNullException(nameof(table));

		Entity.PrimaryKey = new DBAbstractions.Metamodel.Key
		{
			DeclaringEntity = Entity,
			DefaultName = table.PrimaryKey.Name,
			Name = table.PrimaryKey.Name,
			IsPrimaryKey = true,
			Properties = new List<DBAbstractions.Metamodel.Property>()
		};

		foreach (var pkColumn in ((IPrimaryKey)table.PrimaryKey).Columns)
			Entity.PrimaryKey.Properties.Add(Properties[pkColumn]);

		Entity.Keys.Add(Entity.PrimaryKey);

		return Entity.PrimaryKey;
	}

	public DBAbstractions.Metamodel.Key SetFirstColumnAsPrimaryKey(View view)
	{
		Throw.IfArgumentNull(view);

		Entity.PrimaryKey = new DBAbstractions.Metamodel.Key
		{
			DeclaringEntity = Entity,
			DefaultName = $"PK_TMP_{view.Name}",
			Name = $"PK_TMP_{view.Name}",
			IsPrimaryKey = true,
			Properties = []
		};

		Entity.PrimaryKey.Properties.Add(Properties[view.Columns.OrderBy(c => c.OrdinalPosition).FirstOrDefault()]);

		Entity.Keys.Add(Entity.PrimaryKey);

		return Entity.PrimaryKey;
	}

	private DBAbstractions.Metamodel.DeleteBehavior ConvertReferentialActionToDeleteBehavior(ReferentialAction action)
	{
		return action switch
		{
			ReferentialAction.NoAction => DBAbstractions.Metamodel.DeleteBehavior.ClientSetNull,
			ReferentialAction.Restrict => DBAbstractions.Metamodel.DeleteBehavior.Restrict,
			ReferentialAction.Cascade => DBAbstractions.Metamodel.DeleteBehavior.Cascade,
			ReferentialAction.SetNull => DBAbstractions.Metamodel.DeleteBehavior.SetNull,
			ReferentialAction.SetDefault => DBAbstractions.Metamodel.DeleteBehavior.ClientSetNull,
			_ => DBAbstractions.Metamodel.DeleteBehavior.ClientSetNull,
		};
	}

	public void AddAllIndexes(Table table)
	{
		if (table == null)
			throw new ArgumentNullException(nameof(table));

		if (table.Indexes != null)
		{
			foreach (var idx in table.Indexes.Where(idx => !idx.IsPrimary))
			{
				var index = new DBAbstractions.Metamodel.Index
				{
					Name = idx.Name,
					IsUnique = idx.IsUnique,
					//Filter = idx.Filter,
					DeclaringEntity = Entity
				};

				foreach (var col in ((IIndex)idx).Columns)
				{
					var column = Properties[col];
					column.Indexes.Add(index);
					index.Properties.Add(column);
				}

				Entity.Indexes.Add(index);
			}
		}
	}
}
