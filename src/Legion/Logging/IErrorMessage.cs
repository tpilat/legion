namespace Legion.Logging;

#if NET6_0_OR_GREATER
[Legion.Serializer.JsonPolymorphicConverter]
#endif
public interface IErrorMessage : ILogMessage, Serializer.IDictionaryObject
{
	new ErrorMessageDto ToDto(params string[] ignoredPropterties);
	new ErrorMessageDto ToClientDto();
}
