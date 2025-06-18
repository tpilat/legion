using Legion;
using Legion.Database;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Audit;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Model;
using Legion.Transactions;
using System.Runtime.CompilerServices;

namespace Legion.ADF.Auditing.PostgreSQL;

public partial class AuditUnitOfWork : Legion.ADF.Auditing.IAuditUnitOfWork, Legion.EntityFrameworkCore.Model.Repositories.IDbUnitOfWork, Legion.Model.Repositories.IUnitOfWork
{
	public IEFConnectionProvider ConnectionProvider { get; }
	public IAuditEntriesManager? AuditEntriesManager { get; }

	public AuditUnitOfWork(IEFConnectionProvider connectionProvider)
	{
		Throw.IfArgumentNull(connectionProvider);

		ConnectionProvider = connectionProvider;
	}

	public AuditUnitOfWork(Legion.EntityFrameworkCore.Model.Repositories.IDbUnitOfWork dbUnitOfWork)
	{
		Throw.IfArgumentNull(dbUnitOfWork);

		ConnectionProvider = dbUnitOfWork.ConnectionProvider;
	}

	protected Legion.ADF.Auditing.PostgreSQL.IAuditDbContext GetContext(IScopeContext scopeContext)
		=> ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Auditing.PostgreSQL.IAuditDbContext>(scopeContext, null);

	//[Obsolete("Use SaveAsync() method instead.", true, DiagnosticId = "LADF_AudUoW_Save")]
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

	//[Obsolete("Use SaveAsync() method instead.", true, DiagnosticId = "LADF_AudUoW_Save")]
	//public virtual int Save(
	//	IScopeContext scopeContext,
	//	bool autoCommit,
	//	[CallerMemberName] string memberName = "",
	//	[CallerFilePath] string sourceFilePath = "",
	//	[CallerLineNumber] int sourceLineNumber = 0)
	//{
	//	var dbContext = GetContext(scopeContext);
	//	Throw.IfNull(dbContext);
	//	var result = dbContext.Save(scopeContext.WithNewTrace(memberName, sourceFilePath, sourceLineNumber));

	//	if (autoCommit)
	//		ConnectionProvider.TransactionsController.CommitAll(scopeContext);

	//	return result;
	//}

	//[Obsolete("Use SaveAsync() method instead.", true, DiagnosticId = "LADF_AudUoW_Save")]
	//public virtual int Save(
	//	IScopeContext scopeContext,
	//	bool autoCommit,
	//	SaveOptions? options,
	//	[CallerMemberName] string memberName = "",
	//	[CallerFilePath] string sourceFilePath = "",
	//	[CallerLineNumber] int sourceLineNumber = 0)
	//{
	//	var dbContext = GetContext(scopeContext);
	//	Throw.IfNull(dbContext);
	//	var result = dbContext.Save(scopeContext.WithNewTrace(memberName, sourceFilePath, sourceLineNumber), options);

	//	if (autoCommit)
	//		ConnectionProvider.TransactionsController.CommitAll(scopeContext);

	//	return result;
	//}

	//[Obsolete("Use SaveAsync() method instead.", true, DiagnosticId = "LADF_AudUoW_Save")]
	//public virtual int Save(
	//	IScopeContext scopeContext,
	//	bool autoCommit,
	//	bool acceptAllChangesOnSuccess,
	//	[CallerMemberName] string memberName = "",
	//	[CallerFilePath] string sourceFilePath = "",
	//	[CallerLineNumber] int sourceLineNumber = 0)
	//{
	//	var dbContext = GetContext(scopeContext);
	//	Throw.IfNull(dbContext);
	//	var result = dbContext.Save(scopeContext.WithNewTrace(memberName, sourceFilePath, sourceLineNumber), acceptAllChangesOnSuccess);

	//	if (autoCommit)
	//		ConnectionProvider.TransactionsController.CommitAll(scopeContext);

	//	return result;
	//}

	//[Obsolete("Use SaveAsync() method instead.", true, DiagnosticId = "LADF_AudUoW_Save")]
	//public virtual int Save(
	//	IScopeContext scopeContext,
	//	bool autoCommit,
	//	bool acceptAllChangesOnSuccess,
	//	SaveOptions? options,
	//	[CallerMemberName] string memberName = "",
	//	[CallerFilePath] string sourceFilePath = "",
	//	[CallerLineNumber] int sourceLineNumber = 0)
	//{
	//	var dbContext = GetContext(scopeContext);
	//	Throw.IfNull(dbContext);
	//	var result = dbContext.Save(scopeContext.WithNewTrace(memberName, sourceFilePath, sourceLineNumber), acceptAllChangesOnSuccess, options);

	//	if (autoCommit)
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
		bool autoCommit,
		CancellationToken cancellationToken = default,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);
		var result = await dbContext.SaveAsync(scopeContext.CreateNew(memberName, sourceFilePath, sourceLineNumber), cancellationToken);

		if (autoCommit)
			await ConnectionProvider.TransactionsController.CommitAllAsync(scopeContext, false, cancellationToken);

		return result;
	}

	public virtual async Task<int> SaveAsync(
		IScopeContext scopeContext,
		bool autoCommit,
		SaveOptions? options,
		CancellationToken cancellationToken = default,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);
		var result = await dbContext.SaveAsync(scopeContext.CreateNew(memberName, sourceFilePath, sourceLineNumber), options, cancellationToken);

		if (autoCommit)
			await ConnectionProvider.TransactionsController.CommitAllAsync(scopeContext, false, cancellationToken);

		return result;
	}

	public virtual async Task<int> SaveAsync(
		IScopeContext scopeContext,
		bool autoCommit,
		bool acceptAllChangesOnSuccess,
		CancellationToken cancellationToken = default,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);
		var result = await dbContext.SaveAsync(scopeContext.CreateNew(memberName, sourceFilePath, sourceLineNumber), acceptAllChangesOnSuccess, cancellationToken);

		if (autoCommit)
			await ConnectionProvider.TransactionsController.CommitAllAsync(scopeContext, false, cancellationToken);

		return result;
	}

	public virtual async Task<int> SaveAsync(
		IScopeContext scopeContext,
		bool autoCommit,
		bool acceptAllChangesOnSuccess,
		SaveOptions? options,
		CancellationToken cancellationToken = default,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);
		var result = await dbContext.SaveAsync(scopeContext.CreateNew(memberName, sourceFilePath, sourceLineNumber), acceptAllChangesOnSuccess, options, cancellationToken);

		if (autoCommit)
			await ConnectionProvider.TransactionsController.CommitAllAsync(scopeContext, false, cancellationToken);

		return result;
	}


	private Legion.ADF.Auditing.Audit.Repositories.IApplicationEntryRepository applicationEntry;
	public Legion.ADF.Auditing.Audit.Repositories.IApplicationEntryRepository ApplicationEntryRepository
		=> applicationEntry ??= new Legion.ADF.Auditing.PostgreSQL.Audit.Repositories.ApplicationEntryRepository(ConnectionProvider, null);


	private Legion.ADF.Auditing.Audit.Repositories.IApplicationEntryTokenRepository applicationEntryToken;
	public Legion.ADF.Auditing.Audit.Repositories.IApplicationEntryTokenRepository ApplicationEntryTokenRepository
		=> applicationEntryToken ??= new Legion.ADF.Auditing.PostgreSQL.Audit.Repositories.ApplicationEntryTokenRepository(ConnectionProvider, null);


	private Legion.ADF.Auditing.Audit.Repositories.IAuditEntryRepository auditEntry;
	public Legion.ADF.Auditing.Audit.Repositories.IAuditEntryRepository AuditEntryRepository
		=> auditEntry ??= new Legion.ADF.Auditing.PostgreSQL.Audit.Repositories.AuditEntryRepository(ConnectionProvider, null);


	private Legion.ADF.Auditing.Audit.Repositories.IAuditTypeRepository auditType;
	public Legion.ADF.Auditing.Audit.Repositories.IAuditTypeRepository AuditTypeRepository
		=> auditType ??= new Legion.ADF.Auditing.PostgreSQL.Audit.Repositories.AuditTypeRepository(ConnectionProvider, null);
}
