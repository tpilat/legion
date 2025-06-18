using Legion.Exceptions;
using System.Runtime.CompilerServices;
using System.Text;

namespace Legion;

public class TraceFrameStack
{
	private readonly bool _writeable;
	private readonly List<TraceFrame> _stack;

	public IReadOnlyList<string> Stack { get; }

	public string? LastFrame => Stack.FirstOrDefault();

	internal TraceFrame LastTraceFrame => _stack.First();


	//#if NET8_0_OR_GREATER
	//	[System.Text.Json.Serialization.JsonConstructor]
	//#endif
	//	[Newtonsoft.Json.JsonConstructor]
	//	public TraceFrameStack(IReadOnlyList<string> stack)
	//	{
	//		Throw.ArgumentNullOrEmpty(stack);

	//		_writeable = false;
	//		_stack = [];
	//		Stack = stack ?? [];
	//	}

	public TraceFrameStack(TraceFrame traceFrame)
	{
		Throw.IfArgumentNull(traceFrame);

		_writeable = true;
		_stack = [traceFrame];
		Stack = [traceFrame.Frame];
	}

	public TraceFrameStack(
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		: this(new TraceFrame(memberName, sourceFilePath, sourceLineNumber))
	{
	}

	public TraceFrameStack(
		TraceFrameStack? previousStack,
		TraceFrame traceFrame,
		bool removePreviousSameMethodFrame)
	{
		Throw.IfArgumentNull(traceFrame);
		ArgException.ThrowIf(
			previousStack != null && !previousStack._writeable,
			Exceptions.Internal.ErrorCodes.TraceFrameStack.PreviousNotWriteable);

		_writeable = true;
		if (previousStack != null)
		{
			var newCount = previousStack._stack.Count + 1;
			var copyFrom = 0;

			if (removePreviousSameMethodFrame && 0 < previousStack._stack.Count)
			{
				var previousFrame = previousStack._stack[0];
				var isSameMethod = traceFrame.IsNextOrSameFrameInSameMethod(previousFrame);
				if (isSameMethod)
				{
					newCount = previousStack._stack.Count;
					copyFrom = 1;
				}
			}

			var stack = new List<TraceFrame>(newCount)
			{
				traceFrame
			};

			for (int i = copyFrom; i < previousStack._stack.Count; i++)
				stack.Add(previousStack._stack[i]);

			_stack = stack;
		}
		else
		{
			_stack = [traceFrame];
		}

		Stack = _stack.Select(x => x.Frame).ToList();
	}

	public TraceFrameStack(
		TraceFrameStack? previousStack,
		bool removePreviousSameMethodFrame,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		: this(previousStack, new TraceFrame(memberName, sourceFilePath, sourceLineNumber), removePreviousSameMethodFrame)
	{
	}

	//public TraceFrameStack(
	//	IEnumerable<string> previousStack,
	//	TraceFrame traceFrame)
	//{
	//	Throw.ArgumentNull(traceFrame);

	//	if (previousStack?.Any() == true)
	//	{
	//		var previousCount = previousStack.Count();
	//		var stack = new string[previousCount + 1];
	//		stack[0] = traceFrame.Frame;

	//		int i = 0;
	//		foreach (var previous in previousStack)
	//		{
	//			stack[i + 1] = previous;
	//			i++;
	//		}

	//		_stack = stack;
	//	}
	//	else
	//	{
	//		_stack = [traceFrame.Frame];
	//	}
	//}

	//public TraceFrameStack(
	//	IEnumerable<string> previousStack,
	//	[CallerMemberName] string memberName = "",
	//	[CallerFilePath] string sourceFilePath = "",
	//	[CallerLineNumber] int sourceLineNumber = 0)
	//	: this(previousStack, new TraceFrame(memberName, sourceFilePath, sourceLineNumber))
	//{
	//}

	public TraceFrameStack CreateNext(TraceFrame traceFrame, bool removePreviousSameMethodFrame)
		=> new(this, traceFrame, removePreviousSameMethodFrame);

	public TraceFrameStack CreateNext(
		bool removePreviousSameMethodFrame,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> new(this, new TraceFrame(memberName, sourceFilePath, sourceLineNumber), removePreviousSameMethodFrame);

	public override string? ToString()
		=> string.Join(Environment.NewLine, Stack);

	public string ToStringTrace(string? prefix)
	{
		prefix ??= Environment.NewLine;

		var sb = new StringBuilder();
		foreach (var item in Stack)
			sb.Append(prefix).Append(item);

		return sb.ToString();
	}
}
