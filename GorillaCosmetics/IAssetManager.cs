using System;
using System.Collections.Generic;
using System.Text;

namespace GorillaCosmetics
{
	public interface IAssetManager
	{
		T GetAsset<T>(string name) where T : IAsset;
		IEnumerable<T> GetAssets<T>() where T : IAsset;
	}
}
