using GorillaCosmetics.Data;
using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using UnityEngine;
using GorillaCosmetics.Utils;

namespace GorillaCosmetics.HarmonyPatches.Patches
{
    [HarmonyPatch(typeof(VRRig))]
    [HarmonyPatch("Start", MethodType.Normal)]
    internal class GorillaHatAwakePatch2
    {
        private static void Postfix(VRRig __instance)
        {
            CosmeticUtils.ChangeHat(__instance);
        }
    }
}
