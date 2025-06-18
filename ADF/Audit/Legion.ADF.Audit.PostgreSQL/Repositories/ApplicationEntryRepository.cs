using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Audit.PostgreSQL.Model.Repositories;

public partial class ApplicationEntryRepository : Legion.ADF.Audit.PostgreSQL.AuditRepositoryBase, Legion.ADF.Audit.IAuditRepository<Legion.ADF.Audit.Model.ApplicationEntry>, Legion.ADF.Audit.Model.Repositories.IApplicationEntryRepository
{
	private readonly Lazy<Legion.ACL.IAccessControlManager<Legion.ADF.Audit.Model.ApplicationEntry>?> _accessControlManager;

	private Legion.ADF.Audit.PostgreSQL.IAuditDbContext? _context;

	public Legion.ACL.IAccessControlManager<Legion.ADF.Audit.Model.ApplicationEntry>? AccessControlManager => _accessControlManager.Value;

	public ApplicationEntryRepository(IEFConnectionProvider connectionProvider)
		: base(connectionProvider)
	{
		_accessControlManager = new(() => connectionProvider.ServiceProvider.GetService<Legion.ACL.IAccessControlManager<Legion.ADF.Audit.Model.ApplicationEntry>>());
	}

	public IQueryable<Legion.ADF.Audit.Model.ApplicationEntry> AsQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext, false);

	public IQueryable<Legion.ADF.Audit.Model.ApplicationEntry> AsQueryable(IScopeContext scopeContext, bool checkReadPermissions)
	{
		var queryable = (_context ??= ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Audit.PostgreSQL.IAuditDbContext>(scopeContext)).ApplicationEntry;

		if (checkReadPermissions)
			AccessControlManager?.SetAuthorizationQuery(scopeContext, queryable);

		return queryable;
	}

	public IQueryable<Legion.ADF.Audit.Model.ApplicationEntry> AsReadOnlyQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext).AsNoTracking();

	public IQueryable<Legion.ADF.Audit.Model.ApplicationEntry> AsReadOnlyQueryable(IScopeContext scopeContext, bool checkReadPermissions)
		=> AsQueryable(scopeContext, checkReadPermissions).AsNoTracking();
	
	public void Add(IScopeContext scopeContext, Legion.ADF.Audit.Model.ApplicationEntry entity)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.ApplicationEntry.Add(entity);
	}

	public async Task AddAsync(
		IScopeContext scopeContext,
		Legion.ADF.Audit.Model.ApplicationEntry entity,
		CancellationToken cancellationToken = default)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		await dbContext.ApplicationEntry.AddAsync(entity, cancellationToken);
	}

	public void AddRange(IScopeContext scopeContext, IEnumerable<Legion.ADF.Audit.Model.ApplicationEntry> entities)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.ApplicationEntry.AddRange(entities);
	}

	public async Task AddRangeAsync(
		IScopeContext scopeContext,
		IEnumerable<Legion.ADF.Audit.Model.ApplicationEntry> entities,
		CancellationToken cancellationToken = default)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		await dbContext.ApplicationEntry.AddRangeAsync(entities, cancellationToken);
	}

	public void Remove(IScopeContext scopeContext, Legion.ADF.Audit.Model.ApplicationEntry entity)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.ApplicationEntry.Remove(entity);
	}

	public void RemoveRange(
		IScopeContext scopeContext,
		IEnumerable<Legion.ADF.Audit.Model.ApplicationEntry> entities)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.ApplicationEntry.RemoveRange(entities);
	}


	public ulong BulkInsert(
		IEnumerable<Legion.ADF.Audit.Model.ApplicationEntry> entities,
		bool allowCreateNewDbConnection = false)
	{
		var tableInfo = TableInfoProvider.GetApplicationEntryTableInfo();
		string sql = $"COPY {tableInfo.FullTableName} ({tableInfo.CommaSeparatedColumns}) FROM STDIN (FORMAT BINARY)";

		var isNewConnection = false;
		var npgsqlConnection = allowCreateNewDbConnection
			? GetOrCreateNewDbConnection(out isNewConnection)
			: GetDbConnection();

		if (npgsqlConnection == null)
			Throw.InvalidOperationException($"{nameof(npgsqlConnection)} == null");

		Npgsql.NpgsqlBinaryImporter? writer = null;
		ulong result = 0;

		try
		{
			var columnTypes = tableInfo.Columns.ToDictionary(x => x.PropertyName, x => x.DatabaseType);

			writer = npgsqlConnection.BeginBinaryImport(sql);
		
			foreach (var entity in entities)
			{
				writer.StartRow();
				var entityDict = entity.ToDictionary();

				foreach (var kvp in entityDict)
					writer.Write(kvp.Value, columnTypes[kvp.Key]);
			}

			result = writer.Complete();
		}
		finally
		{
			if (isNewConnection)
			{
				writer?.Dispose();
				npgsqlConnection.Dispose();
			}
		}

		return result;
	}

	public async Task<ulong> BulkInsertAsync(
		IEnumerable<Legion.ADF.Audit.Model.ApplicationEntry> entities,
		bool allowCreateNewDbConnection = false,
		CancellationToken cancellationToken = default)
	{
		var tableInfo = TableInfoProvider.GetApplicationEntryTableInfo();
		string sql = $"COPY {tableInfo.FullTableName} ({tableInfo.CommaSeparatedColumns}) FROM STDIN (FORMAT BINARY)";

		var isNewConnection = false;
		var npgsqlConnection = allowCreateNewDbConnection
			? GetOrCreateNewDbConnection(out isNewConnection)
			: GetDbConnection();

		if (npgsqlConnection == null)
			Throw.InvalidOperationException($"{nameof(npgsqlConnection)} == null");

		Npgsql.NpgsqlBinaryImporter? writer = null;
		ulong result = 0;

		try
		{
			var columnTypes = tableInfo.Columns.ToDictionary(x => x.PropertyName, x => x.DatabaseType);

			writer = npgsqlConnection.BeginBinaryImport(sql);

			foreach (var entity in entities)
			{
				await writer.StartRowAsync(cancellationToken).ConfigureAwait(false);
				var entityDict = entity.ToDictionary();

				foreach (var kvp in entityDict)
					await writer.WriteAsync(kvp.Value, columnTypes[kvp.Key], cancellationToken).ConfigureAwait(false);
			}

			result = await writer.CompleteAsync(cancellationToken).ConfigureAwait(false);
		}
		finally
		{
			if (isNewConnection)
			{
				if (writer != null)
					await writer.DisposeAsync();

				await npgsqlConnection.DisposeAsync();
			}
		}

		return result;
	}
}
