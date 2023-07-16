using UnityEngine;
using HarmonyLib;
using GorillaNetworking;
using GorillaExtensions;

namespace GorillaCosmetics.HarmonyPatches.Patches
{
	[HarmonyPatch(typeof(VRRig))]
	[HarmonyPatch("SetColor", MethodType.Normal), HarmonyWrapSafe]
	internal class ColorPatch
	{
		internal static bool Prefix(VRRig __instance, Color color)
		{
			var controller = __instance.gameObject.GetOrAddComponent<CustomCosmeticsController>();
            controller.SetColor(color.r, color.g, color.b);

			Photon.Pun.PhotonView photView = (Photon.Pun.PhotonView)AccessTools.Field(__instance.GetType(), "photonView").GetValue(__instance);
			if (photView != null && !__instance.isOfflineVRRig) 
			{
				__instance.playerText.text = __instance.NormalizeName(true, photView.Owner.NickName);
			} 
			else if (__instance.showName)
			{
				__instance.playerText.text = PlayerPrefs.GetString("playerName");
			}

			return false;

			/*
			if (controller != null)
			{
				controller.SetColor(red, green, blue);
			}
			var boolean = controller?.CurrentMaterial?.Descriptor.CustomColors ?? true;
			return boolean;
			*/
		}
	}

	[HarmonyPatch(typeof(GorillaTagger))]
	[HarmonyPatch("UpdateColor", MethodType.Normal)]
	internal class UpdateColorPatch
	{
		internal static bool Prefix(GorillaTagger __instance, ref float red, ref float green, ref float blue)
		{
			__instance.offlineVRRig.InitializeNoobMaterialLocal(red, green, blue, GorillaComputer.instance?.leftHanded ?? false);
			__instance.offlineVRRig.ChangeMaterialLocal(0);
			return false;
		}
	}
}

