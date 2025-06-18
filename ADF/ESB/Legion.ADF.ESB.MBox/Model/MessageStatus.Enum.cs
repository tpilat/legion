namespace Legion.ADF.ESB.MBox.Model
{
	public enum MessageStatusEnum
	{
		Published = 1,
		Delivered = 2,
		CannotDeliver = 3,
		Dropped = 4
	}

	public static class ExtensionsMessageStatusEnum
	{
		public static Guid ToGuid(this MessageStatusEnum @enum)
			=> @enum switch
			{
				MessageStatusEnum.Published => MessageStatus.Published,
				MessageStatusEnum.Delivered => MessageStatus.Delivered,
				MessageStatusEnum.CannotDeliver => MessageStatus.CannotDeliver,
				MessageStatusEnum.Dropped => MessageStatus.Dropped,
				_ => throw new NotSupportedException($"Invalid {nameof(MessageStatusEnum)} value {@enum}"),
			};
	}
}
