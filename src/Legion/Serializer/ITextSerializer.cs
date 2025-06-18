using System.Text;

namespace Legion.Serializer;

public interface ITextSerializer
{
	void WriteTo(StringBuilder sb, string? before = null, string? after = null);
}
