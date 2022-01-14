using GorillaCosmetics.Data;
using UnityEngine;

namespace GorillaCosmetics
{
	public interface IAsset
	{
        string FileName { get; }
        CosmeticDescriptor Descriptor { get; }
	}
}