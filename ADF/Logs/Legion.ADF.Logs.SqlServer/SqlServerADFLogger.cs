using Legion.ADF.Logs.Services;
using Legion.Database.SqlServer.Extensions;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;

namespace Legion.ADF.Logs.SqlServer;

internal class SqlServerADFLogger : IADFLoggerStore
{
	public int SaveLog(
		IScopeContext scopeContext,
		DbConnection dbConnection,
		Logs.Model.Log log)
	{
		scopeContext = scopeContext.CreateNew();

		if (dbConnection is not SqlConnection sqlConnection)
		{
			Throw.ArgumentException(nameof(dbConnection), $"{nameof(dbConnection)} must be of type {nameof(SqlConnection)}", scopeContext);
			return -1;
		}

		Throw.IfArgumentNull(log, scopeContext);

		var sql = $@"INSERT INTO log.""Log""
(""IdLog"", ""CreatedUtc"", ""InternalMessage"", ""ClientMessage"", ""Detail"", ""StackTrace"", ""Component"", ""OperationName"", ""AggregateName"", ""AggregateIdentifier"", ""CustomCorrelationId"", ""IdApplicationEntry"", ""CorrelationId"", ""ExternalCorrelationId"", ""ContextProperties"", ""IdUser"", ""TenantIdentifier"", ""IdLogLevel"", ""LogCode"", ""SourceSystemName"", ""TraceCorrelationId"", ""TraceFrame"", ""SourceContext"", ""RuntimeUniqueKey"", ""IsValidationError"", ""PropertyName"", ""DisplayPropertyName"", ""ValidationFailure"")
VALUES(@IdLog, @CreatedUtc, @InternalMessage, @ClientMessage, @Detail, @StackTrace, @Component, @OperationName, @AggregateName, @AggregateIdentifier, @CustomCorrelationId, @IdApplicationEntry, @CorrelationId, @ExternalCorrelationId, @ContextProperties, @IdUser, @TenantIdentifier, @IdLogLevel, @LogCode, @SourceSystemName, @TraceCorrelationId, @TraceFrame, @SourceContext, @RuntimeUniqueKey, @IsValidationError, @PropertyName, @DisplayPropertyName, @ValidationFailure);
";

		using var cmd = new SqlCommand(sql, sqlConnection);
		cmd.Parameters.AddWithValue("@IdLog", SqlDbType.UniqueIdentifier, log.IdLog);
		cmd.Parameters.AddWithValue("@CreatedUtc", SqlDbType.DateTime2, log.CreatedUtc);
		cmd.Parameters.AddWithNullableValue("@InternalMessage", SqlDbType.NVarChar, log.InternalMessage);
		cmd.Parameters.AddWithNullableValue("@ClientMessage", SqlDbType.NVarChar, log.ClientMessage);
		cmd.Parameters.AddWithNullableValue("@Detail", SqlDbType.NVarChar, log.Detail);
		cmd.Parameters.AddWithNullableValue("@StackTrace", SqlDbType.NVarChar, log.StackTrace);
		cmd.Parameters.AddWithNullableValue("@Component", SqlDbType.NVarChar, log.Component);
		cmd.Parameters.AddWithNullableValue("@OperationName", SqlDbType.NVarChar, log.OperationName);
		cmd.Parameters.AddWithNullableValue("@AggregateName", SqlDbType.NVarChar, log.AggregateName);
		cmd.Parameters.AddWithNullableValue("@AggregateIdentifier", SqlDbType.NVarChar, log.AggregateIdentifier);
		cmd.Parameters.AddWithNullableValue("@CustomCorrelationId", SqlDbType.NVarChar, log.CustomCorrelationId);
		cmd.Parameters.AddWithNullableValue("@IdApplicationEntry", SqlDbType.UniqueIdentifier, log.IdApplicationEntry);
		cmd.Parameters.AddWithNullableValue("@CorrelationId", SqlDbType.UniqueIdentifier, log.CorrelationId);
		cmd.Parameters.AddWithNullableValue("@ExternalCorrelationId", SqlDbType.NVarChar, log.ExternalCorrelationId);
		cmd.Parameters.AddWithNullableValue("@ContextProperties", SqlDbType.NVarChar, log.ContextProperties);
		cmd.Parameters.AddWithNullableValue("@IdUser", SqlDbType.UniqueIdentifier, log.IdUser);
		cmd.Parameters.AddWithNullableValue("@TenantIdentifier", SqlDbType.UniqueIdentifier, log.TenantIdentifier);
		cmd.Parameters.AddWithValue("@IdLogLevel", SqlDbType.Int, log.IdLogLevel);
		cmd.Parameters.AddWithNullableValue("@LogCode", SqlDbType.NVarChar, log.LogCode);
		cmd.Parameters.AddWithNullableValue("@SourceSystemName", SqlDbType.NVarChar, log.SourceSystemName);
		cmd.Parameters.AddWithNullableValue("@TraceCorrelationId", SqlDbType.UniqueIdentifier, log.TraceCorrelationId);
		cmd.Parameters.AddWithNullableValue("@TraceFrame", SqlDbType.NVarChar, log.TraceFrame);
		cmd.Parameters.AddWithNullableValue("@SourceContext", SqlDbType.NVarChar, log.SourceContext);
		cmd.Parameters.AddWithValue("@RuntimeUniqueKey", SqlDbType.UniqueIdentifier, log.RuntimeUniqueKey);
		cmd.Parameters.AddWithValue("@IsValidationError", SqlDbType.Bit, log.IsValidationError);
		cmd.Parameters.AddWithNullableValue("@PropertyName", SqlDbType.NVarChar, log.PropertyName);
		cmd.Parameters.AddWithNullableValue("@DisplayPropertyName", SqlDbType.NVarChar, log.DisplayPropertyName);
		cmd.Parameters.AddWithNullableValue("@ValidationFailure", SqlDbType.NVarChar, log.ValidationFailure);

		return cmd.ExecuteNonQuery();
	}

	public int SaveUnstructuredLog(
		IScopeContext scopeContext,
		DbConnection dbConnection,
		Logs.Model.UnstructuredLog unstructuredLog)
	{
		scopeContext = scopeContext.CreateNew();

		if (dbConnection is not SqlConnection sqlConnection)
		{
			Throw.ArgumentException(nameof(dbConnection), $"{nameof(dbConnection)} must be of type {nameof(SqlConnection)}", scopeContext);
			return -1;
		}

		Throw.IfArgumentNull(unstructuredLog, scopeContext);

		var sql = $@"INSERT INTO log.""UnstructuredLog""
(""IdUnstructuredLog"", ""CreatedUtc"", ""IdLogLevel"", ""Message"", ""StackTrace"", ""SourceContext"", ""RuntimeUniqueKey"", ""EventName"", ""EventId"")
VALUES(@IdUnstructuredLog, @CreatedUtc, @IdLogLevel, @Message, @StackTrace, @SourceContext, @RuntimeUniqueKey, @EventName, @EventId);";

		using var cmd = new SqlCommand(sql, sqlConnection);
		cmd.Parameters.AddWithValue("@IdUnstructuredLog", SqlDbType.UniqueIdentifier, unstructuredLog.IdUnstructuredLog);
		cmd.Parameters.AddWithValue("@CreatedUtc", SqlDbType.DateTime2, unstructuredLog.CreatedUtc);
		cmd.Parameters.AddWithValue("@IdLogLevel", SqlDbType.Int, unstructuredLog.IdLogLevel);
		cmd.Parameters.AddWithNullableValue("@Message", SqlDbType.NVarChar, unstructuredLog.Message);
		cmd.Parameters.AddWithNullableValue("@StackTrace", SqlDbType.NVarChar, unstructuredLog.StackTrace);
		cmd.Parameters.AddWithNullableValue("@SourceContext", SqlDbType.NVarChar, unstructuredLog.SourceContext);
		cmd.Parameters.AddWithValue("@RuntimeUniqueKey", SqlDbType.UniqueIdentifier, unstructuredLog.RuntimeUniqueKey);
		cmd.Parameters.AddWithNullableValue("@EventName", SqlDbType.NVarChar, unstructuredLog.EventName);
		cmd.Parameters.AddWithNullableValue("@EventId", SqlDbType.Int, unstructuredLog.EventId);

		return cmd.ExecuteNonQuery();
	}
}
