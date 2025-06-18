namespace Legion.ADF.ESB.MBox.Model;

public partial class MessageStatus : MBox.MBoxBaseEntity, Legion.Model.IEntity
{
	/// <summary>
	/// 00000001-0000-0000-0000-000000000000
	/// </summary>
	public static Guid Published { get; }

	/// <summary>
	/// 00000002-0000-0000-0000-000000000000
	/// </summary>
	public static Guid Delivered { get; }

	/// <summary>
	/// 00000003-0000-0000-0000-000000000000
	/// </summary>
	public static Guid CannotDeliver { get; }

	/// <summary>
	/// 00000004-0000-0000-0000-000000000000
	/// </summary>
	public static Guid Dropped { get; }

	static MessageStatus()
	{
		Published = new Guid("00000001-0000-0000-0000-000000000000");
		Delivered = new Guid("00000002-0000-0000-0000-000000000000");
		CannotDeliver = new Guid("00000003-0000-0000-0000-000000000000");
		Dropped = new Guid("00000004-0000-0000-0000-000000000000");

		DefaultDBValidator = SetDBValidatorRules(new Legion.Validation.ValidatorBuilder<MessageStatus>()).Build();
	}

	public static IEnumerable<Guid> AsEnumerable()
	{
		yield return Published;
		yield return Delivered;
		yield return CannotDeliver;
		yield return Dropped;
	}

	public MessageStatusEnum ToEnum()
	{
		if (IdMessageStatus == Published)
			return MessageStatusEnum.Published;

		if (IdMessageStatus == Delivered)
			return MessageStatusEnum.Delivered;

		if (IdMessageStatus == CannotDeliver)
			return MessageStatusEnum.CannotDeliver;

		if (IdMessageStatus == Dropped)
			return MessageStatusEnum.Dropped;

		Legion.Throw.NotSupportedException($"Invalid {nameof(IdMessageStatus)} value {IdMessageStatus}");

		return 0;
	}

	public static implicit operator MessageStatusEnum(MessageStatus status)
		=> status?.ToEnum() ?? 0;

	public static implicit operator MessageStatus(MessageStatusEnum @enum)
		=> @enum switch
		{
			MessageStatusEnum.Published => Published_NewObject,
			MessageStatusEnum.Delivered => Delivered_NewObject,
			MessageStatusEnum.CannotDeliver => CannotDeliver_NewObject,
			MessageStatusEnum.Dropped => Dropped_NewObject,
			_ => throw new NotSupportedException($"Invalid {nameof(MessageStatusEnum)} value {@enum}"),
		};
}
