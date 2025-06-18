using System.Text;

namespace Legion.Generators.AppGen;

public enum ResultSeverity
{
	Error = 0,
	Warning = 1,
	Message = 2
}

public class ResultMessage
{
	public int Id { get; set; }
	public ResultSeverity Severity { get; set; }
	public string Source { get; set; }
	public string Message { get; set; }

	public ResultMessage(ResultSeverity severity, string source, string message)
		: this(0, severity, source, message)
	{
	}

	public ResultMessage(int id, ResultSeverity severity, string source, string message)
	{
		if (string.IsNullOrWhiteSpace(message))
			throw new ArgumentNullException(nameof(message));

		Id = id;
		Severity = severity;
		Source = source;
		Message = message;
	}
}

public class ModelResult
{
	private int _counter = 0;

	public List<ResultMessage> Messages { get; }
	public List<ResultMessage> Warnings { get; }
	public List<ResultMessage> Errors { get; }

	public ModelResult()
	{
		Messages = new List<ResultMessage>();
		Warnings = new List<ResultMessage>();
		Errors = new List<ResultMessage>();
	}

	public List<ResultMessage> GetAllMessages()
	{
		var result = new List<ResultMessage>();
		result.AddRange(Errors);
		result.AddRange(Warnings);
		result.AddRange(Messages);
		return result;
	}

	public bool HasError
		=> 0 < Errors.Count;

	public bool HasWarning
		=> 0 < Warnings.Count;

	public bool HasInfo
		=> 0 < Messages.Count;

	public bool HasAnyMessage
		=> HasError || HasWarning || HasInfo;

	public bool IsOK()
		=> Warnings.Count == 0 && Errors.Count == 0;

	public ModelResult MergeAllMessages(ModelResult result)
	{
		if (result != null)
		{
			Messages.AddRange(result.Messages);
			Warnings.AddRange(result.Warnings);
			Errors.AddRange(result.Errors);
		}

		return this;
	}

	public bool MergeHasError(ModelResult result)
	{
		if (result != null)
		{
			MergeAllMessages(result);
			return 0 < result.Errors.Count;
		}

		return false;
	}

	public ModelResult AddMessage(ResultMessage message)
	{
		if (message != null)
		{
			message.Severity = ResultSeverity.Message;
			message.Id = Interlocked.Increment(ref _counter);
			Messages.Add(message);
		}
		return this;
	}

	public ModelResult AddMessage(string source, string message)
	{
		Messages.Add(new ResultMessage(Interlocked.Increment(ref _counter), ResultSeverity.Message, source, message));
		return this;
	}

	public ModelResult AddWarning(ResultMessage warning)
	{
		if (warning != null)
		{
			warning.Severity = ResultSeverity.Warning;
			warning.Id = Interlocked.Increment(ref _counter);
			Warnings.Add(warning);
		}
		return this;
	}

	public ModelResult AddWarning(string source, string warningMessage)
	{
		Warnings.Add(new ResultMessage(Interlocked.Increment(ref _counter), ResultSeverity.Warning, source, warningMessage));
		return this;
	}

	public ModelResult AddError(ResultMessage error)
	{
		if (error != null)
		{
			error.Severity = ResultSeverity.Error;
			error.Id = Interlocked.Increment(ref _counter);
			Errors.Add(error);
		}
		return this;
	}

	public ModelResult AddError(string source, string errorMessage)
	{
		Errors.Add(new ResultMessage(Interlocked.Increment(ref _counter), ResultSeverity.Error, source, errorMessage));
		return this;
	}

	public string Print()
	{
		StringBuilder sb = new StringBuilder();
		foreach (var error in Errors)
			sb.AppendLine($"ERROR: {error.Message} in {error.Source}");
		foreach (var warning in Warnings)
			sb.AppendLine($"WARNING: {warning.Message} in {warning.Source}");
		foreach (var messge in Messages)
			sb.AppendLine($"MESSAGE: {messge.Message} in {messge.Source}");

		return sb.ToString();
	}
}
