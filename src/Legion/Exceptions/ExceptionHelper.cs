using Legion.Extensions;
using Legion.Logging;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Legion.Exceptions;

public static class ExceptionHelper
{
	public static string ToStringTrace(Exception ex)
	{
		if (ex == null)
			return "";

		var sb = new StringBuilder(ex.ToString());

		if (ex is System.Reflection.ReflectionTypeLoadException rtlEx && 0 < rtlEx.LoaderExceptions.Length)
		{
			sb.AppendLine();
			sb.AppendLine("--- LoaderExceptions ---");
			foreach (Exception? exSub in rtlEx.LoaderExceptions)
			{
				if (exSub != null)
				{
					sb.AppendLine(exSub.ToString());

					if (exSub is FileNotFoundException exFileNotFound)
						if (!string.IsNullOrEmpty(exFileNotFound.FusionLog))
						{
							sb.AppendLine("Fusion Log:");
							sb.AppendLine(exFileNotFound.FusionLog);
						}

					sb.AppendLine();
				}
			}
		}

		if (ex.Data != null && 0 < ex.Data.Count)
		{
			sb.AppendLine();
			sb.AppendLine("--- DATA ---");

			var jsonSerializerSettings = new JsonSerializerSettings
			{
				Formatting = Formatting.Indented,
				ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
				PreserveReferencesHandling = PreserveReferencesHandling.Objects, //PreserveReferencesHandling.All,
				TypeNameHandling = TypeNameHandling.All,
				MaxDepth = 255
			};

			foreach (var item in ex.Data.Keys)
			{
				var obj = ex.Data[item];
				string key = "";
				string value = "";
				try
				{
					key = JsonConvert.SerializeObject(item, jsonSerializerSettings);
				}
				catch { }
				try
				{
					value = JsonConvert.SerializeObject(obj, jsonSerializerSettings);
				}
				catch { }

				sb.AppendLine($"{key}: {value}");
			}
		}

		try
		{
			var serializeFaultExceptionDelegate = CONFIGURATION.Current.SerializeFaultExceptionDelegate;
			serializeFaultExceptionDelegate?.Invoke(sb, ex);
		}
		catch { }

		return sb.ToString();
	}

	[return: NotNullIfNotNull(nameof(logMessage))]
	public static Exception? ToException(ILogMessage logMessage, bool withDetail)
	{
		if (logMessage == null)
			return null;

		if (logMessage.Exception != null)
			return logMessage.Exception;

		var exception = new Exception(logMessage.ToString(true, true, withDetail));
		exception.AddErrorMessage(logMessage);
		return exception;
	}

	public static TException? ToException<TException>(List<ILogMessage> logMessages, Func<string, TException> exceptionFactory, bool withDetail)
		where TException : Exception
	{
		if (logMessages == null || logMessages.Count == 0)
			return null;

		//if (logMessage.Exception != null && logMessage.Exception is TException tException)
		//	return tException;

		var sb = new StringBuilder();
		foreach (var logMessage in logMessages)
			sb.AppendLine(logMessage.ToString(true, true, withDetail));

		var exception = exceptionFactory(sb.ToString());

		foreach (var logMessage in logMessages)
			exception.AddErrorMessage(logMessage);

		return exception;
	}

	/// <summary>
	/// Returns null if no Error message found
	/// </summary>
	public static ResultException? ToException(
		IScopeContext scopeContext,
		IErrorCode? errorCode,
		IResult result,
		bool dataMustBeNotNull,
		bool withErrorMessageDetails)
	{
		if (result == null || (dataMustBeNotNull && !result.HasErrorOrNullData) || (!dataMustBeNotNull && !result.HasError))
			return null;

		if (0 < result.ErrorMessages?.Count)
		{
			var exception = ToException(result.ErrorMessages.Cast<ILogMessage>().ToList(), msg => new ResultException(errorCode, msg), withErrorMessageDetails);
			return exception;
		}
		else if (dataMustBeNotNull && result.HasErrorOrNullData)
		{
			var exception = new ResultException(null, "Result has no data");
			return exception;
		}

		return null;
	}
}
