using HarmonyLib;

namespace GorillaCosmetics.HarmonyPatches.Patches
{
	[HarmonyPatch(typeof(VRRig))]
	[HarmonyPatch("ChangeMaterialLocal", MethodType.Normal)]
	internal class ChangeMaterialPatch
	{
		internal static void Prefix(VRRig __instance, out bool __state, ref int materialIndex)
		{
			bool reset = false;
			var controller = __instance.gameObject.GetComponent<ICustomCosmeticsController>();
			if (controller != null && materialIndex == 0) 
			{
				reset = true;
				materialIndex = controller.MatIndex;
			}

			__state = reset;
		}

		internal static void Postfix(VRRig __instance, in bool __state)
		{
			if(__state) 
			{
				__instance.setMatIndex = 0;
			}
		}
	}
}
