using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.ServiceBus.SqlServer.Model.Repositories;

public partial class JobStatisticsRepository : Legion.ADF.ServiceBus.SqlServer.ServiceBusRepositoryBase, Legion.ADF.ServiceBus.IServiceBusRepository<Legion.ADF.ServiceBus.Model.JobStatistics>, Legion.ADF.ServiceBus.Model.Repositories.IJobStatisticsRepository
{
	private readonly Lazy<Legion.ACL.IAccessControlManager<Legion.ADF.ServiceBus.Model.JobStatistics>?> _accessControlManager;

	private Legion.ADF.ServiceBus.SqlServer.IServiceBusDbContext? _context;

	public Legion.ACL.IAccessControlManager<Legion.ADF.ServiceBus.Model.JobStatistics>? AccessControlManager => _accessControlManager.Value;

	public JobStatisticsRepository(IEFConnectionProvider connectionProvider)
		: base(connectionProvider)
	{
		_accessControlManager = new(() => connectionProvider.ServiceProvider.GetService<Legion.ACL.IAccessControlManager<Legion.ADF.ServiceBus.Model.JobStatistics>>());
	}

	public IQueryable<Legion.ADF.ServiceBus.Model.JobStatistics> AsQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext, false);

	public IQueryable<Legion.ADF.ServiceBus.Model.JobStatistics> AsQueryable(IScopeContext scopeContext, bool checkReadPermissions)
	{
		var queryable = (_context ??= ConnectionProvider.GetOrCreateDbContext<Legion.ADF.ServiceBus.SqlServer.IServiceBusDbContext>(scopeContext)).JobStatistics;

		if (checkReadPermissions)
			AccessControlManager?.SetAuthorizationQuery(scopeContext, queryable);

		return queryable;
	}

	public IQueryable<Legion.ADF.ServiceBus.Model.JobStatistics> AsReadOnlyQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext).AsNoTracking();

	public IQueryable<Legion.ADF.ServiceBus.Model.JobStatistics> AsReadOnlyQueryable(IScopeContext scopeContext, bool checkReadPermissions)
		=> AsQueryable(scopeContext, checkReadPermissions).AsNoTracking();
	
	public Legion.ADF.ServiceBus.Queries.JobStatistics.IGetJobStatisticsByJobId GetJobStatisticsByJobId(
		Legion.ADF.ServiceBus.Queries.JobStatistics.GetJobStatisticsByJobIdQuery getJobStatisticsById)
		=> new Legion.ADF.ServiceBus.Queries.JobStatistics.GetJobStatisticsByJobId(
			ConnectionProvider,
			getJobStatisticsById);

	public Legion.ADF.ServiceBus.Queries.JobStatistics.IGetJobStatisticsByJobIdAndStartHour GetJobStatisticsByJobIdAndStartHour(
		Legion.ADF.ServiceBus.Queries.JobStatistics.GetJobStatisticsByJobIdAndStartHourQuery getJobStatisticsByIdAndStartHour)
		=> new Legion.ADF.ServiceBus.Queries.JobStatistics.GetJobStatisticsByJobIdAndStartHour(
			ConnectionProvider,
			getJobStatisticsByIdAndStartHour);

	public void Add(IScopeContext scopeContext, Legion.ADF.ServiceBus.Model.JobStatistics entity)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.JobStatistics.Add(entity);
	}

	public async Task AddAsync(
		IScopeContext scopeContext,
		Legion.ADF.ServiceBus.Model.JobStatistics entity,
		CancellationToken cancellationToken = default)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		await dbContext.JobStatistics.AddAsync(entity, cancellationToken);
	}

	public void AddRange(IScopeContext scopeContext, IEnumerable<Legion.ADF.ServiceBus.Model.JobStatistics> entities)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.JobStatistics.AddRange(entities);
	}

	public async Task AddRangeAsync(
		IScopeContext scopeContext,
		IEnumerable<Legion.ADF.ServiceBus.Model.JobStatistics> entities,
		CancellationToken cancellationToken = default)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		await dbContext.JobStatistics.AddRangeAsync(entities, cancellationToken);
	}

	public void Remove(IScopeContext scopeContext, Legion.ADF.ServiceBus.Model.JobStatistics entity)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.JobStatistics.Remove(entity);
	}

	public void RemoveRange(
		IScopeContext scopeContext,
		IEnumerable<Legion.ADF.ServiceBus.Model.JobStatistics> entities)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.JobStatistics.RemoveRange(entities);
	}


	public ulong BulkInsert(
		IEnumerable<Legion.ADF.ServiceBus.Model.JobStatistics> entities,
		bool allowCreateNewDbConnection = false)
	{
		var tableInfo = TableInfoProvider.GetJobStatisticsTableInfo();

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
