using Legion.Reflection.ObjectPaths;
using Legion.Validation;

namespace Legion.Http;

public class WebProxySettings
{
	public string Address { get; set; }
	public bool UseProxy { get; set; } = true;
	public bool BypassOnLocal { get; set; }
	public bool UseDefaultCredentials { get; set; }
	public string DomainName { get; set; }
	public string UserName { get; set; }
	public string Password { get; set; }

	public class Validator : ValidatorBase<WebProxySettings>
	{
		public Validator() { }
		public Validator(IObjectPath objectPath) : base(objectPath) { }

		public override void SetDefaultRuels(ValidatorBuilder<WebProxySettings> builder)
			=> RulesBuilder(builder);

		public static void RulesBuilder(ValidatorBuilder<WebProxySettings> builder)
		{
			builder?
				.ForProperty(x => x.Address, v => v.NotDefaultOrWhiteSpace())
			;
		}
	}
}
