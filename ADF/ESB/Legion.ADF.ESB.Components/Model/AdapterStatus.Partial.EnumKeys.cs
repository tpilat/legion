namespace Legion.ADF.ESB.Components.Model;

public partial class AdapterStatus : Components.ComponentsBaseEntity, Legion.Model.IEntity
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
	public static Guid Active { get; }

	/// <summary>
	/// 00000004-0000-0000-0000-000000000000
	/// </summary>
	public static Guid Error { get; }

	/// <summary>
	/// 00000005-0000-0000-0000-000000000000
	/// </summary>
	public static Guid Suspended { get; }

	static AdapterStatus()
	{
		Disabled = new Guid("00000001-0000-0000-0000-000000000000");
		Offline = new Guid("00000002-0000-0000-0000-000000000000");
		Active = new Guid("00000003-0000-0000-0000-000000000000");
		Error = new Guid("00000004-0000-0000-0000-000000000000");
		Suspended = new Guid("00000005-0000-0000-0000-000000000000");

		DefaultDBValidator = SetDBValidatorRules(new Legion.Validation.ValidatorBuilder<AdapterStatus>()).Build();
	}

	public static IEnumerable<Guid> AsEnumerable()
	{
		yield return Disabled;
		yield return Offline;
		yield return Active;
		yield return Error;
		yield return Suspended;
	}

	public AdapterStatusEnum ToEnum()
	{
		if (IdAdapterStatus == Disabled)
			return AdapterStatusEnum.Disabled;

		if (IdAdapterStatus == Offline)
			return AdapterStatusEnum.Offline;

		if (IdAdapterStatus == Active)
			return AdapterStatusEnum.Active;

		if (IdAdapterStatus == Error)
			return AdapterStatusEnum.Error;

		if (IdAdapterStatus == Suspended)
			return AdapterStatusEnum.Suspended;

		Legion.Throw.NotSupportedException($"Invalid {nameof(IdAdapterStatus)} value {IdAdapterStatus}");

		return 0;
	}

	public static implicit operator AdapterStatusEnum(AdapterStatus status)
		=> status?.ToEnum() ?? 0;

	public static implicit operator AdapterStatus(AdapterStatusEnum @enum)
		=> @enum switch
		{
			AdapterStatusEnum.Disabled => Disabled_NewObject,
			AdapterStatusEnum.Offline => Offline_NewObject,
			AdapterStatusEnum.Active => Active_NewObject,
			AdapterStatusEnum.Error => Error_NewObject,
			AdapterStatusEnum.Suspended => Suspended_NewObject,
			_ => throw new NotSupportedException($"Invalid {nameof(AdapterStatusEnum)} value {@enum}"),
		};
}
