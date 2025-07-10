using Legion.Reflection.ObjectPaths;
using Legion.Validation;

namespace Legion.AspNetCore.WebApi.Configurations;

public class SwaggerEndpoint
{
	public string Version { get; set; }

	public string Name { get; set; }

	public string SwaggerDocumentUrl { get; set; }

	public List<SwaggerServer>? Servers { get; set; }

	public class Validator : ValidatorBase<SwaggerEndpoint>
	{
		public Validator() { }
		public Validator(IObjectPath objectPath) : base(objectPath) { }

		public override void SetDefaultRuels(ValidatorBuilder<SwaggerEndpoint> builder)
			=> RulesBuilder(builder);

		public static void RulesBuilder(ValidatorBuilder<SwaggerEndpoint> builder)
		{
			builder?
				.ForProperty(x => x.Version, v => v.NotDefaultOrWhiteSpace())
				.ForProperty(x => x.Name, v => v.NotDefaultOrWhiteSpace())
				.ForProperty(x => x.SwaggerDocumentUrl, v => v.NotDefaultOrWhiteSpace())
				.ForEach(x => x.Servers, SwaggerServer.Validator.RulesBuilder);
		}
	}
}
