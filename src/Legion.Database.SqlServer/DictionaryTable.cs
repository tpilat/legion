using Legion.Extensions;
using Legion.Text;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Legion.Database.SqlServer;

public class DictionaryTable
{
	public string SchemaName { get; }
	public string TableName { get; }
	public IReadOnlyList<string> PropertyNames { get; }
	public IReadOnlyDictionary<string, int> PropertyIndex { get; }
	public IReadOnlyDictionary<string, string> PropertyColumnMapping { get; }
	public IReadOnlyDictionary<string, SqlDbType> PropertyTypeMapping { get; }
	public IReadOnlyDictionary<string, Func<object?, object?>>? PropertyValueConverter { get; }
	public bool UseQuotationMarksForTableName { get; }
	public bool UseQuotationMarksForColumnNames { get; }

	public IReadOnlyList<string> ColumnNames { get; }
	public IReadOnlyDictionary<string, SqlDbType>? ColumnTypes { get; }


	public DictionaryTable(IDictionaryTableOptions options)
		: this(options, false)
	{
	}

	protected DictionaryTable(IDictionaryTableOptions options, bool propertyTypeMappingIsRequired)
	{
		Throw.IfArgumentNull(options);
		Throw.IfArgumentNullOrEmpty(options.PropertyNames);

		if (propertyTypeMappingIsRequired)
			options.PropertyTypeMappingIsRequired = true;

		options.Validate(true, false);

		SchemaName = options.SchemaName ?? "";
		TableName = options.TableName ?? "";
		PropertyNames = options.PropertyNames!.ToList();
		var pcMapping = options.PropertyColumnMapping?.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
		PropertyTypeMapping = options.PropertyTypeMapping?.ToDictionary(kvp => kvp.Key, kvp => kvp.Value) ?? [];
		PropertyValueConverter = options.PropertyValueConverter?.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
		UseQuotationMarksForTableName = options.UseQuotationMarksForTableName;
		UseQuotationMarksForColumnNames = options.UseQuotationMarksForColumnNames;

		var idx = 1;
		PropertyIndex = PropertyNames.Select(x => new { PropertyName = x, Index = idx++ }).ToDictionary(k => k.PropertyName, v => v.Index);

		if (pcMapping == null || pcMapping.Count == 0)
		{
			ColumnNames = PropertyNames;
			PropertyColumnMapping = PropertyNames.ToDictionary(k => k, v => v);

			if (options.PropertyTypeMappingIsRequired || (0 < PropertyTypeMapping.Count))
				ColumnTypes = PropertyNames.ToDictionary(p => p, ConvertType);
		}
		else
		{
			var columnNames = new List<string>();
			var columnTypes = new Dictionary<string, SqlDbType>();

			foreach (var propertyName in PropertyNames)
			{
				if (pcMapping.TryGetValue(propertyName, out string? columnName))
					columnNames.Add(columnName);
				else
				{
					columnNames.Add(propertyName);
					pcMapping.Add(propertyName, propertyName);
				}

				if (options.PropertyTypeMappingIsRequired || (0 < PropertyTypeMapping.Count))
					columnTypes.Add(propertyName, ConvertType(propertyName));
			}

			PropertyColumnMapping = pcMapping;
			ColumnNames = columnNames;

			if (options.PropertyTypeMappingIsRequired || (0 < PropertyTypeMapping.Count))
				ColumnTypes = columnTypes;
		}
	}

	public string GetQualifiedTableName()
	{
		if (UseQuotationMarksForTableName)
			return $"\"{TableName}\"";
		else
			return TableName;
	}

	public string GetQualifiedColumnName(string columnName)
	{
		if (UseQuotationMarksForColumnNames)
			return $"\"{columnName}\"";
		else
			return columnName;
	}

	public string GetColumns()
	{
		string columns;
		if (UseQuotationMarksForColumnNames)
			columns = $"\"{string.Join("\", \"", ColumnNames)}\"";
		else
			columns = string.Join(", ", ColumnNames);

		return columns;
	}

	public string PropertyNameToColumnName(string propertyName)
		=> PropertyColumnMapping.TryGetValue(propertyName, out string? columnName)
			? columnName
			: throw new ArgumentException($"PropertyName {propertyName} is not a valid property mapped to any column.", nameof(propertyName));

	public string GetColumns(List<string>? propertyNames)
	{
		if (propertyNames == null || propertyNames.Count == 0)
			return GetColumns();

		var columnNames = propertyNames.Select(x => PropertyNameToColumnName(x));

		string columns;
		if (UseQuotationMarksForColumnNames)
			columns = $"\"{string.Join("\", \"", columnNames)}\"";
		else
			columns = string.Join(", ", columnNames);

		return columns;
	}

	public static string GetParameterName(int index)
		=> $"@p{index}";

	public string GetColumnSetters(List<string>? propertyNames)
		=> (propertyNames == null || propertyNames.Count == 0)
		? string.Join(", ", PropertyNames.Select(propertyName => $"{GetQualifiedColumnName(propertyName)}={GetParameterName(PropertyIndex[propertyName])}"))
		: string.Join(", ", propertyNames.Select(propertyName => PropertyColumnMapping.TryGetValue(propertyName, out _) ? $"{GetQualifiedColumnName(propertyName)}={GetParameterName(PropertyIndex[propertyName])}" : throw new ArgumentException($"PropertyName {propertyName} is not a valid property mapped to any column.", nameof(propertyNames))));

	public string GetColumnParameters(List<string>? propertyNames)
		=> (propertyNames == null || propertyNames.Count == 0)
		? $"{string.Join(", ", PropertyNames.Select(propertyName => GetParameterName(PropertyIndex[propertyName])))}"
		: $"{string.Join(", ", propertyNames.Select(propertyName => PropertyColumnMapping.TryGetValue(propertyName, out _) ? GetParameterName(PropertyIndex[propertyName]) : throw new ArgumentException($"PropertyName {propertyName} is not a valid property mapped to any column.", nameof(propertyNames))))}";

	public string ToInsertSql(string? returnningColumnName = null, List<string>? propertyNames = null)
		=> $"INSERT INTO {StringHelper.ConcatIfNotNullOrEmpty(SchemaName, ".", (UseQuotationMarksForTableName ? "\"" : ""))}{TableName}{(UseQuotationMarksForTableName ? "\"" : "")} ({GetColumns(propertyNames)})  VALUES({GetColumnParameters(propertyNames)}){(string.IsNullOrWhiteSpace(returnningColumnName) ? "" : $"RETURNING {returnningColumnName}")}";

	public string ToUpdateSql(List<string>? propertyNames = null, string ? where = null)
		=> $"UPDATE {StringHelper.ConcatIfNotNullOrEmpty(SchemaName, ".", (UseQuotationMarksForTableName ? "\"" : ""))}{TableName}{(UseQuotationMarksForTableName ? "\"" : "")} SET {GetColumnSetters(propertyNames)}{(string.IsNullOrWhiteSpace(where) ? "" : $" WHERE {where}")}";

	public SqlBulkCopy ToBulkCopy(SqlConnection sqlConnection)
	{
		var sqlBulkCopy = new SqlBulkCopy(sqlConnection);

		sqlBulkCopy.DestinationTableName = $"{StringHelper.ConcatIfNotNullOrEmpty(SchemaName, ".", (UseQuotationMarksForTableName ? "\"" : ""))}{TableName}{(UseQuotationMarksForTableName ? "\"" : "")}";

		return sqlBulkCopy;
	}

	public void SetParameters(SqlCommand command, IDictionary<string, object?> data)
	{
		Throw.IfArgumentNull(command);
		Throw.IfArgumentNull(data);

		foreach (var propertyName in PropertyNames)
		{
			if (data.TryGetValue(propertyName, out object? value))
			{
				if (PropertyValueConverter != null && PropertyValueConverter.TryGetValue(propertyName, out Func<object?, object?>? converter))
					value = converter(value);
			}

			if (ColumnTypes == null)
				command.Parameters.AddWithValue(GetParameterName(PropertyIndex[propertyName]), value ?? DBNull.Value);
			else
				command.Parameters.AddWithValue(GetParameterName(PropertyIndex[propertyName]), /*ColumnTypes[propertyName],*/ value ?? DBNull.Value);
		}

		foreach (var kvp in data)
			if (kvp.Key.StartsWith("@"))
				command.Parameters.AddWithValue(kvp.Key, kvp.Value ?? DBNull.Value);
	}

	public SqlDbType ConvertType(string memberName)
	{
		if (PropertyTypeMapping.TryGetValue(memberName, out SqlDbType result))
			return result;
		else
			throw new InvalidOperationException($"Property '{memberName}' has no type defined in options.{nameof(IDictionaryTableOptions.PropertyTypeMapping)}");
	}
}
