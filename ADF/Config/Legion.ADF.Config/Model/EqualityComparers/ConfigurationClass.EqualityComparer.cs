using Legion;
using Legion.Model.Comparers;
using System.Diagnostics.CodeAnalysis;

namespace Legion.ADF.Config.Model;

public sealed partial class ConfigurationClass : Config.ConfigBaseEntity, Legion.Model.IEntity
{
	public bool EqualsTo(
		Config.Model.ConfigurationClass? obj,
		ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
		Action<ComparisonConditions<Config.Model.ConfigurationClass>>? conditions = null)
		=> ConfigurationClassEqualityComparer.EqualsTo(
			this,
			obj,
			comparisonOptions,
			conditions,
			cache: null);

	public sealed partial class ConfigurationClassEqualityComparer : IEqualityComparer<ConfigurationClass>
	{
		public static bool EqualsTo(
			Config.Model.ConfigurationClass? obj1,
			Config.Model.ConfigurationClass? obj2,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Config.Model.ConfigurationClass>>? conditions = null,
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
			
			ComparisonConditions<Config.Model.ConfigurationClass>? conds = null;
		
			if ((ComparisonOptions.CompareProperties & comparisonOptions) == ComparisonOptions.CompareProperties)
			{
				if (conditions != null)
				{
					conds = new ComparisonConditions<Config.Model.ConfigurationClass>();
					conditions.Invoke(conds);

					if (conds.CanCompare(obj1, nameof(obj1.IdConfigurationClass)) && obj1.IdConfigurationClass != obj2.IdConfigurationClass)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.RootPath)) && !string.Equals(obj1.RootPath, obj2.RootPath))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.DisplayName)) && !string.Equals(obj1.DisplayName, obj2.DisplayName))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Class)) && !string.Equals(obj1.Class, obj2.Class))
						return false;
				}
				else
				{
					if (obj1.IdConfigurationClass != obj2.IdConfigurationClass)
						return false;
					if (!string.Equals(obj1.RootPath, obj2.RootPath))
						return false;
					if (!string.Equals(obj1.DisplayName, obj2.DisplayName))
						return false;
					if (!string.Equals(obj1.Class, obj2.Class))
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
			Config.Model.ConfigurationClass? obj,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Config.Model.ConfigurationClass>>? conditions = null,
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
		public Action<ComparisonConditions<Config.Model.ConfigurationClass>>? Conditions { get; }
		public Dictionary<object, HashSet<object>>? Cache { get; }
	
		public ConfigurationClassEqualityComparer(
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Config.Model.ConfigurationClass>>? conditions = null,
			Dictionary<object, HashSet<object>>? cache = null)
		{
			Cache = cache ?? [];
			ComparisonOptions = comparisonOptions;
			Conditions = conditions;
		}
	
		public bool Equals(
			Config.Model.ConfigurationClass? obj1,
			Config.Model.ConfigurationClass? obj2)
			=> EqualsTo(
				obj1,
				obj2,
				ComparisonOptions,
				Conditions,
				Cache);
	
		public int GetHashCode([DisallowNull] Config.Model.ConfigurationClass? obj)
			=> GetHashCode(
				obj,
				ComparisonOptions,
				Conditions,
				cache: null);
	}
}
