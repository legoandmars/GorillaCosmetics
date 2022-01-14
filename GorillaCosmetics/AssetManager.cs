using GorillaCosmetics.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GorillaCosmetics
{
	public class AssetManager : IAssetManager
	{
        const string MaterialsLocation = "Materials";
        const string HatsLocation = "Hats";

		List<IAsset> assets;

        public AssetManager()
		{
            assets = GetAllAssets();
		}

		public T GetAsset<T>(string name) where T : IAsset
		{
            string formattedName = name.Trim().ToLower();
			foreach (IAsset asset in assets.Where(x => typeof(T).IsAssignableFrom(x.GetType())))
			{
                // TODO: Check to make sure this works with networking (it probally doesn't)
				if (asset.Descriptor.Name.Trim().ToLower() == formattedName)
				{
                    return (T)asset;
				}
			}

            return default;
		}

		public IEnumerable<T> GetAssets<T>() where T : IAsset
		{
            return (IEnumerable<T>)assets.Where(x => typeof(T).IsAssignableFrom(x.GetType()));
		}

        static List<IAsset> GetAllAssets()
		{
            string folder = Path.GetDirectoryName(typeof(GorillaCosmetics).Assembly.Location);

            // Load Materials
            IEnumerable<string> filter = new List<string> { "*.material", "*.gmat" };
            var materialFiles = GetFileNames($"{folder}\\{MaterialsLocation}", filter, SearchOption.TopDirectoryOnly, false);
            var gorillaMaterialObjects = LoadMaterials(materialFiles);

            // Load Hats
            IEnumerable<string> hatFilter = new List<string> { "*.hat", "*.ghat" };
            var hatFiles = GetFileNames($"{folder}\\{HatsLocation}", hatFilter, SearchOption.TopDirectoryOnly, false);
            var gorillaHatObjects = LoadHats(hatFiles);

            return (gorillaMaterialObjects as IEnumerable<IAsset>).Union(gorillaHatObjects).ToList();
		}

        static IEnumerable<GorillaMaterial> LoadMaterials(IEnumerable<string> materialFiles)
        {
            IList<GorillaMaterial> materials = new List<GorillaMaterial> { new GorillaMaterial("Default") };
            foreach (string materialFile in materialFiles)
            {
                try
                {
					materials.Add(new GorillaMaterial(materialFile));
                }
                catch (Exception ex)
                {
                    Debug.LogError("ERROR!");
                    Debug.LogError(ex);
                }
            }
            return materials;
        }

        static IEnumerable<GorillaHat> LoadHats(IEnumerable<string> hatFiles)
        {
            IList<GorillaHat> hats = new List<GorillaHat>(); // { new GorillaHat("Default") };
            foreach (string hatFile in hatFiles)
            {
                try
                {
					hats.Add(new GorillaHat(hatFile));
                }
                catch (Exception ex)
                {
                    Debug.LogError("ERROR!");
                    Debug.LogError(ex);
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
