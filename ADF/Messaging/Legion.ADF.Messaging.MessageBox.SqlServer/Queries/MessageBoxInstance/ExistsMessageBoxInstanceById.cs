using Legion.ADF.Messaging.MessageBox.SqlServer;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.MessageBox.Queries.MessageBoxInstance;

public class ExistsMessageBoxInstanceById :
	QueryDefinition<
		IMessageBoxDbContext,
		MessageBox.Model.MessageBoxInstance,
		bool,
		ExistsMessageBoxInstanceByIdQuery>,
		IExistsMessageBoxInstanceById
{
	public ExistsMessageBoxInstanceById(
		IEFConnectionProvider connectionProvider,
		ExistsMessageBoxInstanceByIdQuery existsMessageBoxInstanceById)
		: base(connectionProvider, existsMessageBoxInstanceById)
	{
	}

	protected override IQueryable<MessageBox.Model.MessageBoxInstance> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.MessageBoxInstance;
	}

	public override IQueryable<MessageBox.Model.MessageBoxInstance> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return ApplyIncludesThenWhere<IMessagingAccessControlManager>(
			scopeContext,
			QueryRequest.QueryableBuilder,
			QueryRequest.CheckReadPermissions,
			QueryRequest.AsNoTracking,
			imt => imt.IdMessageBoxInstance == QueryRequest.IdMessageBoxInstance);
	}

	public override async Task<bool> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.AnyAsync(cancellationToken);
	}

	public bool ToResult(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext).Any();
	}
}
