using Legion.Exceptions.Internal;
using Legion.Threading;

namespace Legion.Transactions;

public class TransactionsController : ITransactionsController, IDisposable, IAsyncDisposable
{
	private readonly object _lock = new();
	private readonly Lazy<AsyncLock> _asyncLock = new(() => new());

	private readonly List<ITransactionManager> _transactionManagers;

#if TRACK_OBJECTS
	public Guid IdTransactionsController { get; }
#endif

	private TransactionsControllerStatus _transactionStatus;
	public TransactionsControllerStatus TransactionsControllerStatus => _transactionStatus;

	public Guid TransactionsControllerIdentifier { get; }

	public TransactionsController()
	{
#if TRACK_OBJECTS
		IdTransactionsController = Legion.GlobalContext.Instance.NewGuid();
		Trackers.ObjectLifetimeTracker.Track(this, IdTransactionsController.ToString());
#endif

		TransactionsControllerIdentifier = GlobalContext.Instance.NewGuid();
		_transactionStatus = TransactionsControllerStatus.Idle;
		_transactionManagers = [];
	}

	public void RegisterTransactionManager(IScopeContext scopeContext, ITransactionManager transactionManager)
	{
		if (_transactionStatus != TransactionsControllerStatus.Idle)
			Throw.TransactionException(
				ErrorCodes.TransactionException.InvalidTransactionStatus(_transactionStatus, nameof(RegisterTransactionManager)),
				scopeContext);

		lock (_lock)
		{
			if (_transactionStatus != TransactionsControllerStatus.Idle)
				Throw.TransactionException(
					ErrorCodes.TransactionException.InvalidTransactionStatus(_transactionStatus, nameof(RegisterTransactionManager)),
					scopeContext);

			if (!_transactionManagers.Contains(transactionManager))
			{
				_transactionManagers.Add(transactionManager);
				transactionManager.SetTransactionsController(this);
			}
		}
	}

	public void UnregisterTransactionManager(IScopeContext scopeContext, ITransactionManager transactionManager)
	{
		if (_transactionStatus != TransactionsControllerStatus.Idle)
			return;
			//Throw.TransactionException(
			//	ErrorCodes.TransactionException.InvalidTransactionStatus(_transactionStatus, nameof(UnregisterTransactionManager)),
			//	scopeContext);

		lock (_lock)
		{
			if (_transactionStatus != TransactionsControllerStatus.Idle)
				return;
				//Throw.TransactionException(
				//	ErrorCodes.TransactionException.InvalidTransactionStatus(_transactionStatus, nameof(UnregisterTransactionManager)),
				//	scopeContext);

			var existingTransactionManager = _transactionManagers.FirstOrDefault(x => x.Equals(transactionManager));
			if (existingTransactionManager != null)
			{
				_transactionManagers.Remove(existingTransactionManager);
				existingTransactionManager.SetTransactionsController(null);
			}

			transactionManager.SetTransactionsController(null);
		}
	}

	public void SetRecreated()
	{
		_transactionStatus = TransactionsControllerStatus.Idle;
	}

	public IResult CommitAll(IScopeContext scopeContext, TransactionsControllerStatus throwInvalidStatuses = TransactionsControllerStatus.NotIdle)
	{
		var result = new ResultBuilder();
		//Throw.IfArgumentNull(scopeContext);

		if ((throwInvalidStatuses & _transactionStatus) == _transactionStatus)
			return result.WithTransactionException(
					scopeContext,
					ErrorCodes.TransactionException.InvalidTransactionStatus(_transactionStatus, nameof(CommitAll)));

		if (_transactionStatus != TransactionsControllerStatus.Idle)
			return result.Build();

		lock (_lock)
		{
			if ((throwInvalidStatuses & _transactionStatus) == _transactionStatus)
				return result.WithTransactionException(
						scopeContext,
						ErrorCodes.TransactionException.InvalidTransactionStatus(_transactionStatus, nameof(CommitAll)));

			if (_transactionStatus != TransactionsControllerStatus.Idle)
				return result.Build();

			_transactionStatus = TransactionsControllerStatus.Commiting;

			try
			{
				foreach (var transactionManager in _transactionManagers)
					transactionManager.PreCommit(scopeContext);

				foreach (var transactionManager in _transactionManagers)
					transactionManager.Commit(scopeContext);

				foreach (var transactionManager in _transactionManagers)
					transactionManager.PostCommit(scopeContext);
			}
			catch (Exception ex)
			{
				return result.WithTransactionException(
					scopeContext,
					ErrorCodes.TransactionException.Commit,
					x => x.ExceptionInfo(ex));
			}

			_transactionStatus = TransactionsControllerStatus.Commited;

			return result.Build();
		}
	}

	public async Task<IResult> CommitAllAsync(IScopeContext scopeContext, TransactionsControllerStatus throwInvalidStatuses = TransactionsControllerStatus.NotIdle, CancellationToken cancellationToken = default)
	{
		var result = new ResultBuilder();
		//Throw.IfArgumentNull(scopeContext);

		if ((throwInvalidStatuses & _transactionStatus) == _transactionStatus)
			return result.WithTransactionException(
				scopeContext,
				ErrorCodes.TransactionException.InvalidTransactionStatus(_transactionStatus, nameof(CommitAllAsync)));

		if (_transactionStatus != TransactionsControllerStatus.Idle)
			return result.Build();

		using (await _asyncLock.Value.LockAsync().ConfigureAwait(false))
		{
			if ((throwInvalidStatuses & _transactionStatus) == _transactionStatus)
				return result.WithTransactionException(
					scopeContext,
					ErrorCodes.TransactionException.InvalidTransactionStatus(_transactionStatus, nameof(CommitAllAsync)));

			if (_transactionStatus != TransactionsControllerStatus.Idle)
				return result.Build();

			_transactionStatus = TransactionsControllerStatus.Commiting;

			try
			{
				foreach (var transactionManager in _transactionManagers)
					await transactionManager.PreCommitAsync(scopeContext, cancellationToken);

				foreach (var transactionManager in _transactionManagers)
					await transactionManager.CommitAsync(scopeContext, cancellationToken);

				foreach (var transactionManager in _transactionManagers)
					await transactionManager.PostCommitAsync(scopeContext, cancellationToken);
			}
			catch (Exception ex)
			{
				return result.WithTransactionException(
					scopeContext,
					ErrorCodes.TransactionException.Commit,
					x => x.ExceptionInfo(ex));
			}

			_transactionStatus = TransactionsControllerStatus.Commited;

			return result.Build();
		}
	}

	public IResult RollbackAll(IScopeContext scopeContext, Exception? exception, TransactionsControllerStatus throwInvalidStatuses = TransactionsControllerStatus.CommitInProgress)
	{
		var result = new ResultBuilder();
		//Throw.IfArgumentNull(scopeContext);

		if ((throwInvalidStatuses & _transactionStatus) == _transactionStatus)
			return result.WithTransactionException(
					scopeContext,
					ErrorCodes.TransactionException.InvalidTransactionStatus(_transactionStatus, nameof(RollbackAll)));

		if ((TransactionsControllerStatus.NotCommitable & _transactionStatus) == _transactionStatus)
			return result.Build();

		lock (_lock)
		{
			if ((throwInvalidStatuses & _transactionStatus) == _transactionStatus)
				return result.WithTransactionException(
						scopeContext,
						ErrorCodes.TransactionException.InvalidTransactionStatus(_transactionStatus, nameof(RollbackAll)));

			if ((TransactionsControllerStatus.NotCommitable & _transactionStatus) == _transactionStatus)
				return result.Build();

			_transactionStatus = TransactionsControllerStatus.Rollingback;

			try
			{
				foreach (var transactionManager in _transactionManagers)
					transactionManager.PreRollback(scopeContext, exception);

				foreach (var transactionManager in _transactionManagers)
					transactionManager.Rollback(scopeContext, exception);

				foreach (var transactionManager in _transactionManagers)
					transactionManager.PostRollback(scopeContext, exception);
			}
			catch (Exception ex)
			{
				return result.WithTransactionException(
					scopeContext,
					ErrorCodes.TransactionException.Rollback,
					x => x.ExceptionInfo(ex));
			}

			_transactionStatus = TransactionsControllerStatus.Rolledback;

			return result.Build();
		}
	}

	public async Task<IResult> RollbackAllAsync(IScopeContext scopeContext, Exception? exception, TransactionsControllerStatus throwInvalidStatuses = TransactionsControllerStatus.CommitInProgress, CancellationToken cancellationToken = default)
	{
		var result = new ResultBuilder();
		//Throw.IfArgumentNull(scopeContext);

		if ((throwInvalidStatuses & _transactionStatus) == _transactionStatus)
			return result.WithTransactionException(
					scopeContext,
					ErrorCodes.TransactionException.InvalidTransactionStatus(_transactionStatus, nameof(RollbackAllAsync)));

		if ((TransactionsControllerStatus.NotCommitable & _transactionStatus) == _transactionStatus)
			return result.Build();

		using (await _asyncLock.Value.LockAsync().ConfigureAwait(false))
		{
			if ((throwInvalidStatuses & _transactionStatus) == _transactionStatus)
				return result.WithTransactionException(
						scopeContext,
						ErrorCodes.TransactionException.InvalidTransactionStatus(_transactionStatus, nameof(RollbackAllAsync)));

			if ((TransactionsControllerStatus.NotCommitable & _transactionStatus) == _transactionStatus)
				return result.Build();

			_transactionStatus = TransactionsControllerStatus.Rollingback;

			try
			{
				foreach (var transactionManager in _transactionManagers)
					await transactionManager.PreRollbackAsync(scopeContext, exception, cancellationToken);

				foreach (var transactionManager in _transactionManagers)
					await transactionManager.RollbackAsync(scopeContext, exception, cancellationToken);

				foreach (var transactionManager in _transactionManagers)
					await transactionManager.PostRollbackAsync(scopeContext, exception, cancellationToken);
			}
			catch (Exception ex)
			{
				return result.WithTransactionException(
					scopeContext,
					ErrorCodes.TransactionException.Rollback,
					x => x.ExceptionInfo(ex));
			}

			_transactionStatus = TransactionsControllerStatus.Rolledback;

			return result.Build();
		}
	}

	//public IResult CommitAll(IScopeContext scopeContext, bool throwInvalidTransactionStatus)
	//{
	//	var result = new ResultBuilder();
	//	//Throw.IfArgumentNull(scopeContext);

	//	if (_transactionStatus != TransactionsControllerStatus.Idle)
	//	{
	//		if (throwInvalidTransactionStatus)
	//			return result.WithTransactionException(
	//				scopeContext,
	//				ErrorCodes.TransactionException.InvalidTransactionStatus(_transactionStatus, nameof(CommitAll)));

	//		return result.Build();
	//	}

	//	lock (_lock)
	//	{
	//		if (_transactionStatus != TransactionsControllerStatus.Idle)
	//		{
	//			if (throwInvalidTransactionStatus)
	//				return result.WithTransactionException(
	//					scopeContext,
	//					ErrorCodes.TransactionException.InvalidTransactionStatus(_transactionStatus, nameof(CommitAll)));

	//			return result.Build();
	//		}

	//		_transactionStatus = TransactionsControllerStatus.Commiting;

	//		try
	//		{
	//			foreach (var transactionManager in _transactionManagers)
	//				transactionManager.PreCommit(scopeContext);

	//			foreach (var transactionManager in _transactionManagers)
	//				transactionManager.Commit(scopeContext);

	//			foreach (var transactionManager in _transactionManagers)
	//				transactionManager.PostCommit(scopeContext);
	//		}
	//		catch (Exception ex)
	//		{
	//			return result.WithTransactionException(
	//				scopeContext,
	//				ErrorCodes.TransactionException.Commit,
	//				x => x.ExceptionInfo(ex));
	//		}

	//		_transactionStatus = TransactionsControllerStatus.Commited;

	//		return result.Build();
	//	}
	//}

	//public async Task<IResult> CommitAllAsync(IScopeContext scopeContext, bool throwInvalidTransactionStatus, CancellationToken cancellationToken = default)
	//{
	//	var result = new ResultBuilder();
	//	//Throw.IfArgumentNull(scopeContext);

	//	if (_transactionStatus != TransactionsControllerStatus.Idle)
	//	{
	//		if (throwInvalidTransactionStatus)
	//			return result.WithTransactionException(
	//				scopeContext,
	//				ErrorCodes.TransactionException.InvalidTransactionStatus(_transactionStatus, nameof(CommitAllAsync)));

	//		return result.Build();
	//	}

	//	using (await _asyncLock.Value.LockAsync().ConfigureAwait(false))
	//	{
	//		if (_transactionStatus != TransactionsControllerStatus.Idle)
	//		{
	//			if (throwInvalidTransactionStatus)
	//				return result.WithTransactionException(
	//					scopeContext,
	//					ErrorCodes.TransactionException.InvalidTransactionStatus(_transactionStatus, nameof(CommitAllAsync)));

	//			return result.Build();
	//		}

	//		_transactionStatus = TransactionsControllerStatus.Commiting;

	//		try
	//		{
	//			foreach (var transactionManager in _transactionManagers)
	//				await transactionManager.PreCommitAsync(scopeContext, cancellationToken);

	//			foreach (var transactionManager in _transactionManagers)
	//				await transactionManager.CommitAsync(scopeContext, cancellationToken);

	//			foreach (var transactionManager in _transactionManagers)
	//				await transactionManager.PostCommitAsync(scopeContext, cancellationToken);
	//		}
	//		catch (Exception ex)
	//		{
	//			return result.WithTransactionException(
	//				scopeContext,
	//				ErrorCodes.TransactionException.Commit,
	//				x => x.ExceptionInfo(ex));
	//		}

	//		_transactionStatus = TransactionsControllerStatus.Commited;

	//		return result.Build();
	//	}
	//}

	//public IResult RollbackAll(IScopeContext scopeContext, Exception? exception, bool throwInvalidTransactionStatus)
	//{
	//	var result = new ResultBuilder();
	//	//Throw.IfArgumentNull(scopeContext);

	//	if (_transactionStatus != TransactionsControllerStatus.Idle)
	//	{
	//		if (throwInvalidTransactionStatus)
	//			return result.WithTransactionException(
	//				scopeContext,
	//				ErrorCodes.TransactionException.InvalidTransactionStatus(_transactionStatus, nameof(RollbackAll)));

	//		return result.Build();
	//	}

	//	lock (_lock)
	//	{
	//		if (_transactionStatus != TransactionsControllerStatus.Idle)
	//		{
	//			if (throwInvalidTransactionStatus)
	//				return result.WithTransactionException(
	//					scopeContext,
	//					ErrorCodes.TransactionException.InvalidTransactionStatus(_transactionStatus, nameof(RollbackAll)));

	//			return result.Build();
	//		}

	//		_transactionStatus = TransactionsControllerStatus.Rollingback;

	//		try
	//		{
	//			foreach (var transactionManager in _transactionManagers)
	//				transactionManager.PreRollback(scopeContext, exception);

	//			foreach (var transactionManager in _transactionManagers)
	//				transactionManager.Rollback(scopeContext, exception);

	//			foreach (var transactionManager in _transactionManagers)
	//				transactionManager.PostRollback(scopeContext, exception);
	//		}
	//		catch (Exception ex)
	//		{
	//			return result.WithTransactionException(
	//				scopeContext,
	//				ErrorCodes.TransactionException.Rollback,
	//				x => x.ExceptionInfo(ex));
	//		}

	//		_transactionStatus = TransactionsControllerStatus.Rolledback;

	//		return result.Build();
	//	}
	//}

	//public async Task<IResult> RollbackAllAsync(IScopeContext scopeContext, Exception? exception, bool throwInvalidTransactionStatus, CancellationToken cancellationToken = default)
	//{
	//	var result = new ResultBuilder();
	//	//Throw.IfArgumentNull(scopeContext);

	//	if (_transactionStatus != TransactionsControllerStatus.Idle)
	//	{
	//		if (throwInvalidTransactionStatus)
	//			return result.WithTransactionException(
	//				scopeContext,
	//				ErrorCodes.TransactionException.InvalidTransactionStatus(_transactionStatus, nameof(RollbackAllAsync)));

	//		return result.Build();
	//	}

	//	using (await _asyncLock.Value.LockAsync().ConfigureAwait(false))
	//	{
	//		if (_transactionStatus != TransactionsControllerStatus.Idle)
	//		{
	//			if (throwInvalidTransactionStatus)
	//				return result.WithTransactionException(
	//					scopeContext,
	//					ErrorCodes.TransactionException.InvalidTransactionStatus(_transactionStatus, nameof(RollbackAllAsync)));

	//			return result.Build();
	//		}

	//		_transactionStatus = TransactionsControllerStatus.Rollingback;

	//		try
	//		{
	//			foreach (var transactionManager in _transactionManagers)
	//				await transactionManager.PreRollbackAsync(scopeContext, exception, cancellationToken);

	//			foreach (var transactionManager in _transactionManagers)
	//				await transactionManager.RollbackAsync(scopeContext, exception, cancellationToken);

	//			foreach (var transactionManager in _transactionManagers)
	//				await transactionManager.PostRollbackAsync(scopeContext, exception, cancellationToken);
	//		}
	//		catch (Exception ex)
	//		{
	//			return result.WithTransactionException(
	//				scopeContext,
	//				ErrorCodes.TransactionException.Rollback,
	//				x => x.ExceptionInfo(ex));
	//		}

	//		_transactionStatus = TransactionsControllerStatus.Rolledback;

	//		return result.Build();
	//	}
	//}

	/// <inheritdoc/>
	public async ValueTask DisposeAsync()
	{
		if (_transactionStatus == TransactionsControllerStatus.Disposing
			|| _transactionStatus == TransactionsControllerStatus.Disposed)
			return;

		_transactionStatus = TransactionsControllerStatus.Disposing;

		await DisposeAsyncCoreAsync().ConfigureAwait(false);

		Dispose(disposing: false);
		GC.SuppressFinalize(this);

		_transactionStatus = TransactionsControllerStatus.Disposed;
	}

	private async ValueTask DisposeAsyncCoreAsync()
	{
#if TRACK_OBJECTS
		Trackers.ObjectLifetimeTracker.SetDisposed(this, IdTransactionsController.ToString());
#endif

		foreach (var transactionManager in _transactionManagers)
			await transactionManager.DisposeAsync();
	}

	/// <inheritdoc/>
	private void Dispose(bool disposing)
	{
		if (_transactionStatus == TransactionsControllerStatus.Disposing
			|| _transactionStatus == TransactionsControllerStatus.Disposed)
			return;

		if (disposing)
		{
#if TRACK_OBJECTS
			Trackers.ObjectLifetimeTracker.SetDisposed(this, IdTransactionsController.ToString());
#endif

			_transactionStatus = TransactionsControllerStatus.Disposing;

			foreach (var transactionManager in _transactionManagers)
				transactionManager.Dispose();

			_transactionStatus = TransactionsControllerStatus.Disposed;
		}
	}

	/// <inheritdoc/>
	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}
}
