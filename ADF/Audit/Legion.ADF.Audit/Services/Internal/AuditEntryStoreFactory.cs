using Legion.ADF.Audit.Settings;
using Legion.Database;
using Legion.Model.Audit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Legion.ADF.Audit.Services.Internal;

internal class AuditEntryStoreFactory : IAuditEntryStoreFactory
{
	public IAuditEntryStore Create(IConnectionProvider connectionProvider)
	{
		Throw.IfArgumentNull(connectionProvider);

		var auditStoreOptions = connectionProvider.ServiceProvider.GetRequiredService<IOptions<AuditStoreOptions>>();
		var logger = connectionProvider.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger<AuditStore>();

		return new AuditStore(connectionProvider, auditStoreOptions, logger);
	}
}
