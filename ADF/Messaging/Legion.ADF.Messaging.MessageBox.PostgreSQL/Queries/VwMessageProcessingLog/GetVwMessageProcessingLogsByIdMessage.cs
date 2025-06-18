using Legion.ADF.Messaging.MessageBox.PostgreSQL;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.MessageBox.Queries.VwMessageProcessingLog;

public class GetVwMessageProcessingLogsByIdMessage :
	QueryDefinition<
		IMessageBoxQueryDbContext,
		MessageBox.Model.VwMessageProcessingLog,
		List<MessageBox.Model.VwMessageProcessingLog>,
		GetVwMessageProcessingLogsByIdMessageQuery>,
	IGetVwMessageProcessingLogsByIdMessage
{
	public GetVwMessageProcessingLogsByIdMessage(
		IEFConnectionProvider connectionProvider,
		GetVwMessageProcessingLogsByIdMessageQuery getVwMessageProcessingLogByIdMessage)
		: base(connectionProvider, getVwMessageProcessingLogByIdMessage)
	{
	}

	protected override IQueryable<MessageBox.Model.VwMessageProcessingLog> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.VwMessageProcessingLog;
	}

	public override IQueryable<MessageBox.Model.VwMessageProcessingLog> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return ApplyIncludesThenWhere<IMessagingAccessControlManager>(
			scopeContext,
			QueryRequest.QueryableBuilder,
			QueryRequest.CheckReadPermissions,
			QueryRequest.AsNoTracking,
			impl => impl.IdMessage == QueryRequest.IdMessage);
	}

	public override async Task<List<MessageBox.Model.VwMessageProcessingLog>> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.ToListAsync(cancellationToken);
	}

	public List<MessageBox.Model.VwMessageProcessingLog> ToResult(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext).ToList();
	}
}
