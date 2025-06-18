using Legion.MessageBus.Messages;

namespace Legion.ACL;

public interface IAccessControlManagerBase
{
}

public interface IAccessControlManager<T> : IAccessControlManagerBase
	where T :class
{
	IQueryable<T> SetAuthorizationQuery<TQueryRequest, TResponse>(IScopeContext scopeContext, TQueryRequest queryRequest, IQueryable<T> queryable)
		where TQueryRequest : IQueryRequest<T, TResponse>, IQuery<TResponse>, IRequestMessage<TResponse>, IMessage;

	IQueryable<T> SetAuthorizationQuery(IScopeContext scopeContext, IQueryable<T> queryable);

	bool IsAuthorizedFor(IScopeContext scopeContext, string operation, T? entity);
}

public interface IGeneralAccessControlManager
{
	bool IsAuthorizedFor<T>(IScopeContext scopeContext);

	bool IsAuthorizedFor<T>(IScopeContext scopeContext, string operation, T? entity);
}
