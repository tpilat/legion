using Legion.DataWriters;
using Serilog.Core;
using Serilog.Debugging;
using Serilog.Events;

namespace Legion.Logging.Serilog.Sink;

/*
 USAGE:
	Serilog.LoggerConfiguration
		.MinimumLevel.Verbose()
		//.Enrich.WithLogMessage()
		.WriteTo.LegionBatchSink(events => Task.CompletedTask \/* TODO write to output  *\/, new LegionBatchSinkOptions { EagerlyEmitFirstEvent = true })
		.WriteTo.Console())
 */

public class LegionBatchSink : BatchWriter<LogEvent>, ILogEventSink, IDisposable
{
	public LegionBatchSink(
		Func<LogEvent, bool> includeCallBack,
		Func<IEnumerable<LogEvent>, CancellationToken, Task<ulong>> writeBatchCallback,
		IBatchWriterOptions? options,
		Action<string, object?, object?, object?>? errorLogger = null)
		: base(includeCallBack, writeBatchCallback, options, errorLogger ?? SelfLog.WriteLine)
	{
	}

	public void Emit(LogEvent logEvent)
		=> Write(logEvent);
}
