using Legion.Generators.AppGen.Model;

namespace Legion.Generators.AppGen.Internal;

internal class QueryEntityMapper
{
	public ModelMapper ModelMapper { get; }
	public QueryEntityBase QueryEntity { get; }
	public QueryEntityBase? Extension { get; }

	public Dictionary<DBAbstractions.Metamodel.Property, QueryPropertyBase> Properties { get; set; }

	public QueryEntityMapper(ModelMapper modelMapper, DBAbstractions.Metamodel.Entity view, ModelBase model, QueryEntityBase? ext)
	{
		ModelMapper = modelMapper;
		Extension = ext;

		QueryEntity = new QueryEntityBase
		{
			Model = model
		};
		QueryEntity.Init(view, Extension);

		Properties = [];
	}

	public QueryPropertyBase AddQueryProperty(DBAbstractions.Metamodel.Property column)
	{
		var property = new QueryPropertyBase
		{
			DeclaringEntity = QueryEntity
		};

		property.Init(column, Extension?.GetQueryProperty(column.ColumnName));

		Properties.Add(column, property);
		QueryEntity.Properties.Add(property);

		return property;
	}
}
