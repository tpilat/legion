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

namespace Legion.ADF.ESB.MBox.PostgreSQL;

public partial class MBoxUnitOfWork : Legion.ADF.ESB.MBox.IMBoxUnitOfWork, Legion.EntityFrameworkCore.Model.Repositories.IDbUnitOfWork, Legion.Model.Repositories.IUnitOfWork
{
	public IEFConnectionProvider ConnectionProvider { get; }
	public IAuditEntryStore? AuditEntryStore { get; }

	public MBoxUnitOfWork(IEFConnectionProvider connectionProvider, IAuditEntryStore? auditEntryStore)
	{
		Throw.IfArgumentNull(connectionProvider);

		ConnectionProvider = connectionProvider;
		AuditEntryStore = auditEntryStore;
	}

	public MBoxUnitOfWork(Legion.EntityFrameworkCore.Model.Repositories.IDbUnitOfWork dbUnitOfWork)
	{
		Throw.IfArgumentNull(dbUnitOfWork);

		ConnectionProvider = dbUnitOfWork.ConnectionProvider;
		AuditEntryStore = dbUnitOfWork.AuditEntryStore;
	}

	public MBoxUnitOfWork(Legion.EntityFrameworkCore.Model.Repositories.IDbQueryUnitOfWork dbQueryUnitOfWork)
	{
		Throw.IfArgumentNull(dbQueryUnitOfWork);

		ConnectionProvider = dbQueryUnitOfWork.ConnectionProvider;
		AuditEntryStore = dbQueryUnitOfWork.AuditEntryStore;
	}

	public MBoxUnitOfWork(IServiceProvider serviceProvider, string connectionStirng, IAuditEntryStore? auditEntryStore)
	{
		Throw.IfArgumentNull(serviceProvider);
		Throw.IfArgumentNullOrWhiteSpace(connectionStirng);

		var connectionProviderFactory = serviceProvider.GetRequiredService<IEFConnectionProviderFactory>();
		ConnectionProvider = connectionProviderFactory.CreateWithoutTransaction(serviceProvider, connectionStirng);
		AuditEntryStore = auditEntryStore;
	}

	protected Legion.ADF.ESB.MBox.PostgreSQL.IMBoxDbContext GetContext(IScopeContext scopeContext)
		=> ConnectionProvider.GetOrCreateDbContext<Legion.ADF.ESB.MBox.PostgreSQL.IMBoxDbContext>(scopeContext, AuditEntryStore);

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


	private Legion.ADF.ESB.MBox.Model.Repositories.IMessageRepository message;
	public Legion.ADF.ESB.MBox.Model.Repositories.IMessageRepository MessageRepository
		=> message ??= new Legion.ADF.ESB.MBox.PostgreSQL.Model.Repositories.MessageRepository(ConnectionProvider, AuditEntryStore);


	private Legion.ADF.ESB.MBox.Model.Repositories.IMessageContentRepository messageContent;
	public Legion.ADF.ESB.MBox.Model.Repositories.IMessageContentRepository MessageContentRepository
		=> messageContent ??= new Legion.ADF.ESB.MBox.PostgreSQL.Model.Repositories.MessageContentRepository(ConnectionProvider, AuditEntryStore);


	private Legion.ADF.ESB.MBox.Model.Repositories.IMessageProcessingLogRepository messageProcessingLog;
	public Legion.ADF.ESB.MBox.Model.Repositories.IMessageProcessingLogRepository MessageProcessingLogRepository
		=> messageProcessingLog ??= new Legion.ADF.ESB.MBox.PostgreSQL.Model.Repositories.MessageProcessingLogRepository(ConnectionProvider, AuditEntryStore);


	private Legion.ADF.ESB.MBox.Model.Repositories.IMessageProcessingStatusRepository messageProcessingStatus;
	public Legion.ADF.ESB.MBox.Model.Repositories.IMessageProcessingStatusRepository MessageProcessingStatusRepository
		=> messageProcessingStatus ??= new Legion.ADF.ESB.MBox.PostgreSQL.Model.Repositories.MessageProcessingStatusRepository(ConnectionProvider, AuditEntryStore);


	private Legion.ADF.ESB.MBox.Model.Repositories.IMessagePublishingRepository messagePublishing;
	public Legion.ADF.ESB.MBox.Model.Repositories.IMessagePublishingRepository MessagePublishingRepository
		=> messagePublishing ??= new Legion.ADF.ESB.MBox.PostgreSQL.Model.Repositories.MessagePublishingRepository(ConnectionProvider, AuditEntryStore);


	private Legion.ADF.ESB.MBox.Model.Repositories.IMessageStatusRepository messageStatus;
	public Legion.ADF.ESB.MBox.Model.Repositories.IMessageStatusRepository MessageStatusRepository
		=> messageStatus ??= new Legion.ADF.ESB.MBox.PostgreSQL.Model.Repositories.MessageStatusRepository(ConnectionProvider, AuditEntryStore);


	private Legion.ADF.ESB.MBox.Model.Repositories.IMessageTypeRepository messageType;
	public Legion.ADF.ESB.MBox.Model.Repositories.IMessageTypeRepository MessageTypeRepository
		=> messageType ??= new Legion.ADF.ESB.MBox.PostgreSQL.Model.Repositories.MessageTypeRepository(ConnectionProvider, AuditEntryStore);


	private Legion.ADF.ESB.MBox.Model.Repositories.IQueueRepository queue;
	public Legion.ADF.ESB.MBox.Model.Repositories.IQueueRepository QueueRepository
		=> queue ??= new Legion.ADF.ESB.MBox.PostgreSQL.Model.Repositories.QueueRepository(ConnectionProvider, AuditEntryStore);


	private Legion.ADF.ESB.MBox.Model.Repositories.IQueuedMessageRepository queuedMessage;
	public Legion.ADF.ESB.MBox.Model.Repositories.IQueuedMessageRepository QueuedMessageRepository
		=> queuedMessage ??= new Legion.ADF.ESB.MBox.PostgreSQL.Model.Repositories.QueuedMessageRepository(ConnectionProvider, AuditEntryStore);
}
