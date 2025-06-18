using Legion.Validation;

namespace Legion.ADF.Logs.Model;

public sealed partial class EnvironmentInfo : Logs.LogsBaseEntity, Legion.Model.IEntity
{
	public static IValidator<EnvironmentInfo> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdEnvironmentInfo { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NOT NULL
	/// </summary>
	public DateTime CreatedUtc { get; private set; }

	/// <summary>
	/// Database DataType: varchar(127) NULL
	/// </summary>
	public string? ApplicationName { get; private set; }

	/// <summary>
	/// Database DataType: varchar(15) NULL
	/// </summary>
	public string? ApplicationVersion { get; private set; }

	/// <summary>
	/// Database DataType: varchar(255) NULL
	/// </summary>
	public string? RunningEnvironment { get; private set; }

	/// <summary>
	/// Database DataType: varchar(255) NULL
	/// </summary>
	public string? ProcessName { get; private set; }

	/// <summary>
	/// Database DataType: integer NULL
	/// </summary>
	public int? ProcessId { get; private set; }

	/// <summary>
	/// Database DataType: varchar(255) NULL
	/// </summary>
	public string? FrameworkDescription { get; private set; }

	/// <summary>
	/// Database DataType: varchar(255) NULL
	/// </summary>
	public string? TargetFramework { get; private set; }

	/// <summary>
	/// Database DataType: varchar(255) NULL
	/// </summary>
	public string? CLRVersion { get; private set; }

	/// <summary>
	/// Database DataType: varchar(255) NULL
	/// </summary>
	public string? EntryAssemblyName { get; private set; }

	/// <summary>
	/// Database DataType: varchar(255) NULL
	/// </summary>
	public string? EntryAssemblyVersion { get; private set; }

	/// <summary>
	/// Database DataType: varchar(255) NULL
	/// </summary>
	public string? BaseDirectory { get; private set; }

	/// <summary>
	/// Database DataType: varchar(255) NULL
	/// </summary>
	public string? MachineName { get; private set; }

	/// <summary>
	/// Database DataType: varchar(255) NULL
	/// </summary>
	public string? CurrentAppDomainName { get; private set; }

	/// <summary>
	/// Database DataType: boolean NULL
	/// </summary>
	public bool? Is64BitOperatingSystem { get; private set; }

	/// <summary>
	/// Database DataType: boolean NULL
	/// </summary>
	public bool? Is64BitProcess { get; private set; }

	/// <summary>
	/// Database DataType: varchar(255) NULL
	/// </summary>
	public string? OperatingSystemArchitecture { get; private set; }

	/// <summary>
	/// Database DataType: varchar(255) NULL
	/// </summary>
	public string? OperatingSystemPlatform { get; private set; }

	/// <summary>
	/// Database DataType: varchar(255) NULL
	/// </summary>
	public string? OperatingSystemVersion { get; private set; }

	/// <summary>
	/// Database DataType: varchar(255) NULL
	/// </summary>
	public string? ProcessArchitecture { get; private set; }

	/// <summary>
	/// Database DataType: varchar(1023) NULL
	/// </summary>
	public string? CommandLine { get; private set; }

	private EnvironmentInfo()
	{
	}

	static EnvironmentInfo()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<EnvironmentInfo>()).Build();
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdEnvironmentInfo), IdEnvironmentInfo },
			{ nameof(CreatedUtc), CreatedUtc },
			{ nameof(ApplicationName), ApplicationName },
			{ nameof(ApplicationVersion), ApplicationVersion },
			{ nameof(RunningEnvironment), RunningEnvironment },
			{ nameof(ProcessName), ProcessName },
			{ nameof(ProcessId), ProcessId },
			{ nameof(FrameworkDescription), FrameworkDescription },
			{ nameof(TargetFramework), TargetFramework },
			{ nameof(CLRVersion), CLRVersion },
			{ nameof(EntryAssemblyName), EntryAssemblyName },
			{ nameof(EntryAssemblyVersion), EntryAssemblyVersion },
			{ nameof(BaseDirectory), BaseDirectory },
			{ nameof(MachineName), MachineName },
			{ nameof(CurrentAppDomainName), CurrentAppDomainName },
			{ nameof(Is64BitOperatingSystem), Is64BitOperatingSystem },
			{ nameof(Is64BitProcess), Is64BitProcess },
			{ nameof(OperatingSystemArchitecture), OperatingSystemArchitecture },
			{ nameof(OperatingSystemPlatform), OperatingSystemPlatform },
			{ nameof(OperatingSystemVersion), OperatingSystemVersion },
			{ nameof(ProcessArchitecture), ProcessArchitecture },
			{ nameof(CommandLine), CommandLine },
		};

	public void TrimStringValuesToFitDatabaseMaxLengths(string? postfix = null)
	{
		ApplicationName = Legion.Text.StringHelper.TrimToFitMaxLength(ApplicationName, 127, postfix);
		ApplicationVersion = Legion.Text.StringHelper.TrimToFitMaxLength(ApplicationVersion, 15, postfix);
		RunningEnvironment = Legion.Text.StringHelper.TrimToFitMaxLength(RunningEnvironment, 255, postfix);
		ProcessName = Legion.Text.StringHelper.TrimToFitMaxLength(ProcessName, 255, postfix);
		FrameworkDescription = Legion.Text.StringHelper.TrimToFitMaxLength(FrameworkDescription, 255, postfix);
		TargetFramework = Legion.Text.StringHelper.TrimToFitMaxLength(TargetFramework, 255, postfix);
		CLRVersion = Legion.Text.StringHelper.TrimToFitMaxLength(CLRVersion, 255, postfix);
		EntryAssemblyName = Legion.Text.StringHelper.TrimToFitMaxLength(EntryAssemblyName, 255, postfix);
		EntryAssemblyVersion = Legion.Text.StringHelper.TrimToFitMaxLength(EntryAssemblyVersion, 255, postfix);
		BaseDirectory = Legion.Text.StringHelper.TrimToFitMaxLength(BaseDirectory, 255, postfix);
		MachineName = Legion.Text.StringHelper.TrimToFitMaxLength(MachineName, 255, postfix);
		CurrentAppDomainName = Legion.Text.StringHelper.TrimToFitMaxLength(CurrentAppDomainName, 255, postfix);
		OperatingSystemArchitecture = Legion.Text.StringHelper.TrimToFitMaxLength(OperatingSystemArchitecture, 255, postfix);
		OperatingSystemPlatform = Legion.Text.StringHelper.TrimToFitMaxLength(OperatingSystemPlatform, 255, postfix);
		OperatingSystemVersion = Legion.Text.StringHelper.TrimToFitMaxLength(OperatingSystemVersion, 255, postfix);
		ProcessArchitecture = Legion.Text.StringHelper.TrimToFitMaxLength(ProcessArchitecture, 255, postfix);
		CommandLine = Legion.Text.StringHelper.TrimToFitMaxLength(CommandLine, 1023, postfix);
	}

	public override string? GetPrimaryKeyValue()
	{
		return IdEnvironmentInfo.ToString();
	}

	public override string? ToString()
	{
		return IdEnvironmentInfo.ToString();
	}

	public static ValidatorBuilder<EnvironmentInfo> SetDBValidatorRules(ValidatorBuilder<EnvironmentInfo> builder)
		=> builder
			.ForProperty(x => x.IdEnvironmentInfo, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.CreatedUtc, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.ApplicationName, v => v.MaxLength(127))
			.ForProperty(x => x.ApplicationVersion, v => v.MaxLength(15))
			.ForProperty(x => x.RunningEnvironment, v => v.MaxLength(255))
			.ForProperty(x => x.ProcessName, v => v.MaxLength(255))
			.ForProperty(x => x.FrameworkDescription, v => v.MaxLength(255))
			.ForProperty(x => x.TargetFramework, v => v.MaxLength(255))
			.ForProperty(x => x.CLRVersion, v => v.MaxLength(255))
			.ForProperty(x => x.EntryAssemblyName, v => v.MaxLength(255))
			.ForProperty(x => x.EntryAssemblyVersion, v => v.MaxLength(255))
			.ForProperty(x => x.BaseDirectory, v => v.MaxLength(255))
			.ForProperty(x => x.MachineName, v => v.MaxLength(255))
			.ForProperty(x => x.CurrentAppDomainName, v => v.MaxLength(255))
			.ForProperty(x => x.OperatingSystemArchitecture, v => v.MaxLength(255))
			.ForProperty(x => x.OperatingSystemPlatform, v => v.MaxLength(255))
			.ForProperty(x => x.OperatingSystemVersion, v => v.MaxLength(255))
			.ForProperty(x => x.ProcessArchitecture, v => v.MaxLength(255))
			.ForProperty(x => x.CommandLine, v => v.MaxLength(1023))
		;
}
