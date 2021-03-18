using GorillaCosmetics.Data;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GorillaCosmetics.Utils
{
    internal static class CosmeticUtils
    {
        internal static List<Material> GetMaterials()
        {
            return Resources.FindObjectsOfTypeAll<Material>().ToList();
        }

        public static void RefreshPlayer(VRRig __instance)
        {
            if (__instance)
            {
                ChangeMaterial(__instance, __instance.currentMatIndex);
                ChangeHat(__instance);
            }
        }

        public static void RefreshLocalPlayer()
        {
            RefreshPlayer(GorillaTagger.Instance.offlineVRRig);
            RefreshPlayer(GorillaTagger.Instance.myVRRig);
        }

        public static void RefreshAllPlayers()
        {
            VRRig[] allRigs = GameObject.FindObjectsOfType(typeof(VRRig)) as VRRig[];
            foreach(VRRig vrRig in allRigs)
            {
                RefreshPlayer(vrRig);
            }
        }

        public static bool IsLocalPlayer(VRRig rig)
        {
            if (rig.isOfflineVRRig || rig.isMyPlayer || rig.photonView.IsMine) return true;
            return false;
        }

        public static void ChangeMaterial(VRRig __instance, int materialIndex)
        {
            if (materialIndex == 0 && (IsLocalPlayer(__instance) || GorillaCosmetics.applyToOtherPlayers.Value))
            {
                // default mat
                GorillaMaterial material = AssetLoader.SelectedMaterial();
                if (material != null && material.Material != null)
                {
                    Material instantiatedMat = UnityEngine.Object.Instantiate(material.Material);
                    if (material.Descriptor.CustomColors)
                    {
                        Color color = new Color(PlayerPrefs.GetFloat("redValue"), PlayerPrefs.GetFloat("greenValue"), PlayerPrefs.GetFloat("blueValue"));
                        instantiatedMat.color = color;
                    }
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
            else if (materialIndex > 0 && (IsLocalPlayer(__instance) || GorillaCosmetics.applyInfectedToOtherPlayers.Value))
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

        internal static void ChangeHat(VRRig __instance)
        {
            Transform existingHat = __instance.head.rigTarget.Find("Hat");
            if (existingHat != null)
            {
                existingHat.parent = null;
                existingHat.gameObject.SetActive(false);
                UnityEngine.Object.Destroy(existingHat.gameObject);
            }
            if (IsLocalPlayer(__instance) || GorillaCosmetics.applyHatsToOtherPlayers.Value)
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
