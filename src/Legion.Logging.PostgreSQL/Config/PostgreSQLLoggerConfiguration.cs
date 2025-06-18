using Legion.Reflection.ObjectPaths;
using Legion.Validation;
using Microsoft.Extensions.Logging;

namespace Legion.Logging.PostgreSQL.Config;

public sealed class PostgreSQLLoggerConfiguration
{
	public LogLevel LogMessageMinLogLevel { get; set; }


	public class Validator : ValidatorBase<PostgreSQLLoggerConfiguration>
	{
		public Validator() { }
		public Validator(IObjectPath objectPath) : base(objectPath) { }

		public override void SetDefaultRuels(ValidatorBuilder<PostgreSQLLoggerConfiguration> builder)
			=> RulesBuilder(builder);

		public static void RulesBuilder(ValidatorBuilder<PostgreSQLLoggerConfiguration> builder)
		{
			builder?
				.ForProperty(x => x.LogMessageMinLogLevel, v => v.NotNull())
			;
		}
	}
}
