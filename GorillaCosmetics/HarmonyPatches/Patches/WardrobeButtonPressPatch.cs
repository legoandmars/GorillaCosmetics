using HarmonyLib;
using GorillaNetworking;
using GorillaCosmetics.Utils;
using GorillaCosmetics.UI;

namespace GorillaCosmetics.HarmonyPatches.Patches
{
	[HarmonyPatch(typeof(CosmeticsController))]
	[HarmonyPatch("PressWardrobeItemButton", MethodType.Normal)]
	internal class WardrobeItemButtonPatch
	{
		internal static void Postfix(CosmeticsController __instance, CosmeticsController.CosmeticItem cosmeticItem)
		{
			if (CosmeticItemUtils.ContainsHat(cosmeticItem))
			{
				Plugin.SelectionManager.ResetHat();
				__instance.UpdateShoppingCart();
			}
		}
	}

	[HarmonyPatch(typeof(CosmeticsController))]
	[HarmonyPatch("PressFittingRoomButton", MethodType.Normal)]
	internal class FittingRoomButtonPressPatch
	{
		internal static void Postfix(CosmeticsController __instance, FittingRoomButton pressedFittingRoomButton)
		{
			if (CosmeticItemUtils.ContainsHat(pressedFittingRoomButton.currentCosmeticItem))
			{
				Plugin.SelectionManager.ResetHat();
                __instance.UpdateShoppingCart();
            }
		}
	}

	[HarmonyPatch(typeof(CosmeticsController))]
	[HarmonyPatch("PressWardrobeFunctionButton", MethodType.Normal)]
	internal class WardrobeFunctionButtonPatch
	{
		internal static bool Prefix(string function)
			=> !(ToggleEnableButton.isEnabled && (function == "left" || function == "right"));
	}

	// This doesn't account for purchasing cosmetics with shiny rocks.
	// That is too small of an edge case to bother dealing with.
}
