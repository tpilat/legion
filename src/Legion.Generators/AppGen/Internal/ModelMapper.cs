using Legion.Database.Metamodel;
using Legion.Generators.AppGen.Model;
using Legion.Generators.AppGen.Model.Config;

namespace Legion.Generators.AppGen.Internal;

internal class ModelMapper
{
	public ModelResult ModelResult { get; }
	public DatabaseProviderType ProviderType { get; set; }
	public ModelBase? Extension { get; }

	public ModelBase Model { get; }
	public Dictionary<DBAbstractions.Metamodel.Entity, EntityMapper> TableEntityMappers { get; set; }
	public Dictionary<DBAbstractions.Metamodel.Entity, QueryEntityMapper> ViewEntityMappers { get; set; }

	public ModelMapper(ModelBase? modelExtension, DatabaseProviderType providerType)
	{
		ModelResult = new ModelResult();
		ProviderType = providerType;
		Extension = modelExtension;
		Model = new ModelBase
		{
			DefaultSchema = ProviderType == DatabaseProviderType.PostgreSQL
				? "public"
				: "dbo",
			Settings = Extension?.Settings ?? CodeGeneratorSettings.CreateDefaultSettings(ProviderType)
		};
		TableEntityMappers = [];
		ViewEntityMappers = [];
	}

	public EntityBase AddTableEntity(DBAbstractions.Metamodel.Entity ent)
	{
		Throw.IfArgumentNull(ent);

		var id = $"{ent.Schema}.{ent.TableName}";
		var entityExtension = Extension?.GetEntity(id);

		var entityMapper = new EntityMapper(this, ent, Model, entityExtension);
		Model.Entities.Add(entityMapper.Entity);

		var package = Model.GetPackage(entityMapper.Entity.Schema);
		if (package == null)
		{
			package = new AppGen.Model.PackageBase
			{
				Model = Model
			};
			package.Init(entityMapper.Entity.Schema, Extension?.GetPackage(entityMapper.Entity.Schema));
			Model.Packages.Add(package);
		}

		entityMapper.Entity.Package = package;
		package.Entities.Add(entityMapper.Entity);

		TableEntityMappers.Add(ent, entityMapper);

		foreach (var column in ent.Properties.OrderBy(p => p.ColumnOrdinal))
			entityMapper.AddProperty(column);

		var key = entityMapper.AddPrimaryKey(ent);
		//entityMapper.Entity.MainColumnName = key.Properties[0].ColumnName;

		foreach (var nav in ent.Navigations.OrderBy(n => n.Name))
			entityMapper.AddNavigation(nav);

		foreach (var nav in ent.BackNavigations.OrderBy(n => n.Name))
			entityMapper.AddBackNavigation(nav);

		foreach (var fk in ent.ForeignKeys.OrderBy(n => n.DependentToPrincipal.Name))
			entityMapper.AddForeignKey(fk);

		entityMapper.Entity.MainColumn = entityMapper.Entity.Properties[0];

		//******SetReferences******
		if (entityExtension?.MainColumn != null)
		{
			var mainColumn = entityMapper.Entity.GetProperty(entityExtension.MainColumn.ID);
			if (mainColumn != null)
				entityMapper.Entity.MainColumn = mainColumn;
		}

		return entityMapper.Entity;
	}

	public QueryEntityBase AddViewEntity(DBAbstractions.Metamodel.Entity ent)
	{
		Throw.IfArgumentNull(ent);

		var id = $"{ent.Schema}.{ent.TableName}";
		var queryEntityExtension = Extension?.GetQueryEntity(id);
		var queryEntityMapper = new QueryEntityMapper(this, ent, Model, queryEntityExtension);
		Model.QueryEntities.Add(queryEntityMapper.QueryEntity);

		var package = Model.GetPackage(queryEntityMapper.QueryEntity.Schema);
		if (package == null)
		{
			package = new AppGen.Model.PackageBase
			{
				Model = Model
			};
			package.Init(queryEntityMapper.QueryEntity.Schema, Extension?.GetPackage(queryEntityMapper.QueryEntity.Schema));
			Model.Packages.Add(package);
		}

		queryEntityMapper.QueryEntity.Package = package;
		package.QueryEntities.Add(queryEntityMapper.QueryEntity);

		ViewEntityMappers.Add(ent, queryEntityMapper);

		foreach (var column in ent.Properties.OrderBy(p => p.ColumnOrdinal))
			queryEntityMapper.AddQueryProperty(column);

		queryEntityMapper.QueryEntity.MainColumn = queryEntityMapper.QueryEntity.Properties[0];

		//******SetReferences******
		if (queryEntityExtension?.MainColumn != null)
		{
			var mainColumn = queryEntityMapper.QueryEntity.GetQueryProperty(queryEntityExtension.MainColumn.ID);
			if (mainColumn != null)
				queryEntityMapper.QueryEntity.MainColumn = mainColumn;
		}

		return queryEntityMapper.QueryEntity;
	}

	public void AddSequence(DBAbstractions.Metamodel.Sequence seq)
	{
		var sequence = new SequenceBase
		{
			Model = Model
		};

		sequence.Init(seq);
		Model.Sequences.Add(sequence);
	}
}
