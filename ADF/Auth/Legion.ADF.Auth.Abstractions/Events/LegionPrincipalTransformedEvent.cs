using Legion.Model;

namespace Legion.ADF.Auth.Events;

public record LegionPrincipalTransformedEvent : DomainEventBase
{
	public Guid IdUser { get; }
	public string Login { get; }

	public LegionPrincipalTransformedEvent(Guid idUser, string login)
	{
		IdUser = idUser;
		Login = login;
	}
}