using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GorillaExtensions;
using HarmonyLib;
using UnityEngine;

namespace GorillaCosmetics.HarmonyPatches.Patches
{
	[HarmonyPatch(typeof(VRRig))]
	[HarmonyPatch("Start", MethodType.Normal)]
	internal class CustomCosmeticsControllerCreationPatch
	{
		internal static async void Postfix(VRRig __instance)
		{
			await Task.Delay(400);
			Photon.Realtime.Player player = AccessTools.Field(__instance.GetType(), "creator").GetValue(__instance) as Photon.Realtime.Player;

			Plugin.Log($"GorillaCosmetics: Creating CustomCosmeticsController for {player?.NickName ?? "SELF"}");
			__instance.gameObject.GetOrAddComponent<CustomCosmeticsController>();
		}
	}
}
