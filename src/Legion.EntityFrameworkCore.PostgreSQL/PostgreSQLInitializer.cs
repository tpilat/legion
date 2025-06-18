namespace Legion.EntityFrameworkCore.PostgreSQL;

public static class PostgreSQLInitializer
{
	private static bool _initialized = false;

	private static readonly object _initLock = new();
	[Obsolete("Do not use PostgreSQLInitializer.Init(). Use correct postgresql datetime dbType - for UTC use datetime with time zone", true)]
	public static void Init()
	{
		if (_initialized)
			return;

		lock (_initLock)
		{
			if (_initialized)
				return;

			_initialized = true;

			AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
		}
	}
}
