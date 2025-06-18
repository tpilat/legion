using Microsoft.CodeAnalysis;
using System.Text;

namespace Legion.Generators;

internal class IQuerySourceGenerator
{
	private const string EntityFrameworkCoreQueriesNamepsace = "Legion";
	private const string IQueryGenericDefinition = "Legion.MessageBus.Messages.IQuery<>";

	public static string GeneratedSource(ITypeSymbol typeSymbol, QueryMetadata queryMetadata, string modelNamespace, string efNamespace)
	{
		var sb = new StringBuilder();

		sb.AppendLine($@"namespace {typeSymbol.ContainingNamespace.ToString()?.Replace(efNamespace, modelNamespace)};

public partial interface I{typeSymbol.Name}
{{{GenerateProperties(typeSymbol)}{GeneratorHelper.GenerateMethods(typeSymbol)}}}");

		return sb.ToString();
	}

	public static IEnumerable<IPropertySymbol> GetPropeties(ITypeSymbol typeSymbol)
		=> typeSymbol.GetMembers().OfType<IPropertySymbol>()
			.Where(parameter => parameter.Type.ContainingNamespace.ToString()?.StartsWith(EntityFrameworkCoreQueriesNamepsace) != true);

	public static Dictionary<int, List<IParameterSymbol>> GetAllConstructorParameters(ITypeSymbol typeSymbol)
	{
		var i = 0;
		return typeSymbol.GetMembers().OfType<IMethodSymbol>()
			.Where(x => x.MethodKind == MethodKind.Constructor && x.DeclaredAccessibility == Accessibility.Public)?
			.ToDictionary(c => i++, c =>
				c.Parameters
					.Where(parameter => parameter.Type.ContainingNamespace.ToString()?.StartsWith(EntityFrameworkCoreQueriesNamepsace) != true
						|| parameter.Type.AllInterfaces.Any(ifc => 
							ifc.IsGenericType
							&& ifc.ConstructUnboundGenericType().ToString() == IQueryGenericDefinition))
					.ToList())
		?? new();
	}

	private static string GenerateProperties(ITypeSymbol typeSymbol)
	{
		var sb = new StringBuilder();

		var isFirst = true;
		foreach (var propertySymbol in GetPropeties(typeSymbol))
		{
			if (isFirst)
				sb.AppendLine();

			sb.AppendLine($@"	{propertySymbol.Type} {propertySymbol.Name} {{ get; }}");
			isFirst = false;
		}

		var source = sb.ToString();
		return string.IsNullOrWhiteSpace(source) ? "" : source;
	}
}
