using System;
using System.Collections.Generic;
using System.Text;

namespace GorillaCosmetics
{
	public interface IAssetLoader
	{
		T GetAsset<T>(string name) where T : IAsset;
		List<T> GetAssets<T>() where T : IAsset;
	}
}
