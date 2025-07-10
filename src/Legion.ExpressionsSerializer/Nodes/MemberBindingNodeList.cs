using Legion.ExpressionsSerializer.Interfaces;
using System.Linq.Expressions;
using System.Runtime.Serialization;

namespace Legion.ExpressionsSerializer.Nodes;

#region CollectionDataContract
#if !SERIALIZE_LINQ_OPTIMIZE_SIZE
[CollectionDataContract]
#else
    [CollectionDataContract(Name = "MBL")]    
#endif
[Serializable]
#endregion
public class MemberBindingNodeList : List<MemberBindingNode>
{
	public MemberBindingNodeList() { }

	public MemberBindingNodeList(INodeFactory factory, IEnumerable<MemberBinding> items)
	{
		if (factory == null)
			throw new ArgumentNullException(nameof(factory));
		if (items == null)
			throw new ArgumentNullException(nameof(items));
		AddRange(items.Select(m => MemberBindingNode.Create(factory, m)));
	}

	internal IEnumerable<MemberBinding> GetMemberBindings(IExpressionContext context)
	{
		return this.Select(memberBindingEntity => memberBindingEntity.ToMemberBinding(context));
	}
}
