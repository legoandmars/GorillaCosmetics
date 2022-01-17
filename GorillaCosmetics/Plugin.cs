using BepInEx;
using BepInEx.Configuration;
using GorillaCosmetics.HarmonyPatches;
using GorillaCosmetics.UI;
using GorillaNetworking;
using HarmonyLib;
using System;
using System.IO;
using UnityEngine;

namespace GorillaCosmetics
{
    [BepInPlugin("org.legoandmars.gorillatag.gorillacosmetics", "Gorilla Cosmetics", "2.1.1")]
    // TODO: Add utilla as a dependency in mmm
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
    public class Plugin : BaseUnityPlugin
    {
		public static IAssetLoader AssetLoader { get; private set; }
        public static ISelectionManager SelectionManager { get; private set; }

        // TODO: Remove?
        public static ConfigEntry<string> selectedMaterial;
        public static ConfigEntry<string> selectedHat;

        void Start()
        {
            Debug.Log("Starting Gorilla Cosmetics");

            // Config
            var customFile = new ConfigFile(Path.Combine(Paths.ConfigPath, "GorillaCosmetics.cfg"), true);
            selectedMaterial = customFile.Bind("Cosmetics", "SelectedMaterial", "Rainbow", "What material to use from the BepInEx/plugins/GorillaCosmetics/Materials folder. Use Default for none");
            selectedHat = customFile.Bind("Cosmetics", "SelectedHat", "Top Hat", "What hat to use from the BepInEx/plugins/GorillaCosmetics/Hats folder. Use Default for none");

            Utilla.Events.GameInitialized += OnGameInitialized;

            // Harmony Patches
            GorillaCosmeticsPatches.ApplyHarmonyPatches();
		}

		private void OnGameInitialized(object sender, EventArgs e)
		{
			AssetLoader = new AssetLoader();
            SelectionManager = new SelectionManager();
            gameObject.AddComponent<CosmeticsNetworker>();
		}
	}
}