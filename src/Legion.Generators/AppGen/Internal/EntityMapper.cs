using Legion.Generators.AppGen.Model;

namespace Legion.Generators.AppGen.Internal;

internal class EntityMapper
{
	public ModelMapper ModelMapper { get; }
	public EntityBase Entity { get; }
	public EntityBase? Extension { get; }

	public Dictionary<DBAbstractions.Metamodel.Property, PropertyBase> Properties { get; set; }

	public Dictionary<DBAbstractions.Metamodel.Navigation, NavigationBase> Navigations { get; set; }

	public Dictionary<DBAbstractions.Metamodel.Navigation, BackNavigationBase> BackNavigations { get; set; }

	public Dictionary<DBAbstractions.Metamodel.ForeignKey, ForeignKeyBase> ForeignKeys { get; set; }

	public EntityMapper(ModelMapper modelMapper, DBAbstractions.Metamodel.Entity table, ModelBase model, EntityBase? ext)
	{
		ModelMapper = modelMapper;
		Extension = ext;

		Entity = new EntityBase
		{
			Model = model
		};

		Entity.Init(table, ext);

		Properties = [];
		Navigations = [];
		BackNavigations = [];
		ForeignKeys = [];
	}

	public PropertyBase AddProperty(DBAbstractions.Metamodel.Property column)
	{
		var property = new PropertyBase
		{
			DeclaringEntity = Entity
		};

		property.Init(column, Extension?.GetProperty(column.ColumnName));

		Properties.Add(column, property);
		Entity.Properties.Add(property);

		return property;
	}

	public NavigationBase AddNavigation(DBAbstractions.Metamodel.Navigation nav)
	{
		var navigation = new NavigationBase
		{
			DeclaringEntity = Entity
		};

		navigation.Init(nav, Extension?.GetNavigation(nav.Name));

		Navigations.Add(nav, navigation);
		Entity.Navigations.Add(navigation);
		return navigation;
	}

	public BackNavigationBase AddBackNavigation(DBAbstractions.Metamodel.Navigation nav)
	{
		var backNavigation = new BackNavigationBase
		{
			DeclaringEntity = Entity
		};

		backNavigation.Init(nav, Extension?.GetBackNavigation(nav.Name));

		BackNavigations.Add(nav, backNavigation);
		Entity.BackNavigations.Add(backNavigation);
		return backNavigation;
	}

	public ForeignKeyBase AddForeignKey(DBAbstractions.Metamodel.ForeignKey fk)
	{
		var foreignKey = new ForeignKeyBase
		{
			DeclaringEntity = Entity,
			Properties = []
		};

		foreignKey.Init(fk);

		ForeignKeys.Add(fk, foreignKey);
		Entity.ForeignKeys.Add(foreignKey);

		foreach (var prop in fk.Properties)
		{
			var property = Properties[prop];
			foreignKey.Properties.Add(property);
			property.ForeignKey = foreignKey;
			//property.ValidationsCount++;
		}

		return foreignKey;
	}

	public KeyBase? AddPrimaryKey(DBAbstractions.Metamodel.Entity ent)
	{
		Throw.IfArgumentNull(ent);

		if (ent.PrimaryKey == null)
		{
			ModelMapper.ModelResult.AddWarning($"{ent.Schema}.{ent.TableName}", "Has no primary key.");
		}
		else
		{
			Entity.PrimaryKey = new KeyBase
			{
				DeclaringEntity = Entity
			};

			Entity.PrimaryKey.Init(ent.PrimaryKey);

			foreach (var pkProp in ent.PrimaryKey.Properties)
				Entity.PrimaryKey.Properties.Add(Properties[pkProp]);

			Entity.Keys.Add(Entity.PrimaryKey);

			return Entity.PrimaryKey;
		}

		return null;
	}

	public void AddAllIndexes(DBAbstractions.Metamodel.Entity ent)
	{
		Throw.IfArgumentNull(ent);

		if (ent.Indexes != null)
		{
			foreach (var idx in ent.Indexes.OrderBy(i => i.Properties[0].Name))
			{
				var index = new IndexBase
				{
					DeclaringEntity = Entity
				};

				index.Init(idx);

				foreach (var prop in idx.Properties)
				{
					var property = Properties[prop];
					property.Indexes.Add(index);
					index.Properties.Add(property);
				}

				Entity.Indexes.Add(index);
			}
		}
	}
}
