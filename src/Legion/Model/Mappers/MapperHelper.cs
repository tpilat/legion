using System.Diagnostics.CodeAnalysis;

namespace Legion.Model.Mappers;

public static class MapperHelper
{
	[return: NotNullIfNotNull(nameof(source))]
	public static List<TTarget>? MapToList<TSource, TTarget>(
		IEnumerable<TSource>? source,
		List<TTarget>? target,
		Func<TSource, TTarget?, ReferenceModifier, Action<MappingConditions<TSource>>?, Legion.Reflection.InstanceFactory?, Dictionary<object, object>, TTarget?> map,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<TSource>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? dict = null)
	{
		Throw.IfArgumentNull(map);

		if (source == null)
		{
			target?.Clear();
			return null;
		}
		;

		dict ??= [];

		var targetCount = target?.Count ?? 0;

		target ??= [];

		var tar = target.GetEnumerator();
		var targetIndex = 0;
		var mappedCount = 0;
		using (var src = source.GetEnumerator())
		{
			while (src.MoveNext())
			{
				mappedCount++;
				if (targetIndex < targetCount && tar.MoveNext())
				{
					map(src.Current, tar.Current, referenceModifier, conditions, instanceFactory, dict);
				}
				else
				{
					var itemResult = map(src.Current, default, referenceModifier, conditions, instanceFactory, dict);
					if (itemResult != null)
						target.Add(itemResult);
				}
				targetIndex++;
			}
		}

		if (mappedCount == 0)
		{
			target.Clear();
		}
		else if (targetIndex < targetCount)
		{
			TTarget[] tmpTargets = new TTarget[targetCount - targetIndex];
			targetIndex = 0;
			using (tar)
			{
				while (tar.MoveNext())
					tmpTargets[targetIndex++] = tar.Current;
			}

			foreach (var tmp in tmpTargets)
				target.Remove(tmp);
		}

		return target;
	}

	[return: NotNullIfNotNull(nameof(source))]
	public static HashSet<TTarget>? MapToList<TSource, TTarget>(
		IEnumerable<TSource>? source,
		HashSet<TTarget>? target,
		Func<TSource, TTarget?, ReferenceModifier, Action<MappingConditions<TSource>>?, Dictionary<object, object>, TTarget?> map,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<TSource>>? conditions = null,
		Dictionary<object, object>? dict = null)
	{
		Throw.IfArgumentNull(map);

		if (source == null)
		{
			target?.Clear();
			return null;
		}
		;

		dict ??= [];

		var targetCount = target?.Count ?? 0;

		target ??= [];

		var tar = target.GetEnumerator();
		var targetIndex = 0;
		var mappedCount = 0;
		using (var src = source.GetEnumerator())
		{
			while (src.MoveNext())
			{
				mappedCount++;
				if (targetIndex < targetCount && tar.MoveNext())
				{
					map(src.Current, tar.Current, referenceModifier, conditions, dict);
				}
				else
				{
					var itemResult = map(src.Current, default, referenceModifier, conditions, dict);
					if (itemResult != null)
						target.Add(itemResult);
				}
				targetIndex++;
			}
		}

		if (mappedCount == 0)
		{
			target.Clear();
		}
		else if (targetIndex < targetCount)
		{
			TTarget[] tmpTargets = new TTarget[targetCount - targetIndex];
			targetIndex = 0;
			using (tar)
			{
				while (tar.MoveNext())
					tmpTargets[targetIndex++] = tar.Current;
			}

			foreach (var tmp in tmpTargets)
				target.Remove(tmp);
		}

		return target;
	}
}
