using Legion.Extensions;
using Legion.Reflection.ObjectPaths;
using Legion.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Auth.Settings;

public class IdentityStoreOptions
{
	public string IdentityStoreId { get; set; }
	public bool EnableAuditing { get; set; }

	public class Validator : ValidatorBase<IdentityStoreOptions>
	{
		public Validator() { }
		public Validator(IObjectPath objectPath) : base(objectPath) { }

		public override void SetDefaultRuels(ValidatorBuilder<IdentityStoreOptions> builder)
			=> RulesBuilder(builder);

		public static void RulesBuilder(ValidatorBuilder<IdentityStoreOptions> builder)
		{
			builder?
				.ForProperty(x => x.IdentityStoreId, v => v.MinLength(0));
		}
	}
}

public static class IdentityStoreOptionsExtensions
{
	public static IServiceCollection AddIdentityStoreOptions(this IServiceCollection services)
	{
		services
			.AddOptions<IdentityStoreOptions>()
			.BindConfiguration($"{AppSettings.PREFIX}:{nameof(IdentityStoreOptions)}")
			.AddOptionsValidator($"{AppSettings.PREFIX}.{nameof(IdentityStoreOptions)}")
			.ValidateOnStart();

		return services;
	}
}