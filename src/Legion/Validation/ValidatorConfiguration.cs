using Microsoft.Extensions.Localization;

namespace Legion.Validation;

public static class ValidatorConfiguration
{
	public static IStringLocalizer? Localizer { get; set; }
}
