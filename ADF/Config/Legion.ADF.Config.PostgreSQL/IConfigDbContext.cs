using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Legion.ADF.Config.PostgreSQL;

public interface IConfigDbContext : Legion.EntityFrameworkCore.Audit.IAuditableDbContext, Legion.EntityFrameworkCore.IDbContext, IDisposable, IAsyncDisposable
{
	DbSet<Legion.ADF.Config.Model.ConfigurationClass> ConfigurationClass { get; }
	DbSet<Legion.ADF.Config.Model.ConfigurationKeyValue> ConfigurationKeyValue { get; }
}
