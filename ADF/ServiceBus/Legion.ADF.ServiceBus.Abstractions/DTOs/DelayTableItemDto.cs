using Legion.Validation;

namespace Legion.ADF.ServiceBus.DTOs;

public class DelayTableItemDto
{
	public int RetryCount { get; set; }
	public TimeSpan Timeout { get; set; }

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]

	public readonly static Lazy<IValidator<DTOs.DelayTableItemDto>> DefaultValidator = new(() =>
	{
		var builder = new ValidatorBuilder<DelayTableItemDto>();
		RulesBuilder(builder);
		return builder.Build();
	});

	public bool ShouldSerializeDefaultValidator()
		=> false;

	public static void RulesBuilder(ValidatorBuilder<DelayTableItemDto> builder)
		=> builder?
			.ForProperty(x => x.RetryCount, v => v.GreaterThan(0))
			.ForProperty(x => x.Timeout, v => v.GreaterThanOrEqual(TimeSpan.Zero));
}
