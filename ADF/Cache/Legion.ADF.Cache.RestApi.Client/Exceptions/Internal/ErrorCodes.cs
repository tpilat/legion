using Legion.Exceptions;

namespace Legion.ADF.Cache.RestApi.Client.Internal;

public static partial class ErrorCodes
{
	public static partial class ApiClientException
	{
		public static IErrorCode Default(string clientName)
			=> new ErrorCode(
				"Cache_APICLIENT_0000",
				$"{clientName}: Unhandled exception.");

		public static IErrorCode ErrorResponse(string clientName)
			=> new ErrorCode(
				"Cache_APICLIENT_0001",
				$"{clientName}: Error response.");

		public static IErrorCode InvalidJsonResponse(string clientName, string responseType)
			=> new ErrorCode(
				"Cache_APICLIENT_0002",
				$"{clientName}: Invalid response of type {responseType}");
	}
}
