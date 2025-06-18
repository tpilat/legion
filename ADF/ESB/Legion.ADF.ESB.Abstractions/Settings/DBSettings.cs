using Legion.Extensions;
using Legion.Reflection.ObjectPaths;
using Legion.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.ESB.Settings;

public class DBSettings
{
	public Dictionary<string, DbConnectionSettings> DbConnectionSettings { get; }

	public DBSettings()
	{
		DbConnectionSettings = [];
	}

	public class Validator : ValidatorBase<DBSettings>
	{
		public Validator() { }
		public Validator(IObjectPath objectPath) : base(objectPath) { }

		public override void SetDefaultRuels(ValidatorBuilder<DBSettings> builder)
			=> RulesBuilder(builder);

		public static void RulesBuilder(ValidatorBuilder<DBSettings> builder)
		{
			builder?
				.ForProperty(x => x.DbConnectionSettings, v => v.NotDefaultOrEmpty())
				.ForNavigation(
					x => x.DbConnectionSettings,
					v => v.ForEach(
						dict => dict.Values,
						Settings.DbConnectionSettings.Validator.RulesBuilder));
		}
	}
}

public class DbConnectionSettings
{
	public string ConnectionString { get; set; }

	public class Validator : ValidatorBase<DbConnectionSettings>
	{
		public Validator() { }
		public Validator(IObjectPath objectPath) : base(objectPath) { }

		public override void SetDefaultRuels(ValidatorBuilder<DbConnectionSettings> builder)
			=> RulesBuilder(builder);

		public static void RulesBuilder(ValidatorBuilder<DbConnectionSettings> builder)
		{
			builder?
				.ForProperty(x => x.ConnectionString, v => v.NotDefaultOrWhiteSpace());
		}
	}
}

public static class DBSettingsExtensions
{
	public static IServiceCollection AddDBSettings(this IServiceCollection services)
	{
		services
			.AddOptions<DBSettings>()
			.BindConfiguration("ADFESB:DBSettings")
			.AddOptionsValidator("ADFESB.DBSettings")
			.ValidateOnStart();

		return services;
	}
}
