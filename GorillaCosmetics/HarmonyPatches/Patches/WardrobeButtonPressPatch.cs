using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using GorillaNetworking;
using GorillaCosmetics.Utils;

namespace GorillaCosmetics.HarmonyPatches.Patches
{
	[HarmonyPatch(typeof(CosmeticsController))]
	[HarmonyPatch("PressWardrobeItemButton", MethodType.Normal)]
	internal class WardrobeButtonPressPatch
	{
		internal static void Postfix(CosmeticsController __instance, CosmeticsController.CosmeticItem cosmeticItem)
		{
			if (CosmeticItemUtils.ContainsHat(cosmeticItem))
			{
				Plugin.SelectionManager.ResetHat();
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
			}
		}
	}

	// This doesn't account for purchasing cosmetics with shiny rocks.
	// That is too small of an edge case to bother dealing with.
}
