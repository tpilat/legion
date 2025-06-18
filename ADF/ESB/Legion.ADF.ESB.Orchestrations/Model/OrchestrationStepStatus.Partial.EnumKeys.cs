namespace Legion.ADF.ESB.Orchestrations.Model;

public partial class OrchestrationStepStatus : Orchestrations.OrchestrationsBaseEntity, Legion.Model.IEntity
{
	/// <summary>
	/// 00000001-0000-0000-0000-000000000000
	/// </summary>
	public static Guid Idle { get; }

	/// <summary>
	/// 00000002-0000-0000-0000-000000000000
	/// </summary>
	public static Guid Running { get; }

	/// <summary>
	/// 00000003-0000-0000-0000-000000000000
	/// </summary>
	public static Guid Error { get; }

	/// <summary>
	/// 00000004-0000-0000-0000-000000000000
	/// </summary>
	public static Guid Succeeded { get; }

	/// <summary>
	/// 00000005-0000-0000-0000-000000000000
	/// </summary>
	public static Guid Suspended { get; }

	/// <summary>
	/// 00000006-0000-0000-0000-000000000000
	/// </summary>
	public static Guid Skipped { get; }

	static OrchestrationStepStatus()
	{
		Idle = new Guid("00000001-0000-0000-0000-000000000000");
		Running = new Guid("00000002-0000-0000-0000-000000000000");
		Error = new Guid("00000003-0000-0000-0000-000000000000");
		Succeeded = new Guid("00000004-0000-0000-0000-000000000000");
		Suspended = new Guid("00000005-0000-0000-0000-000000000000");
		Skipped = new Guid("00000006-0000-0000-0000-000000000000");

		DefaultDBValidator = SetDBValidatorRules(new Legion.Validation.ValidatorBuilder<OrchestrationStepStatus>()).Build();
	}

	public static IEnumerable<Guid> AsEnumerable()
	{
		yield return Idle;
		yield return Running;
		yield return Error;
		yield return Succeeded;
		yield return Suspended;
		yield return Skipped;
	}

	public OrchestrationStepStatusEnum ToEnum()
	{
		if (IdOrchestrationStepStatus == Idle)
			return OrchestrationStepStatusEnum.Idle;

		if (IdOrchestrationStepStatus == Running)
			return OrchestrationStepStatusEnum.Running;

		if (IdOrchestrationStepStatus == Error)
			return OrchestrationStepStatusEnum.Error;

		if (IdOrchestrationStepStatus == Succeeded)
			return OrchestrationStepStatusEnum.Succeeded;

		if (IdOrchestrationStepStatus == Suspended)
			return OrchestrationStepStatusEnum.Suspended;

		if (IdOrchestrationStepStatus == Skipped)
			return OrchestrationStepStatusEnum.Skipped;

		Legion.Throw.NotSupportedException($"Invalid {nameof(IdOrchestrationStepStatus)} value {IdOrchestrationStepStatus}");

		return 0;
	}

	public static implicit operator OrchestrationStepStatusEnum(OrchestrationStepStatus status)
		=> status?.ToEnum() ?? 0;

	public static implicit operator OrchestrationStepStatus(OrchestrationStepStatusEnum @enum)
		=> @enum switch
		{
			OrchestrationStepStatusEnum.Idle => Idle_NewObject,
			OrchestrationStepStatusEnum.Running => Running_NewObject,
			OrchestrationStepStatusEnum.Error => Error_NewObject,
			OrchestrationStepStatusEnum.Succeeded => Succeeded_NewObject,
			OrchestrationStepStatusEnum.Suspended => Suspended_NewObject,
			OrchestrationStepStatusEnum.Skipped => Skipped_NewObject,
			_ => throw new NotSupportedException($"Invalid {nameof(OrchestrationStepStatusEnum)} value {@enum}"),
		};
}
