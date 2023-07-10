using System.Collections.Generic;

namespace GorillaCosmetics
{
	public interface IAssetLoader
	{
		T GetAsset<T>(string name) where T : IAsset;
		List<T> GetAssets<T>() where T : IAsset;
	}
}
