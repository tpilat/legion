using Legion.MessageBus.Messages;

namespace TestEnterpriseServiceBus.Adapters.SocPoist.Messages;

public record SocPoistRequest : IRequestMessage<SocPoistResponse>
{
}
