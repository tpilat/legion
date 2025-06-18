using Legion.Extensions;
using Legion.MessageBus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Legion.ADF.Logs.IntegrationTests;

[Category("ADFLogger UnstructuredLog tests")]
public class ADFLoggerTests_UnstructuredLog : TestBase
{
	[Test]
	public async Task ILogger_ShouldLogUnstructuredError()
	{
		var sourceSystemName = "TEST ScopeContext";

		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var scopeContext = ScopeContext.Create(sourceSystemName);

		var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
		
		var loggerName = "TEST-ovaci Logger";
		var logger = loggerFactory.CreateLogger(loggerName);

		//var loggerName = typeof(ADFLoggerTests).FullName;
		//var logger = loggerFactory.CreateLogger<ADFLoggerTests>();

		var internalMessage = "test internal message";
		var exception = new Exception(internalMessage);
		var stackTrace = exception.ToStringTrace();

		var message = "moj custom error";
		logger.LogError(exception, message);

		await Task.Delay(100);

		await using var uow = CreateLogsUnitOfWork(scopeContext, sp);
		var unstructuredLog = await uow.UnstructuredLogRepository
			.AsQueryable(scopeContext, checkReadPermissions: true)
			.Where(ul => ul.Message == message)
			.FirstOrDefaultAsync(cancellationToken: default);

		Assert.That(unstructuredLog, Is.Not.EqualTo(null));
		Assert.That(unstructuredLog!.IdLogLevel, Is.EqualTo((int)LogLevel.Error), nameof(unstructuredLog.IdLogLevel));
		Assert.That(unstructuredLog!.Message, Is.EqualTo(message), nameof(unstructuredLog.Message));
		Assert.That(unstructuredLog!.StackTrace, Is.EqualTo(stackTrace), nameof(unstructuredLog.StackTrace));
		Assert.That(unstructuredLog!.SourceContext, Is.EqualTo(loggerName), nameof(unstructuredLog.SourceContext));
		Assert.That(unstructuredLog!.RuntimeUniqueKey, Is.EqualTo(Infrastructure.EnvironmentInfo.RUNTIME_UNIQUE_KEY), nameof(unstructuredLog.RuntimeUniqueKey));
	}
}
