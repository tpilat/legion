using Newtonsoft.Json;

namespace Legion.Generators.AppGen.Model;

public interface IEntity
{
	[JsonProperty]
	string ID { get; set; }

	[JsonProperty]
	string ModelNames { get; set; }

	[JsonProperty]
	string ContextNames { get; set; }

	[JsonProperty]
	string FullName { get; }

	[JsonProperty]
	int EntityId { get; set; }

	[JsonProperty]
	string Schema { get; }

	[JsonProperty]
	string TableName { get; }

	[JsonProperty]
	string Name { get; set; }

	[JsonProperty]
	bool IsDbView { get; }

	[JsonProperty]
	ModelBase Model { get; set; }

	[JsonProperty]
	PackageBase Package { get; set; }

	[JsonProperty]
	string DbSetName { get; set; }
}
