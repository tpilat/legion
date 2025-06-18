using Legion.Exceptions;
using Legion.Logging;
using System.Runtime.ExceptionServices;

namespace Legion.Extensions;

public static class ExceptionExtensions
{
	private const string Legion_LOG_MESSAGE = nameof(Legion_LOG_MESSAGE);

	public static string ToStringTrace(this Exception ex)
	{
		return ExceptionHelper.ToStringTrace(ex);
	}

	/// <summary>
	/// Rethrows the extended <see cref="Exception"/>, <paramref name="exceptionPossiblyToThrow"/>, using the <see cref="ExceptionDispatchInfo"/> class to rethrow it with its original stack trace, if <paramref name="exceptionPossiblyToThrow"/> differs from <paramref name="exceptionToCompare"/>.
	/// </summary>
	/// <param name="exceptionPossiblyToThrow">The exception to throw, if it differs from <paramref name="exceptionToCompare"/></param>
	/// <param name="exceptionToCompare">The exception to compare against.</param>
	public static void RethrowWithOriginalStackTraceIfDiffersFrom(this Exception exceptionPossiblyToThrow, Exception exceptionToCompare)
	{
		if (exceptionPossiblyToThrow != exceptionToCompare)
		{
			ExceptionDispatchInfo.Capture(exceptionPossiblyToThrow).Throw();
		}
	}

	public static T AddErrorMessage<T>(this T exception, ILogMessage logMessage)
		where T : Exception
	{
		Throw.IfArgumentNull(exception);

		if (logMessage != null)
			exception.Data[GetNewKey()] = logMessage;

		return exception;
	}

	private static string GetNewKey()
		=> $"{Legion_LOG_MESSAGE}_{Guid.NewGuid()}";

	public static IEnumerable<ILogMessage> GetLogMessages(this Exception exception)
	{
		Throw.IfArgumentNull(exception);

		var logMessageKeys = exception.Data.Keys.OfType<string>().Where(x => x.StartsWith(Legion_LOG_MESSAGE)).ToList();

		if (logMessageKeys.Count == 0)
		{
			yield break;
		}
		else
		{
			foreach (var logMessageKey in logMessageKeys)
			{
				if (exception.Data[logMessageKey] is ILogMessage logMessage)
					yield return logMessage;
			}
		}
	}
}
