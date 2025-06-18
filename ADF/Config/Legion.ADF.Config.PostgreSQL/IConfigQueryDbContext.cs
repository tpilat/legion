using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Config.PostgreSQL;

public interface IConfigQueryDbContext : Legion.EntityFrameworkCore.IDbContext, IDisposable, IAsyncDisposable
{
	DbSet<Legion.ADF.Config.Model.VwConfigurationClass> VwConfigurationClass { get; set; }
}
