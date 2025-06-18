using System.ComponentModel;
using System.Linq.Expressions;

namespace Legion.Queries.Sorting;

public class SortDescriptorBuilder<T> : ISortDescriptorBuilder<T>, IQueryModifier<T>
{
	private readonly List<SortDescriptor<T>> _sortStack;

	public SortDescriptorBuilder()
	{
		_sortStack = [];
	}

	public IEnumerable<SortDescriptor<T>> ToSortStack()
		=> _sortStack.ToList();

	public ISortDescriptorBuilder<T> Append(ISortDescriptorBuilder<T>? otherSortDescriptorBuilder, bool throwIfEmptySortStack)
	{
		if (otherSortDescriptorBuilder == null)
			return this;

		var sortStack = otherSortDescriptorBuilder.ToSortStack();

		if (throwIfEmptySortStack)
			Throw.IfNullOrEmpty(sortStack);

		return SortBy(sortStack);
	}

	public SortDescriptorBuilder<T> SortBy(Expression<Func<T, object>> memberSelector, ListSortDirection sortDirection = ListSortDirection.Ascending)
	{
		Throw.IfArgumentNull(memberSelector);

		_sortStack.Add(new SortDescriptor<T>
		{
			MemberSelector = memberSelector,
			SortDirection = sortDirection
		});

		return this;
	}

	public SortDescriptorBuilder<T> SortBy(IEnumerable<SortDescriptor<T>> sortDescriptors)
	{
		Throw.IfArgumentNullOrEmpty(sortDescriptors);

		_sortStack.AddRange(sortDescriptors);

		return this;
	}

	ISortDescriptorBuilder<T> ISortDescriptorBuilder<T>.SortBy(Expression<Func<T, object>> memberSelector, ListSortDirection sortDirection)
		=> SortBy(memberSelector, sortDirection);

	ISortDescriptorBuilder<T> ISortDescriptorBuilder<T>.SortBy(IEnumerable<SortDescriptor<T>> sortDescriptors)
		=> SortBy(sortDescriptors);

	IEnumerable<T> IQueryModifier<T>.ApplySort(IEnumerable<T> enumerable)
	{
		Throw.IfArgumentNull(enumerable);

		var first = true;
		IOrderedEnumerable<T>? orderedQueryable = null;
		foreach (var sort in _sortStack)
		{
			if (first)
			{
				if (sort.SortDirection == ListSortDirection.Ascending)
				{
					orderedQueryable = enumerable.OrderBy(sort.MemberDelegate);
				}
				else
				{
					orderedQueryable = enumerable.OrderByDescending(sort.MemberDelegate);
				}
				first = false;
			}
			else
			{
				if (sort.SortDirection == ListSortDirection.Ascending)
				{
					orderedQueryable = orderedQueryable!.ThenBy(sort.MemberDelegate);
				}
				else
				{
					orderedQueryable = orderedQueryable!.ThenByDescending(sort.MemberDelegate);
				}
			}
		}

		return orderedQueryable ?? enumerable;
	}

	IQueryable<T> IQueryModifier<T>.ApplySort(IQueryable<T> queryable)
	{
		Throw.IfArgumentNull(queryable);

		var first = true;
		IOrderedQueryable<T>? orderedQueryable = null;
		foreach (var sort in _sortStack)
		{
			if (first)
			{
				if (sort.SortDirection == ListSortDirection.Ascending)
				{
					orderedQueryable = queryable.OrderBy(sort.MemberSelector);
				}
				else
				{
					orderedQueryable = queryable.OrderByDescending(sort.MemberSelector);
				}
				first = false;
			}
			else
			{
				if (sort.SortDirection == ListSortDirection.Ascending)
				{
					orderedQueryable = orderedQueryable!.ThenBy(sort.MemberSelector);
				}
				else
				{
					orderedQueryable = orderedQueryable!.ThenByDescending(sort.MemberSelector);
				}
			}
		}

		return orderedQueryable ?? queryable;
	}

	IEnumerable<T> IQueryModifier<T>.ApplyPaging(IEnumerable<T> enumerable)
		=> enumerable;

	IQueryable<T> IQueryModifier<T>.ApplyPaging(IQueryable<T> queryable)
		=> queryable;

	IEnumerable<T> IQueryModifier<T>.ApplyIncludes(IEnumerable<T> enumerable)
		=> enumerable;

	IQueryable<T> IQueryModifier<T>.ApplyIncludes(IQueryable<T> queryable)
		=> queryable;

	IEnumerable<T> IQueryModifier<T>.Apply(IEnumerable<T> enumerable)
	{
		Throw.IfArgumentNull(enumerable);

		var first = true;
		IOrderedEnumerable<T>? orderedQueryable = null;
		foreach (var sort in _sortStack)
		{
			if (first)
			{
				if (sort.SortDirection == ListSortDirection.Ascending)
				{
					orderedQueryable = enumerable.OrderBy(sort.MemberDelegate);
				}
				else
				{
					orderedQueryable = enumerable.OrderByDescending(sort.MemberDelegate);
				}
				first = false;
			}
			else
			{
				if (sort.SortDirection == ListSortDirection.Ascending)
				{
					orderedQueryable = orderedQueryable!.ThenBy(sort.MemberDelegate);
				}
				else
				{
					orderedQueryable = orderedQueryable!.ThenByDescending(sort.MemberDelegate);
				}
			}
		}

		return orderedQueryable ?? enumerable;
	}

	IQueryable<T> IQueryModifier<T>.Apply(IQueryable<T> queryable)
	{
		Throw.IfArgumentNull(queryable);

		var first = true;
		IOrderedQueryable<T>? orderedQueryable = null;
		foreach (var sort in _sortStack)
		{
			if (first)
			{
				if (sort.SortDirection == ListSortDirection.Ascending)
				{
					orderedQueryable = queryable.OrderBy(sort.MemberSelector);
				}
				else
				{
					orderedQueryable = queryable.OrderByDescending(sort.MemberSelector);
				}
				first = false;
			}
			else
			{
				if (sort.SortDirection == ListSortDirection.Ascending)
				{
					orderedQueryable = orderedQueryable!.ThenBy(sort.MemberSelector);
				}
				else
				{
					orderedQueryable = orderedQueryable!.ThenByDescending(sort.MemberSelector);
				}
			}
		}

		return orderedQueryable ?? queryable;
	}

	public string Serialize()
	{
		return "";
	}

	public static ISortDescriptorBuilder<T> Deserialize(string json)
	{
		return new SortDescriptorBuilder<T>();
	}
}
