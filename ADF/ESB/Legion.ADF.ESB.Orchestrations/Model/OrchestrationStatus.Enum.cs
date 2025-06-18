namespace Legion.ADF.ESB.Orchestrations.Model
{
	public enum OrchestrationStatusEnum
	{
		Disabled = 1,
		Offline = 2,
		Running = 3,
		Error = 4,
		Succeeded = 5,
		Suspended = 6
	}

	public static class ExtensionsOrchestrationStatusEnum
	{
		public static Guid ToGuid(this OrchestrationStatusEnum @enum)
			=> @enum switch
			{
				OrchestrationStatusEnum.Disabled => OrchestrationStatus.Disabled,
				OrchestrationStatusEnum.Offline => OrchestrationStatus.Offline,
				OrchestrationStatusEnum.Running => OrchestrationStatus.Running,
				OrchestrationStatusEnum.Error => OrchestrationStatus.Error,
				OrchestrationStatusEnum.Succeeded => OrchestrationStatus.Succeeded,
				OrchestrationStatusEnum.Suspended => OrchestrationStatus.Suspended,
				_ => throw new NotSupportedException($"Invalid {nameof(OrchestrationStatusEnum)} value {@enum}"),
			};
	}
}
