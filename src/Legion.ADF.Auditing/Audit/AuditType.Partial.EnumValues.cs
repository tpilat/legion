namespace Legion.ADF.Auditing.Audit;

public partial class AuditType : Auditing.EntityBase, Legion.Model.IEntity, IComparable
{
	/// <summary>
	/// 00000001-0000-0000-0000-000000000000
	/// </summary>
	public static AuditType None_NewObject => new(new Guid("00000001-0000-0000-0000-000000000000"), "None", "None", 0);

	/// <summary>
	/// 00000002-0000-0000-0000-000000000000
	/// </summary>
	public static AuditType Insert_NewObject => new(new Guid("00000002-0000-0000-0000-000000000000"), "Insert", "Insert", 1);

	/// <summary>
	/// 00000003-0000-0000-0000-000000000000
	/// </summary>
	public static AuditType Update_NewObject => new(new Guid("00000003-0000-0000-0000-000000000000"), "Update", "Update", 2);

	/// <summary>
	/// 00000004-0000-0000-0000-000000000000
	/// </summary>
	public static AuditType Delete_NewObject => new(new Guid("00000004-0000-0000-0000-000000000000"), "Delete", "Delete", 3);

	/// <summary>
	/// 00000005-0000-0000-0000-000000000000
	/// </summary>
	public static AuditType Read_NewObject => new(new Guid("00000005-0000-0000-0000-000000000000"), "Read", "Read", 4);

	private AuditType(Guid idAuditType, string code, string name, int itemCode)
		: this()
	{
		IdAuditType = idAuditType;
		Code = code;
		Name = name;
		ItemCode = itemCode;
	}

	public override bool Equals(object? obj)
	{
		if (obj is not AuditType otherValue)
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
		var otherEnumeration = other as AuditType;
		return Code?.CompareTo(otherEnumeration?.Code) ?? 1;
	}

	public static AuditType? FromId(Guid idAuditType)
	{
		return AsEnumerable_NewObjects().FirstOrDefault(x => x.IdAuditType == idAuditType);
	}

	public static AuditType? FromCode(string code)
	{
		return AsEnumerable_NewObjects().FirstOrDefault(v => v.Code?.Equals(code, StringComparison.Ordinal) ?? false);
	}

	public static readonly Lazy<IReadOnlyDictionary<Guid, AuditType>> DictionaryMap = new(() => new Dictionary<Guid, AuditType>
	{
			{ new Guid("00000001-0000-0000-0000-000000000000"), None_NewObject },
			{ new Guid("00000002-0000-0000-0000-000000000000"), Insert_NewObject },
			{ new Guid("00000003-0000-0000-0000-000000000000"), Update_NewObject },
			{ new Guid("00000004-0000-0000-0000-000000000000"), Delete_NewObject },
			{ new Guid("00000005-0000-0000-0000-000000000000"), Read_NewObject }
	});

	public static IEnumerable<AuditType> AsEnumerable_NewObjects()
	{
		yield return None_NewObject;
		yield return Insert_NewObject;
		yield return Update_NewObject;
		yield return Delete_NewObject;
		yield return Read_NewObject;
	}
}
