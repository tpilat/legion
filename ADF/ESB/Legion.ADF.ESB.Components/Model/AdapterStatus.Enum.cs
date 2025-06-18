namespace Legion.ADF.ESB.Components.Model
{
	public enum AdapterStatusEnum
	{
		Disabled = 1,
		Offline = 2,
		Active = 3,
		Error = 4,
		Suspended = 5
	}

	public static class ExtensionsAdapterStatusEnum
	{
		public static Guid ToGuid(this AdapterStatusEnum @enum)
			=> @enum switch
			{
				AdapterStatusEnum.Disabled => AdapterStatus.Disabled,
				AdapterStatusEnum.Offline => AdapterStatus.Offline,
				AdapterStatusEnum.Active => AdapterStatus.Active,
				AdapterStatusEnum.Error => AdapterStatus.Error,
				AdapterStatusEnum.Suspended => AdapterStatus.Suspended,
				_ => throw new NotSupportedException($"Invalid {nameof(AdapterStatusEnum)} value {@enum}"),
			};
	}
}
