using Legion;
using Legion.Database;
using Legion.EntityFrameworkCore;
using Legion.Model.Audit;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Model;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace Legion.ADF.Auth.SqlServer;

internal partial class AuthUnitOfWork : Legion.ADF.Auth.IAuthUnitOfWork, Legion.EntityFrameworkCore.Model.Repositories.IDbUnitOfWork, Legion.Model.Repositories.IUnitOfWork, IDisposable, IAsyncDisposable
{
	private bool _isInternalConnectionProvider;
	private bool _disposed;

#if TRACK_OBJECTS
	public Guid IdAuthUnitOfWork { get; }
#endif

	public IEFConnectionProvider ConnectionProvider { get; }
	Legion.Database.IConnectionProvider Legion.Model.Repositories.IUnitOfWork.ConnectionProvider => ConnectionProvider;
	public IServiceProvider ServiceProvider => ConnectionProvider.ServiceProvider;

	public ILogger Logger => ConnectionProvider.Logger;
	
	public AuthUnitOfWork(IEFConnectionProvider connectionProvider)
	{
#if TRACK_OBJECTS
		IdAuthUnitOfWork = Guid.NewGuid();
		Trackers.ObjectLifetimeTracker.Track(this, IdAuthUnitOfWork.ToString());
#endif

		Throw.IfArgumentNull(connectionProvider);

		ConnectionProvider = connectionProvider;
		_isInternalConnectionProvider = false; //disposed by caller
	}

	public AuthUnitOfWork(Legion.EntityFrameworkCore.Model.Repositories.IDbUnitOfWork dbUnitOfWork)
	{
#if TRACK_OBJECTS
		IdAuthUnitOfWork = Guid.NewGuid();
		Trackers.ObjectLifetimeTracker.Track(this, IdAuthUnitOfWork.ToString());
#endif

		Throw.IfArgumentNull(dbUnitOfWork);

		ConnectionProvider = dbUnitOfWork.ConnectionProvider;
		_isInternalConnectionProvider = false; //disposed by dbUnitOfWork
	}

	public AuthUnitOfWork(Legion.EntityFrameworkCore.Model.Repositories.IDbQueryUnitOfWork dbQueryUnitOfWork)
	{
#if TRACK_OBJECTS
		IdAuthUnitOfWork = Guid.NewGuid();
		Trackers.ObjectLifetimeTracker.Track(this, IdAuthUnitOfWork.ToString());
#endif

		Throw.IfArgumentNull(dbQueryUnitOfWork);

		ConnectionProvider = dbQueryUnitOfWork.ConnectionProvider;
		_isInternalConnectionProvider = false; //disposed by dbQueryUnitOfWork
	}

	public AuthUnitOfWork(
		IServiceProvider serviceProvider,
		string connectionStirng,
		System.Data.IsolationLevel? isolationLevel,
		bool? allowLocking,
		bool createAuditEntryStore)
	{
#if TRACK_OBJECTS
		IdAuthUnitOfWork = Guid.NewGuid();
		Trackers.ObjectLifetimeTracker.Track(this, IdAuthUnitOfWork.ToString());
#endif

		Throw.IfArgumentNull(serviceProvider);
		Throw.IfArgumentNullOrWhiteSpace(connectionStirng);

		var connectionProviderFactory = serviceProvider.GetRequiredService<IEFConnectionProviderFactory>();
		ConnectionProvider = connectionProviderFactory.CreateWithNewTransaction(
			serviceProvider,
			connectionStirng,
			isolationLevel,
			allowLocking,
			createAuditEntryStore);
		_isInternalConnectionProvider = true;
	}

	public AuthUnitOfWork(
		IServiceProvider serviceProvider,
		string connectionStirng,
		bool? allowLocking,
		bool createAuditEntryStore)
	{
#if TRACK_OBJECTS
		IdAuthUnitOfWork = Guid.NewGuid();
		Trackers.ObjectLifetimeTracker.Track(this, IdAuthUnitOfWork.ToString());
#endif

		Throw.IfArgumentNull(serviceProvider);
		Throw.IfArgumentNullOrWhiteSpace(connectionStirng);

		var connectionProviderFactory = serviceProvider.GetRequiredService<IEFConnectionProviderFactory>();
		ConnectionProvider = connectionProviderFactory.CreateWithoutTransaction(
			serviceProvider,
			connectionStirng,
			allowLocking,
			createAuditEntryStore);
		_isInternalConnectionProvider = true;
	}
	protected Legion.ADF.Auth.SqlServer.IAuthDbContext GetContext(IScopeContext scopeContext)
		=> ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Auth.SqlServer.IAuthDbContext>(scopeContext);

	public virtual IResult<int> Save(
		IScopeContext scopeContext,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> Save(
			scopeContext,
			false,
			memberName,
			sourceFilePath,
			sourceLineNumber);

	public virtual IResult<int> Save(
		IScopeContext scopeContext,
		bool autoCommitAllTransactions,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
	{
		var result = new ResultBuilder<int>();

		try
		{
			var dbContext = GetContext(scopeContext);

			if (result.IsNull(scopeContext, dbContext))
				return result.Build();

			var saveResult = dbContext.Save(scopeContext.CreateNew(false, memberName, sourceFilePath, sourceLineNumber));

			if (autoCommitAllTransactions)
			{
				if (result.IsNull(scopeContext, ConnectionProvider.TransactionsController))
					return result.Build();

				var commitResult = ConnectionProvider.CommitAll(scopeContext, TransactionsControllerStatus.NotIdle);
				if (result.MergeHasError(commitResult))
					return result.Build();
			}

			return result.WithData(saveResult).Build();
		}
		catch (Exception ex)
		{
			result.WithError(scopeContext, null, x => x.ExceptionInfo(ex));

			if (autoCommitAllTransactions && ConnectionProvider.TransactionsController != null)
			{
				var rollbackResult = ConnectionProvider.RollbackAll(scopeContext, ex, TransactionsControllerStatus.CommitInProgress);
				if (result.MergeHasError(rollbackResult))
					return result.Build();
			}

			return result.Build();
		}
	}

	public virtual IResult<int> Save(
		IScopeContext scopeContext,
		bool autoCommitAllTransactions,
		SaveOptions? options,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
	{
		var result = new ResultBuilder<int>();

		try
		{
			var dbContext = GetContext(scopeContext);

			if (result.IsNull(scopeContext, dbContext))
				return result.Build();

			var saveResult = dbContext.Save(scopeContext.CreateNew(false, memberName, sourceFilePath, sourceLineNumber), options);

			if (autoCommitAllTransactions)
			{
				if (result.IsNull(scopeContext, ConnectionProvider.TransactionsController))
					return result.Build();

				var commitResult = ConnectionProvider.CommitAll(scopeContext, TransactionsControllerStatus.NotIdle);
				if (result.MergeHasError(commitResult))
					return result.Build();
			}

			return result.WithData(saveResult).Build();
		}
		catch (Exception ex)
		{
			result.WithError(scopeContext, null, x => x.ExceptionInfo(ex));

			if (autoCommitAllTransactions && ConnectionProvider.TransactionsController != null)
			{
				var rollbackResult = ConnectionProvider.RollbackAll(scopeContext, ex, TransactionsControllerStatus.CommitInProgress);
				if (result.MergeHasError(rollbackResult))
					return result.Build();
			}

			return result.Build();
		}
	}

	public virtual IResult<int> Save(
		IScopeContext scopeContext,
		bool autoCommitAllTransactions,
		bool acceptAllChangesOnSuccess,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
	{
		var result = new ResultBuilder<int>();

		try
		{
			var dbContext = GetContext(scopeContext);

			if (result.IsNull(scopeContext, dbContext))
				return result.Build();

			var saveResult = dbContext.Save(scopeContext.CreateNew(false, memberName, sourceFilePath, sourceLineNumber), acceptAllChangesOnSuccess);

			if (autoCommitAllTransactions)
			{
				if (result.IsNull(scopeContext, ConnectionProvider.TransactionsController))
					return result.Build();

				var commitResult = ConnectionProvider.CommitAll(scopeContext, TransactionsControllerStatus.NotIdle);
				if (result.MergeHasError(commitResult))
					return result.Build();
			}

			return result.WithData(saveResult).Build();
		}
		catch (Exception ex)
		{
			result.WithError(scopeContext, null, x => x.ExceptionInfo(ex));

			if (autoCommitAllTransactions && ConnectionProvider.TransactionsController != null)
			{
				var rollbackResult = ConnectionProvider.RollbackAll(scopeContext, ex, TransactionsControllerStatus.CommitInProgress);
				if (result.MergeHasError(rollbackResult))
					return result.Build();
			}

			return result.Build();
		}
	}

	public virtual IResult<int> Save(
		IScopeContext scopeContext,
		bool autoCommitAllTransactions,
		bool acceptAllChangesOnSuccess,
		SaveOptions? options,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
	{
		var result = new ResultBuilder<int>();

		try
		{
			var dbContext = GetContext(scopeContext);

			if (result.IsNull(scopeContext, dbContext))
				return result.Build();

			var saveResult = dbContext.Save(scopeContext.CreateNew(false, memberName, sourceFilePath, sourceLineNumber), acceptAllChangesOnSuccess, options);

			if (autoCommitAllTransactions)
			{
				if (result.IsNull(scopeContext, ConnectionProvider.TransactionsController))
					return result.Build();

				var commitResult = ConnectionProvider.CommitAll(scopeContext, TransactionsControllerStatus.NotIdle);
				if (result.MergeHasError(commitResult))
					return result.Build();
			}

			return result.WithData(saveResult).Build();
		}
		catch (Exception ex)
		{
			result.WithError(scopeContext, null, x => x.ExceptionInfo(ex));

			if (autoCommitAllTransactions && ConnectionProvider.TransactionsController != null)
			{
				var rollbackResult = ConnectionProvider.RollbackAll(scopeContext, ex, TransactionsControllerStatus.CommitInProgress);
				if (result.MergeHasError(rollbackResult))
					return result.Build();
			}

			return result.Build();
		}
	}

	public virtual Task<IResult<int>> SaveAsync(
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

	public virtual async Task<IResult<int>> SaveAsync(
		IScopeContext scopeContext,
		bool autoCommitAllTransactions,
		CancellationToken cancellationToken = default,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
	{
		var result = new ResultBuilder<int>();

		try
		{
			var dbContext = GetContext(scopeContext);
			
			if (result.IsNull(scopeContext, dbContext))
				return result.Build();

			var saveResult = await dbContext.SaveAsync(scopeContext.CreateNew(false, memberName, sourceFilePath, sourceLineNumber), cancellationToken);

			if (autoCommitAllTransactions)
			{
				if (result.IsNull(scopeContext, ConnectionProvider.TransactionsController))
					return result.Build();

				var commitResult = await ConnectionProvider.CommitAllAsync(scopeContext, TransactionsControllerStatus.NotIdle, cancellationToken);
				if (result.MergeHasError(commitResult))
					return result.Build();
			}

			return result.WithData(saveResult).Build();
		}
		catch (Exception ex)
		{
			result.WithError(scopeContext, null, x => x.ExceptionInfo(ex));

			if (autoCommitAllTransactions && ConnectionProvider.TransactionsController != null)
			{
				var rollbackResult = await ConnectionProvider.RollbackAllAsync(scopeContext, ex, TransactionsControllerStatus.CommitInProgress);
				if (result.MergeHasError(rollbackResult))
					return result.Build();
			}

			return result.Build();
		}
	}

	public virtual async Task<IResult<int>> SaveAsync(
		IScopeContext scopeContext,
		bool autoCommitAllTransactions,
		SaveOptions? options,
		CancellationToken cancellationToken = default,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
	{
		var result = new ResultBuilder<int>();

		try
		{
			var dbContext = GetContext(scopeContext);

			if (result.IsNull(scopeContext, dbContext))
				return result.Build();

			var saveResult = await dbContext.SaveAsync(scopeContext.CreateNew(false, memberName, sourceFilePath, sourceLineNumber), options, cancellationToken);

			if (autoCommitAllTransactions)
			{
				if (result.IsNull(scopeContext, ConnectionProvider.TransactionsController))
					return result.Build();

				var commitResult = await ConnectionProvider.CommitAllAsync(scopeContext, TransactionsControllerStatus.NotIdle, cancellationToken);
				if (result.MergeHasError(commitResult))
					return result.Build();
			}

			return result.WithData(saveResult).Build();
		}
		catch (Exception ex)
		{
			result.WithError(scopeContext, null, x => x.ExceptionInfo(ex));

			if (autoCommitAllTransactions && ConnectionProvider.TransactionsController != null)
			{
				var rollbackResult = await ConnectionProvider.RollbackAllAsync(scopeContext, ex, TransactionsControllerStatus.CommitInProgress);
				if (result.MergeHasError(rollbackResult))
					return result.Build();
			}

			return result.Build();
		}
	}

	public virtual async Task<IResult<int>> SaveAsync(
		IScopeContext scopeContext,
		bool autoCommitAllTransactions,
		bool acceptAllChangesOnSuccess,
		CancellationToken cancellationToken = default,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
	{
		var result = new ResultBuilder<int>();

		try
		{
			var dbContext = GetContext(scopeContext);

			if (result.IsNull(scopeContext, dbContext))
				return result.Build();

			var saveResult = await dbContext.SaveAsync(scopeContext.CreateNew(false, memberName, sourceFilePath, sourceLineNumber), acceptAllChangesOnSuccess, cancellationToken);

			if (autoCommitAllTransactions)
			{
				if (result.IsNull(scopeContext, ConnectionProvider.TransactionsController))
					return result.Build();

				var commitResult = await ConnectionProvider.CommitAllAsync(scopeContext, TransactionsControllerStatus.NotIdle, cancellationToken);
				if (result.MergeHasError(commitResult))
					return result.Build();
			}

			return result.WithData(saveResult).Build();
		}
		catch (Exception ex)
		{
			result.WithError(scopeContext, null, x => x.ExceptionInfo(ex));

			if (autoCommitAllTransactions && ConnectionProvider.TransactionsController != null)
			{
				var rollbackResult = await ConnectionProvider.RollbackAllAsync(scopeContext, ex, TransactionsControllerStatus.CommitInProgress);
				if (result.MergeHasError(rollbackResult))
					return result.Build();
			}

			return result.Build();
		}
	}

	public virtual async Task<IResult<int>> SaveAsync(
		IScopeContext scopeContext,
		bool autoCommitAllTransactions,
		bool acceptAllChangesOnSuccess,
		SaveOptions? options,
		CancellationToken cancellationToken = default,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
	{
		var result = new ResultBuilder<int>();

		try
		{
			var dbContext = GetContext(scopeContext);

			if (result.IsNull(scopeContext, dbContext))
				return result.Build();

			var saveResult = await dbContext.SaveAsync(scopeContext.CreateNew(false, memberName, sourceFilePath, sourceLineNumber), acceptAllChangesOnSuccess, options, cancellationToken);

			if (autoCommitAllTransactions)
			{
				if (result.IsNull(scopeContext, ConnectionProvider.TransactionsController))
					return result.Build();

				var commitResult = await ConnectionProvider.CommitAllAsync(scopeContext, TransactionsControllerStatus.NotIdle, cancellationToken);
				if (result.MergeHasError(commitResult))
					return result.Build();
			}

			return result.WithData(saveResult).Build();
		}
		catch (Exception ex)
		{
			result.WithError(scopeContext, null, x => x.ExceptionInfo(ex));

			if (autoCommitAllTransactions && ConnectionProvider.TransactionsController != null)
			{
				var rollbackResult = await ConnectionProvider.RollbackAllAsync(scopeContext, ex, TransactionsControllerStatus.CommitInProgress);
				if (result.MergeHasError(rollbackResult))
					return result.Build();
			}

			return result.Build();
		}
	}

	public async Task<int> ExecuteSqlInterpolatedAsync(
		IScopeContext scopeContext,
		FormattableString sql,
		CancellationToken cancellationToken = default)
	{
		var dbContext = GetContext(scopeContext);
		return await dbContext.Database.ExecuteSqlInterpolatedAsync(sql, cancellationToken);
	}

	public async Task<int> ExecuteSqlRawAsync(
		IScopeContext scopeContext,
		string sql,
		IEnumerable<object> parameters,
		CancellationToken cancellationToken = default)
	{
		var dbContext = GetContext(scopeContext);
		return await dbContext.Database.ExecuteSqlRawAsync(sql, parameters, cancellationToken);
	}

	public int ExecuteSqlInterpolated(
		IScopeContext scopeContext,
		FormattableString sql)
	{
		var dbContext = GetContext(scopeContext);
		return dbContext.Database.ExecuteSqlInterpolated(sql);
	}

	public int ExecuteSqlRaw(
		IScopeContext scopeContext,
		string sql,
		IEnumerable<object> parameters)
	{
		var dbContext = GetContext(scopeContext);
		return dbContext.Database.ExecuteSqlRaw(sql, parameters);
	}

	public virtual IResult<bool?> Commit(
		IScopeContext scopeContext,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> ConnectionProvider.CommitAll(scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber), TransactionsControllerStatus.NotIdle);

	public virtual async Task<IResult<bool?>> CommitAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> await ConnectionProvider.CommitAllAsync(scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber), TransactionsControllerStatus.NotIdle, cancellationToken);

	public virtual IResult<bool?> Rollback(
		IScopeContext scopeContext,
		Exception? exception,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> ConnectionProvider.RollbackAll(scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber), exception, TransactionsControllerStatus.CommitInProgress);

	public virtual async Task<IResult<bool?>> RollbackAsync(
		IScopeContext scopeContext,
		Exception? exception,
		CancellationToken cancellationToken = default,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> await ConnectionProvider.RollbackAllAsync(scopeContext.CreateNew(true, memberName, sourceFilePath, sourceLineNumber), exception, TransactionsControllerStatus.CommitInProgress, cancellationToken);

	private Legion.ADF.Auth.Model.Repositories.IExternalLoginRepository? externalLogin;
	public Legion.ADF.Auth.Model.Repositories.IExternalLoginRepository ExternalLoginRepository
		=> externalLogin ??= new Legion.ADF.Auth.SqlServer.Model.Repositories.ExternalLoginRepository(ConnectionProvider);


	private Legion.ADF.Auth.Model.Repositories.ILoginProviderRepository? loginProvider;
	public Legion.ADF.Auth.Model.Repositories.ILoginProviderRepository LoginProviderRepository
		=> loginProvider ??= new Legion.ADF.Auth.SqlServer.Model.Repositories.LoginProviderRepository(ConnectionProvider);


	private Legion.ADF.Auth.Model.Repositories.IPermissionRepository? permission;
	public Legion.ADF.Auth.Model.Repositories.IPermissionRepository PermissionRepository
		=> permission ??= new Legion.ADF.Auth.SqlServer.Model.Repositories.PermissionRepository(ConnectionProvider);


	private Legion.ADF.Auth.Model.Repositories.IRoleRepository? role;
	public Legion.ADF.Auth.Model.Repositories.IRoleRepository RoleRepository
		=> role ??= new Legion.ADF.Auth.SqlServer.Model.Repositories.RoleRepository(ConnectionProvider);


	private Legion.ADF.Auth.Model.Repositories.IRolePermissionRepository? rolePermission;
	public Legion.ADF.Auth.Model.Repositories.IRolePermissionRepository RolePermissionRepository
		=> rolePermission ??= new Legion.ADF.Auth.SqlServer.Model.Repositories.RolePermissionRepository(ConnectionProvider);


	private Legion.ADF.Auth.Model.Repositories.IUserRepository? user;
	public Legion.ADF.Auth.Model.Repositories.IUserRepository UserRepository
		=> user ??= new Legion.ADF.Auth.SqlServer.Model.Repositories.UserRepository(ConnectionProvider);


	private Legion.ADF.Auth.Model.Repositories.IUserPermissionRepository? userPermission;
	public Legion.ADF.Auth.Model.Repositories.IUserPermissionRepository UserPermissionRepository
		=> userPermission ??= new Legion.ADF.Auth.SqlServer.Model.Repositories.UserPermissionRepository(ConnectionProvider);


	private Legion.ADF.Auth.Model.Repositories.IUserRoleRepository? userRole;
	public Legion.ADF.Auth.Model.Repositories.IUserRoleRepository UserRoleRepository
		=> userRole ??= new Legion.ADF.Auth.SqlServer.Model.Repositories.UserRoleRepository(ConnectionProvider);


	private Legion.ADF.Auth.Model.Repositories.IUserTokenRepository? userToken;
	public Legion.ADF.Auth.Model.Repositories.IUserTokenRepository UserTokenRepository
		=> userToken ??= new Legion.ADF.Auth.SqlServer.Model.Repositories.UserTokenRepository(ConnectionProvider);

	public async ValueTask DisposeAsync()
	{
		if (_disposed)
			return;

		_disposed = true;

		await DisposeAsyncCoreAsync().ConfigureAwait(false);

		Dispose(disposing: false);
		GC.SuppressFinalize(this);
	}

	private async ValueTask DisposeAsyncCoreAsync()
	{
#if TRACK_OBJECTS
		Trackers.ObjectLifetimeTracker.SetDisposed(this, IdAuthUnitOfWork.ToString());
#endif

		if (_isInternalConnectionProvider && ConnectionProvider != null)
		{
			await ConnectionProvider.DisposeAsync();
		}
	}

	private void Dispose(bool disposing)
	{
		if (_disposed)
			return;

		_disposed = true;

		if (disposing)
		{
#if TRACK_OBJECTS
		Trackers.ObjectLifetimeTracker.SetDisposed(this, IdAuthUnitOfWork.ToString());
#endif

			if (_isInternalConnectionProvider)
			{
				ConnectionProvider?.Dispose();
			}
		}
	}

	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}
}
