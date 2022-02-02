using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using GorillaNetworking;

namespace GorillaCosmetics.HarmonyPatches.Patches
{
	[HarmonyPatch(typeof(CosmeticsController))]
	[HarmonyPatch("PressWardrobeItemButton", MethodType.Normal)]
	internal class WardrobeButtonPressPatch
	{
		internal static void Postfix(CosmeticsController __instance, CosmeticsController.CosmeticItem cosmeticItem)
		{
			if (Plugin.SelectionManager.CurrentHat != null && cosmeticItem.itemSlot == "hat" && cosmeticItem.itemName != __instance.nullItem.itemName)
			{
				Plugin.SelectionManager.ResetHat();
			}
		}
	}
}
