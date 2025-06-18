namespace Legion.ADF.ESB.Components.Model;

public partial class AdapterStatus : Components.ComponentsBaseEntity, Legion.Model.IEntity, IComparable
{
	/// <summary>
	/// 00000001-0000-0000-0000-000000000000
	/// </summary>
	public static AdapterStatus Disabled_NewObject => new(new Guid("00000001-0000-0000-0000-000000000000"), "Disabled", "Disabled");

	/// <summary>
	/// 00000002-0000-0000-0000-000000000000
	/// </summary>
	public static AdapterStatus Offline_NewObject => new(new Guid("00000002-0000-0000-0000-000000000000"), "Offline", "Offline");

	/// <summary>
	/// 00000003-0000-0000-0000-000000000000
	/// </summary>
	public static AdapterStatus Active_NewObject => new(new Guid("00000003-0000-0000-0000-000000000000"), "Active", "Active");

	/// <summary>
	/// 00000004-0000-0000-0000-000000000000
	/// </summary>
	public static AdapterStatus Error_NewObject => new(new Guid("00000004-0000-0000-0000-000000000000"), "Error", "Error");

	/// <summary>
	/// 00000005-0000-0000-0000-000000000000
	/// </summary>
	public static AdapterStatus Suspended_NewObject => new(new Guid("00000005-0000-0000-0000-000000000000"), "Suspended", "Suspended");

	private AdapterStatus(Guid idAdapterStatus, string code, string name)
		: this()
	{
		IdAdapterStatus = idAdapterStatus;
		Code = code;
		Name = name;
	}

	public override bool Equals(object? obj)
	{
		if (obj is not AdapterStatus otherValue)
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
		var otherEnumeration = other as AdapterStatus;
		return Code?.CompareTo(otherEnumeration?.Code) ?? 1;
	}

	public static AdapterStatus? FromId(Guid idAdapterStatus)
	{
		return AsEnumerable_NewObjects().FirstOrDefault(x => x.IdAdapterStatus == idAdapterStatus);
	}

	public static AdapterStatus? FromCode(string code)
	{
		return AsEnumerable_NewObjects().FirstOrDefault(v => v.Code?.Equals(code, StringComparison.Ordinal) ?? false);
	}

	public static readonly Lazy<IReadOnlyDictionary<Guid, AdapterStatus>> DictionaryMap = new(() => new Dictionary<Guid, AdapterStatus>
	{
			{ new Guid("00000001-0000-0000-0000-000000000000"), Disabled_NewObject },
			{ new Guid("00000002-0000-0000-0000-000000000000"), Offline_NewObject },
			{ new Guid("00000003-0000-0000-0000-000000000000"), Active_NewObject },
			{ new Guid("00000004-0000-0000-0000-000000000000"), Error_NewObject },
			{ new Guid("00000005-0000-0000-0000-000000000000"), Suspended_NewObject }
	});

	public static IEnumerable<AdapterStatus> AsEnumerable_NewObjects()
	{
		yield return Disabled_NewObject;
		yield return Offline_NewObject;
		yield return Active_NewObject;
		yield return Error_NewObject;
		yield return Suspended_NewObject;
	}
}
