using System.Data;

namespace Legion.Database.Metamodel.Info;

public class TableInfoDataReader : IDataReader
{
	private readonly TableInfo _tableInfo;
	private readonly List<string> _columnNames;
	private readonly List<IDictionary<string, object?>?> _rows;
	private int _currentIndex = -1;

	public TableInfoDataReader(TableInfo tableInfo, IEnumerable<IDictionary<string, object?>?> rows)
	{
		Throw.IfArgumentNull(tableInfo);
		Throw.IfArgumentNull(rows);

		_tableInfo = tableInfo;
		_columnNames = _tableInfo.ColumnNames.ToList();
		_rows = rows.ToList();
	}

	public object GetValue(int i)
	{
		var propertyName = GetName(i);
		return GetValueByName(propertyName);
	}

	private object GetValueByName(string propertyName)
	{
		if (_rows[_currentIndex]?.TryGetValue(propertyName, out object? value) == true)
			return value!;

		return null!;
	}

	public int GetValues(object[] values)
	{
		if (_currentIndex < 0 || _rows.Count <= _currentIndex)
			throw new InvalidOperationException("No current row.");

		int fieldCount = FieldCount;
		for (int i = 0; i < fieldCount; i++)
			values[i] = GetValue(i);

		return fieldCount;
	}

	public bool Read()
	{
		_currentIndex++;
		return _currentIndex < _rows.Count;
	}

	public int FieldCount => _columnNames.Count;

	public string GetName(int i)
		=> _columnNames[i];

	public int GetOrdinal(string name)
		=> _columnNames.IndexOf(name);

	public bool IsDBNull(int i)
		=> GetValue(i) == DBNull.Value;

	// Required methods for IDataReader (but not necessarily implemented for simplicity)
	public void Dispose() { }
	public bool NextResult() => false;
	public void Close() { }
	public DataTable GetSchemaTable() => throw new NotImplementedException();
	public int Depth => 0;
	public bool IsClosed => false;
	public int RecordsAffected => -1;
	public bool GetBoolean(int i) => (bool)GetValue(i);
	public byte GetByte(int i) => (byte)GetValue(i);
	public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length) => throw new NotImplementedException();
	public char GetChar(int i) => (char)GetValue(i);
	public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length) => throw new NotImplementedException();
	public IDataReader GetData(int i) => throw new NotImplementedException();
	public string GetDataTypeName(int i) => GetValue(i).GetType().Name;
	public DateTime GetDateTime(int i) => (DateTime)GetValue(i);
	public decimal GetDecimal(int i) => (decimal)GetValue(i);
	public double GetDouble(int i) => (double)GetValue(i);
	public Type GetFieldType(int i) => GetValue(i).GetType();
	public float GetFloat(int i) => (float)GetValue(i);
	public Guid GetGuid(int i) => (Guid)GetValue(i);
	public short GetInt16(int i) => (short)GetValue(i);
	public int GetInt32(int i) => (int)GetValue(i);
	public long GetInt64(int i) => (long)GetValue(i);
	public string GetString(int i) => (string)GetValue(i);

	public object this[int i] => GetValue(i);
	public object this[string name] => GetValueByName(name);
}
