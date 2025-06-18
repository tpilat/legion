using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Logs.SqlServer;

public interface ILogsQueryDbContext : Legion.EntityFrameworkCore.IDbContext, IDisposable, IAsyncDisposable
{
	DbSet<Legion.ADF.Logs.Model.VwLog> VwLog { get; set; }
}
