namespace Legion.Database.Metamodel.Info;

public class ColumnInfo
{
	public string PropertyName { get; }
	public Type PropertyType { get; }
	public string ColumnName { get; }
	public string ClearedColumnName { get; }
	public string DatabaseType { get; }
	public bool IsNullable { get; }

	public ColumnInfo(
		string propertyName,
		Type propertyType,
		string columnName,
		string databaseType,
		bool isNullable)
	{
		Throw.IfArgumentNullOrWhiteSpace(propertyName);
		Throw.IfArgumentNull(propertyType);
		Throw.IfArgumentNullOrWhiteSpace(columnName);
		Throw.IfArgumentNullOrWhiteSpace(databaseType);

		PropertyName = propertyName;
		PropertyType = propertyType;
		ColumnName = columnName;
		ClearedColumnName = columnName.Replace("[", "").Replace("]", "").Replace("\"", "");
		DatabaseType = databaseType;
		IsNullable = isNullable;
	}

	public override string ToString()
		=> $"{ColumnName}";
}
