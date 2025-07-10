namespace Legion.ADF.ServiceBus.DTOs.Jobs;

public enum JobRunTypeEnum
{
	SequentialTimer = 1,
	PeriodicTimer = 2,
	Cron = 3
}
