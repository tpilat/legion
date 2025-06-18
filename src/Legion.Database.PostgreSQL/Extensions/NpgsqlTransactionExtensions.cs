using Npgsql;
using System.Reflection;

namespace Legion.Extensions;

public static class NpgsqlTransactionExtensions
{
	private static readonly Lazy<Func<NpgsqlTransaction, bool>?> _isDisposedGetter = new(() =>
	{
		var type = typeof(NpgsqlTransaction);
		var connectorProperty = type.GetProperty("IsDisposed", BindingFlags.Instance | BindingFlags.NonPublic);
		if (connectorProperty == null)
			return null;

		var getter = Legion.Reflection.Internal.DelegateFactory.CreateGet<NpgsqlTransaction, bool>(connectorProperty!);
		return getter!;
	});

	private static readonly Lazy<Func<NpgsqlTransaction, bool>?> _isCompletedGetter = new(() =>
	{
		var type = typeof(NpgsqlTransaction);
		var connectorProperty = type.GetProperty("IsCompleted", BindingFlags.Instance | BindingFlags.NonPublic);
		if (connectorProperty == null)
			return null;

		var getter = Legion.Reflection.Internal.DelegateFactory.CreateGet<NpgsqlTransaction, bool>(connectorProperty!);
		return getter!;
	});

	public static bool IsDisposedTransaction(this NpgsqlTransaction transaction)
	{
		Throw.IfArgumentNull(transaction);

		var getter = _isDisposedGetter.Value;
		if (getter == null)
			return false;

		var isDisposed = getter(transaction);
		return isDisposed;
	}

	public static bool IsCompletedTransaction(this NpgsqlTransaction transaction)
	{
		Throw.IfArgumentNull(transaction);

		var getter = _isCompletedGetter.Value;
		if (getter == null)
			return false;

		var isCompleted = getter(transaction);
		return isCompleted;
	}
}
