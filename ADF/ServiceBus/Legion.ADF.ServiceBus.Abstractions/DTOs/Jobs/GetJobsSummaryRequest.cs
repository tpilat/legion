using Legion.Queries.Sorting;

namespace Legion.ADF.ServiceBus.DTOs.Jobs;

public record GetJobsSummaryRequest
{
	public List<SortDescriptorDto>? SortDescriptors { get; set; }
	public int PageIndex { get; set; }
	public int PageSize { get; set; }
}
