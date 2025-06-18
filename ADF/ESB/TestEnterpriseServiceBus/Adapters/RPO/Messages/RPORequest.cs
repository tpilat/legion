using Legion.MessageBus.Messages;

namespace TestEnterpriseServiceBus.Adapters.RPO.Messages;

public record RPORequest : IRequestMessage<RPOResponse>
{
}
