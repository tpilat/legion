using Legion.ExpressionsSerializer.Interfaces;
using System.Reflection;
using System.Runtime.Serialization;

namespace Legion.ExpressionsSerializer.Nodes;

#region DataContract
#if !SERIALIZE_LINQ_OPTIMIZE_SIZE
[CollectionDataContract]
#else
    [CollectionDataContract(Name = "MIL")]
#endif
[Serializable]
#endregion
public class MemberInfoNodeList : List<MemberInfoNode>
{
	public MemberInfoNodeList() { }

	public MemberInfoNodeList(INodeFactory factory, IEnumerable<MemberInfo> items = null)
	{
		if (factory == null)
			throw new ArgumentNullException(nameof(factory));
		if (items != null)
			AddRange(items.Select(m => new MemberInfoNode(factory, m)));
	}

	public IEnumerable<MemberInfo> GetMembers(IExpressionContext context)
	{
		return this.Select(m => m.ToMemberInfo(context));
	}
}
