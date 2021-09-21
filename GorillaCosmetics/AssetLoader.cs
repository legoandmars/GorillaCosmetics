using GorillaCosmetics.Data;
using GorillaCosmetics.Data.Selectors;
using GorillaCosmetics.Data.Previews;
using GorillaCosmetics.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using System.Threading.Tasks;

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

        public static void SelectMaterial(string name)
        {
            selectedMaterial = SelectedMaterialFromConfig(name);
        }
        public static void SelectHat(string name)
        {
            selectedHat = SelectedHatFromConfig(name);
        }

        public static GorillaMaterial GetMaterial(int index)
        {
            if (index > GorillaMaterialObjects.Count) return null;
            return GorillaMaterialObjects[index];
        }

        public static GorillaHat GetHat(int index)
        {
            if (index > GorillaHatObjects.Count) return null;
            return GorillaHatObjects[index];
        }

        public async static void Load()
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
            selectedHat = SelectedHatFromConfig(GorillaCosmetics.selectedHat.Value);

            // Disable old mirror and use it as a base
            GameObject gameMirror = null;
            do {
                gameMirror = GameObject.Find("Level/cosmeticsroom/cosmetics room/shoppingcenter/mirrors2 (1)");
                await Task.Delay(250);
            } while (gameMirror == null);

            //gameMirror.SetActive(false);

            // Load Mirror
            GameObject Mirror = UnityEngine.Object.Instantiate(PackageUtils.AssetBundleFromPackage($"{folder}\\Misc\\Mirror").LoadAsset<GameObject>("_Hat"));
            Mirror.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
            Mirror.transform.position = gameMirror.transform.position + new Vector3(1.3f, 0.55f, 0.2f);
            Mirror.transform.rotation = Quaternion.Euler(0, 64, 0);
            Mirror.transform.parent = gameMirror.transform;
            // Hide the mirror in favor of the in game one
            Mirror.transform.Find("mirror").gameObject.SetActive(false);
            UnityEngine.Object.DontDestroyOnLoad(Mirror);

            // Load Hat Rack
            GameObject HatRack = UnityEngine.Object.Instantiate(PackageUtils.AssetBundleFromPackage($"{folder}\\Misc\\HatRack").LoadAsset<GameObject>("_Hat"));
            HatRack.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
            HatRack.transform.position = gameMirror.transform.position + new Vector3(2.1f, -0.11f, 0.9f);
            HatRack.transform.rotation = Quaternion.Euler(0, 149.65f, 0);
            HatRack.transform.parent = gameMirror.transform;
            UnityEngine.Object.DontDestroyOnLoad(HatRack);
            // how many hats
            int hatCount = GorillaHatObjects.Count;
            // how many hats are on the last rack
            int lastRackCount = hatCount % 6;
            // how many racks are needed for that amount of hats
            int rackCount = (hatCount / 6) + (lastRackCount == 0 ? 0 : 1);

            Debug.Log($"Hat count: {hatCount}, rack count: {rackCount}, last rack hat count: {lastRackCount}");

            // the actual rack transform, so this contains the 6 hats
            // mesh is now seperate, so only the colliders are being switched out
            Transform actualRackTransform = HatRack.transform.Find("HatRack");
            Transform selectionTransform = HatRack.transform.Find("Selection");
            GameObject actualRack = actualRackTransform.gameObject;
            // add rack selector
            HatRackSelector rackSelector = HatRack.AddComponent<HatRackSelector>();
            if(rackCount > 1)
            {
                Collider[] buttonColliders = selectionTransform.gameObject.GetComponentsInChildren<Collider>(true);

                for (int i = 0; i < buttonColliders.Length; i++)
                {
                    Collider collider = buttonColliders[i];
                    HatRackSelectorButton button = collider.gameObject.AddComponent<HatRackSelectorButton>();

                    // what selector does it apply to?
                    button.selector = rackSelector;
                    // should be trigger
                    collider.isTrigger = true;
                    // correct layer for buttons
                    collider.gameObject.layer = 18;
                }

            }
            else
            {
                UnityEngine.Object.Destroy(selectionTransform.gameObject);
            }

            for (int i = 0; i < rackCount; i++)
            {
                int hatsLeft = hatCount - (i * 6);
                if (hatsLeft > 6) // if not the last rack
                {
                    GameObject theRack = UnityEngine.Object.Instantiate(actualRack);
                    UnityEngine.Object.DontDestroyOnLoad(theRack);
                    Transform theRackTransform = theRack.transform;
                    theRack.transform.SetParent(HatRack.transform, false);

                    // add to the rack selector list of racks
                    rackSelector.racks.Add(theRack);

                    Collider[] hatPosColliders = theRack.GetComponentsInChildren<Collider>();

                    // randomize order

                    var index = new List<int>();
                    for (int k = 0; k < 6; k++) index.Add(k);
                    var hatRackRandom = new System.Random();
                    index = index.OrderBy(x => hatRackRandom.Next()).ToList();

                    // create previews for the current 6 hats
                    for (int j = 0; j < 6; j++)
                    {
                        GorillaHat hat = GorillaHatObjects[hatsLeft - index[j] - 1];
                        Collider collider = hatPosColliders[j];
                        new HatPreview(hat, collider);
                    }
                }
                else // if the last one (may or may not be full)
                {
                    // add to rack list
                    rackSelector.racks.Add(actualRack);
                    Collider[] hatPosColliders = actualRack.GetComponentsInChildren<Collider>();

                    // randomize order
                    var index = new List<int>();
                    for (int k = 0; k < hatsLeft; k++) index.Add(k);
                    var hatRackRandom = new System.Random();
                    index = index.OrderBy(x => hatRackRandom.Next()).ToList();

                    // create previews
                    for (int j = 0; j < hatsLeft; j++)
                    {
                        GorillaHat hat = GorillaHatObjects[index[j]];
                        Collider collider = hatPosColliders[j];
                        new HatPreview(hat, collider);
                    }
                }
            }
            rackSelector.UpdateRack();

            // Load Material Previews
            int materialCount = GorillaMaterialObjects.Count;
            int lastMaterialCount = materialCount % 10;
            int materialPageCount = (materialCount / 10) + (lastMaterialCount == 0 ? 0 : 1);

            Transform materialSelectionTransform = Mirror.transform.Find("Selection");
            Transform previewTransform = Mirror.transform.Find("Preview");

            HatRackSelector matSelector = Mirror.AddComponent<HatRackSelector>();
            if (materialPageCount > 1)
            {
                Collider[] buttonColliders = materialSelectionTransform.gameObject.GetComponentsInChildren<Collider>(true);

                for (int i = 0; i < buttonColliders.Length; i++)
                {
                    Collider collider = buttonColliders[i];
                    HatRackSelectorButton button = collider.gameObject.AddComponent<HatRackSelectorButton>();

                    // what selector does it apply to?
                    button.selector = matSelector;
                    // should be trigger
                    collider.isTrigger = true;
                    // correct layer for buttons
                    collider.gameObject.layer = 18;
                }

            }
            else
            {
                UnityEngine.Object.Destroy(materialSelectionTransform.gameObject);
            }

            for (int i = 0; i < materialPageCount; i++)
            {
                int materialsLeft = materialCount - (i * 10);
                if (materialsLeft > 10) // if not the last rack
                {
                    GameObject thePage = UnityEngine.Object.Instantiate(previewTransform.gameObject);
                    UnityEngine.Object.DontDestroyOnLoad(thePage);
                    Transform thePageTransform = thePage.transform;
                    thePage.transform.SetParent(Mirror.transform, false);

                    // add to the rack selector list of racks
                    matSelector.racks.Add(thePage);

                    float scale = 0.21f;
                    for (int j = 0; j < 10; j++)
                    {
                        int matIndex = materialsLeft - j - 1;
                        GorillaMaterial material = GorillaMaterialObjects[matIndex];
                        float height = (-0.5f * scale) - (scale * j) - 0.05f;
                        Vector3 pos = new Vector3(0.0f, height, 0.0f);
                        new MaterialPreview(material, thePageTransform, pos, scale * 0.85f);
                    }
                }
                else // if the last one (may or may not be full)
                {
                    // create previews with a scale of at most 2.1f / 6, or till 0.21f
                    float scale = 2.1f / (materialsLeft > 6 ? materialsLeft : 6);
                    matSelector.racks.Add(previewTransform.gameObject);

                    for (int j = 0; j < materialsLeft; j++)
                    {
                        int matIndex = j;
                        GorillaMaterial material = GorillaMaterialObjects[matIndex];
                        float height = (-0.5f * scale) - (scale * j) - 0.05f;
                        Vector3 pos = new Vector3(0.0f, height, 0.0f);
                        new MaterialPreview(material, previewTransform, pos, scale * 0.85f);
                    }
                }
            }
            matSelector.UpdateRack();

            // Load lava skin as a backup
            Material lavaMat = CosmeticUtils.GetMaterials().First(mat => mat.name == "infected");
            DefaultTagMaterial = new GorillaMaterial("Default");
            DefaultTagMaterial.Material = null;
            DefaultTagMaterial.Descriptor.MaterialName = "Lava";
            DefaultTagMaterial.Descriptor.CustomColors = false;
            if (lavaMat != null) DefaultTagMaterial.Material = lavaMat;

            Loaded = true;
            CosmeticUtils.LocalLoadingCallback();
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

        public static int SelectedHatFromConfig(string configString)
        {
            string selectedHatString = configString.ToLower().Trim();
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
