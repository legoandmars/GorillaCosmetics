using GorillaCosmetics.Data;
using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using UnityEngine;

namespace GorillaCosmetics.HarmonyPatches.Patches
{
    /* 
    // still going to comment this out in case I need it, idk why I was using the collider for position
    [HarmonyPatch(typeof(GorillaTagger))]
    [HarmonyPatch("Awake", MethodType.Normal)]
    internal class GorillaHatAwakePatch
    {
        private static void Postfix(GorillaTagger __instance)
        {
            // technically this could probably be done in the same harmony patch as materials. maybe merge them. or maybe don't in case these methods get too long
            GorillaHat hat = AssetLoader.SelectedHat();
            if(hat != null && hat.Hat != null)
            {
                GameObject hatObject = UnityEngine.Object.Instantiate(hat.Hat);
                hatObject.transform.SetParent(__instance.headCollider.gameObject.transform);
                hatObject.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
                hatObject.transform.localPosition = new Vector3(0.07f, 0.225f, 0f);
                hatObject.transform.localRotation = Quaternion.identity;
                hatObject.transform.Rotate(new Vector3(0, 0, 10));
            }
        }
    }
    */
    [HarmonyPatch(typeof(VRRig))]
    [HarmonyPatch("Start", MethodType.Normal)]
    internal class GorillaHatAwakePatch2
    {
        private static void Postfix(VRRig __instance)
        {
            // technically this could probably be done in the same harmony patch as materials. maybe merge them. or maybe don't in case these methods get too long
            Transform existingHat = __instance.head.rigTarget.Find("Hat");
            if (existingHat != null)
            {
                existingHat.parent = null;
                existingHat.gameObject.SetActive(false);
                UnityEngine.Object.Destroy(existingHat.gameObject);
            }
            if (__instance.isOfflineVRRig || GorillaCosmetics.applyHatsToOtherPlayers.Value)
            {
                GorillaHat hat = AssetLoader.SelectedHat();
                if (hat != null && hat.Hat != null)
                {
                    GameObject hatObject = UnityEngine.Object.Instantiate(hat.Hat);
                    hatObject.name = "Hat";
                    hatObject.transform.SetParent(__instance.head.rigTarget);
                    hatObject.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
                    hatObject.transform.localPosition = new Vector3(0f, 0.365f, 0.04f);
                    hatObject.transform.localRotation = Quaternion.identity;
                    hatObject.transform.Rotate(new Vector3(0, 90, 10));
                }
            }
        }
    }
}
