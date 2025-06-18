using System.Text;

namespace Legion.Generators;

internal static class FileWriter
{
	public const bool WRITE_TO_FILE = true;

	public static void WriteAllText(string path, string? contents, Encoding encoding)
	{
		if (WRITE_TO_FILE)
			File.WriteAllText(path, contents, encoding);
	}
}
