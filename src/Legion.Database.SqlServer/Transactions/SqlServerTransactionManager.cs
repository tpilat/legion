using Legion.Database.Transactions;
using Microsoft.Data.SqlClient;

namespace Legion.Database.SqlServer.Transactions;

public class SqlServerTransactionManager : DbTransactionManager
{
	public SqlServerTransactionManager(SqlTransaction transaction)
		: base(transaction)
	{
	}
}
