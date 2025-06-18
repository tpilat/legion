using Legion.DataWriters;

namespace Legion.Database.SqlServer;

public interface IDbBatchWriterOptions : IDictionaryTableOptions, IBatchWriterOptions
{
	string? ConnectionString { get; set; }
}
