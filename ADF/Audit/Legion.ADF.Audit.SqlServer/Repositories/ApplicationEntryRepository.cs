using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Audit.SqlServer.Model.Repositories;

public partial class ApplicationEntryRepository : Legion.ADF.Audit.SqlServer.AuditRepositoryBase, Legion.ADF.Audit.IAuditRepository<Legion.ADF.Audit.Model.ApplicationEntry>, Legion.ADF.Audit.Model.Repositories.IApplicationEntryRepository
{
	private readonly Lazy<Legion.ACL.IAccessControlManager<Legion.ADF.Audit.Model.ApplicationEntry>?> _accessControlManager;

	private Legion.ADF.Audit.SqlServer.IAuditDbContext? _context;

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
		var queryable = (_context ??= ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Audit.SqlServer.IAuditDbContext>(scopeContext)).ApplicationEntry;

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

		var isNewConnection = false;
		var connection = allowCreateNewDbConnection
			? GetOrCreateNewDbConnection(out isNewConnection)
			: GetDbConnection();

		if (connection == null)
			Throw.InvalidOperationException($"{nameof(connection)} == null");

		ulong result = 0;

		try
		{
			var columnTypes = tableInfo.Columns.ToDictionary(x => x.PropertyName, x => x.DatabaseType);

			var rows = entities.Select(e => e.ToDictionary()).ToList();
			var dataTable = tableInfo.ToDataTable(rows);

			using var bulkCopy = new Microsoft.Data.SqlClient.SqlBulkCopy(connection);

			bulkCopy.DestinationTableName = tableInfo.FullTableName;

			foreach (var column in tableInfo.Columns)
				bulkCopy.ColumnMappings.Add(column.PropertyName, column.ColumnName);

			bulkCopy.WriteToServer(dataTable);
		}
		finally
		{
			if (isNewConnection)
				connection.Dispose();
		}

		return result;
	}
}
