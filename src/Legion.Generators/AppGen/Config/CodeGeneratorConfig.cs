using Legion.Serializer;
using Newtonsoft.Json;

namespace Legion.Generators.AppGen.Model.Config;

[Serializable]
public class CodeGeneratorConfig
{
	private static string _codeGeneratorConfigInstanceFilePath;
	public static CodeGeneratorConfig Instance { get; private set; }

	[JsonProperty]
	public string WorkspacePath { get; set; } = "";

	[JsonProperty]
	public bool CleanWorkspace { get; set; } = true;

	[JsonProperty]
	public List<DatabaseConnection> DatabaseConnections { get; set; }

	[JsonProperty]
	public DatabaseConnection SelectedDatabaseConnection { get; set; }

	[JsonProperty]
	public string SettingsFilePath { get; set; } = "";

	[JsonProperty]
	public bool Compile { get; set; } = false;

	public CodeGeneratorConfig()
	{
		DatabaseConnections = new List<DatabaseConnection>();
	}

	public static CodeGeneratorConfig Create(string codeGeneratorConfigInstanceFilePath)
	{
		if (string.IsNullOrWhiteSpace(codeGeneratorConfigInstanceFilePath))
			throw new ArgumentNullException(nameof(codeGeneratorConfigInstanceFilePath));

		_codeGeneratorConfigInstanceFilePath = codeGeneratorConfigInstanceFilePath;
		Instance = new CodeGeneratorConfig();
		return Instance;
	}

	public static void Serialize()
	{
		using var fs = new FileStream(_codeGeneratorConfigInstanceFilePath, FileMode.Create, FileAccess.Write, FileShare.None);
		JsonSerializerHelper.Serialize(Instance, fs, true);
	}

	public static void Serialize(string filePath)
	{
		_codeGeneratorConfigInstanceFilePath = filePath;
		Serialize();
	}

	public static void Load(string filePath)
	{
		using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None);
		Instance = JsonSerializerHelper.Deserialize<CodeGeneratorConfig>(fs, true)!;
		_codeGeneratorConfigInstanceFilePath = filePath;
	}
}
