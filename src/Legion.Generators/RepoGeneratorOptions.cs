namespace Legion.Generators;

public class RepoGeneratorOptions
{
	public string RepoName { get; set; }
	public string ModelNamespace { get; set; }
	public string EFNamespace { get; set; }
	public string ModelProjectPath { get; set; }
	public Database.Metamodel.DatabaseProviderType DatabaseProviderType { get; set; } = Database.Metamodel.DatabaseProviderType.PostgreSQL;
	public string SQLProjectPath { get; set; }
	public string ContextName { get; set; }
	public string UnitOfWorkName { get; set; }
	public string IRepositry { get; set; }
	public string RepositoryBase { get; set; }
	public string UoWObsoletePrefix { get; set; }
	public List<string> IgnoredTypes { get; set; }
	public List<Type> QueryCompileTypes { get; set; }
}
