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

namespace Legion.ADF.ESB.Components.PostgreSQL;

public partial class ComponentsUnitOfWork : Legion.ADF.ESB.Components.IComponentsUnitOfWork, Legion.EntityFrameworkCore.Model.Repositories.IDbUnitOfWork, Legion.Model.Repositories.IUnitOfWork
{
	public IEFConnectionProvider ConnectionProvider { get; }
	public IAuditEntryStore? AuditEntryStore { get; }

	public ComponentsUnitOfWork(IEFConnectionProvider connectionProvider, IAuditEntryStore? auditEntryStore)
	{
		Throw.IfArgumentNull(connectionProvider);

		ConnectionProvider = connectionProvider;
		AuditEntryStore = auditEntryStore;
	}

	public ComponentsUnitOfWork(Legion.EntityFrameworkCore.Model.Repositories.IDbUnitOfWork dbUnitOfWork)
	{
		Throw.IfArgumentNull(dbUnitOfWork);

		ConnectionProvider = dbUnitOfWork.ConnectionProvider;
		AuditEntryStore = dbUnitOfWork.AuditEntryStore;
	}

	public ComponentsUnitOfWork(Legion.EntityFrameworkCore.Model.Repositories.IDbQueryUnitOfWork dbQueryUnitOfWork)
	{
		Throw.IfArgumentNull(dbQueryUnitOfWork);

		ConnectionProvider = dbQueryUnitOfWork.ConnectionProvider;
		AuditEntryStore = dbQueryUnitOfWork.AuditEntryStore;
	}

	public ComponentsUnitOfWork(IServiceProvider serviceProvider, string connectionStirng, IAuditEntryStore? auditEntryStore)
	{
		Throw.IfArgumentNull(serviceProvider);
		Throw.IfArgumentNullOrWhiteSpace(connectionStirng);

		var connectionProviderFactory = serviceProvider.GetRequiredService<IEFConnectionProviderFactory>();
		ConnectionProvider = connectionProviderFactory.CreateWithoutTransaction(serviceProvider, connectionStirng);
		AuditEntryStore = auditEntryStore;
	}

	protected Legion.ADF.ESB.Components.PostgreSQL.IComponentsDbContext GetContext(IScopeContext scopeContext)
		=> ConnectionProvider.GetOrCreateDbContext<Legion.ADF.ESB.Components.PostgreSQL.IComponentsDbContext>(scopeContext, AuditEntryStore);

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


	private Legion.ADF.ESB.Components.Model.Repositories.IAdapterRepository adapter;
	public Legion.ADF.ESB.Components.Model.Repositories.IAdapterRepository AdapterRepository
		=> adapter ??= new Legion.ADF.ESB.Components.PostgreSQL.Model.Repositories.AdapterRepository(ConnectionProvider, AuditEntryStore);


	private Legion.ADF.ESB.Components.Model.Repositories.IAdapterLogRepository adapterLog;
	public Legion.ADF.ESB.Components.Model.Repositories.IAdapterLogRepository AdapterLogRepository
		=> adapterLog ??= new Legion.ADF.ESB.Components.PostgreSQL.Model.Repositories.AdapterLogRepository(ConnectionProvider, AuditEntryStore);


	private Legion.ADF.ESB.Components.Model.Repositories.IAdapterRequestRepository adapterRequest;
	public Legion.ADF.ESB.Components.Model.Repositories.IAdapterRequestRepository AdapterRequestRepository
		=> adapterRequest ??= new Legion.ADF.ESB.Components.PostgreSQL.Model.Repositories.AdapterRequestRepository(ConnectionProvider, AuditEntryStore);


	private Legion.ADF.ESB.Components.Model.Repositories.IAdapterRequestPayloadRepository adapterRequestPayload;
	public Legion.ADF.ESB.Components.Model.Repositories.IAdapterRequestPayloadRepository AdapterRequestPayloadRepository
		=> adapterRequestPayload ??= new Legion.ADF.ESB.Components.PostgreSQL.Model.Repositories.AdapterRequestPayloadRepository(ConnectionProvider, AuditEntryStore);


	private Legion.ADF.ESB.Components.Model.Repositories.IAdapterResponseRepository adapterResponse;
	public Legion.ADF.ESB.Components.Model.Repositories.IAdapterResponseRepository AdapterResponseRepository
		=> adapterResponse ??= new Legion.ADF.ESB.Components.PostgreSQL.Model.Repositories.AdapterResponseRepository(ConnectionProvider, AuditEntryStore);


	private Legion.ADF.ESB.Components.Model.Repositories.IAdapterResponsePayloadRepository adapterResponsePayload;
	public Legion.ADF.ESB.Components.Model.Repositories.IAdapterResponsePayloadRepository AdapterResponsePayloadRepository
		=> adapterResponsePayload ??= new Legion.ADF.ESB.Components.PostgreSQL.Model.Repositories.AdapterResponsePayloadRepository(ConnectionProvider, AuditEntryStore);


	private Legion.ADF.ESB.Components.Model.Repositories.IAdapterStatusRepository adapterStatus;
	public Legion.ADF.ESB.Components.Model.Repositories.IAdapterStatusRepository AdapterStatusRepository
		=> adapterStatus ??= new Legion.ADF.ESB.Components.PostgreSQL.Model.Repositories.AdapterStatusRepository(ConnectionProvider, AuditEntryStore);


	private Legion.ADF.ESB.Components.Model.Repositories.IJobRepository job;
	public Legion.ADF.ESB.Components.Model.Repositories.IJobRepository JobRepository
		=> job ??= new Legion.ADF.ESB.Components.PostgreSQL.Model.Repositories.JobRepository(ConnectionProvider, AuditEntryStore);


	private Legion.ADF.ESB.Components.Model.Repositories.IJobDataRepository jobData;
	public Legion.ADF.ESB.Components.Model.Repositories.IJobDataRepository JobDataRepository
		=> jobData ??= new Legion.ADF.ESB.Components.PostgreSQL.Model.Repositories.JobDataRepository(ConnectionProvider, AuditEntryStore);


	private Legion.ADF.ESB.Components.Model.Repositories.IJobLogRepository jobLog;
	public Legion.ADF.ESB.Components.Model.Repositories.IJobLogRepository JobLogRepository
		=> jobLog ??= new Legion.ADF.ESB.Components.PostgreSQL.Model.Repositories.JobLogRepository(ConnectionProvider, AuditEntryStore);


	private Legion.ADF.ESB.Components.Model.Repositories.IJobStatusRepository jobStatus;
	public Legion.ADF.ESB.Components.Model.Repositories.IJobStatusRepository JobStatusRepository
		=> jobStatus ??= new Legion.ADF.ESB.Components.PostgreSQL.Model.Repositories.JobStatusRepository(ConnectionProvider, AuditEntryStore);


	private Legion.ADF.ESB.Components.Model.Repositories.IJobTypeRepository jobType;
	public Legion.ADF.ESB.Components.Model.Repositories.IJobTypeRepository JobTypeRepository
		=> jobType ??= new Legion.ADF.ESB.Components.PostgreSQL.Model.Repositories.JobTypeRepository(ConnectionProvider, AuditEntryStore);
}
