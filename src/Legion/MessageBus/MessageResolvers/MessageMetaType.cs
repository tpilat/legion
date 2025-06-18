namespace Legion.MessageBus.MessageResolvers;

public enum MessageMetaType
{
	RequestMessage_WithResponse = 1,
	RequestMessage_Void = 2,
	Event = 3,
	Command_WithResponse = 4,
	Command_Void = 5,
	Query_WithResponse = 6,
	Response_ForRequestMessage = 7,
	Response_ForCommand = 8,
	Response_ForQuery = 9,
}

public static class MessageMetaTypeExtensions
{
	public static bool IsRequest(this MessageMetaType messageMetaType)
		=> messageMetaType == MessageMetaType.RequestMessage_WithResponse
			|| messageMetaType == MessageMetaType.RequestMessage_Void
			|| messageMetaType == MessageMetaType.Event
			|| messageMetaType == MessageMetaType.Command_WithResponse
			|| messageMetaType == MessageMetaType.Command_Void
			|| messageMetaType == MessageMetaType.Query_WithResponse;

	public static bool IsResponse(this MessageMetaType messageMetaType)
		=> messageMetaType == MessageMetaType.Response_ForRequestMessage
			|| messageMetaType == MessageMetaType.Response_ForCommand
			|| messageMetaType == MessageMetaType.Response_ForQuery;
}
