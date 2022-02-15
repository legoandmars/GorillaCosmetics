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
			Debug.Log($"Controller is null: {controller == null}");
			Debug.Log($"CurrentMaterial is null: {controller?.CurrentMaterial == null}");
			Debug.Log($"CustomColors is null: {controller?.CurrentMaterial?.Descriptor.CustomColors == null}");
			Debug.Log($"CustomColors is {controller?.CurrentMaterial?.Descriptor.CustomColors}");
			Debug.Log($"GorillaCosmetics: Continue with method? {boolean}");
			return boolean;
		}
	}
}

