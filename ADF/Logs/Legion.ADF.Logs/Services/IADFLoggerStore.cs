using System.Data.Common;

namespace Legion.ADF.Logs.Services;

public interface IADFLoggerStore
{
	int SaveLog(
		IScopeContext scopeContext,
		DbConnection dbConnection,
		Logs.Model.Log log);

	int SaveUnstructuredLog(
		IScopeContext scopeContext,
		DbConnection dbConnection,
		Logs.Model.UnstructuredLog unstructuredLog);
}
