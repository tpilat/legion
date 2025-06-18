using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;

namespace Legion.Generators;

#pragma warning disable RS1024 // Symbols should be compared for equality

[Generator]
internal class QuerySourceParser : ISourceGenerator
{
	private const string QueryDefinitionFullName = "Legion.EntityFrameworkCore.Queries.QueryDefinition<TContext, Q, TResponse, TQuery>";

	private readonly Dictionary<string, ITypeSymbol> _entityModel;
	public Dictionary<ITypeSymbol, List<QueryMetadata>> EntityModel { get; }

	public string CurrentSourceFilePath { get; set; }

	public QueryMetadata CurrentQueryMetadata { get; set; }

	public QuerySourceParser(Dictionary<ITypeSymbol, List<QueryMetadata>> entityModel)
	{
		EntityModel = entityModel ?? throw new ArgumentNullException(nameof(entityModel));
		_entityModel = EntityModel.ToDictionary(x => x.Key.ToString(), x => x.Key);
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
		//var queryDefinitionInterface = compilation.GetTypeByMetadataName("Legion.EntityFrameworkCore.Queries.IQueryDefinition") ?? throw new InvalidOperationException("Missing ifc Legion.EntityFrameworkCore.Queries.QueryDefinition");

		foreach (var syntaxTree in compilation.SyntaxTrees)
		{
			var semanticModel = compilation.GetSemanticModel(syntaxTree);
			var targetTypes = syntaxTree
				.GetRoot()
				.DescendantNodesAndSelf()
				.OfType<ClassDeclarationSyntax>()
				.Select(x => semanticModel.GetDeclaredSymbol(x))
				.OfType<ITypeSymbol>()
				//.Where(x => x.Interfaces.Contains(queryDefinitionInterface))
				.ToImmutableHashSet();

			var count = 0;
			foreach (var typeSymbol in targetTypes)
			{
				if (typeSymbol.BaseType?.OriginalDefinition?.ToString() != QueryDefinitionFullName)
					continue;

				if (0 < count)
					throw new InvalidOperationException("Multiple types defined in source");

				var queryMetadata = new QueryMetadata
				{
					Query = typeSymbol,
					SourceFilePath = CurrentSourceFilePath,
					Context = typeSymbol.BaseType.TypeArguments[0],
					Entity = typeSymbol.BaseType.TypeArguments[1],
					Properites = IQuerySourceGenerator.GetPropeties(typeSymbol).Select(x => $"{x.Type} {x.Name}").ToList(),
					ConstructorParameterNames = IQuerySourceGenerator.GetAllConstructorParameters(typeSymbol).ToDictionary(x => x.Key, x => x.Value.Select(parameter => parameter.Name).ToList()),
					ConstructorParameters = IQuerySourceGenerator.GetAllConstructorParameters(typeSymbol).ToDictionary(x => x.Key, x => x.Value.Select(paramter => GeneratorHelper.ParameterSymbolToString(paramter)).ToList())
				};

				CurrentQueryMetadata = queryMetadata;

				if (!_entityModel.TryGetValue(queryMetadata.Entity.ToString()!, out var modelTypeSymbol))
					continue; //throw new InvalidOperationException($"{queryMetadata.Entity} is not an 'IEntity'");

				if (!EntityModel.TryGetValue(modelTypeSymbol, out var querySources))
					continue; //throw new InvalidOperationException($"#2 {queryMetadata.Entity} is not an 'IEntity'");

				querySources.Add(queryMetadata);

				count++;

				//context.AddSource($"{targetType.Name}.GEN.cs", source);
			}
		}
	}
}
#pragma warning restore RS1024 // Symbols should be compared for equality
