namespace Legion.ADF.Logs.Model;

public sealed partial class EnvironmentInfo : Logs.LogsBaseEntity, Legion.Model.IEntity
{
	internal static IResult<EnvironmentInfo> CreateEnvironmentInfo(
		IScopeContext scopeContext,
		string applicationName,
		string? appVersion)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<EnvironmentInfo>();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, applicationName))
			return result.Build();

		var infrastructureEnvironmentInfo = Legion.Infrastructure.EnvironmentInfoProvider.GetEnvironmentInfo(applicationName);

		var environmentInfo = new EnvironmentInfo
		{
			__IsNewObject = true,
			IdEnvironmentInfo = infrastructureEnvironmentInfo.RuntimeUniqueKey,
			CreatedUtc = GlobalContext.Instance.UtcNow,
			ApplicationName = infrastructureEnvironmentInfo.ApplicationName,
			ApplicationVersion = appVersion,
			RunningEnvironment = infrastructureEnvironmentInfo.RunningEnvironment,
			ProcessName = infrastructureEnvironmentInfo.ProcessName,
			ProcessId = infrastructureEnvironmentInfo.ProcessId,
			FrameworkDescription = infrastructureEnvironmentInfo.FrameworkDescription,
			TargetFramework = infrastructureEnvironmentInfo.TargetFramework,
			CLRVersion = infrastructureEnvironmentInfo.CLRVersion,
			EntryAssemblyName = infrastructureEnvironmentInfo.EntryAssemblyName,
			EntryAssemblyVersion = infrastructureEnvironmentInfo.EntryAssemblyVersion,
			BaseDirectory = infrastructureEnvironmentInfo.BaseDirectory,
			MachineName = infrastructureEnvironmentInfo.MachineName,
			CurrentAppDomainName = infrastructureEnvironmentInfo.CurrentAppDomainName,
			Is64BitOperatingSystem = infrastructureEnvironmentInfo.Is64BitOperatingSystem,
			Is64BitProcess = infrastructureEnvironmentInfo.Is64BitProcess,
			OperatingSystemArchitecture = infrastructureEnvironmentInfo.OperatingSystemArchitecture,
			OperatingSystemPlatform = infrastructureEnvironmentInfo.OperatingSystemPlatform,
			OperatingSystemVersion = infrastructureEnvironmentInfo.OperatingSystemVersion,
			ProcessArchitecture = infrastructureEnvironmentInfo.ProcessArchitecture,
			CommandLine = infrastructureEnvironmentInfo.CommandLine
		};

		var validationResult =
			DefaultDBValidator
				.Validate(environmentInfo);

		if (result.MergeHasError(scopeContext.CreateNew(), validationResult, true))
			return result.Build();

		return result.WithData(environmentInfo).Build();
	}
}
