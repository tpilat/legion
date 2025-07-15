namespace Legion.ADF.ServiceBus.IntegrationTests;

[Category("ADFServiceBus ReasApi tests")]
public class ADFServiceBus_RestApiTests : TestBase
{
	[Test]
	public async Task ServiceBus_CheckIsDBAlive()
	{
		var idUser = GlobalContext.Instance.NewGuid();

		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var monitor = GetServiceBusMonitor(sp);

		var scopeContext = ScopeContext.Create("TEST ScopeContext")
			.AppendTraceFrameWithIduser(idUser, true);

		var isAlive = await monitor.IsAliveAsync(scopeContext, cancellationToken: default);
		Assert.That(isAlive.ErrorMessages.Count, Is.EqualTo(0));
		Assert.That(isAlive.Data, Is.EqualTo(true));
	}

	[Test]
	public async Task ServiceBus_ShouldGetInstances()
	{
		var idUser = GlobalContext.Instance.NewGuid();

		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var monitor = GetServiceBusMonitor(sp);

		var scopeContext = ScopeContext.Create("TEST ScopeContext")
			.AppendTraceFrameWithIduser(idUser, true);

		var instancesResult = await monitor.GetServiceBusInstancesAsync(scopeContext, cancellationToken: default);
		Assert.That(instancesResult.ErrorMessages.Count, Is.EqualTo(0));
		Assert.That(instancesResult.Data?.Hosts.Count == 1);
		Assert.That(instancesResult.Data?.Jobs.Count == 1);
		Assert.That(instancesResult.Data?.IsDistributedManagerAvailable == true);
	}

	[Test]
	public async Task ServiceBus_ShouldGetHostAndLogs()
	{
		var idUser = GlobalContext.Instance.NewGuid();

		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var monitor = GetServiceBusMonitor(sp);

		var scopeContext = ScopeContext.Create("TEST ScopeContext")
			.AppendTraceFrameWithIduser(idUser, true);

		var instancesResult = await monitor.GetServiceBusInstancesAsync(scopeContext, cancellationToken: default);
		Assert.That(instancesResult.ErrorMessages.Count, Is.EqualTo(0));
		Assert.That(instancesResult.Data?.Hosts.Count == 1);
		Assert.That(instancesResult.Data?.Jobs.Count == 1);
		Assert.That(instancesResult.Data?.IsDistributedManagerAvailable == true);

		var host1 = instancesResult.Data.Hosts[0];
		
		var hostDetailResult = await monitor.GetHostDetailAsync(scopeContext, host1.IdHost, cancellationToken: default);
		Assert.That(hostDetailResult.ErrorMessages.Count, Is.EqualTo(0));
		Assert.That(!string.IsNullOrWhiteSpace(hostDetailResult.Data?.Name));

		var hostLogsResult = await monitor.GetHostLogsAsync(
			scopeContext,
			new DTOs.Hosts.GetHostLogsRequest
			{
				IdHost = host1.IdHost,
				FromUtc = GlobalContext.Instance.UtcNow.AddMonths(-1),
				ToUtc = GlobalContext.Instance.UtcNow.AddMonths(1)
			},
			cancellationToken: default);
		Assert.That(hostLogsResult.ErrorMessages.Count, Is.EqualTo(0));
		Assert.That(0 < hostLogsResult.Data?.Count);
	}

	[Test]
	public async Task ServiceBus_ShouldGetJobStatisticsExecutionsAndLogs()
	{
		var idUser = GlobalContext.Instance.NewGuid();

		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var monitor = GetServiceBusMonitor(sp);

		var scopeContext = ScopeContext.Create("TEST ScopeContext")
			.AppendTraceFrameWithIduser(idUser, true);

		var instancesResult = await monitor.GetServiceBusInstancesAsync(scopeContext, cancellationToken: default);
		Assert.That(instancesResult.ErrorMessages.Count, Is.EqualTo(0));
		Assert.That(instancesResult.Data?.Hosts.Count == 1);
		Assert.That(instancesResult.Data?.Jobs.Count == 1);
		Assert.That(instancesResult.Data?.IsDistributedManagerAvailable == true);

		var job1 = instancesResult.Data.Jobs[0];

		var jobDetailResult = await monitor.GetJobDetailAsync(scopeContext, job1.IdJob, cancellationToken: default);
		Assert.That(jobDetailResult.ErrorMessages.Count, Is.EqualTo(0));
		Assert.That(!string.IsNullOrWhiteSpace(jobDetailResult.Data?.Name));

		var jobStatisticsResult = await monitor.GetJobStatisticsAsync(
			scopeContext,
			new DTOs.Jobs.GetJobStatisticsRequest
			{
				IdJob = job1.IdJob,
				FromUtc = GlobalContext.Instance.UtcNow.AddMonths(-1),
				ToUtc = GlobalContext.Instance.UtcNow.AddMonths(1)
			},
			cancellationToken: default);
		Assert.That(jobStatisticsResult.ErrorMessages.Count, Is.EqualTo(0));
		Assert.That(0 < jobStatisticsResult.Data?.Count);

		var jobExecutionsResult = await monitor.GetJobExecutionsAsync(
			scopeContext,
			new DTOs.Jobs.GetJobExecutionsRequest
			{
				IdJob = job1.IdJob,
				FromUtc = GlobalContext.Instance.UtcNow.AddMonths(-1),
				ToUtc = GlobalContext.Instance.UtcNow.AddMonths(1)
			},
			cancellationToken: default);
		Assert.That(jobExecutionsResult.ErrorMessages.Count, Is.EqualTo(0));
		Assert.That(0 < jobExecutionsResult.Data?.Count);

		var jobLogsResult = await monitor.GetJobLogsAsync(
			scopeContext,
			new DTOs.Jobs.GetJobLogsRequest
			{
				IdJob = job1.IdJob,
				FromUtc = GlobalContext.Instance.UtcNow.AddMonths(-1),
				ToUtc = GlobalContext.Instance.UtcNow.AddMonths(1)
			},
			cancellationToken: default);
		Assert.That(jobLogsResult.ErrorMessages.Count, Is.EqualTo(0));
		Assert.That(0 < jobLogsResult.Data?.Count);
	}
}
