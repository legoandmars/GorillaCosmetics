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
    [BepInPlugin("org.legoandmars.gorillatag.gorillacosmetics", "Gorilla Cosmetics", "3.0.2")]
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
    public class Plugin : BaseUnityPlugin
    {
		public static IAssetLoader AssetLoader { get; private set; }
        public static ISelectionManager SelectionManager { get; private set; }
        public static ICosmeticsNetworker CosmeticsNetworker { get; private set; }

        void Start()
        {
            Utilla.Events.GameInitialized += OnGameInitialized;

            GorillaCosmeticsPatches.ApplyHarmonyPatches();
		}

		private void OnGameInitialized(object sender, EventArgs e)
		{
			AssetLoader = new AssetLoader();
            SelectionManager = new SelectionManager();
		    CosmeticsNetworker = gameObject.AddComponent<CosmeticsNetworker>();
		}

        public static void Log(object message)
        {
# if DEBUG
            Debug.Log(message);
#endif
        }

# if DEBUG
        void OnGUI()
		{
            int y = 50;
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

            if (GUI.Button(new Rect(20, GetY(), 100, 20), "Button 1"))
			{
                PressButton(0);
			}
            if (GUI.Button(new Rect(20, GetY(), 100, 20), "Button 2"))
			{
                PressButton(1);
			}
            if (GUI.Button(new Rect(20, GetY(), 100, 20), "Button 3"))
			{
                PressButton(2);
			}

            void PressButton(int x)
			{
                SelectionManager sm = SelectionManager as SelectionManager;
                if (sm?.hatButtons?.Count > 0)
				{
					sm.hatButtons[x].ButtonActivation();
				} else
				{
					sm.materialButtons[x].ButtonActivation();
				}
			}
		}
#endif
    }
}
