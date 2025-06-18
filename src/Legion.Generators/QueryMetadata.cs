using Microsoft.CodeAnalysis;

namespace Legion.Generators;

internal class QueryMetadata
{
	public ITypeSymbol Query { get; set; }
	public string SourceFilePath { get; set; }
	public ITypeSymbol Context { get; set; }
	public ITypeSymbol Entity { get; set; }
	public List<string> Properites { get; set; }
	public Dictionary<int, List<string>> ConstructorParameterNames { get; set; }
	public Dictionary<int, List<string>> ConstructorParameters { get; set; }

	public string IQueryFullName => $"{Query.ContainingNamespace}.I{Query.Name}";
	public string QueryFullName => $"{Query.ContainingNamespace}.{Query.Name}";

	public QueryMetadata()
	{
		Properites = new();
		ConstructorParameterNames = new();
		ConstructorParameters = new();
	}
}
