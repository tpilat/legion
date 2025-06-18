using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;

namespace Legion.Generators;

#pragma warning disable RS1024 // Symbols should be compared for equality

[Generator]
internal class ModelSourceParser : ISourceGenerator
{
	private const string _entity = "Legion.Model.IEntity";
	private const string _queryEntity = "Legion.Model.IQueryEntity";

	private readonly List<string> _ignoredEntities;
	private readonly HashSet<string> _entityFullNames;
	private readonly HashSet<string> _queryEntityFullNames;
	public Dictionary<ITypeSymbol, List<QueryMetadata>> EntityModel { get; }
	public Dictionary<ITypeSymbol, List<QueryMetadata>> QueryEntityModel { get; }

	public string CurrentSourceFilePath { get; set; }

	public ModelSourceParser(List<string> ignoredEntities)
	{
		_entityFullNames = new();
		_queryEntityFullNames = new();
		_ignoredEntities = ignoredEntities ?? new();
		EntityModel = new();
		QueryEntityModel = new();
		CurrentSourceFilePath = null!;
	}

	public void Initialize(GeneratorInitializationContext context)
	{
		if (string.IsNullOrWhiteSpace(CurrentSourceFilePath))
			throw new InvalidOperationException($"{nameof(CurrentSourceFilePath)} == null");
	}

	public void Execute(GeneratorExecutionContext context)
	{
		if (string.IsNullOrWhiteSpace(CurrentSourceFilePath))
			throw new InvalidOperationException($"{nameof(CurrentSourceFilePath)} == null");

		var compilation = context.Compilation;
		var entityInterface = compilation.GetTypeByMetadataName(_entity) ?? throw new InvalidOperationException($"Missing ifc {_entity}");
		var queryEntityInterface = compilation.GetTypeByMetadataName(_queryEntity) ?? throw new InvalidOperationException($"Missing ifc {_queryEntity}");

		foreach (var syntaxTree in compilation.SyntaxTrees)
		{
			var semanticModel = compilation.GetSemanticModel(syntaxTree);
			var allTypes =
				syntaxTree
					.GetRoot()
					.DescendantNodesAndSelf()
					.OfType<ClassDeclarationSyntax>()
					.Select(x => semanticModel.GetDeclaredSymbol(x))
					.OfType<ITypeSymbol>()
					.ToList();

			var entityTypes =
				allTypes
					.Where(x => x.Interfaces.Contains(entityInterface))
					.ToImmutableHashSet();

			var queryEntityTypes =
				allTypes
					.Where(x => x.Interfaces.Contains(queryEntityInterface))
					.ToImmutableHashSet();

			foreach (var typeSymbol in entityTypes)
			{
				var fullName = typeSymbol.ToString()!;
				if (_ignoredEntities.Contains(fullName))
					continue;

				if (_entityFullNames.Add(fullName))
					EntityModel.TryAdd(typeSymbol, new List<QueryMetadata>());
			}

			foreach (var typeSymbol in queryEntityTypes)
			{
				var fullName = typeSymbol.ToString()!;
				if (_ignoredEntities.Contains(fullName))
					continue;

				if (_queryEntityFullNames.Add(fullName))
					QueryEntityModel.TryAdd(typeSymbol, new List<QueryMetadata>());
			}
		}
	}
}
#pragma warning restore RS1024 // Symbols should be compared for equality
