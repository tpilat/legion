using Legion.Exceptions;
using Legion.Extensions;
using Legion.Model.Repositories;

namespace Legion.EntityFrameworkCore.Exceptions.Internal;

public static partial class ErrorCodes
{
	public static partial class UniqueConstraintException
	{
		public static IErrorCode Default => _default.Value;
		private static readonly Lazy<IErrorCode> _default = new(() =>
			new ErrorCode(
				"LEF_UNIQUE_0001",
				"Unique constraint violation."));
	}

	public static partial class UnitOfWorkException
	{
		public static IErrorCode UnhandledUnitOfWorkFactory(Type uowType)
			=> new ErrorCode(
				"LEF_UOW_0001",
				$"Cannot create unit of work of type {uowType.ToFriendlyFullName()}.");

		public static IErrorCode UnitOfWorkHasDbContext(Type dbContextType)
			=> new ErrorCode(
				"LEF_UOW_0003",
				 $"UnitOfWork DbContext has already been set | {nameof(dbContextType)} = {dbContextType.ToFriendlyFullName()}");
	}

	public static partial class DbContext
	{
		public static IErrorCode InvalidTransaction(Guid transactionId)
			=> new ErrorCode(
				"LEF_INVTRAN_0001",
				$"DbContext already has set another transaction with id {transactionId}");

		public static IErrorCode NoConnectionString => _noConnectionString.Value;
		private static readonly Lazy<IErrorCode> _noConnectionString = new(() =>
			new ErrorCode(
				"LEF_DB_CONN_0001",
				"NULL connection string."));

		public static IErrorCode ConnectionMismatch(string connection1, string connection2)
			=> new ErrorCode(
				"LEF_DB_CONN_0002",
				$"DB Connection mismatch between {connection1} and {connection2}");

		public static IErrorCode RegisterToNullTransactionsController => _registerToNullTransactionsController.Value;
		private static readonly Lazy<IErrorCode> _registerToNullTransactionsController = new(() =>
			new ErrorCode(
				"LEF_NULL_CTRL_0001",
				"Cannot register transaction manager to null transactions controller."));

		public static IErrorCode MismatchLocking(Type dbContextType, bool? expextecAllowLocking, bool? existingAllowLocking)
			=> new ErrorCode(
				"LEF_DB_INV_LOCK_0001",
				$"Cannot create DbContext {dbContextType.FullName} with setting AllowLocking = {expextecAllowLocking}. Same DbContext already exists with settings AllowLocking = {existingAllowLocking}");
	}
}
