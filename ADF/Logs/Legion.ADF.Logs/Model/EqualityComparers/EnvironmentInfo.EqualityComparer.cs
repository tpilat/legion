using Legion;
using Legion.Model.Comparers;
using System.Diagnostics.CodeAnalysis;

namespace Legion.ADF.Logs.Model;

public sealed partial class EnvironmentInfo : Logs.LogsBaseEntity, Legion.Model.IEntity
{
	public bool EqualsTo(
		Logs.Model.EnvironmentInfo? obj,
		ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
		Action<ComparisonConditions<Logs.Model.EnvironmentInfo>>? conditions = null)
		=> EnvironmentInfoEqualityComparer.EqualsTo(
			this,
			obj,
			comparisonOptions,
			conditions,
			cache: null);

	public sealed partial class EnvironmentInfoEqualityComparer : IEqualityComparer<EnvironmentInfo>
	{
		public static bool EqualsTo(
			Logs.Model.EnvironmentInfo? obj1,
			Logs.Model.EnvironmentInfo? obj2,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Logs.Model.EnvironmentInfo>>? conditions = null,
			Dictionary<object, HashSet<object>>? cache = null)
		{
			if (obj1 == null && obj2 == null)
				return true;

			if (obj1 == null || obj2 == null)
				return false;

			if (ReferenceEquals(obj1, obj2))
				return true;

			cache ??= [];

			cache.TryGetValue(obj1, out HashSet<object>? cachedHashSet);
			if (cachedHashSet?.Contains(obj2) == true)
				return true;
			
			ComparisonConditions<Logs.Model.EnvironmentInfo>? conds = null;
		
			if ((ComparisonOptions.CompareProperties & comparisonOptions) == ComparisonOptions.CompareProperties)
			{
				if (conditions != null)
				{
					conds = new ComparisonConditions<Logs.Model.EnvironmentInfo>();
					conditions.Invoke(conds);

					if (conds.CanCompare(obj1, nameof(obj1.IdEnvironmentInfo)) && obj1.IdEnvironmentInfo != obj2.IdEnvironmentInfo)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.CreatedUtc)) && obj1.CreatedUtc != obj2.CreatedUtc)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.ApplicationName)) && !string.Equals(obj1.ApplicationName, obj2.ApplicationName))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.ApplicationVersion)) && !string.Equals(obj1.ApplicationVersion, obj2.ApplicationVersion))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.RunningEnvironment)) && !string.Equals(obj1.RunningEnvironment, obj2.RunningEnvironment))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.ProcessName)) && !string.Equals(obj1.ProcessName, obj2.ProcessName))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.ProcessId)) && obj1.ProcessId != obj2.ProcessId)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.FrameworkDescription)) && !string.Equals(obj1.FrameworkDescription, obj2.FrameworkDescription))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.TargetFramework)) && !string.Equals(obj1.TargetFramework, obj2.TargetFramework))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.CLRVersion)) && !string.Equals(obj1.CLRVersion, obj2.CLRVersion))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.EntryAssemblyName)) && !string.Equals(obj1.EntryAssemblyName, obj2.EntryAssemblyName))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.EntryAssemblyVersion)) && !string.Equals(obj1.EntryAssemblyVersion, obj2.EntryAssemblyVersion))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.BaseDirectory)) && !string.Equals(obj1.BaseDirectory, obj2.BaseDirectory))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.MachineName)) && !string.Equals(obj1.MachineName, obj2.MachineName))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.CurrentAppDomainName)) && !string.Equals(obj1.CurrentAppDomainName, obj2.CurrentAppDomainName))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Is64BitOperatingSystem)) && obj1.Is64BitOperatingSystem != obj2.Is64BitOperatingSystem)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Is64BitProcess)) && obj1.Is64BitProcess != obj2.Is64BitProcess)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.OperatingSystemArchitecture)) && !string.Equals(obj1.OperatingSystemArchitecture, obj2.OperatingSystemArchitecture))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.OperatingSystemPlatform)) && !string.Equals(obj1.OperatingSystemPlatform, obj2.OperatingSystemPlatform))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.OperatingSystemVersion)) && !string.Equals(obj1.OperatingSystemVersion, obj2.OperatingSystemVersion))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.ProcessArchitecture)) && !string.Equals(obj1.ProcessArchitecture, obj2.ProcessArchitecture))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.CommandLine)) && !string.Equals(obj1.CommandLine, obj2.CommandLine))
						return false;
				}
				else
				{
					if (obj1.IdEnvironmentInfo != obj2.IdEnvironmentInfo)
						return false;
					if (obj1.CreatedUtc != obj2.CreatedUtc)
						return false;
					if (!string.Equals(obj1.ApplicationName, obj2.ApplicationName))
						return false;
					if (!string.Equals(obj1.ApplicationVersion, obj2.ApplicationVersion))
						return false;
					if (!string.Equals(obj1.RunningEnvironment, obj2.RunningEnvironment))
						return false;
					if (!string.Equals(obj1.ProcessName, obj2.ProcessName))
						return false;
					if (obj1.ProcessId != obj2.ProcessId)
						return false;
					if (!string.Equals(obj1.FrameworkDescription, obj2.FrameworkDescription))
						return false;
					if (!string.Equals(obj1.TargetFramework, obj2.TargetFramework))
						return false;
					if (!string.Equals(obj1.CLRVersion, obj2.CLRVersion))
						return false;
					if (!string.Equals(obj1.EntryAssemblyName, obj2.EntryAssemblyName))
						return false;
					if (!string.Equals(obj1.EntryAssemblyVersion, obj2.EntryAssemblyVersion))
						return false;
					if (!string.Equals(obj1.BaseDirectory, obj2.BaseDirectory))
						return false;
					if (!string.Equals(obj1.MachineName, obj2.MachineName))
						return false;
					if (!string.Equals(obj1.CurrentAppDomainName, obj2.CurrentAppDomainName))
						return false;
					if (obj1.Is64BitOperatingSystem != obj2.Is64BitOperatingSystem)
						return false;
					if (obj1.Is64BitProcess != obj2.Is64BitProcess)
						return false;
					if (!string.Equals(obj1.OperatingSystemArchitecture, obj2.OperatingSystemArchitecture))
						return false;
					if (!string.Equals(obj1.OperatingSystemPlatform, obj2.OperatingSystemPlatform))
						return false;
					if (!string.Equals(obj1.OperatingSystemVersion, obj2.OperatingSystemVersion))
						return false;
					if (!string.Equals(obj1.ProcessArchitecture, obj2.ProcessArchitecture))
						return false;
					if (!string.Equals(obj1.CommandLine, obj2.CommandLine))
						return false;
				}
			}

			if (cachedHashSet == null)
			{
				cachedHashSet = [];
				cache[obj1] = cachedHashSet;
			}

			cachedHashSet.Add(obj2);

			return true;
		}

		public static int GetHashCode(
			Logs.Model.EnvironmentInfo? obj,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Logs.Model.EnvironmentInfo>>? conditions = null,
			HashSet<object>? cache = null)
		{
			if (obj == null)
				return 0;

			cache ??= [];

			if (cache.Contains(obj))
				return 0;

				var hash = 1;
			return hash;
		}

		public ComparisonOptions ComparisonOptions { get; }
		public Action<ComparisonConditions<Logs.Model.EnvironmentInfo>>? Conditions { get; }
		public Dictionary<object, HashSet<object>>? Cache { get; }
	
		public EnvironmentInfoEqualityComparer(
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Logs.Model.EnvironmentInfo>>? conditions = null,
			Dictionary<object, HashSet<object>>? cache = null)
		{
			Cache = cache ?? [];
			ComparisonOptions = comparisonOptions;
			Conditions = conditions;
		}
	
		public bool Equals(
			Logs.Model.EnvironmentInfo? obj1,
			Logs.Model.EnvironmentInfo? obj2)
			=> EqualsTo(
				obj1,
				obj2,
				ComparisonOptions,
				Conditions,
				Cache);
	
		public int GetHashCode([DisallowNull] Logs.Model.EnvironmentInfo? obj)
			=> GetHashCode(
				obj,
				ComparisonOptions,
				Conditions,
				cache: null);
	}
}
