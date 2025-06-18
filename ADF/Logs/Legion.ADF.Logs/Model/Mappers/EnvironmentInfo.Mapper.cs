using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.Logs.Model;

public sealed partial class EnvironmentInfo : Logs.LogsBaseEntity, Legion.Model.IEntity
{
	public static Logs.Model.EnvironmentInfo? Map(
		Logs.Model.EnvironmentInfo source,
		Logs.Model.EnvironmentInfo? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Logs.Model.EnvironmentInfo>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public Logs.Model.EnvironmentInfo? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Logs.Model.EnvironmentInfo>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public Logs.Model.EnvironmentInfo? MapTo(
		Logs.Model.EnvironmentInfo? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Logs.Model.EnvironmentInfo>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.Logs.Model.EnvironmentInfo>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.Logs.Model.EnvironmentInfo();

		if (cache.TryGetValue(this, out var cached))
			return (Logs.Model.EnvironmentInfo)cached;
			
		MappingConditions<Logs.Model.EnvironmentInfo>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<Logs.Model.EnvironmentInfo>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdEnvironmentInfo)))
				target.IdEnvironmentInfo = IdEnvironmentInfo;
			if (conds.CanMap(this, nameof(CreatedUtc)))
				target.CreatedUtc = CreatedUtc;
			if (conds.CanMap(this, nameof(ApplicationName)))
				target.ApplicationName = ApplicationName;
			if (conds.CanMap(this, nameof(ApplicationVersion)))
				target.ApplicationVersion = ApplicationVersion;
			if (conds.CanMap(this, nameof(RunningEnvironment)))
				target.RunningEnvironment = RunningEnvironment;
			if (conds.CanMap(this, nameof(ProcessName)))
				target.ProcessName = ProcessName;
			if (conds.CanMap(this, nameof(ProcessId)))
				target.ProcessId = ProcessId;
			if (conds.CanMap(this, nameof(FrameworkDescription)))
				target.FrameworkDescription = FrameworkDescription;
			if (conds.CanMap(this, nameof(TargetFramework)))
				target.TargetFramework = TargetFramework;
			if (conds.CanMap(this, nameof(CLRVersion)))
				target.CLRVersion = CLRVersion;
			if (conds.CanMap(this, nameof(EntryAssemblyName)))
				target.EntryAssemblyName = EntryAssemblyName;
			if (conds.CanMap(this, nameof(EntryAssemblyVersion)))
				target.EntryAssemblyVersion = EntryAssemblyVersion;
			if (conds.CanMap(this, nameof(BaseDirectory)))
				target.BaseDirectory = BaseDirectory;
			if (conds.CanMap(this, nameof(MachineName)))
				target.MachineName = MachineName;
			if (conds.CanMap(this, nameof(CurrentAppDomainName)))
				target.CurrentAppDomainName = CurrentAppDomainName;
			if (conds.CanMap(this, nameof(Is64BitOperatingSystem)))
				target.Is64BitOperatingSystem = Is64BitOperatingSystem;
			if (conds.CanMap(this, nameof(Is64BitProcess)))
				target.Is64BitProcess = Is64BitProcess;
			if (conds.CanMap(this, nameof(OperatingSystemArchitecture)))
				target.OperatingSystemArchitecture = OperatingSystemArchitecture;
			if (conds.CanMap(this, nameof(OperatingSystemPlatform)))
				target.OperatingSystemPlatform = OperatingSystemPlatform;
			if (conds.CanMap(this, nameof(OperatingSystemVersion)))
				target.OperatingSystemVersion = OperatingSystemVersion;
			if (conds.CanMap(this, nameof(ProcessArchitecture)))
				target.ProcessArchitecture = ProcessArchitecture;
			if (conds.CanMap(this, nameof(CommandLine)))
				target.CommandLine = CommandLine;
		}
		else
		{
			target.IdEnvironmentInfo = IdEnvironmentInfo;
			target.CreatedUtc = CreatedUtc;
			target.ApplicationName = ApplicationName;
			target.ApplicationVersion = ApplicationVersion;
			target.RunningEnvironment = RunningEnvironment;
			target.ProcessName = ProcessName;
			target.ProcessId = ProcessId;
			target.FrameworkDescription = FrameworkDescription;
			target.TargetFramework = TargetFramework;
			target.CLRVersion = CLRVersion;
			target.EntryAssemblyName = EntryAssemblyName;
			target.EntryAssemblyVersion = EntryAssemblyVersion;
			target.BaseDirectory = BaseDirectory;
			target.MachineName = MachineName;
			target.CurrentAppDomainName = CurrentAppDomainName;
			target.Is64BitOperatingSystem = Is64BitOperatingSystem;
			target.Is64BitProcess = Is64BitProcess;
			target.OperatingSystemArchitecture = OperatingSystemArchitecture;
			target.OperatingSystemPlatform = OperatingSystemPlatform;
			target.OperatingSystemVersion = OperatingSystemVersion;
			target.ProcessArchitecture = ProcessArchitecture;
			target.CommandLine = CommandLine;
		}

		cache.Add(this, target);

		return target;
	}
}
