using GorillaCosmetics.Data;
using GorillaCosmetics.Data.Previews;
using GorillaCosmetics.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace GorillaCosmetics
{
    public static class AssetLoader
    {
        // there's way too much doubling in this class. make it more modular or something, this hurts to look at.
        static string MaterialsLocation = "Materials";
        static string HatsLocation = "Hats";

        public static bool Loaded = false;
        public static int selectedMaterial = 0;
        public static int selectedInfectedMaterial = 0; // TODO: better selection with UI
        public static int selectedHat = 0;

        public static IEnumerable<string> MaterialFiles { get; private set; } = Enumerable.Empty<string>();
        public static IList<GorillaMaterial> GorillaMaterialObjects { get; private set; }
        public static IEnumerable<string> HatFiles { get; private set; } = Enumerable.Empty<string>();
        public static IList<GorillaHat> GorillaHatObjects { get; private set; }
        public static GorillaMaterial DefaultTagMaterial { get; private set; }

        public static GorillaMaterial SelectedMaterial()
        {
            if (!Loaded) return null;
            return GorillaMaterialObjects[selectedMaterial];
        }
        public static GorillaMaterial SelectedInfectedMaterial()
        {
            if (!Loaded) return null;
            if (selectedInfectedMaterial == 0) return DefaultTagMaterial;
            return GorillaMaterialObjects[selectedInfectedMaterial];
        }
        public static GorillaHat SelectedHat()
        {
            if (!Loaded) return null;
            return GorillaHatObjects[selectedHat];
        }

        public static void Load()
        {
            if (Loaded) return;
            string folder = Path.GetDirectoryName(typeof(GorillaCosmetics).Assembly.Location);

            // Load Materials
            IEnumerable<string> filter = new List<string> { "*.material", "*.gmat" };
            MaterialFiles = GetFileNames($"{folder}\\{MaterialsLocation}", filter, SearchOption.TopDirectoryOnly, false);
            GorillaMaterialObjects = LoadMaterials(MaterialFiles);

            // Load Hats
            IEnumerable<string> hatFilter = new List<string> { "*.hat", "*.ghat" };
            HatFiles = GetFileNames($"{folder}\\{HatsLocation}", hatFilter, SearchOption.TopDirectoryOnly, false);
            GorillaHatObjects = LoadHats(HatFiles);

            // Parse Configs
            selectedMaterial = SelectedMaterialFromConfig(GorillaCosmetics.selectedMaterial.Value);
            selectedInfectedMaterial = SelectedMaterialFromConfig(GorillaCosmetics.selectedInfectedMaterial.Value);
            selectedHat = SelectedHatFromConfig();

            // Load Mirror
            GameObject Mirror = UnityEngine.Object.Instantiate(PackageUtils.AssetBundleFromPackage($"{folder}\\Misc\\Mirror").LoadAsset<GameObject>("_Hat"));
            Mirror.transform.localScale = new Vector3(0.29f, 0.29f, 0.29f);
            Mirror.transform.position = new Vector3(-68.5f, 11.96f, -81.595f);
            Mirror.transform.rotation = Quaternion.Euler(0.21f, -153.2f, -4.6f);
            UnityEngine.Object.DontDestroyOnLoad(Mirror);

            // Load Hat Rack
            GameObject HatRack = UnityEngine.Object.Instantiate(PackageUtils.AssetBundleFromPackage($"{folder}\\Misc\\HatRack").LoadAsset<GameObject>("_Hat"));
            HatRack.transform.localScale = new Vector3(3.696f, 3.696f, 0.677f);
            HatRack.transform.position = new Vector3(-68.003f, 11.471f, -80.637f);
            HatRack.transform.rotation = Quaternion.Euler(-90f, 0, 0);
            UnityEngine.Object.DontDestroyOnLoad(HatRack);

            // Check if we have enough hats for a second one
            GameObject HatRack2 = null;
            if(GorillaHatObjects.Count > 6)
            {
                HatRack2 = UnityEngine.Object.Instantiate(HatRack);
                HatRack2.transform.position = new Vector3(-67.895f, 11.511f, -80.41f);
                HatRack2.transform.rotation = Quaternion.Euler(-90f, 0, -68.608f);
                UnityEngine.Object.DontDestroyOnLoad(HatRack2);
            }

            // Load Material Previews
            float scale = (0.8f/GorillaMaterialObjects.Count);
            for (int i = 0; i < GorillaMaterialObjects.Count; i++)
            {
                var material = GorillaMaterialObjects[i];
                Vector3 pos = new Vector3(-68.287f, 12.04f - (scale*i), -81.251f);
                new MaterialPreview(material, pos, scale*0.85f);
            }

            // Load Hat Rack Previews
            Collider[] hatPosColliders = HatRack.transform.GetComponentsInChildren<Collider>();

            System.Random random = new System.Random();
            Collider[] RandomColliderArray = hatPosColliders.OrderBy(x => random.Next()).ToArray();

            for (int i = 0; i < Math.Min(GorillaHatObjects.Count, 6); i++)
            {
                var hat = GorillaHatObjects[i];
                new HatPreview(hat, RandomColliderArray[i]);
            }

            // Load Hat Rack Preview Again, if needed
            if(HatRack2 != null)
            {
                Collider[] hatPosColliders2 = HatRack2.transform.GetComponentsInChildren<Collider>();

                Collider[] RandomColliderArray2 = hatPosColliders2.OrderBy(x => random.Next()).ToArray();

                for (int i = 6; i < Math.Min(GorillaHatObjects.Count, 12); i++)
                {
                    var hat = GorillaHatObjects[i];
                    new HatPreview(hat, RandomColliderArray2[i-6]);
                }
            }

            // Load lava skin as a backup
            Material lavaMat = CosmeticUtils.GetMaterials().First(mat => mat.name == "infected");
            DefaultTagMaterial = new GorillaMaterial("Default");
            DefaultTagMaterial.Material = null;
            DefaultTagMaterial.Descriptor.MaterialName = "Lava";
            DefaultTagMaterial.Descriptor.CustomColors = false;
            if (lavaMat != null) DefaultTagMaterial.Material = lavaMat;

            Loaded = true;
        }

        public static int SelectedMaterialFromConfig(string configString)
        {
            string selectedMatString = configString.ToLower().Trim();
            for (int i = 1; i < GorillaMaterialObjects.Count; i++)
            {
                GorillaMaterial gorillaMaterialObject = GorillaMaterialObjects[i];
                if (gorillaMaterialObject == null) return 0;
                if (gorillaMaterialObject.Descriptor.MaterialName.ToLower().Trim() == selectedMatString)
                {
                    return i;
                }
                else if (Path.GetFileNameWithoutExtension(gorillaMaterialObject.FileName).ToLower().Trim() == selectedMatString)
                {
                    return i;
                }
            }
            return 0;
        }

        public static int SelectedHatFromConfig()
        {
            string selectedHatString = GorillaCosmetics.selectedHat.Value.ToLower().Trim();
            for (int i = 1; i < GorillaHatObjects.Count; i++)
            {
                GorillaHat gorillaHatObject = GorillaHatObjects[i];
                if (gorillaHatObject == null) return 0;
                if (gorillaHatObject.Descriptor.HatName.ToLower().Trim() == selectedHatString)
                {
                    return i;
                }
                else if (Path.GetFileNameWithoutExtension(gorillaHatObject.FileName).ToLower().Trim() == selectedHatString)
                {
                    return i;
                }
            }
            return 0;
        }

        public static IList<GorillaMaterial> LoadMaterials(IEnumerable<string> materialFiles)
        {
            IList<GorillaMaterial> materials = new List<GorillaMaterial> { new GorillaMaterial("Default") };
            foreach (string materialFile in materialFiles)
            {
                try
                {
                    GorillaMaterial material = new GorillaMaterial(materialFile);
                    materials.Add(material);
                }
                catch (Exception ex)
                {
                    Debug.Log("ERROR!");
                    Debug.Log(ex);
                }
            }
            return materials;
        }

        public static IList<GorillaHat> LoadHats(IEnumerable<string> hatFiles)
        {
            IList<GorillaHat> hats = new List<GorillaHat> { new GorillaHat("Default") };
            foreach (string hatFile in hatFiles)
            {
                try
                {
                    GorillaHat hat = new GorillaHat(hatFile);
                    hats.Add(hat);
                }
                catch (Exception ex)
                {
                    Debug.Log("ERROR!");
                    Debug.Log(ex);
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
