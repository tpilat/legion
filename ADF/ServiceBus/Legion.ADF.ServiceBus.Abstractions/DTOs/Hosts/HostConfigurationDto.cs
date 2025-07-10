using Legion.Extensions;
using Legion.Validation;
using Legion.Validation.Results;

namespace Legion.ADF.ServiceBus.DTOs.Hosts;

public class HostConfigurationDto
{
	public const int MAX_TIMEOUT_SECONDS = 60;

	public int HeartbeatInSeconds { get; set; }
	public List<DelayTableItemDto> ErrorDelayTable { get; set; }

	public TimeSpan GetDelay(int count)
	{
		if (ErrorDelayTable == null || ErrorDelayTable.Count == 0)
			return TimeSpan.FromSeconds(MAX_TIMEOUT_SECONDS);

		var result = TimeSpan.FromSeconds(MAX_TIMEOUT_SECONDS);
		int? bestDelta = null;
		foreach (var item in ErrorDelayTable)
		{
			var delta = Math.Abs(item.RetryCount - count);
			if (bestDelta.HasValue)
			{
				if ((delta < bestDelta.Value)
					|| (delta == bestDelta.Value && item.Timeout < result))
				{
					bestDelta = delta;
					result = item.Timeout;
				}
			}
			else
			{
				bestDelta = delta;
				result = item.Timeout;
			}
		}

		return result;
	}



	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]

	public readonly static Lazy<IValidator<HostConfigurationDto>> DefaultValidator = new(() => SetValidatorRules(new ValidatorBuilder<HostConfigurationDto>()).Build());

	public bool ShouldSerializeDefaultValidator()
		=> false;

	public static ValidatorBuilder<HostConfigurationDto> SetValidatorRules(ValidatorBuilder<HostConfigurationDto> builder)
		=> builder
			.ForProperty(x => x.HeartbeatInSeconds, v => v.GreaterThan(0).LessThanOrEqual(MAX_TIMEOUT_SECONDS))
			.ForProperty(x => x.ErrorDelayTable, v => v.NotDefaultOrEmpty())
			.ForEach(x => x.ErrorDelayTable, DelayTableItemDto.RulesBuilder)
			.WithPropertyError(
				x => x.ErrorDelayTable, (obj, parent) =>
				{
					if (obj?.ErrorDelayTable == null)
						return ValidationResultFactory.Success();

					if (obj.ErrorDelayTable.HasDuplicates(x => x?.RetryCount))
					{
						return ValidationResultFactory.Failure(
							obj,
							x => x.ErrorDelayTable,
							objectPathIndexes: null,
							Exceptions.Internal.ErrorCodes.HostConfigurationException.DuplicatedRetryCount,
							nameof(ErrorDelayTable),
							"Duplicated retry count");
					}
					else
					{
						return ValidationResultFactory.Success();
					}
				});
}
