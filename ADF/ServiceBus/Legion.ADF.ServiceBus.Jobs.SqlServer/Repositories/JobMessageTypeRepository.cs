using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.ServiceBus.Jobs.SqlServer.Model.Repositories;

public partial class JobMessageTypeRepository : Legion.ADF.ServiceBus.Jobs.SqlServer.JobsRepositoryBase, Legion.ADF.ServiceBus.Jobs.IJobsRepository<Legion.ADF.ServiceBus.Jobs.Model.JobMessageType>, Legion.ADF.ServiceBus.Jobs.Model.Repositories.IJobMessageTypeRepository
{
	private readonly Lazy<Legion.ACL.IAccessControlManager<Legion.ADF.ServiceBus.Jobs.Model.JobMessageType>?> _accessControlManager;

	private Legion.ADF.ServiceBus.Jobs.SqlServer.IJobsDbContext? _context;

	public Legion.ACL.IAccessControlManager<Legion.ADF.ServiceBus.Jobs.Model.JobMessageType>? AccessControlManager => _accessControlManager.Value;

	public JobMessageTypeRepository(IEFConnectionProvider connectionProvider)
		: base(connectionProvider)
	{
		_accessControlManager = new(() => connectionProvider.ServiceProvider.GetService<Legion.ACL.IAccessControlManager<Legion.ADF.ServiceBus.Jobs.Model.JobMessageType>>());
	}

	public IQueryable<Legion.ADF.ServiceBus.Jobs.Model.JobMessageType> AsQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext, false);

	public IQueryable<Legion.ADF.ServiceBus.Jobs.Model.JobMessageType> AsQueryable(IScopeContext scopeContext, bool checkReadPermissions)
	{
		var queryable = (_context ??= ConnectionProvider.GetOrCreateDbContext<Legion.ADF.ServiceBus.Jobs.SqlServer.IJobsDbContext>(scopeContext)).JobMessageType;

		if (checkReadPermissions)
			AccessControlManager?.SetAuthorizationQuery(scopeContext, queryable);

		return queryable;
	}

	public IQueryable<Legion.ADF.ServiceBus.Jobs.Model.JobMessageType> AsReadOnlyQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext).AsNoTracking();

	public IQueryable<Legion.ADF.ServiceBus.Jobs.Model.JobMessageType> AsReadOnlyQueryable(IScopeContext scopeContext, bool checkReadPermissions)
		=> AsQueryable(scopeContext, checkReadPermissions).AsNoTracking();
	
	public void Add(IScopeContext scopeContext, Legion.ADF.ServiceBus.Jobs.Model.JobMessageType entity)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.JobMessageType.Add(entity);
	}

	public async Task AddAsync(
		IScopeContext scopeContext,
		Legion.ADF.ServiceBus.Jobs.Model.JobMessageType entity,
		CancellationToken cancellationToken = default)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		await dbContext.JobMessageType.AddAsync(entity, cancellationToken);
	}

	public void AddRange(IScopeContext scopeContext, IEnumerable<Legion.ADF.ServiceBus.Jobs.Model.JobMessageType> entities)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.JobMessageType.AddRange(entities);
	}

	public async Task AddRangeAsync(
		IScopeContext scopeContext,
		IEnumerable<Legion.ADF.ServiceBus.Jobs.Model.JobMessageType> entities,
		CancellationToken cancellationToken = default)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		await dbContext.JobMessageType.AddRangeAsync(entities, cancellationToken);
	}

	public void Remove(IScopeContext scopeContext, Legion.ADF.ServiceBus.Jobs.Model.JobMessageType entity)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.JobMessageType.Remove(entity);
	}

	public void RemoveRange(
		IScopeContext scopeContext,
		IEnumerable<Legion.ADF.ServiceBus.Jobs.Model.JobMessageType> entities)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.JobMessageType.RemoveRange(entities);
	}


	public ulong BulkInsert(
		IEnumerable<Legion.ADF.ServiceBus.Jobs.Model.JobMessageType> entities,
		bool allowCreateNewDbConnection = false)
	{
		var tableInfo = TableInfoProvider.GetJobMessageTypeTableInfo();

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
