using Legion.DataWriters;

namespace Legion.Database.PostgreSQL;

public interface IDbBatchWriterOptions : IDictionaryTableOptions, IBatchWriterOptions
{
	string? ConnectionString { get; set; }
}
