using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Immutable;
using System.Reflection;

namespace Legion.Generators;

internal class Compilator
{
	public static Compilation CreateCompilation(string source, params Type[] references)
	{
		var compilation = CSharpCompilation.Create("compilation",
			new[] { CSharpSyntaxTree.ParseText(source, new CSharpParseOptions(LanguageVersion.Latest)) },
			references?.Select(x => MetadataReference.CreateFromFile(x.GetTypeInfo().Assembly.Location)),
			new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

		return compilation;
	}

	private static GeneratorDriver CreateDriver(params ISourceGenerator[] generators)
		=> CSharpGeneratorDriver.Create(generators);

	public static Compilation RunGenerators(Compilation compilation, out ImmutableArray<Diagnostic> diagnostics, params ISourceGenerator[] generators)
	{
		CreateDriver(generators).RunGeneratorsAndUpdateCompilation(compilation, out var newCompilation, out diagnostics);
		return newCompilation;
	}
}
