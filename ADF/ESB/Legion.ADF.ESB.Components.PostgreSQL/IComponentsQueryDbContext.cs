using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.ESB.Components.PostgreSQL;

public interface IComponentsQueryDbContext : Legion.EntityFrameworkCore.IDbContext, IDisposable, IAsyncDisposable
{
	DbSet<Legion.ADF.ESB.Components.Model.VwJob> VwJob { get; set; }
}
