using Microsoft.Data.SqlClient;
using System.Reflection;

namespace Legion.Extensions;

public static class SqlServerTransactionExtensions
{
	private static readonly Lazy<Func<SqlTransaction, bool>?> _isDisposedGetter = new(() =>
	{
		var type = typeof(SqlTransaction);
		var connectorProperty = type.GetProperty("IsDisposed", BindingFlags.Instance | BindingFlags.NonPublic);
		if (connectorProperty == null)
			return null;

		var getter = Legion.Reflection.Internal.DelegateFactory.CreateGet<SqlTransaction, bool>(connectorProperty!);
		return getter!;
	});

	private static readonly Lazy<Func<SqlTransaction, bool>?> _isCompletedGetter = new(() =>
	{
		var type = typeof(SqlTransaction);
		var connectorProperty = type.GetProperty("IsCompleted", BindingFlags.Instance | BindingFlags.NonPublic);
		if (connectorProperty == null)
			return null;

		var getter = Legion.Reflection.Internal.DelegateFactory.CreateGet<SqlTransaction, bool>(connectorProperty!);
		return getter!;
	});

	public static bool IsDisposedTransaction(this SqlTransaction transaction)
	{
		Throw.IfArgumentNull(transaction);

		var getter = _isDisposedGetter.Value;
		if (getter == null)
			return false;

		var isDisposed = getter(transaction);
		return isDisposed;
	}

	public static bool IsCompletedTransaction(this SqlTransaction transaction)
	{
		Throw.IfArgumentNull(transaction);

		var getter = _isCompletedGetter.Value;
		if (getter == null)
			return false;

		var isCompleted = getter(transaction);
		return isCompleted;
	}
}
