namespace Legion.ADF.ESB.MBox.Model
{
	public enum MessageProcessingStatusEnum
	{
		Delivered = 1,
		Processing = 2,
		Processed = 3,
		Terminated = 4
	}

	public static class ExtensionsMessageProcessingStatusEnum
	{
		public static Guid ToGuid(this MessageProcessingStatusEnum @enum)
			=> @enum switch
			{
				MessageProcessingStatusEnum.Delivered => MessageProcessingStatus.Delivered,
				MessageProcessingStatusEnum.Processing => MessageProcessingStatus.Processing,
				MessageProcessingStatusEnum.Processed => MessageProcessingStatus.Processed,
				MessageProcessingStatusEnum.Terminated => MessageProcessingStatus.Terminated,
				_ => throw new NotSupportedException($"Invalid {nameof(MessageProcessingStatusEnum)} value {@enum}"),
			};
	}
}
