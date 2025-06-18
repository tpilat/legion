using Legion.Extensions;
using Legion.Reflection.ObjectPaths;
using Legion.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Audit.Settings;

public class AuditStoreOptions
{
	public string AuditStoreId { get; set; }

	public class Validator : ValidatorBase<AuditStoreOptions>
	{
		public Validator() { }
		public Validator(IObjectPath objectPath) : base(objectPath) { }

		public override void SetDefaultRuels(ValidatorBuilder<AuditStoreOptions> builder)
			=> RulesBuilder(builder);

		public static void RulesBuilder(ValidatorBuilder<AuditStoreOptions> builder)
		{
			builder?
				.ForProperty(x => x.AuditStoreId, v => v.MinLength(0));
		}
	}
}

public static class AuditStoreOptionsExtensions
{
	public static IServiceCollection AddAuditStoreOptions(this IServiceCollection services)
	{
		services
			.AddOptions<AuditStoreOptions>()
			.BindConfiguration($"{AppSettings.PREFIX}:{nameof(AuditStoreOptions)}")
			.AddOptionsValidator($"{AppSettings.PREFIX}.{nameof(AuditStoreOptions)}")
			.ValidateOnStart();

		return services;
	}
}