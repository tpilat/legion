using Legion.Queries;

namespace Legion.MessageBus.Messages;

/// <summary>
/// Marker interface for queries.
/// </summary>
/// <typeparam name="TResponse">The response message type associated with the query</typeparam>
public interface IQuery<out TResponse> : IRequestMessage<TResponse>, IMessage
{
}

public interface IQueryRequest<T, out TResponse> : IQuery<TResponse>, IRequestMessage<TResponse>, IMessage
	where T : class
{
	Action<IQueryableBuilder<T>>? QueryableBuilder { get; }
}