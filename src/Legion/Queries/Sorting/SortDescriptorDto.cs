using System.ComponentModel;

namespace Legion.Queries.Sorting;

public class SortDescriptorDto
{
	public string MemberSelector { get; set; }
	public ListSortDirection SortDirection { get; set; }
}
