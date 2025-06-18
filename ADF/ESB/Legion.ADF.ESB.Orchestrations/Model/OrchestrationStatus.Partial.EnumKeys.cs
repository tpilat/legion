namespace Legion.ADF.ESB.Orchestrations.Model;

public partial class OrchestrationStatus : Orchestrations.OrchestrationsBaseEntity, Legion.Model.IEntity
{
	/// <summary>
	/// 00000001-0000-0000-0000-000000000000
	/// </summary>
	public static Guid Disabled { get; }

	/// <summary>
	/// 00000002-0000-0000-0000-000000000000
	/// </summary>
	public static Guid Offline { get; }

	/// <summary>
	/// 00000003-0000-0000-0000-000000000000
	/// </summary>
	public static Guid Running { get; }

	/// <summary>
	/// 00000004-0000-0000-0000-000000000000
	/// </summary>
	public static Guid Error { get; }

	/// <summary>
	/// 00000005-0000-0000-0000-000000000000
	/// </summary>
	public static Guid Succeeded { get; }

	/// <summary>
	/// 00000006-0000-0000-0000-000000000000
	/// </summary>
	public static Guid Suspended { get; }

	static OrchestrationStatus()
	{
		Disabled = new Guid("00000001-0000-0000-0000-000000000000");
		Offline = new Guid("00000002-0000-0000-0000-000000000000");
		Running = new Guid("00000003-0000-0000-0000-000000000000");
		Error = new Guid("00000004-0000-0000-0000-000000000000");
		Succeeded = new Guid("00000005-0000-0000-0000-000000000000");
		Suspended = new Guid("00000006-0000-0000-0000-000000000000");

		DefaultDBValidator = SetDBValidatorRules(new Legion.Validation.ValidatorBuilder<OrchestrationStatus>()).Build();
	}

	public static IEnumerable<Guid> AsEnumerable()
	{
		yield return Disabled;
		yield return Offline;
		yield return Running;
		yield return Error;
		yield return Succeeded;
		yield return Suspended;
	}

	public OrchestrationStatusEnum ToEnum()
	{
		if (IdOrchestrationStatus == Disabled)
			return OrchestrationStatusEnum.Disabled;

		if (IdOrchestrationStatus == Offline)
			return OrchestrationStatusEnum.Offline;

		if (IdOrchestrationStatus == Running)
			return OrchestrationStatusEnum.Running;

		if (IdOrchestrationStatus == Error)
			return OrchestrationStatusEnum.Error;

		if (IdOrchestrationStatus == Succeeded)
			return OrchestrationStatusEnum.Succeeded;

		if (IdOrchestrationStatus == Suspended)
			return OrchestrationStatusEnum.Suspended;

		Legion.Throw.NotSupportedException($"Invalid {nameof(IdOrchestrationStatus)} value {IdOrchestrationStatus}");

		return 0;
	}

	public static implicit operator OrchestrationStatusEnum(OrchestrationStatus status)
		=> status?.ToEnum() ?? 0;

	public static implicit operator OrchestrationStatus(OrchestrationStatusEnum @enum)
		=> @enum switch
		{
			OrchestrationStatusEnum.Disabled => Disabled_NewObject,
			OrchestrationStatusEnum.Offline => Offline_NewObject,
			OrchestrationStatusEnum.Running => Running_NewObject,
			OrchestrationStatusEnum.Error => Error_NewObject,
			OrchestrationStatusEnum.Succeeded => Succeeded_NewObject,
			OrchestrationStatusEnum.Suspended => Suspended_NewObject,
			_ => throw new NotSupportedException($"Invalid {nameof(OrchestrationStatusEnum)} value {@enum}"),
		};
}
