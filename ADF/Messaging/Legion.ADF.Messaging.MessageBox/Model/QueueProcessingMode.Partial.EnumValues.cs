namespace Legion.ADF.Messaging.MessageBox.Model;

public partial class QueueProcessingMode : MessageBox.MessageBoxBaseEntity, Legion.Model.IEntity, IComparable
{
	/// <summary>
	/// 00000001-0000-0000-0000-000000000000
	/// </summary>
	public static QueueProcessingMode NoAction_NewObject => new(new Guid("00000001-0000-0000-0000-000000000000"), "NoAction", "NoAction");

	/// <summary>
	/// 00000002-0000-0000-0000-000000000000
	/// </summary>
	public static QueueProcessingMode Archivate_NewObject => new(new Guid("00000002-0000-0000-0000-000000000000"), "Archivate", "Archivate");

	/// <summary>
	/// 00000003-0000-0000-0000-000000000000
	/// </summary>
	public static QueueProcessingMode Delete_NewObject => new(new Guid("00000003-0000-0000-0000-000000000000"), "Delete", "Delete");

	private QueueProcessingMode(Guid idQueueProcessingMode, string code, string name)
		: this()
	{
		IdQueueProcessingMode = idQueueProcessingMode;
		Code = code;
		Name = name;
	}

	public override bool Equals(object? obj)
	{
		if (obj is not QueueProcessingMode otherValue)
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
		var otherEnumeration = other as QueueProcessingMode;
		return Code?.CompareTo(otherEnumeration?.Code) ?? 1;
	}

	public static QueueProcessingMode? FromId(Guid idQueueProcessingMode)
	{
		return AsEnumerable_NewObjects().FirstOrDefault(x => x.IdQueueProcessingMode == idQueueProcessingMode);
	}

	public static QueueProcessingMode? FromCode(string code)
	{
		return AsEnumerable_NewObjects().FirstOrDefault(v => v.Code?.Equals(code, StringComparison.Ordinal) ?? false);
	}

	public static readonly Lazy<IReadOnlyDictionary<Guid, QueueProcessingMode>> DictionaryMap = new(() => new Dictionary<Guid, QueueProcessingMode>
	{
			{ new Guid("00000001-0000-0000-0000-000000000000"), NoAction_NewObject },
			{ new Guid("00000002-0000-0000-0000-000000000000"), Archivate_NewObject },
			{ new Guid("00000003-0000-0000-0000-000000000000"), Delete_NewObject }
	});

	public static IEnumerable<QueueProcessingMode> AsEnumerable_NewObjects()
	{
		yield return NoAction_NewObject;
		yield return Archivate_NewObject;
		yield return Delete_NewObject;
	}
}
