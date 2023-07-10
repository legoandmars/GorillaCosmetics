using GorillaCosmetics.Data;

namespace GorillaCosmetics
{
	public interface IAsset
	{
        string FileName { get; }
        CosmeticDescriptor Descriptor { get; }
	}
}