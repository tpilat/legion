//using Legion.Model;

//namespace Legion.ADF.Auth.Events;

//public record ClaimsTransformationFailedEvent : DomainEventBase
//{
//	public string? IdUser { get; }
//	public string Reason { get; }

//	public ClaimsTransformationFailedEvent(string? idUser, string reason)
//	{
//		Throw.IfArgumentNullOrWhiteSpace(reason);

//		IdUser = idUser;
//		Reason = reason;
//	}
//}