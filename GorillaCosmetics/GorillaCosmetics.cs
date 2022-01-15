using BepInEx;
using BepInEx.Configuration;
using GorillaCosmetics.HarmonyPatches;
using GorillaNetworking;
using HarmonyLib;
using System.IO;
using UnityEngine;

namespace GorillaCosmetics
{
    [BepInPlugin("org.legoandmars.gorillatag.gorillacosmetics", "Gorilla Cosmetics", "2.1.1")]
    public class GorillaCosmetics : BaseUnityPlugin
    {
		public static IAssetLoader AssetManager { get; private set; }
		public static ICosmeticManager CosmeticManager { get; private set; }

        public static ConfigEntry<string> selectedMaterial;
        public static ConfigEntry<string> selectedHat;
        void Start()
        {
            Debug.Log("Starting Gorilla Cosmetics");

            // Config
            var customFile = new ConfigFile(Path.Combine(Paths.ConfigPath, "GorillaCosmetics.cfg"), true);
            selectedMaterial = customFile.Bind("Cosmetics", "SelectedMaterial", "Rainbow", "What material to use from the BepInEx/plugins/GorillaCosmetics/Materials folder. Use Default for none");
            selectedHat = customFile.Bind("Cosmetics", "SelectedHat", "Top Hat", "What hat to use from the BepInEx/plugins/GorillaCosmetics/Hats folder. Use Default for none");

			AssetManager = new AssetLoader();

            // Harmony Patches
            GorillaCosmeticsPatches.ApplyHarmonyPatches();
		}
	}

	[HarmonyPatch(typeof(CosmeticsController))]
	[HarmonyPatch("Awake", MethodType.Normal)]
	internal class CosmeticsControllerPatch
	{

        public static void Prefix(CosmeticsController __instance)
		{
			CosmeticsController.CosmeticItem myItem = new CosmeticsController.CosmeticItem
			{
				itemName = "MOD_MYITEM.",
				displayName = "MOD MYITEM",
				canTryOn = true,
				itemSlot = "hat", // hat, badge face
			};

			Debug.LogWarning("Cosmetics Controller Initialization");

			GameObject template = GameObject.CreatePrimitive(PrimitiveType.Cube);
			template.name = myItem.displayName;
			AddCosmetic(__instance, myItem, template);

			Debug.LogWarning(__instance.allCosmetics.Count);
			Debug.LogWarning(__instance.allCosmeticsDict.Count);
			Debug.LogWarning(__instance.allCosmeticsItemIDsfromDisplayNamesDict.Count);

		}

		private static void AddCosmetic(CosmeticsController __instance, CosmeticsController.CosmeticItem item, GameObject template)
		{
			__instance.allCosmetics.Add(item);
			__instance.allCosmeticsDict.Add(item.itemName, item);
			__instance.allCosmeticsItemIDsfromDisplayNamesDict.Add(item.displayName, item.itemName);
			__instance.concatStringCosmeticsAllowed += item.itemName;

			__instance.unlockedCosmetics.Add(item);
			// TODO: hardcoded
			__instance.unlockedHats.Add(item);

			foreach (var wardrobe in __instance.wardrobes)
			{
				foreach(var button in wardrobe.wardrobeItemButtons)
				{
					AddItemToHeadController(template, button.controlledModel);
				}

				AddItemToHeadController(template, wardrobe.selfDoll);
			}
		}

		private static void AddItemToHeadController(GameObject template, HeadModel model)
		{
			GameObject newGameObject = GameObject.Instantiate(template);
			newGameObject.transform.SetParent(model.transform);
			newGameObject.transform.localPosition = Vector3.zero;
			newGameObject.transform.localRotation = Quaternion.identity;
			newGameObject.transform.localScale = Vector3.one;
		}
	}
}