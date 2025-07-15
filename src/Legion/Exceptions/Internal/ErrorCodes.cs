using Legion.Extensions;
using Legion.Transactions;
using System.Runtime.CompilerServices;

namespace Legion.Exceptions.Internal;

public static partial class ErrorCodes
{
	public static partial class LegionException
	{
		public static IErrorCode Default => _default.Value;
		private static readonly Lazy<IErrorCode> _default = new(() =>
			new ErrorCode(
				"L_0000",
				"Exception was thrown."));
	}

	public static partial class ArgException
	{
		public static IErrorCode Default => _default.Value;
		private static readonly Lazy<IErrorCode> _default = new(() =>
			new ErrorCode(
				"L_Arg_0001",
				"Invalid argument."));
	}

	public static partial class ArgNullException
	{
		public static IErrorCode Default => _default.Value;
		private static readonly Lazy<IErrorCode> _default = new(() =>
			new ErrorCode(
				"L_ArgNull_0001",
				"Value cannot be null."));
	}

	public static partial class ArgDefaultValueException
	{
		public static IErrorCode Default => _default.Value;
		private static readonly Lazy<IErrorCode> _default = new(() =>
			new ErrorCode(
				"L_ArgDefVal_0001",
				"The value cannot be the default value."));
	}

	public static partial class ArgEmptyValueException
	{
		public static IErrorCode Default => _default.Value;
		private static readonly Lazy<IErrorCode> _default = new(() =>
			new ErrorCode(
				"L_ArgEmptyVal_0001",
				"The value cannot be empty."));

		public static IErrorCode EmptyString => _EmptyString.Value;
		private static readonly Lazy<IErrorCode> _EmptyString = new(() =>
			new ErrorCode(
				"L_ArgEmptyVal_0002",
				"The value cannot be an empty string."));

		public static IErrorCode WhiteSpace => _WhiteSpace.Value;
		private static readonly Lazy<IErrorCode> _WhiteSpace = new(() =>
			new ErrorCode(
				"L_ArgEmptyVal_0003",
				"The value cannot be an empty string or composed entirely of whitespace."));

		public static IErrorCode Collection => _Collection.Value;
		private static readonly Lazy<IErrorCode> _Collection = new(() =>
			new ErrorCode(
				"L_ArgEmptyVal_0004",
				"The value cannot be an empty collection."));

		public static IErrorCode Array => _Array.Value;
		private static readonly Lazy<IErrorCode> _Array = new(() =>
			new ErrorCode(
				"L_ArgEmptyVal_0005",
				"The value cannot be an empty array."));

		public static IErrorCode Enumerable => _Enumerable.Value;
		private static readonly Lazy<IErrorCode> _Enumerable = new(() =>
			new ErrorCode(
				"L_ArgEmptyVal_0006",
				"The value cannot be an empty enumerable."));
	}

	public static partial class ArgOutOfRangeException
	{
		public static IErrorCode Default => _default.Value;
		private static readonly Lazy<IErrorCode> _default = new(() =>
			new ErrorCode(
				"L_ArgOutRange_0001",
				"Specified argument was out of the range of valid values."));

		public static IErrorCode NotEqual<T>(T value, T other, [CallerArgumentExpression(nameof(value))] string? paramName = null)
			=> new ErrorCode(
				"L_ArgNotEq_0001",
				$"{paramName} ('{value}') must not be equal to '{other}'.");

		public static IErrorCode Equal<T>(T value, T other, [CallerArgumentExpression(nameof(value))] string? paramName = null)
			=> new ErrorCode(
				"L_ArgEq_0001",
				$"{paramName} ('{value}') must be equal to '{other}'.");

		public static IErrorCode NotIn<T>(T value, IEnumerable<T>? others, [CallerArgumentExpression(nameof(value))] string? paramName = null)
			=> new ErrorCode(
				"L_ArgNotIn_0001",
				others == null
					? $"{paramName} ('{value}') must not be one of [ null ]."
					: $"{paramName} ('{value}') must not be one of ['{string.Join(",", others)}'].");

		public static IErrorCode In<T>(T value, IEnumerable<T>? others, [CallerArgumentExpression(nameof(value))] string? paramName = null)
			=> new ErrorCode(
				"L_ArgIn_0001",
				others == null
					? $"{paramName} ('{value}') must be one of [ null ]."
					: $"{paramName} ('{value}') must be one of ['{string.Join(",", others)}'].");

		public static IErrorCode Greater<T>(T value, T other, [CallerArgumentExpression(nameof(value))] string? paramName = null)
			=> new ErrorCode(
				"L_ArgGt_0001",
				$"{paramName} ('{value}') must be greater than '{other}'.");

		public static IErrorCode GreaterOrEqual<T>(T value, T other, [CallerArgumentExpression(nameof(value))] string? paramName = null)
			=> new ErrorCode(
				"L_ArgGtOrEq_0001",
				$"{paramName} ('{value}') must be greater than or equal to '{other}'.");

		public static IErrorCode Less<T>(T value, T other, [CallerArgumentExpression(nameof(value))] string? paramName = null)
			=> new ErrorCode(
				"L_ArgLt_0001",
				$"{paramName} ('{value}') must be less than '{other}'.");

		public static IErrorCode LessOrEqual<T>(T value, T other, [CallerArgumentExpression(nameof(value))] string? paramName = null)
			=> new ErrorCode(
				"L_ArgLtOrEq_0001",
				$"{paramName} ('{value}') must be less than or equal to '{other}'.");

		public static IErrorCode NonNegativeNonZero<T>(T value, [CallerArgumentExpression(nameof(value))] string? paramName = null)
			=> new ErrorCode(
				"L_ArgNonNegNonZero_0001",
				$"{paramName} ('{value}')must be a non-negative and non-zero value.");

		public static IErrorCode NonNegative<T>(T value, [CallerArgumentExpression(nameof(value))] string? paramName = null)
			=> new ErrorCode(
				"L_ArgNonNeg_0001",
				$"{paramName} ('{value}') must be a non-negative value.");

		public static IErrorCode NonZero<T>(T value, [CallerArgumentExpression(nameof(value))] string? paramName = null)
			=> new ErrorCode(
				"L_ArgNonZero_0001",
				$"{paramName} ('{value}') must be a non-zero value.");
	}






	public static partial class NullValueException
	{
		public static IErrorCode Default => _default.Value;
		private static readonly Lazy<IErrorCode> _default = new(() =>
			new ErrorCode(
				"L_NullVal_0001",
				"Value cannot be null."));

		public static IErrorCode CustomValidation(string customMessage)
			=> new ErrorCode(
				"L_NullVal_0002",
				customMessage);

		public static IErrorCode NotNullCustomValidation(string customMessage)
			=> new ErrorCode(
				"L_NotNullVal_0001",
				customMessage);
	}

	public static partial class DefaultValueException
	{
		public static IErrorCode Default => _default.Value;
		private static readonly Lazy<IErrorCode> _default = new(() =>
			new ErrorCode(
				"L_DefVal_0001",
				"The value cannot be the default value."));

		public static IErrorCode CustomValidation(string customMessage)
			=> new ErrorCode(
				"L_DefVal_0002",
				customMessage);

		public static IErrorCode NotDefaultCustomValidation(string customMessage)
			=> new ErrorCode(
				"L_NotDefVal_0001",
				customMessage);
	}

	public static partial class EmptyValueException
	{
		public static IErrorCode Default => _default.Value;
		private static readonly Lazy<IErrorCode> _default = new(() =>
			new ErrorCode(
				"L_EmptyVal_0001",
				"The value cannot be empty."));

		public static IErrorCode CustomValidation(string customMessage)
			=> new ErrorCode(
				"L_EmptyVal_0002",
				customMessage);

		public static IErrorCode EmptyString => _EmptyString.Value;
		private static readonly Lazy<IErrorCode> _EmptyString = new(() =>
			new ErrorCode(
				"L_EmptyStr_0001",
				"The value cannot be an empty string."));

		public static IErrorCode EmptyStringValidation(string customMessage)
			=> new ErrorCode(
				"L_EmptyStr_0002",
				customMessage);

		public static IErrorCode NotEmptyStringValidation(string customMessage)
			=> new ErrorCode(
				"L_NotEmptyStr_0001",
				customMessage);

		public static IErrorCode WhiteSpace => _WhiteSpace.Value;
		private static readonly Lazy<IErrorCode> _WhiteSpace = new(() =>
			new ErrorCode(
				"L_WhiteStr_0001",
				"The value cannot be an empty string or composed entirely of whitespace."));

		public static IErrorCode WhiteSpaceValidation(string customMessage)
			=> new ErrorCode(
				"L_WhiteStr_0002",
				customMessage);

		public static IErrorCode NotWhiteSpaceStringValidation(string customMessage)
			=> new ErrorCode(
				"L_NotWhiteStr_0001",
				customMessage);

		public static IErrorCode Collection => _Collection.Value;
		private static readonly Lazy<IErrorCode> _Collection = new(() =>
			new ErrorCode(
				"L_EmptyCol_0001",
				"The value cannot be an empty collection."));

		public static IErrorCode EmptyCollectionValidation(string customMessage)
			=> new ErrorCode(
				"L_EmptyCol_0002",
				customMessage);

		public static IErrorCode NotEmptyCollectionValidation(string customMessage)
			=> new ErrorCode(
				"L_NotEmptyCol_0001",
				customMessage);

		public static IErrorCode Array => _Array.Value;
		private static readonly Lazy<IErrorCode> _Array = new(() =>
			new ErrorCode(
				"L_EmptyArr_0001",
				"The value cannot be an empty array."));

		public static IErrorCode EmptyArrayValidation(string customMessage)
			=> new ErrorCode(
				"L_EmptyArr_0002",
				customMessage);

		public static IErrorCode NotEmptyArrayValidation(string customMessage)
			=> new ErrorCode(
				"L_NotEmptyArr_0001",
				customMessage);

		public static IErrorCode Enumerable => _Enumerable.Value;
		private static readonly Lazy<IErrorCode> _Enumerable = new(() =>
			new ErrorCode(
				"L_EmptyEnumrbl_0001",
				"The value cannot be an empty enumerable."));

		public static IErrorCode EmptyEnumerableValidation(string customMessage)
			=> new ErrorCode(
				"L_EmptyEnumrbl_0002",
				customMessage);

		public static IErrorCode NotEmptyEnumerableValidation(string customMessage)
			=> new ErrorCode(
				"L_NotEmptyEnumrbl_0001",
				customMessage);
	}

	public static partial class OutOfRangeException
	{
		public static IErrorCode Default => _default.Value;
		private static readonly Lazy<IErrorCode> _default = new(() =>
			new ErrorCode(
				"L_OutRange_0001",
				"Specified value was out of the range of valid values."));

		public static IErrorCode CustomValidation(string customMessage)
			=> new ErrorCode(
				"L_OutRange_0002",
				customMessage);

		public static IErrorCode NotEqual<T>(T value, T other, [CallerArgumentExpression(nameof(value))] string? paramName = null)
			=> new ErrorCode(
				"L_NotEqVal_0001",
				$"{paramName} ('{value}') must not be equal to '{other}'.");

		public static IErrorCode NotEqualValidation(string customMessage)
			=> new ErrorCode(
				"L_ArgNotEq_0002",
				customMessage);

		public static IErrorCode MultiNotEqualValidation(string customMessage)
			=> new ErrorCode(
				"L_ArgNotEq_0003",
				customMessage);

		public static IErrorCode Equal<T>(T value, T other, [CallerArgumentExpression(nameof(value))] string? paramName = null)
			=> new ErrorCode(
				"L_EqVal_0001",
				$"{paramName} ('{value}') must be equal to '{other}'.");

		public static IErrorCode EqualValidation(string customMessage)
			=> new ErrorCode(
				"L_ArgEq_0002",
				customMessage);

		public static IErrorCode MultiEqualValidation(string customMessage)
			=> new ErrorCode(
				"L_ArgEq_0003",
				customMessage);

		public static IErrorCode NotIn<T>(T value, IEnumerable<T>? others, [CallerArgumentExpression(nameof(value))] string? paramName = null)
			=> new ErrorCode(
				"L_ArgNotIn_0001",
				others == null
					? $"{paramName} ('{value}') must not be one of [ null ]."
					: $"{paramName} ('{value}') must not be one of ['{string.Join(",", others)}'].");

		public static IErrorCode In<T>(T value, IEnumerable<T>? others, [CallerArgumentExpression(nameof(value))] string? paramName = null)
			=> new ErrorCode(
				"L_ArgIn_0001",
				others == null
					? $"{paramName} ('{value}') must be one of [ null ]."
					: $"{paramName} ('{value}') must be one of ['{string.Join(",", others)}'].");

		public static IErrorCode Greater<T>(T value, T other, [CallerArgumentExpression(nameof(value))] string? paramName = null)
			=> new ErrorCode(
				"L_GtVal_0001",
				$"{paramName} ('{value}') must be greater than '{other}'.");

		public static IErrorCode GreaterValidation(string message)
			=> new ErrorCode(
				"L_GtVal_0002",
				message);

		public static IErrorCode GreaterOrEqual<T>(T value, T other, [CallerArgumentExpression(nameof(value))] string? paramName = null)
			=> new ErrorCode(
				"L_GtOrEqVal_0001",
				$"{paramName} ('{value}') must be greater than or equal to '{other}'.");

		public static IErrorCode GreaterOrEqualValidation(string message)
			=> new ErrorCode(
				"L_GtOrEqVal_0002",
				message);

		public static IErrorCode Less<T>(T value, T other, [CallerArgumentExpression(nameof(value))] string? paramName = null)
			=> new ErrorCode(
				"L_LtVal_0001",
				$"{paramName} ('{value}') must be less than '{other}'.");

		public static IErrorCode LessValidation(string message)
			=> new ErrorCode(
				"L_LtVal_0002",
				message);

		public static IErrorCode LessOrEqual<T>(T value, T other, [CallerArgumentExpression(nameof(value))] string? paramName = null)
			=> new ErrorCode(
				"L_LtOrEqVal_0001",
				$"{paramName} ('{value}') must be less than or equal to '{other}'.");

		public static IErrorCode LessOrEqualValidation(string message)
			=> new ErrorCode(
				"L_LtOrEqVal_0002",
				message);

		public static IErrorCode NonNegativeNonZero<T>(T value, [CallerArgumentExpression(nameof(value))] string? paramName = null)
			=> new ErrorCode(
				"L_NonNegNonZeroVal_0001",
				$"{paramName} ('{value}')must be a non-negative and non-zero value.");

		public static IErrorCode NonNegative<T>(T value, [CallerArgumentExpression(nameof(value))] string? paramName = null)
			=> new ErrorCode(
				"L_NonNegVal_0001",
				$"{paramName} ('{value}') must be a non-negative value.");

		public static IErrorCode NonZero<T>(T value, [CallerArgumentExpression(nameof(value))] string? paramName = null)
			=> new ErrorCode(
				"L_NonZeroVal_0001",
				$"{paramName} ('{value}') must be a non-zero value.");

		public static IErrorCode InclusiveBetweenValidation(string message)
			=> new ErrorCode(
				"L_InclusBetw_0001",
				message);

		public static IErrorCode InclusiveExclusiveBetweenValidation(string message)
			=> new ErrorCode(
				"L_InclusExlusBetw_0001",
				message);

		public static IErrorCode ExclusiveInclusiveBetweenValidation(string message)
			=> new ErrorCode(
				"L_ExclusInclusBetw_0001",
				message);

		public static IErrorCode ExclusiveBetweenalidation(string message)
			=> new ErrorCode(
				"L_ExclusBetw_0001",
				message);

		public static IErrorCode PrecisionValidation(string customMessage)
			=> new ErrorCode(
				"L_Precision_0001",
				customMessage);

		public static IErrorCode LengthValidation(string customMessage)
			=> new ErrorCode(
				"L_LENGTH_0001",
				customMessage);

		public static IErrorCode ExactLengthValidation(string customMessage)
			=> new ErrorCode(
				"L_LENGTH_0002",
				customMessage);

		public static IErrorCode RegExValidation(string customMessage)
			=> new ErrorCode(
				"L_REGEX_0001",
				customMessage);
	}







	public static partial class AuthenticationException
	{
		public static IErrorCode Default => _default.Value;
		private static readonly Lazy<IErrorCode> _default = new(() =>
			new ErrorCode(
				"L_AUTH401_0001",
				"Invalid authentication."));

		public static IErrorCode MissingTenant(Guid IdTenant, [CallerArgumentExpression(nameof(IdTenant))] string? paramName = null)
			=> new ErrorCode(
				"L_AUTH401_0002",
				$"Missing {paramName} == {IdTenant}");
	}

	public static partial class UnauthorizedException
	{
		public static IErrorCode Default => _default.Value;
		private static readonly Lazy<IErrorCode> _default = new(() =>
			new ErrorCode(
				"L_UNAUTH403_0001",
				"Invalid authorization."));
	}

	public static partial class InvalidOpException
	{
		public static IErrorCode Default => _default.Value;
		private static readonly Lazy<IErrorCode> _default = new(() =>
			new ErrorCode(
				"L_INVOP_0001",
				"Invalid operation."));
	}

	public static partial class ResultException
	{
		public static IErrorCode Default => _default.Value;
		private static readonly Lazy<IErrorCode> _default = new(() =>
			new ErrorCode(
				"L_RESULT_0001",
				"Invalid result."));

		public static IErrorCode Unhandled => _unhandled.Value;
		private static readonly Lazy<IErrorCode> _unhandled = new(() =>
			new ErrorCode(
				"L_RESULT_0002",
				"Result unhandled exception."));

		public static IErrorCode InvocationResult => _invocationResult.Value;
		private static readonly Lazy<IErrorCode> _invocationResult = new(() =>
			new ErrorCode(
				"L_RESULT_0003",
				"Invocation result callback error."));

		public static IErrorCode Logger => _logger.Value;
		private static readonly Lazy<IErrorCode> _logger = new(() =>
			new ErrorCode(
				"L_RESULT_0004",
				"Logger error."));
	}

	public static partial class OpCanceledException
	{
		public static IErrorCode Default => _default.Value;
		private static readonly Lazy<IErrorCode> _default = new(() =>
			new ErrorCode(
				"L_CANCELOP_0001",
				"The operation was canceled.")); //"The task was not completed before being canceled."));
	}

	public static partial class ObjDisposedException
	{
		public static IErrorCode Default => _default.Value;
		private static readonly Lazy<IErrorCode> _default = new(() =>
			new ErrorCode(
				"L_DISPOS_0001",
				"Cannot access a disposed object."));
	}

	public static partial class NotSupportedException
	{
		public static IErrorCode Default => _default.Value;
		private static readonly Lazy<IErrorCode> _default = new(() =>
			new ErrorCode(
				"L_notSup_0001",
				"Not supported."));
	}

	public static partial class NotImplementedException
	{
		public static IErrorCode Default => _default.Value;
		private static readonly Lazy<IErrorCode> _default = new(() =>
			new ErrorCode(
				"L_NOTIMPL_0001",
				"Not implemented."));
	}

	public static partial class ConfigurationException
	{
		public static IErrorCode Default => _default.Value;
		private static readonly Lazy<IErrorCode> _default = new(() =>
			new ErrorCode(
				"L_CFG_0001",
				"Invalid configuration."));

		public static IErrorCode InvalidConfigMessage(string message)
			=> new ErrorCode(
				"L_CFG_0002",
				message);

		public static IErrorCode MissingConfiguratoinSection(string configSectionPath)
			=> new ErrorCode(
				"L_CFG_0003",
				$"Missing configuration {configSectionPath}");
	}

	public static partial class InitializationException
	{
		public static IErrorCode Default => _default.Value;
		private static readonly Lazy<IErrorCode> _default = new(() =>
			new ErrorCode(
				"L_NOT_INIT_0001",
				"Not initialized."));

		public static IErrorCode NotInitialized(string paramName)
			=> new ErrorCode(
				"L_NOT_INIT_0001",
				$"Not initialized - {paramName}");
	}

	public static partial class ValidationException
	{
		public static IErrorCode Default => _default.Value;
		private static readonly Lazy<IErrorCode> _default = new(() =>
			new ErrorCode(
				"L_VALIDATION_0001",
				"Validation errors."));

		public static IErrorCode ValidationMessage(string message)
			=> new ErrorCode(
				"L_INVALID_0001",
				message);

		public static IErrorCode InvalidEmail(string message)
			=> new ErrorCode(
				"L_INVALID_EMAIL_0001",
				message);

		public static IErrorCode ErrorValidation(string message)
			=> new ErrorCode(
				"L_INVALID_ERR_0001",
				message);
	}

	public static partial class DecorationException
	{		public static IErrorCode Default => _default.Value;
		private static readonly Lazy<IErrorCode> _default = new(() =>
			new ErrorCode(
				"L_DECOR_0001",
				"Could not find registered service."));

		public static IErrorCode MissingServiceType(Type serviceType)
			=> new ErrorCode(
				"L_DECOR_0002",
				$"Could not find any registered services for type '{serviceType.ToFriendlyName()}'.");
	}

	public static partial class TransactionException
	{
		public static IErrorCode Default => _default.Value;
		private static readonly Lazy<IErrorCode> _default = new(() =>
			new ErrorCode(
				"L_TRANS_0001",
				"Cannot complete transaction operation."));

		public static IErrorCode Commit => _commit.Value;
		private static readonly Lazy<IErrorCode> _commit = new(() =>
			new ErrorCode(
				"L_TRANS_COM_0001",
				"Cannot complete transaction commit."));

		public static IErrorCode Rollback => _rollback.Value;
		private static readonly Lazy<IErrorCode> _rollback = new(() =>
			new ErrorCode(
				"L_TRANS_ROL_0001",
				"Cannot complete transaction rollback."));

		public static IErrorCode InvalidTransactionStatus(TransactionsControllerStatus transactionStatus, string transactionOperation)
			=> new ErrorCode(
				"L_TRANS_0002",
				$"Invalid transaction status '{transactionStatus}'. Cannot {transactionOperation}.");
	}

	public static partial class CronFormatException
	{
		public static IErrorCode Default => _default.Value;
		private static readonly Lazy<IErrorCode> _default = new(() =>
			new ErrorCode(
				"L_CRON_0001",
				"Invalid cron format."));
	}

	public static partial class DatabaseModelException
	{
		public static IErrorCode InvalidForeignKey(string message)
			=> new ErrorCode(
				"L_DB_MODEL_FK_0001",
				message);

		public static IErrorCode InvalidPrimarynKey(string message)
			=> new ErrorCode(
				"L_DB_MODEL_PK_0001",
				message);

		public static IErrorCode InvalidUnique(string message)
			=> new ErrorCode(
				"L_DB_MODEL_UQ_0001",
				message);

		public static IErrorCode InvalidIndex(string message)
			=> new ErrorCode(
				"L_DB_MODEL_IX_0001",
				message);
	}




	public static partial class TraceFrameStack
	{
		public static IErrorCode PreviousNotWriteable => _previousNotWriteable.Value;
		private static readonly Lazy<IErrorCode> _previousNotWriteable = new(() =>
			new ErrorCode(
				"L_TraceFrmStck_0001",
				$"Previous {nameof(TraceFrameStack)} is not writeable."));
	}




	public static partial class Logger
	{
		public static IErrorCode LoggerException(int logLevel)
			=> new ErrorCode(
				"L_LOG_0000",
				$"Cannot log message with log level = {logLevel}");
	}




	public static partial class Bus
	{
		public static IErrorCode NoHandlerRegistered => _noHandlerRegistered.Value;
		private static readonly Lazy<IErrorCode> _noHandlerRegistered = new(() =>
			new ErrorCode(
				"L_BUS_REG_0001",
				"No message/event handler registered."));

		public static IErrorCode CreateHandlerProcessorException(string handlerType)
			=> new ErrorCode(
				"L_BUS_PROC_0001",
				$"Cannot create handler processor of type {handlerType}");

		public static IErrorCode CreateHandlerException(string handlerType)
			=> new ErrorCode(
				"L_BUS_HAND_0001",
				$"Cannot create handler of type {handlerType}");

		public static IErrorCode UnhandledHandlerForMessageException(string messageType, string handlerType)
			=> new ErrorCode(
				"L_BUS_HAND_0002",
				$"Cannot handle message {messageType} by handler {handlerType}");

		public static IErrorCode UnhandledHandlerForEventException(string eventType, string handlerType)
			=> new ErrorCode(
				"L_BUS_HAND_0003",
				$"Cannot handle event {eventType} by handler {handlerType}");
	}

	public static partial class DirectoryException
	{
		public static IErrorCode UnableToMoveUp(string path1, int count)
			=> new ErrorCode(
				"L_Dir_0001",
				$"Unable to move up from directory '{path1}' {count} times");
	}

	public static partial class UnitOfWorkException
	{
		public static IErrorCode InvalidUoW => _invalidUoW.Value;
		private static readonly Lazy<IErrorCode> _invalidUoW = new(() =>
			new ErrorCode(
				"L_UoW_0001",
				$"Cannot create UnitOfWork"));
	}

	//public static partial class ChangeTrackingVsCachingException
	//{
	//	public static IErrorCode Default => _default.Value;
	//	private static readonly Lazy<IErrorCode> _default = new(() =>
	//		new ErrorCode(
	//			"L_TrackCache_0001",
	//			$"Cannot use change tracking with caching"));
	//}
}
