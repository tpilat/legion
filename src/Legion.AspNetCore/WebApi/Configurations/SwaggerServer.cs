using Legion.Reflection.ObjectPaths;
using Legion.Validation;

namespace Legion.AspNetCore.WebApi.Configurations;

public class SwaggerServer
{
	public string Url { get; set; }

	public string Description { get; set; }

	public class Validator : ValidatorBase<SwaggerServer>
	{
		public Validator() { }
		public Validator(IObjectPath objectPath) : base(objectPath) { }

		public override void SetDefaultRuels(ValidatorBuilder<SwaggerServer> builder)
			=> RulesBuilder(builder);

		public static void RulesBuilder(ValidatorBuilder<SwaggerServer> builder)
		{
			builder?
				.ForProperty(x => x.Url, v => v.NotDefaultOrWhiteSpace())
				.ForProperty(x => x.Description, v => v.NotDefaultOrWhiteSpace());
		}
	}
}
