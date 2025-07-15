using Legion.Extensions;
using Legion.Logging;
using Legion.MessageBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Legion.ADF.Logs.IntegrationTests;

[Category("ADFLogger LogMessage tests")]
public class ADFLoggerTests_LogMessage : TestBase
{
	[Test]
	public async Task ILogger_ShouldLogErrorMessage()
	{
		var idApplicationEntry = GlobalContext.Instance.NewGuid();
		var component = "com1";
		var sourceSystemName = "TEST ScopeContext";
		var correlationId = GlobalContext.Instance.NewGuid();
		var externalCorrelationId = "EXT_CORR";
		var customCorrelationId = "CUSTOM_CORR";
		var contextPropertyKey = "cpKey";
		var contextPropertyValue = "cpValue";
		var contextProperties = $"{{\"{contextPropertyKey}\":\"{contextPropertyValue}\"}}";
		var idUser = GlobalContext.Instance.NewGuid();
		var tenantIdentifier = GlobalContext.Instance.NewGuid();
		var propertyName = "My Property name";
		var displayPropertyName = "display my Property name";

		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var scopeContext = ScopeContext.Create(sourceSystemName, correlationId: correlationId, externalCorrelationId: externalCorrelationId)
			.AppendTraceFrameWithComponent(component, true)
			.AppendTraceFrameWithIdApplicationEntry(idApplicationEntry, true)
			.AppendTraceFrameWithCustomCorrelationId(customCorrelationId, true)
			.AddContextProperty(contextPropertyKey, contextPropertyValue)
			.AppendTraceFrameWithIduser(idUser, true)
			.AppendTraceFrameWithTenantIdentifier(tenantIdentifier, true);

		var loggerFactory = sp.GetRequiredService<ILoggerFactory>();

		//var loggerName = "TEST-ovaci Logger";
		//var logger = loggerFactory.CreateLogger(loggerName);

		var loggerName = typeof(ADFLoggerTests_LogMessage).FullName;
		var logger = loggerFactory.CreateLogger<ADFLoggerTests_LogMessage>();

		var internalMessage = "test internal message";
		var detail = "de ta il";
		var clientMessage = "CLIENT msg";
		var aggregateIdentifier = GlobalContext.Instance.NewGuid().ToString();
		var aggregateName = "agregaat";

		var exception = new Exception(internalMessage);
		var stackTrace = exception.ToStringTrace();

		var error = ErrorMessage.CreateErrorMessage(
			scopeContext,
			Exceptions.Internal.ErrorCodes.UserException.InvalidNormalizedLogin,
			x => x//.InternalMessage(internalMessage)
				.ExceptionInfo(exception)
				.Detail(detail)
				.ClientMessage(clientMessage)
				.AggregateIdentifier(aggregateIdentifier)
				.AggregateName(aggregateName)
				.PropertyName(propertyName)
				.DisplayPropertyName(displayPropertyName, true)
			);

		logger.LogErrorMessage(error);

		await Task.Delay(100);

		var query = new Logs.Queries.Log.GetLogByIdQuery(error.IdLogMessage, CheckReadPermissions: true, AsNoTracking: true);
		var result = await messageBus.SendAsync(scopeContext, query);

		Assert.That(!result.HasError);
		Assert.That(result.Data != null);
		Assert.That(result.Data!.IdLogLevel, Is.EqualTo((int)LogLevel.Error), nameof(result.Data.IdLogLevel));
		Assert.That(result.Data!.SourceContext, Is.EqualTo(loggerName), nameof(result.Data.SourceContext));
		Assert.That(result.Data!.InternalMessage, Is.EqualTo(internalMessage), nameof(result.Data.InternalMessage));
		Assert.That(result.Data!.StackTrace, Does.StartWith(stackTrace), nameof(result.Data.StackTrace));
		Assert.That(result.Data!.Detail, Is.EqualTo(detail), nameof(result.Data.Detail));
		Assert.That(result.Data!.ClientMessage, Is.EqualTo(clientMessage), nameof(result.Data.ClientMessage));
		Assert.That(result.Data!.Component, Is.EqualTo(component), nameof(result.Data.Component));
		Assert.That(result.Data!.LogCode, Is.Not.Null, nameof(result.Data.LogCode));
		Assert.That(result.Data!.SourceSystemName, Is.EqualTo(sourceSystemName), nameof(result.Data.SourceSystemName));
		Assert.That(result.Data!.AggregateIdentifier, Is.EqualTo(aggregateIdentifier), nameof(result.Data.AggregateIdentifier));
		Assert.That(result.Data!.AggregateName, Is.EqualTo(aggregateName), nameof(result.Data.AggregateName));
		Assert.That(result.Data!.IdApplicationEntry, Is.EqualTo(idApplicationEntry), nameof(result.Data.IdApplicationEntry));
		Assert.That(result.Data!.CorrelationId, Is.EqualTo(correlationId), nameof(result.Data.CorrelationId));
		Assert.That(result.Data!.ExternalCorrelationId, Is.EqualTo(externalCorrelationId), nameof(result.Data.ExternalCorrelationId));
		Assert.That(result.Data!.CustomCorrelationId, Is.EqualTo(customCorrelationId), nameof(result.Data.CustomCorrelationId));
		Assert.That(result.Data!.ContextProperties, Is.EqualTo(contextProperties), nameof(result.Data.ContextProperties));
		Assert.That(result.Data!.TenantIdentifier, Is.EqualTo(tenantIdentifier), nameof(result.Data.TenantIdentifier));
		Assert.That(result.Data!.IdUser, Is.EqualTo(idUser), nameof(result.Data.IdUser));
		Assert.That(result.Data!.PropertyName, Is.EqualTo(propertyName), nameof(result.Data.PropertyName));
		Assert.That(result.Data!.DisplayPropertyName, Is.EqualTo(displayPropertyName), nameof(result.Data.DisplayPropertyName));
		Assert.That(result.Data!.RuntimeUniqueKey, Is.EqualTo(Infrastructure.EnvironmentInfo.RUNTIME_UNIQUE_KEY), nameof(result.Data.RuntimeUniqueKey));
	}
}
