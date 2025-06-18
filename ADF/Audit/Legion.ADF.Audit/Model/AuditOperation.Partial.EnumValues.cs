namespace Legion.ADF.Audit.Model;

public partial class AuditOperation : Audit.AuditBaseEntity, Legion.Model.IEntity, IComparable
{
	/// <summary>
	/// 00000001-0000-0000-0000-000000000000
	/// </summary>
	public static AuditOperation None_NewObject => new(new Guid("00000001-0000-0000-0000-000000000000"), "None", "None");

	/// <summary>
	/// 00000002-0000-0000-0000-000000000000
	/// </summary>
	public static AuditOperation Read_NewObject => new(new Guid("00000002-0000-0000-0000-000000000000"), "Read", "Read");

	/// <summary>
	/// 00000003-0000-0000-0000-000000000000
	/// </summary>
	public static AuditOperation Insert_NewObject => new(new Guid("00000003-0000-0000-0000-000000000000"), "Insert", "Insert");

	/// <summary>
	/// 00000004-0000-0000-0000-000000000000
	/// </summary>
	public static AuditOperation Update_NewObject => new(new Guid("00000004-0000-0000-0000-000000000000"), "Update", "Update");

	/// <summary>
	/// 00000005-0000-0000-0000-000000000000
	/// </summary>
	public static AuditOperation Delete_NewObject => new(new Guid("00000005-0000-0000-0000-000000000000"), "Delete", "Delete");

	private AuditOperation(Guid idAuditOperation, string code, string name)
		: this()
	{
		IdAuditOperation = idAuditOperation;
		Code = code;
		Name = name;
	}

	public override bool Equals(object? obj)
	{
		if (obj is not AuditOperation otherValue)
			return false;

		var typeMatches = GetType().Equals(obj.GetType());
		var valueMatches = Code?.Equals(otherValue.Code) ?? (otherValue.Code == null);

		return typeMatches && valueMatches;
	}

	public override int GetHashCode()
	{
		return Code?.GetHashCode() ?? 0;
	}

	public int CompareTo(object? other)
	{
		var otherEnumeration = other as AuditOperation;
		return Code?.CompareTo(otherEnumeration?.Code) ?? 1;
	}

	public static AuditOperation? FromId(Guid idAuditOperation)
	{
		return AsEnumerable_NewObjects().FirstOrDefault(x => x.IdAuditOperation == idAuditOperation);
	}

	public static AuditOperation? FromCode(string code)
	{
		return AsEnumerable_NewObjects().FirstOrDefault(v => v.Code?.Equals(code, StringComparison.Ordinal) ?? false);
	}

	public static readonly Lazy<IReadOnlyDictionary<Guid, AuditOperation>> DictionaryMap = new(() => new Dictionary<Guid, AuditOperation>
	{
			{ new Guid("00000001-0000-0000-0000-000000000000"), None_NewObject },
			{ new Guid("00000002-0000-0000-0000-000000000000"), Read_NewObject },
			{ new Guid("00000003-0000-0000-0000-000000000000"), Insert_NewObject },
			{ new Guid("00000004-0000-0000-0000-000000000000"), Update_NewObject },
			{ new Guid("00000005-0000-0000-0000-000000000000"), Delete_NewObject }
	});

	public static IEnumerable<AuditOperation> AsEnumerable_NewObjects()
	{
		yield return None_NewObject;
		yield return Read_NewObject;
		yield return Insert_NewObject;
		yield return Update_NewObject;
		yield return Delete_NewObject;
	}
}
