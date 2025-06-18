namespace Legion.ADF.ESB.Components.Model
{
	public enum JobStatusEnum
	{
		Disabled = 1,
		Offline = 2,
		Running = 3,
		Idle = 4,
		Error = 5,
		Suspended = 6
	}

	public static class ExtensionsJobStatusEnum
	{
		public static Guid ToGuid(this JobStatusEnum @enum)
			=> @enum switch
			{
				JobStatusEnum.Disabled => JobStatus.Disabled,
				JobStatusEnum.Offline => JobStatus.Offline,
				JobStatusEnum.Running => JobStatus.Running,
				JobStatusEnum.Idle => JobStatus.Idle,
				JobStatusEnum.Error => JobStatus.Error,
				JobStatusEnum.Suspended => JobStatus.Suspended,
				_ => throw new NotSupportedException($"Invalid {nameof(JobStatusEnum)} value {@enum}"),
			};
	}
}
