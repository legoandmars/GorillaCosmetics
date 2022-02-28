using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using UnityEngine;

namespace GorillaCosmetics.HarmonyPatches.Patches
{
	[HarmonyPatch(typeof(VRRig))]
	[HarmonyPatch("Start", MethodType.Normal)]
	internal class CustomCosmeticsControllerCreationPatch
	{
		internal static void Postfix(VRRig __instance)
		{
			Photon.Realtime.Player player = __instance.photonView?.Owner;

			Plugin.Log($"GorillaCosmetics: Creating CustomCosmeticsController for {player?.NickName ?? "SELF"}");

			var tempMatArray = __instance.materialsToChangeTo;
			__instance.materialsToChangeTo = new Material[tempMatArray.Length + 1];

			for (int index = 0; index < tempMatArray.Length; index++) {
				__instance.materialsToChangeTo[index] = tempMatArray[index];
			}

			__instance.materialsToChangeTo[__instance.materialsToChangeTo.Length - 1] = tempMatArray[0];

			__instance.gameObject.AddComponent<CustomCosmeticsController>();
		}
	}
}
