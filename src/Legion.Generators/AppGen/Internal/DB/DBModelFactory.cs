using Legion.Database.Metamodel;

namespace Legion.Generators.AppGen.Model.Internal.DB;

internal class DBModelFactory
{
	public DBAbstractions.Metamodel.Model CreateModel(string _connectionString, DatabaseProviderType providerType)
	{
		IDatabaseModel databaseModel;

		if (providerType == DatabaseProviderType.PostgreSQL)
		{
			var postgreSQLMetamodelProvider = new Legion.Database.PostgreSQL.MetadataProvider();
			postgreSQLMetamodelProvider.LoadMetadata(_connectionString);
			databaseModel = postgreSQLMetamodelProvider.Model!;
		}
		else if (providerType == DatabaseProviderType.SqlServer)
		{
			var sqlServerMetamodelProvider = new Legion.Database.SqlServer.MetadataProvider();
			sqlServerMetamodelProvider.LoadMetadata(_connectionString);
			databaseModel = sqlServerMetamodelProvider.Model!;
		}
		else
		{
			Throw.NotSupportedException($"Invalid {nameof(providerType)} = {providerType}");
			return null;
		}

		var modelMapper = new DBModelMapper(providerType);

		foreach (var table in databaseModel.Tables)
			modelMapper.AddTableEntity(table);

		foreach (var view in databaseModel.Views)
			modelMapper.AddViewEntity(view);

		foreach (var table in databaseModel.Tables)
		{
			var entityMapper = modelMapper.TableEntityMappers[table];

			if (table.ForeignKeys != null)
			{
				foreach (var fk in table.ForeignKeys)
				{
					var foreignEntity = modelMapper.TableEntityMappers[fk.ToColumn.Table];
					var navigation = entityMapper.Navigations[fk];
					navigation.TargetType = foreignEntity.Entity;

					foreignEntity.AddBackNavigation(fk, navigation.ForeignKey);
				}
			}

			entityMapper.AddAllIndexes(table);
		}

		foreach (var sequence in databaseModel.Sequences)
			modelMapper.AddSequence(sequence);

		return modelMapper.Model;
	}
}
