using System.ComponentModel;
using System.Linq.Expressions;

namespace Legion.Queries.Sorting;

public interface ISortDescriptorBuilder<T> : IQueryModifier<T>
{
	IEnumerable<SortDescriptor<T>> ToSortStack();

	ISortDescriptorBuilder<T> Append(ISortDescriptorBuilder<T>? otherSortDescriptorBuilder, bool throwIfEmptySortStack);

	ISortDescriptorBuilder<T> SortBy(Expression<Func<T, object>> memberSelector, ListSortDirection sortDirection = ListSortDirection.Ascending);

	ISortDescriptorBuilder<T> SortBy(IEnumerable<SortDescriptor<T>> sortDescriptors);

	string Serialize();
}
