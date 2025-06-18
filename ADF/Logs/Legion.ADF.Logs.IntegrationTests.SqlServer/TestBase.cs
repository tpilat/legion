using Legion.Database.SqlServer;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

namespace Legion.ADF.Logs.IntegrationTests;

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

		using var connection = new SqlConnection(SetUp.ConncetionString);
		connection.Open();

		executeResult = SqlScript.Execute(connection, File.ReadAllText(Path.Combine(baseDir, "DB", "truncate_all.sql"), encoding), true);
		if (!string.IsNullOrWhiteSpace(executeResult))
			Throw.InvalidOperationException(executeResult);

		//executeResult = SqlScript.Execute(connection, File.ReadAllText(Path.Combine(baseDir, "DB", "data_initial.sql"), encoding), true);
		//if (!string.IsNullOrWhiteSpace(executeResult))
		//	Throw.InvalidOperationException(executeResult);

		SetupTestInternal();
	}

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

	protected ILogsUnitOfWork CreateLogsUnitOfWork(IScopeContext scopeContext, IServiceProvider serviceProvider)
	{
		Throw.IfArgumentNull(scopeContext);
		Throw.IfArgumentNull(serviceProvider);

		var connectionStringProvider = serviceProvider.GetRequiredService<ConnectionStringProvider>();
		var uowFactory = serviceProvider.GetRequiredService<ILogsUnitOfWorkFactory>();
		var uow = uowFactory.Create(
			serviceProvider,
			connectionStringProvider.GetDefaultConncetionString(),
			isolationLevel: null,
			false,
			false);

		return uow;
	}
}
