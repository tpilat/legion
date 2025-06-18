using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Legion.Extensions;

public static class OptionsBuilderExtensions
{
	public static OptionsBuilder<TOptions> AddOptionsValidator<TOptions>(this OptionsBuilder<TOptions> builder, string? basePath = null)
		where TOptions : class
	{
		builder.Services.TryAddSingleton<IValidateOptions<TOptions>>(
			sp => new Validation.Options.OptionsValidator<TOptions>(sp, basePath, builder.Name));

		return builder;
	}
}
