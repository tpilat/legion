using Legion.Database.Metamodel;

namespace Legion.Database;

public interface IDatabaseMetadataProvider
{
	IDatabaseModel? Model { get; }

	bool LoadMetadata(string connectionString, string databaseName);
}
