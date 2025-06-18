using Legion.Extensions;
using System.Diagnostics;
using System.Text;

namespace Legion.Processes;

public class ProcessHelper
{
	public enum ReturnValue : int
	{
		Ok = 0,
		Error = 1,
		Warning = 2,
	}

	public static ReturnValue RunExeCommand(
		string executablePath,
		string arguments,
		StringBuilder? sb,
		bool waitForExit = true,
		bool hidden = true,
		bool useShellExecute = false,
		bool redirectInput = true,
		bool redirectOutput = true,
		bool redirectError = true)
	{
		var num = ReturnValue.Ok;

		if (string.IsNullOrWhiteSpace(executablePath))
		{
			sb?.AppendLine($"{nameof(executablePath)} == null");
			return ReturnValue.Error;
		}

		try
		{
			var startInfo = new ProcessStartInfo
			{
				UseShellExecute = useShellExecute,
				FileName = executablePath,
				RedirectStandardInput = redirectInput,
				RedirectStandardOutput = redirectOutput,
				RedirectStandardError = redirectError,
			};

			if (!string.IsNullOrWhiteSpace(arguments))
				startInfo.Arguments = arguments;

			if (hidden)
			{
				startInfo.CreateNoWindow = true;
				startInfo.WindowStyle = ProcessWindowStyle.Hidden;
			}
			else
			{
				startInfo.CreateNoWindow = false;
				startInfo.WindowStyle = ProcessWindowStyle.Normal;
			}

			using var process = Process.Start(startInfo);
			if (process == null)
			{
				sb?.AppendLine($"{nameof(process)} == null");
				return ReturnValue.Error;
			}

			if (waitForExit)
			{
				process.WaitForExit();

				num = process.ExitCode != 0
					? ReturnValue.Error
					: ReturnValue.Ok;

				string? output = null;
				if (redirectOutput)
					output = process.StandardOutput.ReadToEnd();

				string? error = null;
				if (redirectError)
					error = process.StandardError.ReadToEnd();

				output = output?.Replace("\0", "");
				error = error?.Replace("\0", "");

				if (!string.IsNullOrEmpty(error))
				{
					sb?.AppendLine("Error");
					sb?.AppendLine(error);
				}

				if (!string.IsNullOrEmpty(output))
				{
					sb?.AppendLine("Output");
					sb?.AppendLine(output);
				}
			}

			return num;
		}
		catch (Exception ex)
		{
			sb?.AppendLine(ex.ToStringTrace());
			return ReturnValue.Error;
		}
	}

	public static Process StartProcess(
		string executablePath,
		string arguments,
		Action<string>? output,
		bool hidden = true,
		bool useShellExecute = false,
		bool redirectInput = true,
		bool redirectOutput = true,
		bool redirectError = true,
		string? workingDirectory = null,
		Action? onExit = null)
	{
		Throw.IfArgumentNullOrWhiteSpace(executablePath);

		var process = new Process
		{
			StartInfo = new ProcessStartInfo
			{
				UseShellExecute = useShellExecute,
				FileName = executablePath,
				RedirectStandardInput = redirectInput,
				RedirectStandardOutput = redirectOutput,
				RedirectStandardError = redirectError
			},
			EnableRaisingEvents = onExit != null
		};

		if (!string.IsNullOrWhiteSpace(workingDirectory))
			process.StartInfo.WorkingDirectory = workingDirectory;

		if (!string.IsNullOrWhiteSpace(arguments))
			process.StartInfo.Arguments = arguments;

		if (hidden)
		{
			process.StartInfo.CreateNoWindow = true;
			process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
		}
		else
		{
			process.StartInfo.CreateNoWindow = false;
			process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
		}

		if (onExit != null)
			process.Exited += (sender, e) => onExit();

		if (output != null)
		{
			if (redirectOutput)
			{
				process.OutputDataReceived += (s, e) =>
				{
					if (e.Data != null)
						output(e.Data);
				};
			}

			if (redirectError)
			{
				process.ErrorDataReceived += (s, e) =>
				{
					if (e.Data != null)
						output(e.Data);
				};
			}
		}

		bool isStarted = process.Start();

		if (isStarted)
		{
			process.BeginOutputReadLine();
			process.BeginErrorReadLine();
		}

		return process;
	}

	public static bool KillProcess(Process process)
	{
		if (process == null || process.HasExited)
			return false;

		process.Kill();
		process.Dispose();
		return true;
	}

	private static async Task ReadStreamAsync(StreamReader reader, StringBuilder sb)
	{
		string? line;
		while ((line = await reader.ReadToEndAsync()) != null)
		{
			sb.AppendLine(line?.Replace("\0", ""));
		}
	}

	public static ReturnValue RunMsiCommand(
		string executablePath,
		string arguments,
		StringBuilder? sb,
		string allUsers = "1",
		bool waitForExit = true,
		bool hidden = true,
		bool useShellExecute = false,
		bool redirectInput = true,
		bool redirectOutput = true,
		bool redirectError = true)
	{
		var num = ReturnValue.Ok;

		if (string.IsNullOrWhiteSpace(executablePath))
		{
			sb?.AppendLine($"{nameof(executablePath)} == null");
			return ReturnValue.Error;
		}

		try
		{
			var startInfo = new ProcessStartInfo
			{
				UseShellExecute = useShellExecute,
				FileName = "msiexec.exe",
				Arguments = $"{(hidden ? "/qn " : string.Empty)}/i \"{executablePath}{(string.IsNullOrWhiteSpace(arguments) ? string.Empty : $" {arguments}")}\" ALLUSERS={allUsers}",
				RedirectStandardInput = redirectInput,
				RedirectStandardOutput = redirectOutput,
				RedirectStandardError = redirectError,
			};

			if (hidden)
			{
				startInfo.CreateNoWindow = true;
				startInfo.WindowStyle = ProcessWindowStyle.Hidden;
			}
			else
			{
				startInfo.CreateNoWindow = false;
				startInfo.WindowStyle = ProcessWindowStyle.Normal;
			}

			using var process = Process.Start(startInfo);
			if (process == null)
				throw new InvalidOperationException($"{nameof(process)} == null");

			if (waitForExit)
			{
				process.WaitForExit();

				num = process.ExitCode != 0
					? ReturnValue.Error
					: ReturnValue.Ok;

				string? output = null;
				if (redirectOutput)
					output = process.StandardOutput.ReadToEnd();

				string? error = null;
				if (redirectError)
					error = process.StandardError.ReadToEnd();

				output = output?.Replace("\0", "");
				error = error?.Replace("\0", "");

				if (!string.IsNullOrEmpty(error))
				{
					sb?.AppendLine("Error");
					sb?.AppendLine(error);
				}

				if (!string.IsNullOrEmpty(output))
				{
					sb?.AppendLine("Output");
					sb?.AppendLine(output);
				}
			}

			return num;
		}
		catch (Exception ex)
		{
			sb?.AppendLine(ex.ToStringTrace());
			return ReturnValue.Error;
		}
	}
}
