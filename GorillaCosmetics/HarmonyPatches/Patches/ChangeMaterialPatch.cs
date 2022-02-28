using HarmonyLib;

namespace GorillaCosmetics.HarmonyPatches.Patches
{
	[HarmonyPatch(typeof(VRRig))]
	[HarmonyPatch("ChangeMaterialLocal", MethodType.Normal)]
	internal class ChangeMaterialPatch
	{
		internal static void Prefix(VRRig __instance, ref int materialIndex)
		{
			var controller = __instance.gameObject.GetComponent<ICustomCosmeticsController>();
			if (controller != null && materialIndex == 0) 
			{
				materialIndex = controller.MatIndex;
			}
		}
	}
}
