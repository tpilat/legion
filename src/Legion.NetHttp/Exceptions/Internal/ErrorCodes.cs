using Legion.Exceptions;

namespace Legion.NetHttp.Exceptions.Internal;

public static partial class ErrorCodes
{
	public static partial class HttpApiClientException
	{
		public static IErrorCode SendError => _sendError.Value;
		private static readonly Lazy<IErrorCode> _sendError = new(() =>
			new ErrorCode(
				"L_NETHTTP_SEND_0001",
				"Sending  request error."));

		public static IErrorCode MissingServiceProvider => _missingServiceProvider.Value;
		private static readonly Lazy<IErrorCode> _missingServiceProvider = new(() =>
			new ErrorCode(
				"L_NETHTTP_MISS_SP_0001",
				"No ServiceProvider."));

		public static IErrorCode MissingScopeContext => _missingScopeContext.Value;
		private static readonly Lazy<IErrorCode> _missingScopeContext = new(() =>
			new ErrorCode(
				"L_NETHTTP_MISS_SC_0001",
				"No ScopeContext."));
	}
}
