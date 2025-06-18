using Legion;
using Legion.Model.Comparers;
using System.Diagnostics.CodeAnalysis;

namespace Legion.ADF.Logs.Model;

public sealed partial class RemoteSystem : Logs.LogsBaseEntity, Legion.Model.IEntity
{
	public bool EqualsTo(
		Logs.Model.RemoteSystem? obj,
		ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
		Action<ComparisonConditions<Logs.Model.RemoteSystem>>? conditions = null)
		=> RemoteSystemEqualityComparer.EqualsTo(
			this,
			obj,
			comparisonOptions,
			conditions,
			cache: null);

	public sealed partial class RemoteSystemEqualityComparer : IEqualityComparer<RemoteSystem>
	{
		public static bool EqualsTo(
			Logs.Model.RemoteSystem? obj1,
			Logs.Model.RemoteSystem? obj2,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Logs.Model.RemoteSystem>>? conditions = null,
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
			
			ComparisonConditions<Logs.Model.RemoteSystem>? conds = null;
		
			if ((ComparisonOptions.CompareProperties & comparisonOptions) == ComparisonOptions.CompareProperties)
			{
				if (conditions != null)
				{
					conds = new ComparisonConditions<Logs.Model.RemoteSystem>();
					conditions.Invoke(conds);

					if (conds.CanCompare(obj1, nameof(obj1.IdRemoteSystem)) && obj1.IdRemoteSystem != obj2.IdRemoteSystem)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Code)) && !string.Equals(obj1.Code, obj2.Code))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Name)) && !string.Equals(obj1.Name, obj2.Name))
						return false;
				}
				else
				{
					if (obj1.IdRemoteSystem != obj2.IdRemoteSystem)
						return false;
					if (!string.Equals(obj1.Code, obj2.Code))
						return false;
					if (!string.Equals(obj1.Name, obj2.Name))
						return false;
				}
			}

			if (cachedHashSet == null)
			{
				cachedHashSet = [];
				cache[obj1] = cachedHashSet;
			}

			cachedHashSet.Add(obj2);

			if ((ComparisonOptions.CompareReferences & comparisonOptions) == ComparisonOptions.CompareReferences)
			{
				if (!ComparisonHelper.SequenceEqual(obj1.LocalRequests, obj2.LocalRequests, new LocalRequest.LocalRequestEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.LocalRequests), cache)))
					return false;
				if (!ComparisonHelper.SequenceEqual(obj1.RemoteRequests, obj2.RemoteRequests, new RemoteRequest.RemoteRequestEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.RemoteRequests), cache)))
					return false;
		}

			return true;
		}

		public static int GetHashCode(
			Logs.Model.RemoteSystem? obj,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Logs.Model.RemoteSystem>>? conditions = null,
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
		public Action<ComparisonConditions<Logs.Model.RemoteSystem>>? Conditions { get; }
		public Dictionary<object, HashSet<object>>? Cache { get; }
	
		public RemoteSystemEqualityComparer(
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Logs.Model.RemoteSystem>>? conditions = null,
			Dictionary<object, HashSet<object>>? cache = null)
		{
			Cache = cache ?? [];
			ComparisonOptions = comparisonOptions;
			Conditions = conditions;
		}
	
		public bool Equals(
			Logs.Model.RemoteSystem? obj1,
			Logs.Model.RemoteSystem? obj2)
			=> EqualsTo(
				obj1,
				obj2,
				ComparisonOptions,
				Conditions,
				Cache);
	
		public int GetHashCode([DisallowNull] Logs.Model.RemoteSystem? obj)
			=> GetHashCode(
				obj,
				ComparisonOptions,
				Conditions,
				cache: null);
	}
}
