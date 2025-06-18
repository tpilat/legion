using Legion.Validation;

namespace Legion.ADF.Logs.Model;

public sealed partial class RemoteSystem : Logs.LogsBaseEntity, Legion.Model.IEntity
{
	private List<Logs.Model.LocalRequest> _localRequests;
	private List<Logs.Model.RemoteRequest> _remoteRequests;

	public static IValidator<RemoteSystem> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdRemoteSystem { get; private set; }

	/// <summary>
	/// Database DataType: varchar(127) NOT NULL
	/// </summary>
	public string Code { get; private set; }

	/// <summary>
	/// Database DataType: varchar(127) NOT NULL
	/// </summary>
	public string Name { get; private set; }


	/// <summary>
	/// N:_1 Logs.Model.LocalRequest.IdRemoteSystem | FK_LocalRequest_IdRemoteSystem
	/// </summary>
	public IReadOnlyList<Logs.Model.LocalRequest> LocalRequests => _localRequests;

	/// <summary>
	/// N:_1 Logs.Model.RemoteRequest.IdRemoteSystem | FK_RemoteRequest_IdRemoteSystem
	/// </summary>
	public IReadOnlyList<Logs.Model.RemoteRequest> RemoteRequests => _remoteRequests;

	private RemoteSystem()
	{
		_localRequests = [];
		_remoteRequests = [];
	}

	static RemoteSystem()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<RemoteSystem>()).Build();
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdRemoteSystem), IdRemoteSystem },
			{ nameof(Code), Code },
			{ nameof(Name), Name },
		};

	public void TrimStringValuesToFitDatabaseMaxLengths(string? postfix = null)
	{
		Code = Legion.Text.StringHelper.TrimToFitMaxLength(Code, 127, postfix);
		Name = Legion.Text.StringHelper.TrimToFitMaxLength(Name, 127, postfix);
	}

	public override string? GetPrimaryKeyValue()
	{
		return IdRemoteSystem.ToString();
	}

	public override string? ToString()
	{
		return IdRemoteSystem.ToString();
	}

	public static ValidatorBuilder<RemoteSystem> SetDBValidatorRules(ValidatorBuilder<RemoteSystem> builder)
		=> builder
			.ForProperty(x => x.IdRemoteSystem, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.Code, v => v.NotDefaultOrEmpty().MaxLength(127))
			.ForProperty(x => x.Name, v => v.NotDefaultOrEmpty().MaxLength(127))
		;
}
