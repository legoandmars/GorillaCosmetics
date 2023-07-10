using GorillaCosmetics.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace GorillaCosmetics
{
	public class AssetLoader : IAssetLoader
	{
		const string MaterialsLocation = "Materials";
		const string HatsLocation = "Hats";

		//List<IAsset> assets;
		Dictionary<Type, List<IAsset>> assets;

		public AssetLoader()
		{
			assets = GetAllAssets();
		}

		public T GetAsset<T>(string name) where T : IAsset
		{
			if (name == null)
			{
				return default;
			}

			string formattedName = name.Trim().ToLower();

			if (assets.TryGetValue(typeof(T), out var assetList))
			{
				foreach(IAsset asset in assetList)
				{
					if (asset.Descriptor.Name.Trim().ToLower() == formattedName)
					{
						return (T)asset;
					}
				}
			}

			return default;
		}

		public List<T> GetAssets<T>() where T : IAsset
		{
			if (assets.TryGetValue(typeof(T), out var assetList))
			{
				List<T> list = new();
				foreach(var asset in assetList)
				{
					list.Add((T)asset);
				}
				return list;
			} else
			{
				return default;
			}
		}

		static Dictionary<Type, List<IAsset>> GetAllAssets()
		{
			Dictionary<Type, List<IAsset>> newAssets = new();
			string folder = Path.GetDirectoryName(typeof(Plugin).Assembly.Location);

			// Load Materials
			IEnumerable<string> filter = new List<string> { "*.material", "*.gmat" };
			var materialFiles = GetFileNames($"{folder}\\{MaterialsLocation}", filter, SearchOption.TopDirectoryOnly, false);
			var gorillaMaterialObjects = (IEnumerable<IAsset>)LoadMaterials(materialFiles);
			newAssets.Add(typeof(GorillaMaterial), gorillaMaterialObjects.ToList());

			// Load Hats
			IEnumerable<string> hatFilter = new List<string> { "*.hat", "*.ghat" };
			var hatFiles = GetFileNames($"{folder}\\{HatsLocation}", hatFilter, SearchOption.TopDirectoryOnly, false);
			var gorillaHatObjects = (IEnumerable<IAsset>)LoadHats(hatFiles);
			newAssets.Add(typeof(GorillaHat), gorillaHatObjects.ToList());

			return newAssets;
		}

		static IEnumerable<GorillaMaterial> LoadMaterials(IEnumerable<string> materialFiles)
		{
			List<GorillaMaterial> materials = new();
			foreach (string materialFile in materialFiles)
			{
				try
				{
					materials.Add(new GorillaMaterial(materialFile));
				}
				catch
				{
					File.Move(materialFile, $"{materialFile}.broken");
					Debug.LogWarning($"Removed broken cosmetic: {materialFile}");
				}
			}
			return materials;
		}

		static IEnumerable<GorillaHat> LoadHats(IEnumerable<string> hatFiles)
		{
			List<GorillaHat> hats = new();
			foreach (string hatFile in hatFiles)
			{
				try
				{
					hats.Add(new GorillaHat(hatFile));
				}
				catch
				{
					File.Move(hatFile, $"{hatFile}.broken");
					Debug.LogWarning($"Removed broken cosmetic: {hatFile}");
				}
			}
			return hats;
		}

		/// <summary>
		/// Gets every file matching the filter in a path.
		/// </summary>
		/// <param name="path">Directory to search in.</param>
		/// <param name="filters">Pattern(s) to search for.</param>
		/// <param name="searchOption">Search options.</param>
		/// <param name="returnShortPath">Remove path from filepaths.</param>
		public static IEnumerable<string> GetFileNames(string path, IEnumerable<string> filters, SearchOption searchOption, bool returnShortPath = false)
		{
			IList<string> filePaths = new List<string>();

			foreach (string filter in filters)
			{
				IEnumerable<string> directoryFiles = Directory.GetFiles(path, filter, searchOption);

				if (returnShortPath)
				{
					foreach (string directoryFile in directoryFiles)
					{
						string filePath = directoryFile.Replace(path, "");
						if (filePath.Length > 0 && filePath.StartsWith(@"\"))
						{
							filePath = filePath.Substring(1, filePath.Length - 1);
						}

						if (!string.IsNullOrWhiteSpace(filePath) && !filePaths.Contains(filePath))
						{
							filePaths.Add(filePath);
						}
					}
				}
				else
				{
					filePaths = filePaths.Union(directoryFiles).ToList();
				}
			}

			return filePaths.Distinct();
		}
	}
}
