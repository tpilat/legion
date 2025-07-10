using Legion.Reflection.ObjectPaths;
using Legion.Validation;
using Legion.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Swashbuckle.AspNetCore.Filters;

namespace Legion.AspNetCore.WebApi.Configurations;

public class WebApiOptions
{
	public string? ApiPrefix { get; set; }

	public bool EnableSwagger { get; set; }

	public List<SwaggerEndpoint>? SwaggerEndpoints { get; set; }

	public IEnumerable<Assembly>? SwaggerDocAssemblies { get; set; }

	public class Validator : ValidatorBase<WebApiOptions>
	{
		public Validator() { }
		public Validator(IObjectPath objectPath) : base(objectPath) { }

		public override void SetDefaultRuels(ValidatorBuilder<WebApiOptions> builder)
			=> RulesBuilder(builder);

		public static void RulesBuilder(ValidatorBuilder<WebApiOptions> builder)
		{
			builder?
				.ForProperty(x => x.SwaggerEndpoints, v => v.NotDefaultOrEmpty())
				.ForEach(x => x.SwaggerEndpoints, SwaggerEndpoint.Validator.RulesBuilder);
		}
	}
}

public static class WebApiOptionsExtensions
{
	public static IServiceCollection AddWebApiOptions(
		this IServiceCollection services,
		string configBindingPath,
		IEnumerable<Assembly>? swaggerDocAssemblies = null)
	{
		Throw.IfArgumentNullOrWhiteSpace(configBindingPath);

		var assemblies = swaggerDocAssemblies?.ToList() ?? [];

		var legionAssembly = typeof(IResult).Assembly;
		var legionAspNetCoreAssembly = typeof(WebApiOptions).Assembly;

		if (assemblies.All(x => x != legionAssembly))
			assemblies.Add(legionAssembly);

		if (assemblies.All(x => x != legionAspNetCoreAssembly))
			assemblies.Add(legionAspNetCoreAssembly);

		services.AddSwaggerExamplesFromAssemblies(assemblies.ToArray());

		services
			.AddOptions<WebApiOptions>()
			.BindConfiguration($"{configBindingPath}")
			.Configure(o =>
			{
				o.SwaggerDocAssemblies = assemblies;
			})
			.AddOptionsValidator($"{configBindingPath}")
			.ValidateOnStart();

		return services;
	}
}
