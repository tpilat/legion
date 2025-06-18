using System.ComponentModel;

namespace Legion.EntityFrameworkCore.Expressions;

public class SortDescriptor
{
	public string Member { get; set; }

	public ListSortDirection SortDirection { get; set; }
}
