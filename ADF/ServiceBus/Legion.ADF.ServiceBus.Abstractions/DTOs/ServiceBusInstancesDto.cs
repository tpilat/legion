using Legion.ADF.ServiceBus.DTOs.Hosts;
using Legion.ADF.ServiceBus.DTOs.Jobs;

namespace Legion.ADF.ServiceBus.DTOs;

public class ServiceBusInstancesDto
{
	public bool IsDistributedManagerAvailable { get; set; }

	public List<HostDto> Hosts { get; set; }

	public List<JobDto> Jobs { get; set; }

	//public List<OrchestrationSummaryDto> Orchestrations { get; set; }
}
