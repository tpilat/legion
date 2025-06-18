using System.Data;

namespace Legion.Database.Readers;

public interface ISqlReader
{
	DataSet LoadAllData(string dbSchemaName, string dbObjectName);
}
