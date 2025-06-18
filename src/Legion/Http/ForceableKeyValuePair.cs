using Legion.Reflection.ObjectPaths;
using Legion.Validation;

namespace Legion.Http;

public class ForceableKeyValuePair
{
	public string Key { get; set; }
	public string Value { get; set; }
	public bool Force { get; set; }

	public class Validator : ValidatorBase<ForceableKeyValuePair>
	{
		public Validator() { }
		public Validator(IObjectPath objectPath) : base(objectPath) { }

		public override void SetDefaultRuels(ValidatorBuilder<ForceableKeyValuePair> builder)
			=> RulesBuilder(builder);

		public static void RulesBuilder(ValidatorBuilder<ForceableKeyValuePair> builder)
		{
			builder?
				.ForProperty(x => x.Key, v => v.NotDefaultOrWhiteSpace())
			;
		}
	}
}

public class ForceableKeyValuePairList
{
	public string Key { get; set; }
	public IEnumerable<string> Values { get; set; }
	public bool Force { get; set; }

	public class Validator : ValidatorBase<ForceableKeyValuePairList>
	{
		public Validator() { }
		public Validator(IObjectPath objectPath) : base(objectPath) { }

		public override void SetDefaultRuels(ValidatorBuilder<ForceableKeyValuePairList> builder)
			=> RulesBuilder(builder);

		public static void RulesBuilder(ValidatorBuilder<ForceableKeyValuePairList> builder)
		{
			builder?
				.ForProperty(x => x.Key, v => v.NotDefaultOrWhiteSpace())
			;
		}
	}
}
