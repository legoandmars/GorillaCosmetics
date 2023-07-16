using GorillaExtensions;
using HarmonyLib;
using Photon.Pun;
using UnityEngine;

namespace GorillaCosmetics.HarmonyPatches.Patches
{
	[HarmonyPatch]
	internal class CustomCosmeticsControllerPatches
	{
		internal static void InstantiateSetupPatch(MonoBehaviour __instance)
		{
			Photon.Realtime.Player SerializedPlayer = __instance.GetComponent<PhotonView>().Owner;
            VRRig SerializedRig = (VRRig)AccessTools.Field(__instance.GetType(), "vrrig").GetValue(__instance);

            Plugin.Log($"GorillaCosmetics: Creating CustomCosmeticsController for {SerializedPlayer?.NickName ?? "SELF"}");
			SerializedRig.gameObject.GetOrAddComponent<CustomCosmeticsController>().Player = __instance.GetComponent<PhotonView>().Owner;
        }

		internal static void CleanUpPatch(MonoBehaviour __instance, bool netDestroy)
		{
            VRRig SerializedRig = (VRRig)AccessTools.Field(__instance.GetType(), "vrrig").GetValue(__instance);
			if (!netDestroy || SerializedRig == null || (SerializedRig != null && SerializedRig.isOfflineVRRig)) return;

			if (SerializedRig.TryGetComponent(out CustomCosmeticsController SerializedController))
			{
				SerializedController.ResetHat();
				SerializedController.ResetMaterial();
				Object.Destroy(SerializedController);
            }
		}
	}
}
