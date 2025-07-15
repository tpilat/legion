using System.Collections.Concurrent;

namespace Legion.ADF.ServiceBus.Services.Internal.Dto;

internal class HostContext
{
	internal const int _heartbeatDelayDeltaInSeconds = 5;

	public Guid? IdHost { get; private set; }
	public bool Started { get; private set;}
	public DTOs.Hosts.HostConfigurationDto HostConfiguration { get; private set; }
	public int HeartbeatInSeconds { get; private set;}
	public int ErrorCount { get; private set;}
	public ServiceBusInstances ServiceBusInstances { get; private set; }
	public ConcurrentDictionary<Guid, JobService> RunningJobs { get; }

	public HostContext()
	{
		RunningJobs = [];
	}

	public void SetIdHost(Guid idHost)
	{
		IdHost = idHost;
	}

	public void SetHostConfiguration(DTOs.Hosts.HostConfigurationDto hostConfiguration)
	{
		HostConfiguration = hostConfiguration;
	}

	public void SetStarted(int heartbeatInSeconds)
	{
		ErrorCount = 0;
		HeartbeatInSeconds = heartbeatInSeconds;
		Started = true;
	}

	public void SetHeartbeatSuccess()
	{
		ErrorCount = 0;
	}

	public void SetServiceBusInstances(ServiceBusInstances serviceBusInstances)
	{
		ServiceBusInstances = serviceBusInstances;
	}

	public void IncrementError()
	{
		ErrorCount++;
	}

	public TimeSpan GetErrorDelay()
		=> HostConfiguration?.GetDelay(ErrorCount) ?? TimeSpan.FromSeconds(DTOs.Hosts.HostConfigurationDto.MAX_TIMEOUT_SECONDS);
}
