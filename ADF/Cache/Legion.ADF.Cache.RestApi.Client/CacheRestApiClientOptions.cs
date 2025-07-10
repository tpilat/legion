using Legion.Extensions;
using Legion.NetHttp;
using Legion.Reflection.ObjectPaths;
using Legion.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Cache.RestApi.Client;

public class CacheRestApiClientOptions : HttpApiClientOptions
{
	public string BaseAddress { get; set; }

	public class Validator : ValidatorBase<CacheRestApiClientOptions>
	{
		public Validator() { }
		public Validator(IObjectPath objectPath) : base(objectPath) { }

		public override void SetDefaultRuels(ValidatorBuilder<CacheRestApiClientOptions> builder)
			=> RulesBuilder(builder);

		public static void RulesBuilder(ValidatorBuilder<CacheRestApiClientOptions> builder)
		{
			//builder?
			//	.ForProperty(x => x.BaseAddress, v => v.NotDefaultOrWhiteSpace());
		}
	}
}

public static class CacheRestApiClientOptionsExtensions
{
	public static IServiceCollection AddCacheRestApiClientOptions(this IServiceCollection services, string configBindingPath)
	{
		Throw.IfArgumentNullOrWhiteSpace(configBindingPath);

		services
			.AddOptions<CacheRestApiClientOptions>()
			.BindConfiguration($"{configBindingPath}")
			.AddOptionsValidator($"{configBindingPath}")
			.ValidateOnStart();

		return services;
	}
}
