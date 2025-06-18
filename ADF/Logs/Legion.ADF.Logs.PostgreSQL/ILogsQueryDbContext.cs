using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Logs.PostgreSQL;

public interface ILogsQueryDbContext : Legion.EntityFrameworkCore.IDbContext, IDisposable, IAsyncDisposable
{
	DbSet<Legion.ADF.Logs.Model.VwLog> VwLog { get; set; }
}
