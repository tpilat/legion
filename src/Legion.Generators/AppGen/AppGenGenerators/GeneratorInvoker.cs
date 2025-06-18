using Legion.Generator;

namespace Legion.Generators.AppGen.AppGenGenerators;

internal static class GeneratorInvoker
{
	public static ModelResult Generate<G>(string filePath, Dictionary<string, object> args = null)
		where G : GeneratorBase, new()
	{
		if (string.IsNullOrWhiteSpace(filePath))
			throw new ArgumentNullException(nameof(filePath));

		var generator = new G()
		{
			WriteMode = GeneratorBase.WriteModes.Overwritte
		};
		if (args != null)
			foreach (var kvp in args)
				generator.SetParam(kvp.Key, kvp.Value);

		try
		{
			string targetFolder = Path.GetDirectoryName(filePath);
			if (!Directory.Exists(targetFolder))
				Directory.CreateDirectory(targetFolder);

			generator.TransformText();
		}
		catch (Exception ex)
		{
			generator.AddError(filePath, ex.ToString());
			generator.Process();
		}

		var modelResult = new ModelResult();
		foreach (var compilerError in generator.Errors)
		{
			if (compilerError.IsWarning)
				modelResult.AddWarning(compilerError.FileName, compilerError.ErrorText);
			else
				modelResult.AddError(compilerError.FileName, compilerError.ErrorText);
		}

		return modelResult;
	}
}
