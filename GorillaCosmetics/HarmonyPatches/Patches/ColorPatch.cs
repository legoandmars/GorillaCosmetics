using System;
using UnityEngine;
using HarmonyLib;

namespace GorillaCosmetics.HarmonyPatches.Patches
{
	[HarmonyPatch(typeof(VRRig))]
	[HarmonyPatch("InitializeNoobMaterialLocal", MethodType.Normal)]
	internal class ColorPatch
	{
		internal static bool Prefix(VRRig __instance, float red, float green, float blue)
		{
			var controller = __instance.gameObject.GetComponent<ICustomCosmeticsController>();
			if (controller != null)
			{
				controller.SetColor(red, green, blue);
			}
			var boolean = controller?.CurrentMaterial?.Descriptor.CustomColors ?? true;
			return boolean;
		}
	}
}

