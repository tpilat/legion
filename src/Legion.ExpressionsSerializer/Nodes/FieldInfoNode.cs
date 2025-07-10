using Legion.ExpressionsSerializer.Interfaces;
using System.Reflection;
using System.Runtime.Serialization;

namespace Legion.ExpressionsSerializer.Nodes;

#region DataContract
#if !SERIALIZE_LINQ_OPTIMIZE_SIZE
[DataContract]
#else
    [DataContract(Name = "FI")]
#endif
[Serializable]
#endregion
public class FieldInfoNode : MemberNode<FieldInfo>
{
	public FieldInfoNode() { }

	public FieldInfoNode(INodeFactory factory, FieldInfo memberInfo)
		: base(factory, memberInfo) { }

	protected override IEnumerable<FieldInfo> GetMemberInfosForType(IExpressionContext context, Type type)
	{
		return type.GetFields();
	}
}
