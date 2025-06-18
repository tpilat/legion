using Legion.EntityFrameworkCore.Audit;
using Legion.EntityFrameworkCore.Audit.Internal;
using Legion.EntityFrameworkCore.QueryCache;
using Legion.Extensions;
using Legion.Model;
using Legion.Model.Audit;
using Legion.Model.Concurrence;
using Legion.Model.Correlation;
using Legion.Model.Synchronyzation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.Common;

namespace Legion.EntityFrameworkCore;

public abstract class DbContextBase : Microsoft.EntityFrameworkCore.DbContext, IDbContext, IAuditableDbContext, IDisposable, IAsyncDisposable
{
	private bool _disposed;
	protected readonly ILogger _logger;

#if TRACK_OBJECTS
	public Guid IdDbContextBase { get; }
#endif

	protected bool? IsDbContextOptionsBuilderPreconfigured { get; private set; }
	protected IEFConnectionProvider? ConnectionProvider { get; private set; }
	protected internal IDbContextSettintgs DbContextSettintgs { get; private set; }
	protected internal QueryCacheManager QueryCacheManager { get; private set; }

	public ILogger DbContextLogger => _logger;

	private DbConnection? dbConnection;
	public DbConnection DbConnection
	{
		get
		{
			dbConnection ??= this.Database.GetDbConnection();

			return dbConnection;
		}
	}

	public IDbContextTransaction? DbContextTransaction => Database?.CurrentTransaction;
	public DbTransaction? DbTransaction => Database?.CurrentTransaction?.GetDbTransaction();

	private string? _dbConnectionString;
	public string DBConnectionString
	{
		get
		{
			_dbConnectionString ??= DbConnection.ConnectionString;

			return _dbConnectionString;
		}
	}

	public DbContextBase(DbContextOptions options, ILogger logger)
		: base(options)
	{
#if TRACK_OBJECTS
		IdDbContextBase = Guid.NewGuid();
		Trackers.ObjectLifetimeTracker.Track(this, IdDbContextBase.ToString());
#endif

		Throw.IfArgumentNull(logger);

		_logger = logger;
		QueryCacheManager = new QueryCacheManager(false);
	}

	protected DbContextBase(ILogger logger)
		: base()
	{
#if TRACK_OBJECTS
		IdDbContextBase = Guid.NewGuid();
		Trackers.ObjectLifetimeTracker.Track(this, IdDbContextBase.ToString());
#endif

		Throw.IfArgumentNull(logger);

		_logger = logger;
		QueryCacheManager = new QueryCacheManager(false);
	}

	private bool _initialized = false;
	private readonly object _initLock = new();
	public virtual void Initialize(
		IDbContextSettintgs dbContextSettintgs,
		IEFConnectionProvider connectionProvider)
	{
		Throw.IfArgumentNull(connectionProvider);
		Throw.IfArgumentNull(dbContextSettintgs);

		if (_initialized)
			return;

		lock (_initLock)
		{
			if (_initialized)
				return;

			DbContextSettintgs = dbContextSettintgs;
			ConnectionProvider = connectionProvider;

			_initialized = true;
		}
	}

	public bool? WithAllowedLocking => DbContextSettintgs?.AllowLocking;
	public virtual bool IsAuditDbContext => false;
	public virtual bool IsDomainEventContext => false;

#pragma warning disable CS0809 // Obsolete member overrides non-obsolete member
	[Obsolete("Use Save() method instead.", true, DiagnosticId = "L_DbCtx_Save")]
	public override int SaveChanges()
	{
		throw new NotSupportedException($"Use {nameof(Save)}() method instead.");
	}

	[Obsolete("Use Save(bool acceptAllChangesOnSuccess) method instead.", true, DiagnosticId = "L_DbCtx_Save")]
	public override int SaveChanges(bool acceptAllChangesOnSuccess)
	{
		throw new NotSupportedException($"Use {nameof(Save)}(bool {nameof(acceptAllChangesOnSuccess)}) method instead.");
	}

	[Obsolete("Use SaveAsync(CancellationToken cancellationToken) method instead.", true, DiagnosticId = "L_DbCtx_Save")]
	public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		throw new NotSupportedException($"Use {nameof(SaveAsync)}({nameof(CancellationToken)} {nameof(cancellationToken)}) method instead.");
	}

	[Obsolete("Use SaveAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken) method instead.", true, DiagnosticId = "L_DbCtx_Save")]
	public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
	{
		throw new NotSupportedException($"Use {nameof(SaveAsync)}(bool {nameof(acceptAllChangesOnSuccess)}, {nameof(CancellationToken)} {nameof(cancellationToken)}) method instead.");
	}
#pragma warning restore CS0809 // Obsolete member overrides non-obsolete member

	public virtual int Save(
		IScopeContext scopeContext)
		=> Save(scopeContext, true, null);

	public virtual int Save(
		IScopeContext scopeContext,
		SaveOptions? options)
		=> Save(scopeContext, true, options);

	public virtual int Save(
		IScopeContext scopeContext,
		bool acceptAllChangesOnSuccess)
		=> Save(scopeContext, acceptAllChangesOnSuccess, null);

	public virtual int Save(
		IScopeContext scopeContext,
		bool acceptAllChangesOnSuccess,
		SaveOptions? options)
	{
		Throw.IfArgumentNull(scopeContext);

		if (ConnectionProvider == null && IsDbContextOptionsBuilderPreconfigured != true)
			Throw.InitializationException(ConnectionProvider, scopeContext);

		//TODO: nuget EntityFramework.Exceptions
		//try
		//{
			var auditCorrelationId = Guid.NewGuid();
			var auditEntriesWithTempProperty = OnBeforeSaveChanges(auditCorrelationId, options, scopeContext);

			var result = base.SaveChanges(acceptAllChangesOnSuccess);

			if (!IsAuditDbContext)
				DbContextSettintgs.AuditEntryStore?.Save(scopeContext, false, acceptAllChangesOnSuccess);

			if (!IsDomainEventContext)
				DbContextSettintgs.DomainEventStore?.Save(scopeContext, false, acceptAllChangesOnSuccess);

		if (0 < auditEntriesWithTempProperty.Count)
			{
				OnAfterSaveChanges(auditCorrelationId, auditEntriesWithTempProperty, options, scopeContext);
				var tmpResult = base.SaveChanges(acceptAllChangesOnSuccess);
			
				if (!IsAuditDbContext)
					DbContextSettintgs.AuditEntryStore?.Save(scopeContext, false, acceptAllChangesOnSuccess);

				if (!IsDomainEventContext)
					DbContextSettintgs.DomainEventStore?.Save(scopeContext, false, acceptAllChangesOnSuccess);

				result += tmpResult;
			}

			return result;
		//}
		//catch (DbUpdateException dbUpdateException)
		//{
		//	if (eventData.Exception.GetBaseException() is T providerException)
		//	{
		//		var error = GetDatabaseError(providerException);

		//		if (error != null && dbUpdateException != null)
		//		{
		//			var exception = ExceptionFactory.Create(error.Value, dbUpdateException, dbUpdateException.Entries);
		//			throw exception;
		//		}
		//	}

		//	throw;
		//}
	}

	public virtual Task<int> SaveAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
		=> SaveAsync(scopeContext, true, null, cancellationToken);

	public virtual Task<int> SaveAsync(
		IScopeContext scopeContext,
		SaveOptions? options,
		CancellationToken cancellationToken = default)
		=> SaveAsync(scopeContext, true, options, cancellationToken);

	public virtual Task<int> SaveAsync(
		IScopeContext scopeContext,
		bool acceptAllChangesOnSuccess,
		CancellationToken cancellationToken = default)
		=> SaveAsync(scopeContext, acceptAllChangesOnSuccess, null, cancellationToken);

	public virtual async Task<int> SaveAsync(
		IScopeContext scopeContext,
		bool acceptAllChangesOnSuccess,
		SaveOptions? options,
		CancellationToken cancellationToken = default)
	{
		Throw.IfArgumentNull(scopeContext);

		if (ConnectionProvider == null && IsDbContextOptionsBuilderPreconfigured != true)
			Throw.InitializationException(ConnectionProvider, scopeContext);

		//TODO: nuget EntityFramework.Exceptions
		//try
		//{
			var auditCorrelationId = Guid.NewGuid();
			var auditEntriesWithTempProperty = OnBeforeSaveChanges(auditCorrelationId, options, scopeContext);

			var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

			if (!IsAuditDbContext && DbContextSettintgs.AuditEntryStore != null)
				await DbContextSettintgs.AuditEntryStore.SaveAsync(scopeContext, false, acceptAllChangesOnSuccess, cancellationToken);

			if (DbContextSettintgs.DomainEventStore != null && !IsDomainEventContext)
				await DbContextSettintgs.DomainEventStore.SaveAsync(scopeContext, false, acceptAllChangesOnSuccess, cancellationToken);

		if (0 < auditEntriesWithTempProperty.Count)
			{
				OnAfterSaveChanges(auditCorrelationId, auditEntriesWithTempProperty, options, scopeContext);
				var tmpResult = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
			
				if (!IsAuditDbContext && DbContextSettintgs.AuditEntryStore != null)
					await DbContextSettintgs.AuditEntryStore.SaveAsync(scopeContext, false, acceptAllChangesOnSuccess, cancellationToken);
			
				if (DbContextSettintgs.DomainEventStore != null && !IsDomainEventContext)
					await DbContextSettintgs.DomainEventStore.SaveAsync(scopeContext, false, acceptAllChangesOnSuccess, cancellationToken);

				result += tmpResult;
			}

			return result;
		//}
		//catch (DbUpdateException dbUpdateException)
		//{
		//	if (eventData.Exception.GetBaseException() is T providerException)
		//	{
		//		var error = GetDatabaseError(providerException);

		//		if (error != null && dbUpdateException != null)
		//		{
		//			var exception = ExceptionFactory.Create(error.Value, dbUpdateException, dbUpdateException.Entries);
		//			throw exception;
		//		}
		//	}

		//	throw;
		//}
	}

	protected static void RegisterUnaccentFunction(ModelBuilder modelBuilder)
	{
		modelBuilder
			.HasDbFunction(() => DbFunc.Unaccent(default))
			.HasName("unaccent");
	}

	private List<AuditEntryInternal> OnBeforeSaveChanges(Guid auditCorrelationId, SaveOptions? options, IScopeContext scopeContext)
	{
		foreach (var entry in ChangeTracker.Entries())
			if (entry.Entity is Legion.Model.IEntity ientity)
				ientity.__IsNewObject = false;

		if (options != null
			&& options.SetConcurrencyToken == false
			&& options.SetSyncToken == false
			&& options.SetCorrelationId == false
			&& options.SetSelfAuditInfo == false
			&& (IsAuditDbContext || DbContextSettintgs.AuditEntryStore == null || options.SaveAuditEntries == false)
			&& (IsDomainEventContext || DbContextSettintgs.DomainEventStore == null))
			return [];

		string? traceFrame = null;

		if (!IsAuditDbContext && DbContextSettintgs.AuditEntryStore != null && (options == null || options.SaveAuditEntries != false))
			traceFrame = scopeContext.TraceFrameStack.ToString();

		ChangeTracker.DetectChanges();

		var auditEntries = new List<AuditEntryInternal>();

		var nowUtc = GlobalContext.Instance.UtcNow;
		foreach (var entry in ChangeTracker.Entries())
		{
			if (entry.Entity is IAuditEntry || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
				continue;

			var postModifiedProperties = new List<string>();

			if ((options == null || options.SetConcurrencyToken != false) && entry.Entity is IConcurrent concurrent)
			{
				switch (entry.State)
				{
					case EntityState.Added:
					case EntityState.Modified:
						concurrent.SetNewConcurrencyToken();
						postModifiedProperties.Add(concurrent.ConcurrencyTokenPropertyName);
						break;

					default:
						break;
				}
			}

			if ((options == null || options.SetSyncToken != false) && entry.Entity is ISynchronizable synchronizable)
			{
				switch (entry.State)
				{
					case EntityState.Added:
						if (Guid.Empty.Equals(synchronizable.SyncToken))
						{
							synchronizable.SyncToken = Guid.NewGuid();
							postModifiedProperties.Add(nameof(synchronizable.SyncToken));
						}
						break;
					case EntityState.Modified:
						if (WasModifiedNotIgnorredProperty(entry, synchronizable))
						{
							synchronizable.SyncToken = Guid.NewGuid();
							postModifiedProperties.Add(nameof(synchronizable.SyncToken));
						}
						break;

					default:
						break;
				}
			}

			if ((options == null || options.SetCorrelationId != false) && entry.Entity is ICorrelable correlable)
			{
				switch (entry.State)
				{
					case EntityState.Added:
						if (Guid.Empty.Equals(correlable.CorrelationId))
						{
							correlable.CorrelationId = Guid.NewGuid();
							postModifiedProperties.Add(nameof(correlable.CorrelationId));
						}
						break;
					case EntityState.Modified:
						var originalCorrelationId = entry.OriginalValues.GetValue<Guid>(nameof(correlable.CorrelationId));
						if (!correlable.CorrelationId.Equals(originalCorrelationId))
						{
							correlable.CorrelationId = originalCorrelationId;
							postModifiedProperties.Add(nameof(correlable.CorrelationId));
						}
						break;

					default:
						break;
				}
			}

			if ((options == null || options.SetSelfAuditInfo != false) && entry.Entity is ISelfAuditableEntity selfAuditableEntity)
			{
				switch (entry.State)
				{
					case EntityState.Added:
						selfAuditableEntity.SetAuditCreated(nowUtc, scopeContext.IdUser);
						postModifiedProperties.Add(nameof(selfAuditableEntity.AuditCreatedUtc));
						postModifiedProperties.Add(nameof(selfAuditableEntity.IdAuditCreatedBy));
						break;

					case EntityState.Modified:
						if (entry.Properties.Any(x => x.IsModified))
						{
							selfAuditableEntity.SetAuditModified(nowUtc, scopeContext.IdUser);
							postModifiedProperties.Add(nameof(selfAuditableEntity.AuditModifiedUtc));
							postModifiedProperties.Add(nameof(selfAuditableEntity.IdAuditModifiedBy));
						}
						break;

					default:
						break;
				}
			}

			if (!IsAuditDbContext && DbContextSettintgs.AuditEntryStore != null && (options == null || options.SaveAuditEntries != false) && entry.Entity is IAuditableEntity auditableEntity)
			{
				var auditEntry = new AuditEntryInternal(entry)
				{
					AuditCorrelationId = auditCorrelationId,
					IdUser = scopeContext.IdUser,
					CreatedUtc = nowUtc,
					CorrelationId = scopeContext.CorrelationId,
					TraceFrame = traceFrame
				};

				var ignoredAuditProperties = auditableEntity.GetIgnoredAuditPropertiesWithDefaultValue() ?? new Dictionary<string, string>();

				auditEntries.Add(auditEntry);
				foreach (var property in entry.Properties)
				{
					if (property.IsTemporary)
					{
						auditEntry.TemporaryProperties.Add(property);
						continue;
					}

					string propertyName = property.Metadata.Name;
					var isIgnored = ignoredAuditProperties.TryGetValue(propertyName, out var defaultIgnoredValue);

					if (property.Metadata.IsPrimaryKey())
					{
						auditEntry.KeyValuesDict[propertyName] = GetValue(property.CurrentValue, isIgnored, defaultIgnoredValue);
						continue;
					}

					var isPostModified = postModifiedProperties.Contains(propertyName);

					switch (entry.State)
					{
						case EntityState.Added:
							auditEntry.IdAuditOperation = AuditOperation.Insert;
							auditEntry.NewValuesDict[propertyName] = GetValue(property.CurrentValue, isIgnored, defaultIgnoredValue);
							break;

						case EntityState.Deleted:
							auditEntry.IdAuditOperation = AuditOperation.Delete;
							auditEntry.OldValuesDict[propertyName] = GetValue(property.OriginalValue, isIgnored, defaultIgnoredValue);
							break;

						case EntityState.Modified:
							if (property.IsModified || isPostModified)
							{
								auditEntry.ChangedColumns.Add(propertyName);
								auditEntry.IdAuditOperation = AuditOperation.Update;
								auditEntry.OldValuesDict[propertyName] = GetValue(property.OriginalValue, isIgnored, defaultIgnoredValue);
								auditEntry.NewValuesDict[propertyName] = GetValue(property.CurrentValue, isIgnored, defaultIgnoredValue);
							}
							break;
					}
				}
			}
			else
			{
				foreach (var postModifiedProperty in postModifiedProperties)
					entry.Property(postModifiedProperty).IsModified = true;
			}

			if (!IsDomainEventContext && DbContextSettintgs.DomainEventStore != null && entry.Entity is IEntity entity)
			{
				var domainEvents = entity.GetDomainEvents();

				if (0 < domainEvents?.Count)
				{
					DbContextSettintgs.DomainEventStore.AddDomainEvents(
						scopeContext,
						domainEvents,
						entity.GetType().GetSimplifiedAssemblyQualifiedName(),
						entity.GetPrimaryKeyValue());

					//can clear events after adding them to the DomainEventStore's dbContext.ChangeTracking
					//so no event would be lost even if error occures during SaveChanges
					entity.ClearDomainEvents();
				}
			}
		}

		if (options == null || options.SaveAuditEntries != false)
		{
			if (!IsAuditDbContext)
			{
				//insert audit entries without TemporaryProperties
				DbContextSettintgs.AuditEntryStore?.AddAuditEntries(scopeContext, auditEntries.Where(ae => !ae.HasTemporaryProperties).Cast<IAuditEntry>().ToList());
			}
		}

		return auditEntries.Where(ae => ae.HasTemporaryProperties).ToList();
	}

	private static object? GetValue(object? value, bool isIgnored, string? defaultIgnoredValue)
	{
		if (value == null)
			return value;

		return isIgnored ? defaultIgnoredValue : value;
	}

	private void OnAfterSaveChanges(Guid auditCorrelationId, List<AuditEntryInternal> auditEntriesWithTempProperty, SaveOptions? options, IScopeContext scopeContext)
	{
		if (IsAuditDbContext || DbContextSettintgs.AuditEntryStore == null || (options != null && options.SaveAuditEntries == false))
			return;

		var updatedAuditEntries = new List<IAuditEntry>();

		foreach (var auditEntry in auditEntriesWithTempProperty)
		{
			if (auditEntry.Entry.Entity is IAuditableEntity auditableEntity)
			{
				var ignoredAuditProperties = auditableEntity.GetIgnoredAuditPropertiesWithDefaultValue() ?? new Dictionary<string, string>();

				foreach (var prop in auditEntry.TemporaryProperties)
				{
					string propertyName = prop.Metadata.Name;
					var isIgnored = ignoredAuditProperties.TryGetValue(propertyName, out var defaultIgnoredValue);

					if (prop.Metadata.IsPrimaryKey())
					{
						auditEntry.KeyValuesDict[propertyName] = GetValue(prop.CurrentValue, isIgnored, defaultIgnoredValue);
					}
					else
					{
						auditEntry.NewValuesDict[propertyName] = GetValue(prop.CurrentValue, isIgnored, defaultIgnoredValue);
					}
				}

				updatedAuditEntries.Add(auditEntry);
			}
		}

		if (0 < updatedAuditEntries.Count)
			DbContextSettintgs.AuditEntryStore.AddAuditEntries(scopeContext, updatedAuditEntries);
	}

	protected static bool WasModifiedNotIgnorredProperty(EntityEntry entry, ISynchronizable synchronizable)
	{
		if (entry == null || synchronizable == null)
			return false;

		var ignoredProperties = synchronizable.GetIgnoredSynchronizationProperties();
		if (ignoredProperties == null || ignoredProperties.Count == 0)
			return true;

		return entry.Properties.Any(prop => prop.IsModified && !ignoredProperties.Contains(prop.Metadata.Name));
	}

	private readonly object _configureDbContextCacheLock = new();
	public void ConfigureQueryCacheManager(Action<QueryCacheManager> configure, bool force)
	{
		if (configure == null || (!force && QueryCacheManager.IsEnabled))
			return;

		lock (_configureDbContextCacheLock)
		{
			if (force || !QueryCacheManager.IsEnabled)
				configure(QueryCacheManager);
		}
	}

	public void EnableQueryCacheManager()
		=> ConfigureQueryCacheManager(c => c.IsEnabled = true, false);

	public void SetDbTransaction(
		IScopeContext scopeContext,
		IDbContextTransaction? existingDbContextTransaction,
		out IDbContextTransaction? newDbContextTransaction,
		TransactionUsage transactionUsage,
		IsolationLevel? transactionIsolationLevel)
		=> DbContextFactory.SetDbTransaction(
			scopeContext,
			this,
			existingDbContextTransaction,
			out newDbContextTransaction,
			transactionUsage,
			transactionIsolationLevel);

	public void SetDbTransaction(
		IScopeContext scopeContext,
		DbTransaction? existingTransaction,
		out IDbContextTransaction? newDbContextTransaction,
		TransactionUsage transactionUsage,
		IsolationLevel? transactionIsolationLevel)
		=> DbContextFactory.SetDbTransaction(
			scopeContext,
			this,
			existingTransaction,
			out newDbContextTransaction,
			transactionUsage,
			transactionIsolationLevel);

	private readonly object _preconfiguredOptionsLock = new();
	protected bool SetIsDbContextOptionsBuilderPreconfigured()
	{
		if (IsDbContextOptionsBuilderPreconfigured.HasValue)
			return false;

		lock (_preconfiguredOptionsLock)
		{
			if (IsDbContextOptionsBuilderPreconfigured.HasValue)
				return false;

			IsDbContextOptionsBuilderPreconfigured = true;
			return true;
		}
	}

	public override async ValueTask DisposeAsync()
	{
		if (_disposed)
			return;

		_disposed = true;

		await DisposeAsyncCoreAsync().ConfigureAwait(false);

		Dispose(disposing: false);
		GC.SuppressFinalize(this);
	}

	protected virtual async ValueTask DisposeAsyncCoreAsync()
	{
#if TRACK_OBJECTS
		Trackers.ObjectLifetimeTracker.SetDisposed(this, IdDbContextBase.ToString());
#endif

		QueryCacheManager.Dispose();
		await base.DisposeAsync().ConfigureAwait(false);

		ConnectionProvider = null;
		QueryCacheManager = null!;
	}

	protected virtual void Dispose(bool disposing)
	{
		if (_disposed)
			return;

		_disposed = true;

		if (disposing)
		{
#if TRACK_OBJECTS
			Trackers.ObjectLifetimeTracker.SetDisposed(this, IdDbContextBase.ToString());
#endif

			QueryCacheManager.Dispose();
			base.Dispose();

			ConnectionProvider = null;
			QueryCacheManager = null!;
		}
	}

	public override void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}
