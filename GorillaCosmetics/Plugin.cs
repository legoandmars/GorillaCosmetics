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
    // TODO: Update readme build instructions
    [BepInPlugin("org.legoandmars.gorillatag.gorillacosmetics", "Gorilla Cosmetics", "2.1.1")]
    // TODO: Add utilla as a dependency in mmm
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
    public class Plugin : BaseUnityPlugin
    {
		public static IAssetLoader AssetLoader { get; private set; }
        public static ISelectionManager SelectionManager { get; private set; }

        void Start()
        {
            Utilla.Events.GameInitialized += OnGameInitialized;

            GorillaCosmeticsPatches.ApplyHarmonyPatches();
		}

		private void OnGameInitialized(object sender, EventArgs e)
		{
			AssetLoader = new AssetLoader();
            SelectionManager = new SelectionManager();
            gameObject.AddComponent<CosmeticsNetworker>();
		}

# if DEBUG
        void OnGUI()
		{
            int y = 0;
            int GetY()
			{
                return y += 20;
			}

            if (GUI.Button(new Rect(20, GetY(), 100, 20), "Next Page"))
			{
                SelectionManager.NextPage();
			}
            if (GUI.Button(new Rect(20, GetY(), 100, 20), "Previous Page"))
			{
                SelectionManager.PreviousPage();
			}
            if (GUI.Button(new Rect(20, GetY(), 100, 20), "Hats"))
			{
                SelectionManager.SetView(ISelectionManager.SelectionView.Hat);
			}
            if (GUI.Button(new Rect(20, GetY(), 100, 20), "Materials"))
			{
                SelectionManager.SetView(ISelectionManager.SelectionView.Material);
			}
            if (GUI.Button(new Rect(20, GetY(), 100, 20), "Enable"))
			{
                SelectionManager.Enable();
			}
            if (GUI.Button(new Rect(20, GetY(), 100, 20), "Disable"))
			{
                SelectionManager.Disable();
			}
		}
#endif
    }
}