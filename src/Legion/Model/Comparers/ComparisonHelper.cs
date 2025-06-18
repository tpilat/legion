using System.Diagnostics.CodeAnalysis;

namespace Legion.Model.Comparers;

public static class ComparisonHelper
{
	[return: NotNullIfNotNull(nameof(list1))]
	public static bool SequenceEqual<T>(
		IEnumerable<T>? list1,
		IEnumerable<T>? list2,
		IEqualityComparer<T> comparer)
	{
		Throw.IfArgumentNull(comparer);

		if (list1 == null && list2 == null)
			return true;

		if (list1 == null || list2 == null)
			return false;

		return list1.SequenceEqual(list2, comparer);
	}

	public static int SequenceHashCode<T>(
		IEnumerable<T>? list,
		IEqualityComparer<T> comparer)
	{
		if (list == null || !list.Any())
			return 0;

		Throw.IfArgumentNull(comparer);

		int hash = comparer.GetHashCode(list.First()!);

		//return list.Aggregate(17, (hash, item) => hash * 31 + (item != null ? comparer.GetHashCode(item) : 0));

		foreach (var item in list.Skip(1))
			hash = HashCode.Combine(hash, item != null ? comparer.GetHashCode(item) : 0);

		return hash;
	}
}
