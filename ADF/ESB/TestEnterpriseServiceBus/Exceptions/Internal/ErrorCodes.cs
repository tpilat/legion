using Legion;
using Legion.Exceptions;

namespace TestEnterpriseServiceBus.Exceptions.Internal;

public static partial class ErrorCodes
{
	public static partial class RPOHttpClientException
	{
		public static IErrorCode Default => _default.Value;
		private static readonly Lazy<IErrorCode> _default = new(() =>
			new ErrorCode(
				"TEST_RPO_0001",
				"Call exception."));
	}

	public static partial class SocPoistHttpClientException
	{
		public static IErrorCode Default => _default.Value;
		private static readonly Lazy<IErrorCode> _default = new(() =>
			new ErrorCode(
				"TEST_SocPoist_0001",
				"Call exception."));
	}
}
