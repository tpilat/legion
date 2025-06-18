using Legion.Database;
using System.Runtime.CompilerServices;

namespace Legion.Model.Repositories;

public partial interface IUnitOfWork : IDisposable, IAsyncDisposable
{
	IConnectionProvider ConnectionProvider { get; }
	IServiceProvider ServiceProvider { get; }

	IResult<int> Save(
		IScopeContext scopeContext,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0);

	IResult<int> Save(
		IScopeContext scopeContext,
		bool autoCommitAllTransactions,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0);

	IResult<int> Save(
		IScopeContext scopeContext,
		bool autoCommitAllTransactions,
		SaveOptions? options,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0);

	IResult<int> Save(
		IScopeContext scopeContext,
		bool autoCommitAllTransactions,
		bool acceptAllChangesOnSuccess,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0);

	IResult<int> Save(
		IScopeContext scopeContext,
		bool autoCommitAllTransactions,
		bool acceptAllChangesOnSuccess,
		SaveOptions? options,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0);

	Task<IResult<int>> SaveAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0);

	Task<IResult<int>> SaveAsync(
		IScopeContext scopeContext,
		bool autoCommit,
		CancellationToken cancellationToken = default,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0);

	Task<IResult<int>> SaveAsync(
		IScopeContext scopeContext,
		bool autoCommit,
		SaveOptions? options,
		CancellationToken cancellationToken = default,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0);

	Task<IResult<int>> SaveAsync(
		IScopeContext scopeContext,
		bool autoCommit,
		bool acceptAllChangesOnSuccess,
		CancellationToken cancellationToken = default,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0);

	Task<IResult<int>> SaveAsync(
		IScopeContext scopeContext,
		bool autoCommit,
		bool acceptAllChangesOnSuccess,
		SaveOptions? options,
		CancellationToken cancellationToken = default,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0);

	Task<int> ExecuteSqlInterpolatedAsync(
		IScopeContext scopeContext,
		FormattableString sql,
		CancellationToken cancellationToken = default);

	Task<int> ExecuteSqlRawAsync(
		IScopeContext scopeContext,
		string sql,
		IEnumerable<object> parameters,
		CancellationToken cancellationToken = default);

	int ExecuteSqlInterpolated(
		IScopeContext scopeContext,
		FormattableString sql);

	int ExecuteSqlRaw(
		IScopeContext scopeContext,
		string sql,
		IEnumerable<object> parameters);

	IResult<bool?> Commit(
		IScopeContext scopeContext,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0);

	Task<IResult<bool?>> CommitAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0);

	IResult<bool?> Rollback(
		IScopeContext scopeContext,
		Exception? exception,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0);

	Task<IResult<bool?>> RollbackAsync(
		IScopeContext scopeContext,
		Exception? exception,
		CancellationToken cancellationToken = default,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0);
}
