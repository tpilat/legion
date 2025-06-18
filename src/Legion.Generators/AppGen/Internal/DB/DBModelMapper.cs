using Legion.Database.Internal;
using Legion.Database.Metamodel;

namespace Legion.Generators.AppGen.Model.Internal.DB;

internal class DBModelMapper
{
	private readonly DatabaseProviderType _providerType;

	public DBAbstractions.Metamodel.Model Model { get; }
	public Dictionary<ITable, DBEntityMapper> TableEntityMappers { get; set; }
	public Dictionary<IView, DBEntityMapper> ViewEntityMappers { get; set; }

	public DBModelMapper(DatabaseProviderType providerType)
	{
		Model = new DBAbstractions.Metamodel.Model();
		TableEntityMappers = [];
		ViewEntityMappers = [];
		_providerType = providerType;
	}

	public DBAbstractions.Metamodel.Entity AddTableEntity(Table table)
	{
		Throw.IfArgumentNull(table);

		var entityMapper = new DBEntityMapper(table, Model);
		Model.TableEntities.Add(entityMapper.Entity);
		TableEntityMappers.Add(table, entityMapper);

		foreach (var column in table.Columns)
			entityMapper.AddProperty(column, _providerType);

		if (table.PrimaryKey != null)
			entityMapper.AddPrimaryKey(table);

		if (table.ForeignKeys != null)
			foreach (var fk in table.ForeignKeys)
				entityMapper.AddNavigation(fk);

		return entityMapper.Entity;
	}

	public DBAbstractions.Metamodel.Entity AddViewEntity(View view)
	{
		Throw.IfArgumentNull(view);

		var entityMapper = new DBEntityMapper(view, Model);
		Model.ViewEntities.Add(entityMapper.Entity);
		ViewEntityMappers.Add(view, entityMapper);

		foreach (var column in view.Columns)
			entityMapper.AddProperty(column, _providerType);

		entityMapper.SetFirstColumnAsPrimaryKey(view);

		return entityMapper.Entity;
	}

	public void AddSequence(Sequence sequence)
	{
		Model.Sequences.Add(new DBAbstractions.Metamodel.Sequence
		{
			Model = Model,
			Schema = sequence.Schema.Name,
			Name = sequence.Name,
			ClrType = sequence.CsharpType,
			StartValue = sequence.StartValue ?? 1,
			IncrementBy = sequence.IncrementBy ?? 1,
			MinValue = sequence.MinValue,
			MaxValue = sequence.MaxValue,
			IsCyclic = sequence.IsCyclic ?? false
		});
	}
}
