using Legion.ADF.Logs.Services;
using Npgsql;
using System.Data.Common;

namespace Legion.ADF.Logs.PostgreSQL;

internal class PostgreSqlADFLogger : IADFLoggerStore
{
	public int SaveLog(
		IScopeContext scopeContext,
		DbConnection dbConnection,
		Logs.Model.Log log)
	{
		scopeContext = scopeContext.CreateNew();

		if (dbConnection is not NpgsqlConnection npgsqlConnection)
		{
			Throw.ArgumentException(nameof(dbConnection), $"{nameof(dbConnection)} must be of type {nameof(NpgsqlConnection)}", scopeContext);
			return -1;
		}

		Throw.IfArgumentNull(log, scopeContext);

		var sql = $@"INSERT INTO log.""Log""
(""IdLog"", ""CreatedUtc"", ""InternalMessage"", ""ClientMessage"", ""Detail"", ""StackTrace"", ""Component"", ""OperationName"", ""AggregateName"", ""AggregateIdentifier"", ""CustomCorrelationId"", ""IdApplicationEntry"", ""CorrelationId"", ""ExternalCorrelationId"", ""ContextProperties"", ""IdUser"", ""TenantIdentifier"", ""IdLogLevel"", ""LogCode"", ""SourceSystemName"", ""TraceCorrelationId"", ""TraceFrame"", ""SourceContext"", ""RuntimeUniqueKey"", ""IsValidationError"", ""PropertyName"", ""DisplayPropertyName"", ""ValidationFailure"")
VALUES(@IdLog, @CreatedUtc, @InternalMessage, @ClientMessage, @Detail, @StackTrace, @Component, @OperationName, @AggregateName, @AggregateIdentifier, @CustomCorrelationId, @IdApplicationEntry, @CorrelationId, @ExternalCorrelationId, @ContextProperties, @IdUser, @TenantIdentifier, @IdLogLevel, @LogCode, @SourceSystemName, @TraceCorrelationId, @TraceFrame, @SourceContext, @RuntimeUniqueKey, @IsValidationError, @PropertyName, @DisplayPropertyName, @ValidationFailure);
";

		using var cmd = new Npgsql.NpgsqlCommand(sql, npgsqlConnection);
		cmd.Parameters.AddWithValue("@IdLog", NpgsqlTypes.NpgsqlDbType.Uuid, log.IdLog);
		cmd.Parameters.AddWithValue("@CreatedUtc", NpgsqlTypes.NpgsqlDbType.TimestampTz, log.CreatedUtc);
		cmd.Parameters.AddWithNullableValue("@InternalMessage", NpgsqlTypes.NpgsqlDbType.Text, log.InternalMessage);
		cmd.Parameters.AddWithNullableValue("@ClientMessage", NpgsqlTypes.NpgsqlDbType.Text, log.ClientMessage);
		cmd.Parameters.AddWithNullableValue("@Detail", NpgsqlTypes.NpgsqlDbType.Text, log.Detail);
		cmd.Parameters.AddWithNullableValue("@StackTrace", NpgsqlTypes.NpgsqlDbType.Text, log.StackTrace);
		cmd.Parameters.AddWithNullableValue("@Component", NpgsqlTypes.NpgsqlDbType.Varchar, log.Component);
		cmd.Parameters.AddWithNullableValue("@OperationName", NpgsqlTypes.NpgsqlDbType.Varchar, log.OperationName);
		cmd.Parameters.AddWithNullableValue("@AggregateName", NpgsqlTypes.NpgsqlDbType.Varchar, log.AggregateName);
		cmd.Parameters.AddWithNullableValue("@AggregateIdentifier", NpgsqlTypes.NpgsqlDbType.Varchar, log.AggregateIdentifier);
		cmd.Parameters.AddWithNullableValue("@CustomCorrelationId", NpgsqlTypes.NpgsqlDbType.Varchar, log.CustomCorrelationId);
		cmd.Parameters.AddWithNullableValue("@IdApplicationEntry", NpgsqlTypes.NpgsqlDbType.Uuid, log.IdApplicationEntry);
		cmd.Parameters.AddWithNullableValue("@CorrelationId", NpgsqlTypes.NpgsqlDbType.Uuid, log.CorrelationId);
		cmd.Parameters.AddWithNullableValue("@ExternalCorrelationId", NpgsqlTypes.NpgsqlDbType.Varchar, log.ExternalCorrelationId);
		cmd.Parameters.AddWithNullableValue("@ContextProperties", NpgsqlTypes.NpgsqlDbType.Jsonb, log.ContextProperties);
		cmd.Parameters.AddWithNullableValue("@IdUser", NpgsqlTypes.NpgsqlDbType.Uuid, log.IdUser);
		cmd.Parameters.AddWithNullableValue("@TenantIdentifier", NpgsqlTypes.NpgsqlDbType.Uuid, log.TenantIdentifier);
		cmd.Parameters.AddWithValue("@IdLogLevel", NpgsqlTypes.NpgsqlDbType.Integer, log.IdLogLevel);
		cmd.Parameters.AddWithNullableValue("@LogCode", NpgsqlTypes.NpgsqlDbType.Varchar, log.LogCode);
		cmd.Parameters.AddWithNullableValue("@SourceSystemName", NpgsqlTypes.NpgsqlDbType.Varchar, log.SourceSystemName);
		cmd.Parameters.AddWithNullableValue("@TraceCorrelationId", NpgsqlTypes.NpgsqlDbType.Uuid, log.TraceCorrelationId);
		cmd.Parameters.AddWithNullableValue("@TraceFrame", NpgsqlTypes.NpgsqlDbType.Text, log.TraceFrame);
		cmd.Parameters.AddWithNullableValue("@SourceContext", NpgsqlTypes.NpgsqlDbType.Text, log.SourceContext);
		cmd.Parameters.AddWithValue("@RuntimeUniqueKey", NpgsqlTypes.NpgsqlDbType.Uuid, log.RuntimeUniqueKey);
		cmd.Parameters.AddWithValue("@IsValidationError", NpgsqlTypes.NpgsqlDbType.Boolean, log.IsValidationError);
		cmd.Parameters.AddWithNullableValue("@PropertyName", NpgsqlTypes.NpgsqlDbType.Varchar, log.PropertyName);
		cmd.Parameters.AddWithNullableValue("@DisplayPropertyName", NpgsqlTypes.NpgsqlDbType.Varchar, log.DisplayPropertyName);
		cmd.Parameters.AddWithNullableValue("@ValidationFailure", NpgsqlTypes.NpgsqlDbType.Text, log.ValidationFailure);

		return cmd.ExecuteNonQuery();
	}

	public int SaveUnstructuredLog(
		IScopeContext scopeContext,
		DbConnection dbConnection,
		Logs.Model.UnstructuredLog unstructuredLog)
	{
		scopeContext = scopeContext.CreateNew();

		if (dbConnection is not NpgsqlConnection npgsqlConnection)
		{
			Throw.ArgumentException(nameof(dbConnection), $"{nameof(dbConnection)} must be of type {nameof(NpgsqlConnection)}", scopeContext);
			return -1;
		}

		Throw.IfArgumentNull(unstructuredLog, scopeContext);

		var sql = $@"INSERT INTO log.""UnstructuredLog""
(""IdUnstructuredLog"", ""CreatedUtc"", ""IdLogLevel"", ""Message"", ""StackTrace"", ""SourceContext"", ""RuntimeUniqueKey"", ""EventName"", ""EventId"")
VALUES(@IdUnstructuredLog, @CreatedUtc, @IdLogLevel, @Message, @StackTrace, @SourceContext, @RuntimeUniqueKey, @EventName, @EventId);";

		using var cmd = new Npgsql.NpgsqlCommand(sql, npgsqlConnection);
		cmd.Parameters.AddWithValue("@IdUnstructuredLog", NpgsqlTypes.NpgsqlDbType.Uuid, unstructuredLog.IdUnstructuredLog);
		cmd.Parameters.AddWithValue("@CreatedUtc", NpgsqlTypes.NpgsqlDbType.TimestampTz, unstructuredLog.CreatedUtc);
		cmd.Parameters.AddWithValue("@IdLogLevel", NpgsqlTypes.NpgsqlDbType.Integer, unstructuredLog.IdLogLevel);
		cmd.Parameters.AddWithNullableValue("@Message", NpgsqlTypes.NpgsqlDbType.Text, unstructuredLog.Message);
		cmd.Parameters.AddWithNullableValue("@StackTrace", NpgsqlTypes.NpgsqlDbType.Text, unstructuredLog.StackTrace);
		cmd.Parameters.AddWithNullableValue("@SourceContext", NpgsqlTypes.NpgsqlDbType.Text, unstructuredLog.SourceContext);
		cmd.Parameters.AddWithValue("@RuntimeUniqueKey", NpgsqlTypes.NpgsqlDbType.Uuid, unstructuredLog.RuntimeUniqueKey);
		cmd.Parameters.AddWithNullableValue("@EventName", NpgsqlTypes.NpgsqlDbType.Text, unstructuredLog.EventName);
		cmd.Parameters.AddWithNullableValue("@EventId", NpgsqlTypes.NpgsqlDbType.Integer, unstructuredLog.EventId);

		return cmd.ExecuteNonQuery();
	}
}
