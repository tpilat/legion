using Legion.Extensions;
using System.Text;

namespace Legion.Database.PostgreSQL.Deploy;

internal class Logger
{
	private readonly StringBuilder _sb;
	public bool HasError { get; private set; }


	public Logger()
	{
		_sb = new StringBuilder();
	}

	public void LogNewLine()
	{
		Console.WriteLine();
		_sb.AppendLine();
	}

	public bool LogInfo(string message)
	{
		if (!string.IsNullOrWhiteSpace(message))
		{
			//var msg = $"{DateTime.Now:dd.MM.yyyy HH.mm.ss} - INFO: {message}";
			var msg = message;
			Console.WriteLine(msg);
			_sb.AppendLine(msg);
			return true;
		}

		return false;
	}

	public bool LogNotice(string message)
	{
		if (!string.IsNullOrWhiteSpace(message))
		{
			//var msg = $"{DateTime.Now:dd.MM.yyyy HH.mm.ss} - NOTICE: {message}";
			var msg = message;
			Console.WriteLine(msg);
			_sb.AppendLine(msg);
			return true;
		}

		return false;
	}

	public bool LogSuccess(string message)
	{
		if (!string.IsNullOrWhiteSpace(message))
		{
			Console.ForegroundColor = ConsoleColor.Green;
			//var msg = $"{DateTime.Now:dd.MM.yyyy HH.mm.ss} - SUCCESS: {message}";
			var msg = message;
			Console.WriteLine(msg);
			_sb.AppendLine(msg);
			Console.ForegroundColor = ConsoleColor.White;
			return true;
		}

		return false;
	}

	public bool LogWarning(string message)
	{
		if (!string.IsNullOrWhiteSpace(message))
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			//var msg = $"{DateTime.Now:dd.MM.yyyy HH.mm.ss} - SUCCESS: {message}";
			var msg = message;
			Console.WriteLine(msg);
			_sb.AppendLine(msg);
			Console.ForegroundColor = ConsoleColor.White;
			return true;
		}

		return false;
	}

	public bool LogError(string message)
	{
		if (!string.IsNullOrWhiteSpace(message))
		{
			//var msg = $"{DateTime.Now:dd.MM.yyyy HH.mm.ss} - ERROR: {message}";
			var msg = message;
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine(msg);
			_sb.AppendLine(msg);
			Console.ForegroundColor = ConsoleColor.White;
			HasError = true;
			return true;
		}

		return false;
	}

	public bool LogError(string message, Exception ex)
	{
		if (LogError(message))
		{
			if (ex != null)
			{
				var msg = ex.ToStringTrace();
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine(msg);
				_sb.AppendLine(msg);
				Console.ForegroundColor = ConsoleColor.White;
				HasError = true;
			}

			return true;
		}
		else
		{
			return LogError(ex);
		}
	}

	public bool LogError(Exception ex)
	{
		if (ex != null)
		{
			//var msg = $"{DateTime.Now:dd.MM.yyyy HH.mm.ss} - ERROR: {ex.ToStringTrace()}";
			var msg = ex.ToStringTrace();
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine(msg);
			_sb.AppendLine(msg);
			Console.ForegroundColor = ConsoleColor.White;
			HasError = true;
			return true;
		}

		return false;
	}

	public override string ToString()
		=> _sb.ToString();
}
