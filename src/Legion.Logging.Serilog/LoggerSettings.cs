using Serilog.Core;

namespace Legion.Logging.Serilog;

public static class LoggerSettings
{
	public static readonly LoggingLevelSwitch LevelSwitch = new(global::Serilog.Events.LogEventLevel.Information);
}
