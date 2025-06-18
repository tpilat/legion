namespace Legion.ADF.ESB.MBox.Model;

public partial class MessageProcessingStatus : MBox.MBoxBaseEntity, Legion.Model.IEntity
{
	/// <summary>
	/// 00000001-0000-0000-0000-000000000000
	/// </summary>
	public static Guid Delivered { get; }

	/// <summary>
	/// 00000002-0000-0000-0000-000000000000
	/// </summary>
	public static Guid Processing { get; }

	/// <summary>
	/// 00000003-0000-0000-0000-000000000000
	/// </summary>
	public static Guid Processed { get; }

	/// <summary>
	/// 00000004-0000-0000-0000-000000000000
	/// </summary>
	public static Guid Terminated { get; }

	static MessageProcessingStatus()
	{
		Delivered = new Guid("00000001-0000-0000-0000-000000000000");
		Processing = new Guid("00000002-0000-0000-0000-000000000000");
		Processed = new Guid("00000003-0000-0000-0000-000000000000");
		Terminated = new Guid("00000004-0000-0000-0000-000000000000");

		DefaultDBValidator = SetDBValidatorRules(new Legion.Validation.ValidatorBuilder<MessageProcessingStatus>()).Build();
	}

	public static IEnumerable<Guid> AsEnumerable()
	{
		yield return Delivered;
		yield return Processing;
		yield return Processed;
		yield return Terminated;
	}

	public MessageProcessingStatusEnum ToEnum()
	{
		if (IdMessageProcessingStatus == Delivered)
			return MessageProcessingStatusEnum.Delivered;

		if (IdMessageProcessingStatus == Processing)
			return MessageProcessingStatusEnum.Processing;

		if (IdMessageProcessingStatus == Processed)
			return MessageProcessingStatusEnum.Processed;

		if (IdMessageProcessingStatus == Terminated)
			return MessageProcessingStatusEnum.Terminated;

		Legion.Throw.NotSupportedException($"Invalid {nameof(IdMessageProcessingStatus)} value {IdMessageProcessingStatus}");

		return 0;
	}

	public static implicit operator MessageProcessingStatusEnum(MessageProcessingStatus status)
		=> status?.ToEnum() ?? 0;

	public static implicit operator MessageProcessingStatus(MessageProcessingStatusEnum @enum)
		=> @enum switch
		{
			MessageProcessingStatusEnum.Delivered => Delivered_NewObject,
			MessageProcessingStatusEnum.Processing => Processing_NewObject,
			MessageProcessingStatusEnum.Processed => Processed_NewObject,
			MessageProcessingStatusEnum.Terminated => Terminated_NewObject,
			_ => throw new NotSupportedException($"Invalid {nameof(MessageProcessingStatusEnum)} value {@enum}"),
		};
}
