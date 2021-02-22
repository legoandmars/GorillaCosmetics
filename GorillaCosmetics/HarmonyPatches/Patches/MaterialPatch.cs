using GorillaCosmetics.Data;
using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using UnityEngine;
using Photon.Pun;

namespace GorillaCosmetics.HarmonyPatches.Patches
{
    internal static class MaterialPatchUtils
    {
        internal static void ChangeMaterial(VRRig __instance, int materialIndex)
        {
            if (materialIndex == 0 && (__instance.isOfflineVRRig || GorillaCosmetics.applyToOtherPlayers.Value))
            {
                // default mat
                GorillaMaterial material = AssetLoader.SelectedMaterial();
                if (material != null && material.Material != null)
                {
                    Material instantiatedMat = UnityEngine.Object.Instantiate(material.Material);
                    if (material.Descriptor.CustomColors) instantiatedMat.color = __instance.mainSkin.material.color;
                    __instance.mainSkin.material = instantiatedMat;
                    try
                    {
                        Debug.Log($"Changed the skin of {__instance.photonView.Owner.UserId}");
                    }
                    catch
                    {
                        Debug.Log("Couldn't find name of user.");
                    }
                }
            }
            else if (materialIndex > 0 && (__instance.isOfflineVRRig || GorillaCosmetics.applyInfectedToOtherPlayers.Value))
            {
                GorillaMaterial material = AssetLoader.SelectedInfectedMaterial();
                if (material != null && material.Material != null)
                {
                    Material instantiatedMat = UnityEngine.Object.Instantiate(material.Material);
                    if (material.Descriptor.CustomColors) instantiatedMat.color = __instance.mainSkin.material.color;
                    __instance.mainSkin.material = instantiatedMat;
                }
            }
        }
    }

    [HarmonyPatch(typeof(VRRig))]
    [HarmonyPatch("ChangeMaterial", MethodType.Normal)]
    internal class VRRigChangeMaterialPatch
    {
        private static void Postfix(VRRig __instance, int materialIndex)
        {
            MaterialPatchUtils.ChangeMaterial(__instance, materialIndex);
        }
    }

    [HarmonyPatch(typeof(VRRig))]
    [HarmonyPatch("Start", MethodType.Normal)]
    internal class VRRigStartPatch
    {
        private static void Postfix(VRRig __instance)
        {
            if (!__instance.isOfflineVRRig)
            {
                if (__instance.photonView != null)
                {
                    MaterialPatchUtils.ChangeMaterial(__instance, __instance.setMatIndex);
                }
                else if(__instance.setMatIndex != null)
                {
                    MaterialPatchUtils.ChangeMaterial(__instance, __instance.setMatIndex);
                }
            }
        }
    }
}
