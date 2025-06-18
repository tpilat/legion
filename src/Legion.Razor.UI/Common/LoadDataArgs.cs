namespace Legion.Razor.UI;

/// <summary>
/// Supplies information about a <see cref="PagedDataBoundComponent{TItem}.LoadData" /> event that is being raised.
/// </summary>
public class LoadDataArgs
{
	/// <summary>
	/// Gets how many items to skip. Related to paging and the current page. Usually used with the <see cref="Enumerable.Skip{TSource}(IEnumerable{TSource}, int)"/> LINQ method.
	/// </summary>
	public int? Skip { get; set; }
	/// <summary>
	/// Gets how many items to take. Related to paging and the current page size. Usually used with the <see cref="Enumerable.Take{TSource}(IEnumerable{TSource}, int)"/> LINQ method.
	/// </summary>
	/// <value>The top.</value>
	public int? Top { get; set; }
	/// <summary>
	/// Gets the sort expression as a string.
	/// </summary>
	public string OrderBy { get; set; }
	/// <summary>
	/// Gets the filter expression as a string.
	/// </summary>
	/// <value>The filter.</value>
	public string Filter { get; set; }
	/// <summary>
	/// Gets the filter expression as a collection of filter descriptors.
	/// </summary>
	public IEnumerable<FilterDescriptor> Filters { get; set; }
	/// <summary>
	/// Gets the sort expression as a collection of sort descriptors.
	/// </summary>
	/// <value>The sorts.</value>
	public IEnumerable<SortDescriptor> Sorts { get; set; }
}
