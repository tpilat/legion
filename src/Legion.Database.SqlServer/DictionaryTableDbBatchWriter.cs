using Legion.DataWriters;

namespace Legion.Database.SqlServer;

public abstract class DictionaryTableDbBatchWriter<T> : BatchWriter<T>, IDisposable
{
	private readonly string? _connectionString;
	protected DictionaryTableBulkInsert DictionaryTableBulkInsert { get; }

	public DictionaryTableDbBatchWriter(IDbBatchWriterOptions? options, Action<string, object?, object?, object?>? errorLogger = null)
		: base(options, errorLogger)
	{
		Throw.IfArgumentNull(options);

		options.Validate(true, true);

		_connectionString = options.ConnectionString;
		
		Throw.IfNullOrWhiteSpace(_connectionString);

		DictionaryTableBulkInsert = new DictionaryTableBulkInsert(options);
	}

	public abstract IDictionary<string, object?>? ToDictionary(T obj);

	protected override async Task<ulong> WriteBatchAsync(IEnumerable<T> batch, CancellationToken cancellationToken = default)
	{
		var rows = batch?.Where(x => x != null).Select(obj => ToDictionary(obj)).Where(x => x != null).ToList();
		return await DictionaryTableBulkInsert.WriteBatchAsync(rows, _connectionString!, cancellationToken).ConfigureAwait(false);
	}
}
