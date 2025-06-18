using Legion.Model;
using System.Runtime.CompilerServices;
using System.Text;

namespace Legion;

public sealed class TraceFrame : ValueObject
{
	private readonly string _memberName;
	private readonly string _sourceFilePath;
	private readonly int _sourceLineNumber;

	public string Frame { get; }

	public TraceFrame(
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
	{
		_memberName = memberName;
		_sourceFilePath = sourceFilePath;
		_sourceLineNumber = sourceLineNumber;

		var empty = true;
		var sb = new StringBuilder();

		if (!string.IsNullOrWhiteSpace(sourceFilePath))
		{
			var callerFileName = sourceFilePath!.Trim().EndsWith(".cs", StringComparison.InvariantCultureIgnoreCase)
				? $"{Directory.GetParent(sourceFilePath)?.Name}\\{Path.GetFileName(sourceFilePath)}"
				: sourceFilePath;

			sb.Append(callerFileName);
			empty = false;
		}

		if (!string.IsNullOrWhiteSpace(memberName))
		{
			if (empty)
				sb.Append(memberName);
			else
				sb.Append(" > ").Append(memberName);
		}

		if (0 < sourceLineNumber)
			sb.Append(" <<r.").Append(sourceLineNumber).Append(">>");

		Frame = sb.ToString();
	}

	public override string? ToString()
		=> Frame;

	protected override IEnumerable<object> GetAtomicValues()
	{
		yield return Frame;
	}

	public bool IsNextOrSameFrameInSameMethod(TraceFrame previousFrame)
	{
		if (previousFrame == null)
			return false;

		return
			_memberName == previousFrame._memberName
			&& _sourceFilePath == previousFrame._sourceFilePath
			&& previousFrame._sourceLineNumber <= _sourceLineNumber;
	}

	public static implicit operator string(TraceFrame traceFrame) => traceFrame.Frame;
}
