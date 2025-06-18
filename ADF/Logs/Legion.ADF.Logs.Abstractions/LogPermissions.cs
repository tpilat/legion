namespace Legion.ADF.Logs;

public static class LogPermissions
{
	public enum EnvironmentInfo
	{
		SaveEnvironmentInfo
	}

	public enum EventCounterData
	{
		SaveEventCounterData
	}

	public enum LocalRequestResponse
	{
		SaveLocalRequest,
		SaveLocalResponse,
		SaveLocalRequestPayload,
		SaveLocalResponsePayload
	}

	public enum Log
	{
		SaveLog
	}

	public enum RemoteRequestResponse
	{
		SaveRemoteRequest,
		SaveRemoteResponse,
		SaveRemoteRequestPayload,
		SaveRemoteResponsePayload
	}

	public enum UnstructuredLog
	{
		SaveUnstructuredLog
	}
}
