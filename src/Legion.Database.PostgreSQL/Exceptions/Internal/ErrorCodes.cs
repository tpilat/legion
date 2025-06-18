using Legion.Exceptions;

namespace Legion.Database.PostgreSQL.Exceptions.Internal;

public static partial class ErrorCodes
{
	public static partial class ConncetionStringException
	{
		public static IErrorCode Default => _default.Value;
		private static readonly Lazy<IErrorCode> _default = new(() =>
			new ErrorCode(
				"DBDep_PgSql_ConStr_0001",
				"Invalid connection string."));
	}

	public static partial class TransactionException
	{
		public static IErrorCode NoTransaction => _noTransaction.Value;
		private static readonly Lazy<IErrorCode> _noTransaction = new(() =>
			new ErrorCode(
				"DBDep_PgSql_Tran_0001",
				"No transaction was created."));
	}

	public static partial class SqlFileException
	{
		public static IErrorCode FileNotExists => _fileNotExists.Value;
		private static readonly Lazy<IErrorCode> _fileNotExists = new(() =>
			new ErrorCode(
				"DBDep_PgSql_File_0001",
				"File not exists."));
	}

	public static partial class PatchException
	{
		public static IErrorCode ReadFromPatchTable => _readFromPatchTable.Value;
		private static readonly Lazy<IErrorCode> _readFromPatchTable = new(() =>
			new ErrorCode(
				"DBDep_PgSql_ReadDB_0001",
				"Could not read data from patch table."));

		public static IErrorCode InvalidPatchFileName => _invalidPatchFileName.Value;
		private static readonly Lazy<IErrorCode> _invalidPatchFileName = new(() =>
			new ErrorCode(
				"DBDep_PgSql_PatcFile_0001",
				"Invalid patch fileName."));

		public static IErrorCode NotInitialized => _notInitialized.Value;
		private static readonly Lazy<IErrorCode> _notInitialized = new(() =>
			new ErrorCode(
				"DBDep_PgSql_PatcMngr_0001",
				"Patch manager is not initialized."));
	}
}
