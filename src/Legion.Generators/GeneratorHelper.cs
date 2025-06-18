using Legion.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Text;

namespace Legion.Generators;

internal class GeneratorHelper
{
	public static string GenerateMethods(ITypeSymbol typeSymbol)
	{
		var sb = new StringBuilder();

		foreach (var methodSymbol in typeSymbol.GetMembers().OfType<IMethodSymbol>()
			.Where(x => x.MethodKind == MethodKind.Ordinary && x.DeclaredAccessibility == Accessibility.Public))
		{
			sb.AppendLine();

			var sbParams = new StringBuilder();
			int i = 0;
			var count = methodSymbol.Parameters.Length;

			foreach (var parameter in methodSymbol.Parameters)
			{
				var parameterString = ParameterSymbolToString(parameter);

				i++;
				sbParams.AppendLine();
				sbParams.Append($"		{parameterString}{(i == count ? "" : ",")}");
			}

			var parameters = sbParams.ToString();
			parameters = string.IsNullOrWhiteSpace(parameters) ? "" : parameters;
			sb.AppendLine($@"	{methodSymbol.ReturnType} {methodSymbol.Name}({parameters});");
		}

		var source = sb.ToString();
		return string.IsNullOrWhiteSpace(source) ? "" : source;
	}

	public static string ParameterSymbolToString(IParameterSymbol parameter)
	{
		var equalsSyntax = parameter.DeclaringSyntaxReferences[0].GetSyntax() switch
		{
			ParameterSyntax par => par.Default,
			PropertyDeclarationSyntax prop => prop.Initializer,
			VariableDeclaratorSyntax variab => variab.Initializer,
			_ => throw new Exception("Unknown declaration syntax")
		};

		string defaultValue = "";
		if (equalsSyntax is not null)
			defaultValue = equalsSyntax.Value.ToString();

		return $"{parameter.Type} {parameter.Name}{(string.IsNullOrWhiteSpace(defaultValue) ? "" : $" = {defaultValue}")}";
	}

	public static string AsFieldName(string value)
		=> string.IsNullOrWhiteSpace(value)
			? value
			: value.ToCammelCase(removeUnderscores: false).FirstToLower();

	public static string AsPrivateFieldName(string value)
	{
		if (string.IsNullOrWhiteSpace(value))
			return value;

		string result = value.ToCammelCase().FirstToLower();

		if (!result.StartsWith("_"))
		{
			result = "_" + result;
		}

		return result;
	}
}
