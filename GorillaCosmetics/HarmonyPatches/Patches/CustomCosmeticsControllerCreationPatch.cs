using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;

namespace GorillaCosmetics.HarmonyPatches.Patches
{
	[HarmonyPatch(typeof(VRRig))]
	[HarmonyPatch("Start", MethodType.Normal)]
	internal class CustomCosmeticsControllerCreationPatch
	{
		internal static void Postfix(VRRig __instance)
		{
			__instance.gameObject.AddComponent<CustomCosmeticsController>();
		}
	}
}
