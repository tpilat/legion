using Legion.Caching;
using Legion.Database.PostgreSQL;
using Legion.Locks;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using System.Text;

namespace Legion.ADF.Cache.IntegrationTests;

public abstract class TestBase
{
	internal static Legion.Exceptions.ErrorCode TestErrorCode
		=> new("test", "test", "test");

	internal static string GetDatetimeTicks()
		=> GlobalContext.Instance.Now.Ticks.ToString();

	[SetUp]
	public void SetupTest()
	{
		var encoding = new UTF8Encoding(false);
		var baseDir = AppDomain.CurrentDomain.BaseDirectory;
		string? executeResult;

		using var connection = new NpgsqlConnection(SetUp.ConncetionString);
		connection.Open();

		executeResult = SqlScript.Execute(connection, File.ReadAllText(Path.Combine(baseDir, "DB", "truncate_all.sql"), encoding), true);
		if (!string.IsNullOrWhiteSpace(executeResult))
			Throw.InvalidOperationException(executeResult);

		executeResult = SqlScript.Execute(connection, File.ReadAllText(Path.Combine(baseDir, "DB", "Logs", "truncate_all.sql"), encoding), true);
		if (!string.IsNullOrWhiteSpace(executeResult))
			Throw.InvalidOperationException(executeResult);

		SetupTestInternal();
	}

	public ISimplePersistentCache GetSimplePersistentCache(IServiceProvider sp)
		=> sp.GetRequiredService<ISimplePersistentCache>();

	public IDistributedLockProvider GetDistributedLockProvider(IServiceProvider sp)
		=> sp.GetRequiredService<IDistributedLockProvider>();

	[TearDown]
	public async Task TearDownTest()
	{
		GC.Collect();
		GC.WaitForPendingFinalizers();

		var objectsLifetimes = Trackers.ObjectLifetimeTracker.GetObjectsLifetimeStatus();

		Assert.That(objectsLifetimes.AliveCount, Is.EqualTo(0), $"Alived objects count: {objectsLifetimes.AliveCount}");
	}

	protected virtual void SetupTestInternal()
	{
	}
}
