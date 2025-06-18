using Legion;
using Legion.DataWriters;
using Legion.Logging.Serilog.Sink;
using Serilog.Configuration;
using Serilog.Events;

namespace Serilog;

public static class SerilogExtensions
{
	public static LoggerConfiguration LegionBatchSink(
		this LoggerSinkConfiguration loggerConfiguration,
		Func<IEnumerable<LogEvent>, CancellationToken, Task<ulong>> writeBatchCallback,
		LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum)
		=> LegionBatchSink(
			loggerConfiguration,
			writeBatchCallback,
			null,
			restrictedToMinimumLevel);

	public static LoggerConfiguration LegionBatchSink(
		this LoggerSinkConfiguration loggerConfiguration,
		Func<IEnumerable<LogEvent>, CancellationToken, Task<ulong>> writeBatchCallback,
		IBatchWriterOptions? options,
		LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum)
		=> LegionBatchSink(
			loggerConfiguration,
			e => true,
			writeBatchCallback,
			options,
			restrictedToMinimumLevel);

	public static LoggerConfiguration LegionBatchSink(
		this LoggerSinkConfiguration loggerConfiguration,
		Func<LogEvent, bool> includeCallBack,
		Func<IEnumerable<LogEvent>, CancellationToken, Task<ulong>> writeBatchCallback,
		IBatchWriterOptions? options,
		LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum)
	{
		Throw.IfArgumentNull(loggerConfiguration);

		var sink = new LegionBatchSink(
			includeCallBack,
			writeBatchCallback,
			options);

		return loggerConfiguration.Sink(sink, restrictedToMinimumLevel);
	}
}
