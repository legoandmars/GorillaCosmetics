using GorillaCosmetics.Data;
using System;
using GorillaCosmetics.Utils;
using HarmonyLib;

namespace GorillaCosmetics.HarmonyPatches.Patches
{
    /*[HarmonyPatch(typeof(VRRig))]
    [HarmonyPatch("ChangeMaterial", MethodType.Normal)]
    internal class VRRigChangeMaterialPatch
    {
        private static void Postfix(VRRig __instance, int materialIndex)
        {
            CosmeticUtils.ChangeMaterial(__instance, materialIndex);
        }
    }

    [HarmonyPatch(typeof(VRRig))]
    [HarmonyPatch("Start", MethodType.Normal)]
    internal class VRRigStartPatch
    {
        private static void Postfix(VRRig __instance)
        {
            CosmeticUtils.ChangeMaterial(__instance, __instance.setMatIndex);
        }
    }*/
}
