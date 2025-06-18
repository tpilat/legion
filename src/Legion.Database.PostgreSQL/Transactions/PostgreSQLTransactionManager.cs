using Legion.Database.Transactions;
using Npgsql;

namespace Legion.Database.PostgreSQL.Transactions;

public class PostgreSQLTransactionManager : DbTransactionManager
{
	public PostgreSQLTransactionManager(NpgsqlTransaction transaction)
		: base(transaction)
	{
	}
}
