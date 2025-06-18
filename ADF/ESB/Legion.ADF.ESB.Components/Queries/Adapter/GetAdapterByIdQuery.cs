using Legion.MessageBus.Messages;

namespace Legion.ADF.ESB.Components.Queries.Adapter;

public record GetAdapterByIdQuery(
	Guid IdAdapter,
	Action<Legion.Queries.IQueryableBuilder<Components.Model.Adapter>>? QueryableBuilder)
	: IQueryRequest<Components.Model.Adapter, Components.Model.Adapter?>;
