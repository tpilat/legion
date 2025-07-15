namespace Legion.ADF.ServiceBus.Services.Internal.Dto;

internal class ServiceBusInstances
{
	public Model.Host MyHost { get; set; }

	public List<Model.Host> Hosts { get; set; }

	public List<Model.Job> Jobs { get; set; }

	public ServiceBusInstances Clone()
		=> new()
		{
			MyHost = MyHost,
			Hosts = Hosts?.ToList() ?? [],
			Jobs = Jobs?.ToList() ?? []
		};
}
