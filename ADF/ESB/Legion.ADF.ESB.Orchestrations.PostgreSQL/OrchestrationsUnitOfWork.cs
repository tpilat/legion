using Legion;
using Legion.Database;
using Legion.EntityFrameworkCore;
using Legion.Model.Audit;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Model;
using Legion.Transactions;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

namespace Legion.ADF.ESB.Orchestrations.PostgreSQL;

public partial class OrchestrationsUnitOfWork : Legion.ADF.ESB.Orchestrations.IOrchestrationsUnitOfWork, Legion.EntityFrameworkCore.Model.Repositories.IDbUnitOfWork, Legion.Model.Repositories.IUnitOfWork
{
	public IEFConnectionProvider ConnectionProvider { get; }
	public IAuditEntryStore? AuditEntryStore { get; }

	public OrchestrationsUnitOfWork(IEFConnectionProvider connectionProvider, IAuditEntryStore? auditEntryStore)
	{
		Throw.IfArgumentNull(connectionProvider);

		ConnectionProvider = connectionProvider;
		AuditEntryStore = auditEntryStore;
	}

	public OrchestrationsUnitOfWork(Legion.EntityFrameworkCore.Model.Repositories.IDbUnitOfWork dbUnitOfWork)
	{
		Throw.IfArgumentNull(dbUnitOfWork);

		ConnectionProvider = dbUnitOfWork.ConnectionProvider;
		AuditEntryStore = dbUnitOfWork.AuditEntryStore;
	}

	public OrchestrationsUnitOfWork(Legion.EntityFrameworkCore.Model.Repositories.IDbQueryUnitOfWork dbQueryUnitOfWork)
	{
		Throw.IfArgumentNull(dbQueryUnitOfWork);

		ConnectionProvider = dbQueryUnitOfWork.ConnectionProvider;
		AuditEntryStore = dbQueryUnitOfWork.AuditEntryStore;
	}

	public OrchestrationsUnitOfWork(IServiceProvider serviceProvider, string connectionStirng, IAuditEntryStore? auditEntryStore)
	{
		Throw.IfArgumentNull(serviceProvider);
		Throw.IfArgumentNullOrWhiteSpace(connectionStirng);

		var connectionProviderFactory = serviceProvider.GetRequiredService<IEFConnectionProviderFactory>();
		ConnectionProvider = connectionProviderFactory.CreateWithoutTransaction(serviceProvider, connectionStirng);
		AuditEntryStore = auditEntryStore;
	}

	protected Legion.ADF.ESB.Orchestrations.PostgreSQL.IOrchestrationsDbContext GetContext(IScopeContext scopeContext)
		=> ConnectionProvider.GetOrCreateDbContext<Legion.ADF.ESB.Orchestrations.PostgreSQL.IOrchestrationsDbContext>(scopeContext, AuditEntryStore);

	//[Obsolete("Use SaveAsync() method instead.", true, DiagnosticId = "MM_ModUoW_Save")]
	//public virtual int Save(
	//	IScopeContext scopeContext,
	//	[CallerMemberName] string memberName = "",
	//	[CallerFilePath] string sourceFilePath = "",
	//	[CallerLineNumber] int sourceLineNumber = 0)
	//	=> Save(
	//		scopeContext,
	//		false,
	//		memberName,
	//		sourceFilePath,
	//		sourceLineNumber);

	//[Obsolete("Use SaveAsync() method instead.", true, DiagnosticId = "MM_ModUoW_Save")]
	//public virtual int Save(
	//	IScopeContext scopeContext,
	//	bool autoCommitAllTransactions,
	//	[CallerMemberName] string memberName = "",
	//	[CallerFilePath] string sourceFilePath = "",
	//	[CallerLineNumber] int sourceLineNumber = 0)
	//{
	//	var dbContext = GetContext(scopeContext);
	//	Throw.IfNull(dbContext);
	//	var result = dbContext.Save(scopeContext.CreateNew(false, memberName, sourceFilePath, sourceLineNumber));

	//	if (autoCommitAllTransactions)
	//		ConnectionProvider.TransactionsController.CommitAll(scopeContext);

	//	return result;
	//}

	//[Obsolete("Use SaveAsync() method instead.", true, DiagnosticId = "MM_ModUoW_Save")]
	//public virtual int Save(
	//	IScopeContext scopeContext,
	//	bool autoCommitAllTransactions,
	//	SaveOptions? options,
	//	[CallerMemberName] string memberName = "",
	//	[CallerFilePath] string sourceFilePath = "",
	//	[CallerLineNumber] int sourceLineNumber = 0)
	//{
	//	var dbContext = GetContext(scopeContext);
	//	Throw.IfNull(dbContext);
	//	var result = dbContext.Save(scopeContext.CreateNew(false, memberName, sourceFilePath, sourceLineNumber), options);

	//	if (autoCommitAllTransactions)
	//		ConnectionProvider.TransactionsController.CommitAll(scopeContext);

	//	return result;
	//}

	//[Obsolete("Use SaveAsync() method instead.", true, DiagnosticId = "MM_ModUoW_Save")]
	//public virtual int Save(
	//	IScopeContext scopeContext,
	//	bool autoCommitAllTransactions,
	//	bool acceptAllChangesOnSuccess,
	//	[CallerMemberName] string memberName = "",
	//	[CallerFilePath] string sourceFilePath = "",
	//	[CallerLineNumber] int sourceLineNumber = 0)
	//{
	//	var dbContext = GetContext(scopeContext);
	//	Throw.IfNull(dbContext);
	//	var result = dbContext.Save(scopeContext.CreateNew(false, memberName, sourceFilePath, sourceLineNumber), acceptAllChangesOnSuccess);

	//	if (autoCommitAllTransactions)
	//		ConnectionProvider.TransactionsController.CommitAll(scopeContext);

	//	return result;
	//}

	//[Obsolete("Use SaveAsync() method instead.", true, DiagnosticId = "MM_ModUoW_Save")]
	//public virtual int Save(
	//	IScopeContext scopeContext,
	//	bool autoCommitAllTransactions,
	//	bool acceptAllChangesOnSuccess,
	//	SaveOptions? options,
	//	[CallerMemberName] string memberName = "",
	//	[CallerFilePath] string sourceFilePath = "",
	//	[CallerLineNumber] int sourceLineNumber = 0)
	//{
	//	var dbContext = GetContext(scopeContext);
	//	Throw.IfNull(dbContext);
	//	var result = dbContext.Save(scopeContext.CreateNew(false, memberName, sourceFilePath, sourceLineNumber), acceptAllChangesOnSuccess, options);

	//	if (autoCommitAllTransactions)
	//		ConnectionProvider.TransactionsController.CommitAll(scopeContext);

	//	return result;
	//}

	public virtual Task<int> SaveAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> SaveAsync(
			scopeContext,
			false,
			cancellationToken,
			memberName,
			sourceFilePath,
			sourceLineNumber);

	public virtual async Task<int> SaveAsync(
		IScopeContext scopeContext,
		bool autoCommitAllTransactions,
		CancellationToken cancellationToken = default,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);
		var result = await dbContext.SaveAsync(scopeContext.CreateNew(false, memberName, sourceFilePath, sourceLineNumber), cancellationToken);

		if (autoCommitAllTransactions)
			await ConnectionProvider.TransactionsController.CommitAllAsync(scopeContext, false, cancellationToken);

		return result;
	}

	public virtual async Task<int> SaveAsync(
		IScopeContext scopeContext,
		bool autoCommitAllTransactions,
		SaveOptions? options,
		CancellationToken cancellationToken = default,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);
		var result = await dbContext.SaveAsync(scopeContext.CreateNew(false, memberName, sourceFilePath, sourceLineNumber), options, cancellationToken);

		if (autoCommitAllTransactions)
			await ConnectionProvider.TransactionsController.CommitAllAsync(scopeContext, false, cancellationToken);

		return result;
	}

	public virtual async Task<int> SaveAsync(
		IScopeContext scopeContext,
		bool autoCommitAllTransactions,
		bool acceptAllChangesOnSuccess,
		CancellationToken cancellationToken = default,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);
		var result = await dbContext.SaveAsync(scopeContext.CreateNew(false, memberName, sourceFilePath, sourceLineNumber), acceptAllChangesOnSuccess, cancellationToken);

		if (autoCommitAllTransactions)
			await ConnectionProvider.TransactionsController.CommitAllAsync(scopeContext, false, cancellationToken);

		return result;
	}

	public virtual async Task<int> SaveAsync(
		IScopeContext scopeContext,
		bool autoCommitAllTransactions,
		bool acceptAllChangesOnSuccess,
		SaveOptions? options,
		CancellationToken cancellationToken = default,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);
		var result = await dbContext.SaveAsync(scopeContext.CreateNew(false, memberName, sourceFilePath, sourceLineNumber), acceptAllChangesOnSuccess, options, cancellationToken);

		if (autoCommitAllTransactions)
			await ConnectionProvider.TransactionsController.CommitAllAsync(scopeContext, false, cancellationToken);

		return result;
	}


	private Legion.ADF.ESB.Orchestrations.Model.Repositories.IOrchestrationRepository orchestration;
	public Legion.ADF.ESB.Orchestrations.Model.Repositories.IOrchestrationRepository OrchestrationRepository
		=> orchestration ??= new Legion.ADF.ESB.Orchestrations.PostgreSQL.Model.Repositories.OrchestrationRepository(ConnectionProvider, AuditEntryStore);


	private Legion.ADF.ESB.Orchestrations.Model.Repositories.IOrchestrationInstanceRepository orchestrationInstance;
	public Legion.ADF.ESB.Orchestrations.Model.Repositories.IOrchestrationInstanceRepository OrchestrationInstanceRepository
		=> orchestrationInstance ??= new Legion.ADF.ESB.Orchestrations.PostgreSQL.Model.Repositories.OrchestrationInstanceRepository(ConnectionProvider, AuditEntryStore);


	private Legion.ADF.ESB.Orchestrations.Model.Repositories.IOrchestrationStatusRepository orchestrationStatus;
	public Legion.ADF.ESB.Orchestrations.Model.Repositories.IOrchestrationStatusRepository OrchestrationStatusRepository
		=> orchestrationStatus ??= new Legion.ADF.ESB.Orchestrations.PostgreSQL.Model.Repositories.OrchestrationStatusRepository(ConnectionProvider, AuditEntryStore);


	private Legion.ADF.ESB.Orchestrations.Model.Repositories.IOrchestrationStepRepository orchestrationStep;
	public Legion.ADF.ESB.Orchestrations.Model.Repositories.IOrchestrationStepRepository OrchestrationStepRepository
		=> orchestrationStep ??= new Legion.ADF.ESB.Orchestrations.PostgreSQL.Model.Repositories.OrchestrationStepRepository(ConnectionProvider, AuditEntryStore);


	private Legion.ADF.ESB.Orchestrations.Model.Repositories.IOrchestrationStepInstanceRepository orchestrationStepInstance;
	public Legion.ADF.ESB.Orchestrations.Model.Repositories.IOrchestrationStepInstanceRepository OrchestrationStepInstanceRepository
		=> orchestrationStepInstance ??= new Legion.ADF.ESB.Orchestrations.PostgreSQL.Model.Repositories.OrchestrationStepInstanceRepository(ConnectionProvider, AuditEntryStore);


	private Legion.ADF.ESB.Orchestrations.Model.Repositories.IOrchestrationStepLogRepository orchestrationStepLog;
	public Legion.ADF.ESB.Orchestrations.Model.Repositories.IOrchestrationStepLogRepository OrchestrationStepLogRepository
		=> orchestrationStepLog ??= new Legion.ADF.ESB.Orchestrations.PostgreSQL.Model.Repositories.OrchestrationStepLogRepository(ConnectionProvider, AuditEntryStore);


	private Legion.ADF.ESB.Orchestrations.Model.Repositories.IOrchestrationStepStatusRepository orchestrationStepStatus;
	public Legion.ADF.ESB.Orchestrations.Model.Repositories.IOrchestrationStepStatusRepository OrchestrationStepStatusRepository
		=> orchestrationStepStatus ??= new Legion.ADF.ESB.Orchestrations.PostgreSQL.Model.Repositories.OrchestrationStepStatusRepository(ConnectionProvider, AuditEntryStore);


	private Legion.ADF.ESB.Orchestrations.Model.Repositories.IStepDirectionRepository stepDirection;
	public Legion.ADF.ESB.Orchestrations.Model.Repositories.IStepDirectionRepository StepDirectionRepository
		=> stepDirection ??= new Legion.ADF.ESB.Orchestrations.PostgreSQL.Model.Repositories.StepDirectionRepository(ConnectionProvider, AuditEntryStore);
}
