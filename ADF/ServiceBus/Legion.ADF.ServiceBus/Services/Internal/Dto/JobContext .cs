namespace Legion.ADF.ServiceBus.Services.Internal.Dto;

internal class JobContext
{
	public Guid IdJob { get; }
	public string JobName { get; private set; }


	public bool Started { get; private set;}
	public DTOs.Jobs.JobConfigurationDto JobConfiguration { get; private set; }
	public int HeartbeatInSeconds { get; private set;}
	public int ErrorCount { get; private set;}
	public ServiceBusInstances ServiceBusInstances { get; private set; }

	public JobContext(Guid idJob, string jobName)
	{
		Throw.IfArgumentNullOrWhiteSpace(jobName);

		IdJob = idJob;
		JobName = jobName;
	}

	public void UpdateJobInfo(Model.Job job)
	{
		Throw.IfArgumentNull(job);

		JobName = job.Name;
	}

	public void SetJobConfiguration(DTOs.Jobs.JobConfigurationDto jobConfiguration)
	{
		JobConfiguration = jobConfiguration;
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
		=> JobConfiguration?.GetDelay(ErrorCount) ?? TimeSpan.FromSeconds(DTOs.Jobs.JobConfigurationDto.MAX_TIMEOUT_SECONDS);
}
