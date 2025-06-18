using Legion.ADF.Logs.Settings;
using Legion.Database;
using Legion.DataWriters;
using Microsoft.Extensions.Options;

namespace Legion.ADF.Logs.Services;

public class BatchUnstructuredLogStore : BatchWriter<DTOs.UnstructuredLog>, IDisposable
{
	private readonly ITableInfoBulkInsert<Model.UnstructuredLog> _tableInfoBulkInsert;

	public BatchUnstructuredLogStore(
		ConnectionStringProvider connectionStringProvider,
		ITableInfoProvider tableInfoProvider,
		ITableInfoBulkInsertFactory tableInfoBulkInsertFactory,
		IOptions<BatchUnstructuredLogStoreOptions> options)
		: base(options?.Value, errorLogger: null)
	{
		Throw.IfArgumentNull(connectionStringProvider);
		Throw.IfArgumentNull(tableInfoProvider);
		Throw.IfArgumentNull(tableInfoBulkInsertFactory);

		var connectionString = connectionStringProvider.GetDefaultConncetionString();

		Throw.IfNullOrWhiteSpace(connectionString);

		_tableInfoBulkInsert = tableInfoBulkInsertFactory.Create<Model.UnstructuredLog>(tableInfoProvider, log => log.ToDictionary(), connectionString);
	}

	protected override Task<ulong> WriteBatchAsync(
		IEnumerable<DTOs.UnstructuredLog> batch,
		CancellationToken cancellationToken = default)
	{
		if (batch == null || batch.Any() == false)
			return Task.FromResult((ulong)0);

		var scopeContext = ScopeContext.Create(nameof(BatchUnstructuredLogStore));

		var entities = batch.Select(unstructuredLog =>
		{
			var createResult = Model.UnstructuredLog.CreateUnstructuredLog(scopeContext, unstructuredLog, null);
			createResult.ThrowIfErrorOrNullData(scopeContext, null, true);
			return createResult.Data!;
		});

		var result = _tableInfoBulkInsert.BulkInsert(entities, true);
		return Task.FromResult(result);
	}

	private bool _disposed;
	protected override void Dispose(bool disposing)
	{
		base.Dispose(disposing);

		if (_disposed)
			return;

		_disposed = true;

		if (disposing)
			_tableInfoBulkInsert.Dispose();
	}
}
