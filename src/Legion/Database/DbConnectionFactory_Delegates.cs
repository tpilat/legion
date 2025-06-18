using System.Data.Common;

namespace Legion.Database;

public delegate DbConnection DbConnectionFactory(string connectionId);

public delegate Task<DbConnection> DbConnectionFactoryAsync(
		string connectionId,
		CancellationToken cancellationToken = default);
