using Legion.Exceptions;

namespace Legion.ADF.ESB.Components.Exceptions.Internal;

public static partial class ErrorCodes
{
	public static partial class AdapterException
	{
		public static IErrorCode NoInboundNoOutbound => _noInboundNoOutbound.Value;
		private static readonly Lazy<IErrorCode> _noInboundNoOutbound = new(() =>
			new ErrorCode(
				"ADFESB_COMP_ADAP_0001",
				"The adapter must be at least inbound or outbound."));
	}
}
