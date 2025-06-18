using Legion.Extensions;

namespace Legion.Logging.Serilog;

public static class DefaultErrorLoggerDelegate
{
#pragma warning disable IDE0060 // Remove unused parameter
	public static void Log(string message, object? batchWriter, object? exception, object? @null)
#pragma warning restore IDE0060 // Remove unused parameter
	{
		string msg;
		if (exception is Exception ex)
		{
			msg = string.Format(message, batchWriter, ex.ToStringTrace());
			global::Serilog.Log.Logger.Error(ex, msg);
		}
		else
		{
			msg = string.Format(message, batchWriter, exception);
			global::Serilog.Log.Logger.Error(msg);
		}
	}
}
