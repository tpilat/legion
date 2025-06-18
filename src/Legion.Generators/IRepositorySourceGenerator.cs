using Legion.Extensions;
using Microsoft.CodeAnalysis;
using System.Text;

namespace Legion.Generators;

internal class IRepositorySourceGenerator
{
	public static string GeneratedSource(
		ITypeSymbol typeSymbol,
		List<QueryMetadata> queryMetadata,
		string modelNamespace,
		string efNamespace,
		string ientityRepositry)
	{
		var queries = GenereateQueries(queryMetadata, modelNamespace, efNamespace);

		var sb = new StringBuilder();

		sb.AppendLine($@"namespace {GetRepositryFullNamespaceName(typeSymbol)};

public partial interface I{typeSymbol.Name}Repository : {ientityRepositry}<{typeSymbol}>
{{
	Legion.ACL.IAccessControlManager<{typeSymbol}>? AccessControlManager {{ get; }}
{(string.IsNullOrWhiteSpace(queries) ? $"{Environment.NewLine}" : queries)}}}");

		return sb.ToString();
	}

	public static string GetRepositryFullNamespaceName(ITypeSymbol typeSymbol)
		=> $"{typeSymbol.ContainingNamespace}.Repositories";

	public static string GetRepositryRelativeNamespaceName(ITypeSymbol typeSymbol)
		=> $"{typeSymbol.ContainingNamespace}.Repositories";

	private static string GenereateQueries(
		List<QueryMetadata> queryMetadata,
		string modelNamespace,
		string efNamespace)
	{
		if (queryMetadata?.Any() != true)
			return "";

		var sb = new StringBuilder();

		var ignoredConstructorParams = new List<string> { "string repositoryIdentifier" };

		foreach (var metadata in queryMetadata)
		{
			for (int cp = 0; cp < metadata.ConstructorParameters.Count; cp++)
			{
				var constructorParameters = metadata.ConstructorParameters[cp].Where(x => !ignoredConstructorParams.Contains(x)).ToList();
				var sbParams = new StringBuilder();
				int i = 0;
				var count = constructorParameters.Count;

				foreach (var parameterString in constructorParameters)
				{
					i++;
					sbParams.AppendLine();
					sbParams.Append($"		{parameterString}{(i == count ? "" : ",")}");
				}

				var parameters = sbParams.ToString();
				parameters = string.IsNullOrWhiteSpace(parameters) ? "" : parameters;

				sb.AppendLine($@"
	{metadata.IQueryFullName.Replace(efNamespace, modelNamespace)} {metadata.Query.Name}({parameters});");
			}
		}

		var result = sb.ToString();
		result = string.IsNullOrWhiteSpace(result) ? "" : result;
		return result;
	}
}
