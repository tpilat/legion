using Legion.Reflection.ObjectPaths;
using Legion.Validation;

namespace Legion.Http;

public class RequestOptions
{
	public Dictionary<string, string> StaticQueryStrings { get; set; }
	public bool ForceStaticQueryStrings { get; set; }
	public List<ForceableKeyValuePair> StaticHeaders { get; set; }
	public List<ForceableKeyValuePairList> StaticHeaderCollections { get; set; }
	public List<ForceableKeyValuePair> StaticCookies { get; set; }
	public List<ForceableKeyValuePair> StaticFormData { get; set; }

	public class Validator : ValidatorBase<RequestOptions>
	{
		public Validator() { }
		public Validator(IObjectPath objectPath) : base(objectPath) { }

		public override void SetDefaultRuels(ValidatorBuilder<RequestOptions> builder)
			=> RulesBuilder(builder);

		public static void RulesBuilder(ValidatorBuilder<RequestOptions> builder)
		{
			builder?
				.ForEach(
					x => x.StaticHeaders,
					ForceableKeyValuePair.Validator.RulesBuilder)
				.ForEach(
					x => x.StaticHeaderCollections,
					ForceableKeyValuePairList.Validator.RulesBuilder)
				.ForEach(
					x => x.StaticCookies,
					ForceableKeyValuePair.Validator.RulesBuilder)
				.ForEach(
					x => x.StaticFormData,
					ForceableKeyValuePair.Validator.RulesBuilder)
			;
		}
	}
}
