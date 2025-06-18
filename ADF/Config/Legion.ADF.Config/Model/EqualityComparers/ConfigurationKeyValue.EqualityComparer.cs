using Legion;
using Legion.Model.Comparers;
using System.Diagnostics.CodeAnalysis;

namespace Legion.ADF.Config.Model;

public sealed partial class ConfigurationKeyValue : Config.ConfigBaseEntity, Legion.Model.Audit.ISelfAuditableEntity, Legion.Model.Concurrence.IConcurrent, Legion.Model.Audit.IAuditableEntity, Legion.Model.IEntity
{
	public bool EqualsTo(
		Config.Model.ConfigurationKeyValue? obj,
		ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
		Action<ComparisonConditions<Config.Model.ConfigurationKeyValue>>? conditions = null)
		=> ConfigurationKeyValueEqualityComparer.EqualsTo(
			this,
			obj,
			comparisonOptions,
			conditions,
			cache: null);

	public sealed partial class ConfigurationKeyValueEqualityComparer : IEqualityComparer<ConfigurationKeyValue>
	{
		public static bool EqualsTo(
			Config.Model.ConfigurationKeyValue? obj1,
			Config.Model.ConfigurationKeyValue? obj2,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Config.Model.ConfigurationKeyValue>>? conditions = null,
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
			
			ComparisonConditions<Config.Model.ConfigurationKeyValue>? conds = null;
		
			if ((ComparisonOptions.CompareProperties & comparisonOptions) == ComparisonOptions.CompareProperties)
			{
				if (conditions != null)
				{
					conds = new ComparisonConditions<Config.Model.ConfigurationKeyValue>();
					conditions.Invoke(conds);

					if (conds.CanCompare(obj1, nameof(obj1.IdConfigurationKeyValue)) && obj1.IdConfigurationKeyValue != obj2.IdConfigurationKeyValue)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Key)) && !string.Equals(obj1.Key, obj2.Key))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Value)) && !string.Equals(obj1.Value, obj2.Value))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.AuditCreatedUtc)) && obj1.AuditCreatedUtc != obj2.AuditCreatedUtc)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.AuditModifiedUtc)) && obj1.AuditModifiedUtc != obj2.AuditModifiedUtc)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdAuditCreatedBy)) && obj1.IdAuditCreatedBy != obj2.IdAuditCreatedBy)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdAuditModifiedBy)) && obj1.IdAuditModifiedBy != obj2.IdAuditModifiedBy)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.ConcurrencyToken)) && obj1.ConcurrencyToken != obj2.ConcurrencyToken)
						return false;
				}
				else
				{
					if (obj1.IdConfigurationKeyValue != obj2.IdConfigurationKeyValue)
						return false;
					if (!string.Equals(obj1.Key, obj2.Key))
						return false;
					if (!string.Equals(obj1.Value, obj2.Value))
						return false;
					if (obj1.AuditCreatedUtc != obj2.AuditCreatedUtc)
						return false;
					if (obj1.AuditModifiedUtc != obj2.AuditModifiedUtc)
						return false;
					if (obj1.IdAuditCreatedBy != obj2.IdAuditCreatedBy)
						return false;
					if (obj1.IdAuditModifiedBy != obj2.IdAuditModifiedBy)
						return false;
					if (obj1.ConcurrencyToken != obj2.ConcurrencyToken)
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
			Config.Model.ConfigurationKeyValue? obj,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Config.Model.ConfigurationKeyValue>>? conditions = null,
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
		public Action<ComparisonConditions<Config.Model.ConfigurationKeyValue>>? Conditions { get; }
		public Dictionary<object, HashSet<object>>? Cache { get; }
	
		public ConfigurationKeyValueEqualityComparer(
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Config.Model.ConfigurationKeyValue>>? conditions = null,
			Dictionary<object, HashSet<object>>? cache = null)
		{
			Cache = cache ?? [];
			ComparisonOptions = comparisonOptions;
			Conditions = conditions;
		}
	
		public bool Equals(
			Config.Model.ConfigurationKeyValue? obj1,
			Config.Model.ConfigurationKeyValue? obj2)
			=> EqualsTo(
				obj1,
				obj2,
				ComparisonOptions,
				Conditions,
				Cache);
	
		public int GetHashCode([DisallowNull] Config.Model.ConfigurationKeyValue? obj)
			=> GetHashCode(
				obj,
				ComparisonOptions,
				Conditions,
				cache: null);
	}
}
