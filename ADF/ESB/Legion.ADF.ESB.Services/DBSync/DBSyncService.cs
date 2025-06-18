using Legion.ADF.ESB.ComponentsModel;
using Legion.Logging;
using Microsoft.Extensions.Logging;

namespace Legion.ADF.ESB.Services.DBSync;

internal class DBSyncService
{
	public Task<IResult> SyncAsync(IScopeContext scopeContext, CancellationToken cancellationToken = default)
		//TODO Dispose invocationContext
		=> Result.CallAsync(SyncAsync, new InvocationContextBuilder(scopeContext, unhandledErrorCode: null).Build(), cancellationToken);

	public async Task<IResult> SyncAsync(IInvocationContext invocationContext, CancellationToken cancellationToken = default)
	{
		var result = new ResultBuilder();

		invocationContext = invocationContext
			.InvocationAppendTraceFrameWithBusinessProcess("my svc", false)
			.InvocationAppendTraceFrameWithComponent("my comp", false);

		IComponentsUnitOfWork uow = null;

		var logMessageBuilder = new LogMessageBuilder(invocationContext.CreateNew(), null)
			.LogLevel(LogLevel.Information)
			.InternalMessage("test");

		var lm = logMessageBuilder.Build();

		return result.Build();
	}
}
