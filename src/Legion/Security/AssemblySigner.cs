using System.Diagnostics;

namespace Legion.Security;

public static class AssemblySigner
{
	public static bool SignAssembly(
		string assemblyFilePath,
		string certificatePfxPath,
		string certificatePassword,
		string timestampUrl = "http://timestamp.digicert.com", //http://timestamp.sectigo.com
		string signToolFilePath = "signtool.exe")
	{
		Throw.IfArgumentNullOrWhiteSpace(assemblyFilePath);
		Throw.IfArgumentNullOrWhiteSpace(certificatePfxPath);
		Throw.IfArgumentNullOrWhiteSpace(certificatePassword);
		Throw.IfArgumentNullOrWhiteSpace(timestampUrl);
		Throw.IfArgumentNullOrWhiteSpace(signToolFilePath);

		var process = new Process
		{
			StartInfo = new ProcessStartInfo
			{
				FileName = signToolFilePath,
				Arguments = $"sign /f \"{certificatePfxPath}\" /p \"{certificatePassword}\" /tr \"{timestampUrl}\" /td sha256 /fd sha256 \"{assemblyFilePath}\"",
				CreateNoWindow = true,
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				UseShellExecute = false
			}
		};

		process.Start();
		process.WaitForExit();

		string output = process.StandardOutput.ReadToEnd();
		string error = process.StandardError.ReadToEnd();

		if (process.ExitCode != 0)
			Throw.InvalidOperationException(error);

		//Console.WriteLine($"Signing succeeded: {output}");
		return true;
	}
}
