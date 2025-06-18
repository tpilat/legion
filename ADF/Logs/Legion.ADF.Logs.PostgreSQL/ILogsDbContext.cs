using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Legion.ADF.Logs.PostgreSQL;

public interface ILogsDbContext : Legion.EntityFrameworkCore.Audit.IAuditableDbContext, Legion.EntityFrameworkCore.IDbContext, IDisposable, IAsyncDisposable
{
	DbSet<Legion.ADF.Logs.Model.EnvironmentInfo> EnvironmentInfo { get; }
	DbSet<Legion.ADF.Logs.Model.EventCounter> EventCounter { get; }
	DbSet<Legion.ADF.Logs.Model.EventCounterCategory> EventCounterCategory { get; }
	DbSet<Legion.ADF.Logs.Model.EventCounterData> EventCounterData { get; }
	DbSet<Legion.ADF.Logs.Model.LocalRequest> LocalRequest { get; }
	DbSet<Legion.ADF.Logs.Model.LocalRequestPayload> LocalRequestPayload { get; }
	DbSet<Legion.ADF.Logs.Model.LocalResponse> LocalResponse { get; }
	DbSet<Legion.ADF.Logs.Model.LocalResponsePayload> LocalResponsePayload { get; }
	DbSet<Legion.ADF.Logs.Model.Log> Log { get; }
	DbSet<Legion.ADF.Logs.Model.LogLevel> LogLevel { get; }
	DbSet<Legion.ADF.Logs.Model.RemoteRequest> RemoteRequest { get; }
	DbSet<Legion.ADF.Logs.Model.RemoteRequestPayload> RemoteRequestPayload { get; }
	DbSet<Legion.ADF.Logs.Model.RemoteResponse> RemoteResponse { get; }
	DbSet<Legion.ADF.Logs.Model.RemoteResponsePayload> RemoteResponsePayload { get; }
	DbSet<Legion.ADF.Logs.Model.RemoteSystem> RemoteSystem { get; }
	DbSet<Legion.ADF.Logs.Model.UnstructuredLog> UnstructuredLog { get; }
}
