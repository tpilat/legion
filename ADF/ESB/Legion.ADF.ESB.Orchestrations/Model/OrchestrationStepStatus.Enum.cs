namespace Legion.ADF.ESB.Orchestrations.Model
{
	public enum OrchestrationStepStatusEnum
	{
		Idle = 1,
		Running = 2,
		Error = 3,
		Succeeded = 4,
		Suspended = 5,
		Skipped = 6
	}

	public static class ExtensionsOrchestrationStepStatusEnum
	{
		public static Guid ToGuid(this OrchestrationStepStatusEnum @enum)
			=> @enum switch
			{
				OrchestrationStepStatusEnum.Idle => OrchestrationStepStatus.Idle,
				OrchestrationStepStatusEnum.Running => OrchestrationStepStatus.Running,
				OrchestrationStepStatusEnum.Error => OrchestrationStepStatus.Error,
				OrchestrationStepStatusEnum.Succeeded => OrchestrationStepStatus.Succeeded,
				OrchestrationStepStatusEnum.Suspended => OrchestrationStepStatus.Suspended,
				OrchestrationStepStatusEnum.Skipped => OrchestrationStepStatus.Skipped,
				_ => throw new NotSupportedException($"Invalid {nameof(OrchestrationStepStatusEnum)} value {@enum}"),
			};
	}
}
