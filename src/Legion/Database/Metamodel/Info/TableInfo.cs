using Legion.Extensions;
using System.Data;

namespace Legion.Database.Metamodel.Info;

public class TableInfo
{
	public string SchemaName { get; }
	public string TableName { get; }
	public string FullTableName { get; }

	public IReadOnlyList<ColumnInfo> Columns { get; }
	public IReadOnlyList<string> ColumnNames { get; }
	public string CommaSeparatedColumns { get; }

	public TableInfo(
		string schemaName,
		string tableName,
		List<ColumnInfo> columns)
	{
		Throw.IfArgumentNullOrWhiteSpace(schemaName);
		Throw.IfArgumentNullOrWhiteSpace(tableName);
		Throw.IfArgumentNullOrEmpty(columns);

		SchemaName = schemaName;
		TableName = tableName;
		FullTableName = $"{SchemaName}.{TableName}";

		Columns = columns;
		ColumnNames = Columns.Select(c => c.ColumnName).ToList();
		CommaSeparatedColumns = string.Join(", ", ColumnNames);
	}

	public DataTable ToDataTable(IEnumerable<IDictionary<string, object?>?> rows)
	{
		Throw.IfArgumentNull(rows);

		var dt = new DataTable();

		foreach (var column in Columns)
			dt.Columns.Add(column.ClearedColumnName, column.PropertyType.GetUnderlyingNullableType());

		//dt.Constraints.Clear();

		dt.MinimumCapacity = rows.Count();

		dt.BeginLoadData();

		foreach (var row in rows.Where(r => r != null))
		{
			var dataRow = dt.NewRow();

			foreach (var propertyKVP in row!)
				dataRow[propertyKVP.Key] = propertyKVP.Value ?? DBNull.Value;

			dt.Rows.Add(dataRow);

			dataRow.AcceptChanges();
		}

		dt.EndLoadData();

		return dt;
	}

	public IDataReader ToDataReader(IEnumerable<IDictionary<string, object?>?> rows)
	{
		Throw.IfArgumentNull(rows);

		return new TableInfoDataReader(this, rows);
	}

	public override string ToString()
		=> $"{SchemaName}.{TableName}";
}
