using Legion.EntityFrameworkCore.Database;
using Legion.EntityFrameworkCore.Extensions;
using Legion.EntityFrameworkCore.PostgreSQL.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Legion.ADF.Auditing.PostgreSQL;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddAuditingContexts(this IServiceCollection services)
	{
		services.AddPostgreSQLConnectionProvider();
		services.AddUnitOfWork();
		services.AddDbContext<AuditDbContext>(options => options.AddRowNumberSupport(), ServiceLifetime.Transient);
		services.TryAddTransient<IAuditDbContext, AuditDbContext>();
		services.AddDbContext<AuditQueryDbContext>(options => options.AddRowNumberSupport(), ServiceLifetime.Transient);
		services.TryAddTransient<IAuditQueryDbContext, AuditQueryDbContext>();
		return services;
	}
}
