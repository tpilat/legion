namespace Legion.ADF.ESB.Components.Model
{
	public enum JobTypeEnum
	{
		SequentialIntervalTimer = 1,
		ExactPeriodicTimer = 2,
		CronTimer = 3
	}

	public static class ExtensionsJobTypeEnum
	{
		public static Guid ToGuid(this JobTypeEnum @enum)
			=> @enum switch
			{
				JobTypeEnum.SequentialIntervalTimer => JobType.SequentialIntervalTimer,
				JobTypeEnum.ExactPeriodicTimer => JobType.ExactPeriodicTimer,
				JobTypeEnum.CronTimer => JobType.CronTimer,
				_ => throw new NotSupportedException($"Invalid {nameof(JobTypeEnum)} value {@enum}"),
			};
	}
}
