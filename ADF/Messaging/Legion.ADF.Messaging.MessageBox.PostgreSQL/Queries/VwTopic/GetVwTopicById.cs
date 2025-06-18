using Legion.ADF.Messaging.MessageBox.PostgreSQL;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.MessageBox.Queries.VwTopic;

public class GetVwTopicById :
	QueryDefinition<
		IMessageBoxQueryDbContext,
		MessageBox.Model.VwTopic,
		MessageBox.Model.VwTopic?,
		GetVwTopicByIdQuery>,
		IGetVwTopicById
{
	public GetVwTopicById(
		IEFConnectionProvider connectionProvider,
		GetVwTopicByIdQuery getVwTopicById)
		: base(connectionProvider, getVwTopicById)
	{
	}

	protected override IQueryable<MessageBox.Model.VwTopic> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.VwTopic;
	}

	public override IQueryable<MessageBox.Model.VwTopic> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return ApplyIncludesThenWhere<IMessagingAccessControlManager>(
			scopeContext,
			QueryRequest.QueryableBuilder,
			QueryRequest.CheckReadPermissions,
			QueryRequest.AsNoTracking,
			im => im.IdTopic == QueryRequest.IdTopic);
	}

	public override async Task<MessageBox.Model.VwTopic?> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.FirstOrDefaultAsync(cancellationToken);
	}

	public MessageBox.Model.VwTopic? ToResult(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext).FirstOrDefault();
	}
}
