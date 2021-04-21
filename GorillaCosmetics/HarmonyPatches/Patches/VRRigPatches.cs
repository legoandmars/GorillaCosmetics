using System;
using System.Collections.Generic;
using System.Text;
using GorillaCosmetics.Utils;
using GorillaCosmetics.Data;
using HarmonyLib;
using Photon.Pun;
using Photon.Realtime;
using Newtonsoft.Json;
using UnityEngine;

namespace GorillaCosmetics.HarmonyPatches
{
    [HarmonyPatch(typeof(VRRig))]
    [HarmonyPatch("Start", MethodType.Normal)]
    internal class VRRigStartsPatch
    {
        private static void Postfix(VRRig __instance)
        {
            PhotonView photonView = __instance.photonView;
            Player owner = photonView ? photonView.Owner : null;
            string UserID = "";
            if (owner != null && owner.UserId != null && owner.UserId != "")
            {
                UserID = owner.UserId;
            }

            // get hat for user from the cached list
            string hat = PlayerCosmeticsList.GetHat(UserID);

            // if it's a custom one, set that as active
            if (hat.StartsWith("custom:"))
            {
                CosmeticUtils.ChangeHat(__instance, hat.Remove(0, 7));
            }
            else CosmeticUtils.ChangeHat(__instance, "None");

            int setMatIndex = __instance.setMatIndex;
            string material = PlayerCosmeticsList.GetMaterial(UserID);
            CosmeticUtils.ChangeMaterial(__instance, setMatIndex, material);
        }
    }

    [HarmonyPatch(typeof(VRRig))]
    [HarmonyPatch("ChangeMaterial", MethodType.Normal)]
    internal class VRRigChangeMaterialsPatch
    {
        private static void Postfix(VRRig __instance, int materialIndex)
        {
            PhotonView photonView = __instance.photonView;
            Player owner = photonView ? photonView.Owner : null;
            string UserID = "";
            if (owner != null && owner.UserId != null && owner.UserId != "")
            {
                UserID = owner.UserId;
            }

            string material = PlayerCosmeticsList.GetMaterial(UserID);
            CosmeticUtils.ChangeMaterial(__instance, materialIndex, material);
        }
    }

    [HarmonyPatch(typeof(VRRig))]
    [HarmonyPatch("UpdateCosmetics", MethodType.Normal)]
    internal class VRRigUpdateCosmeticsPatch
    {
        private static void Prefix(VRRig __instance, string newBadge, string newFace, string newHat, PhotonMessageInfo info)
        {
            string hat = "";
            string material = "default";

            if (newHat.Contains("}") && newHat.Contains("{"))
            {
                // it's probably json. I really should implement a better check for this.
                var json = JsonConvert.DeserializeObject<VRRigHatJSON>(newHat);
                hat = json.hat;
                material = json.material;

                PhotonView photonView = __instance.photonView;
                Player owner = photonView ? photonView.Owner : null;
                string UserID = "";
                if (owner != null && owner.UserId != null && owner.UserId != "")
                {
                    UserID = owner.UserId;
                }
                PlayerCosmeticsList.SetPlayer(UserID, hat, material);
            }
            else hat = newHat;

            newHat = hat;
        }

        private static void Postfix(VRRig __instance, string newBadge, string newFace, string newHat, PhotonMessageInfo info)
        {
            Player player = info.photonView.Owner;
            PhotonView photonView = __instance.photonView;
            Player owner = photonView ? photonView.Owner : null;
            string UserID = "";
            if (owner != null && owner.UserId != null && owner.UserId != "")
            {
                UserID = owner.UserId;
            }

            if (player != null && player == owner)
            {
                Debug.Log("Update requested for " + player.NickName);
                string PlayerHat = PlayerCosmeticsList.GetHat(UserID);
                if (PlayerHat.StartsWith("custom:")) CosmeticUtils.ChangeHat(__instance, PlayerHat.Remove(0, 7));
                else CosmeticUtils.ChangeHat(__instance, "None");
                CosmeticUtils.ChangeMaterial(__instance, __instance.setMatIndex, PlayerCosmeticsList.GetMaterial(UserID));
            }
            else
            {
                Debug.Log("PLAYER AND OWNER ARE NOT EQUAL!");
            }
        }
    }

    [HarmonyPatch(typeof(VRRig))]
    [HarmonyPatch("LocalUpdateCosmetics", MethodType.Normal)]
    internal class VRRigLocalUpdateCosmeticsPatch
    {
        private static void Prefix(VRRig __instance, string newBadge, string newFace, string newHat)
        {
            string hat = "";
            string material = "default";

            if (newHat.Contains("}") && newHat.Contains("{"))
            {
                // it's probably json. I really should implement a better check for this.
                var json = JsonConvert.DeserializeObject<VRRigHatJSON>(newHat);
                hat = json.hat;
                material = json.material;

                PhotonView photonView = __instance.photonView;
                Player owner = photonView ? photonView.Owner : null;
                string UserID = "";
                if (owner != null && owner.UserId != null && owner.UserId != "")
                {
                    UserID = owner.UserId;
                }
                PlayerCosmeticsList.SetPlayer(UserID, hat, material);
            }
            else hat = newHat;

            newHat = hat;
        }

        private static void Postfix(VRRig __instance, string newBadge, string newFace, string newHat)
        {
            string PlayerHat = PlayerCosmeticsList.GetHat("");
            if (PlayerHat.StartsWith("custom:")) CosmeticUtils.ChangeHat(__instance, PlayerHat.Remove(0, 7));
            else CosmeticUtils.ChangeHat(__instance, "None");
            CosmeticUtils.ChangeMaterial(__instance, __instance.setMatIndex, PlayerCosmeticsList.GetMaterial(""));
        }
    }

    [HarmonyPatch(typeof(VRRig))]
    [HarmonyPatch("RequestCosmetics", MethodType.Normal)]
    internal class RequestCosmeticsPatch
    {
        private static void Prefix(VRRig __instance, ref string ___badge, ref string ___face, ref string ___hat, PhotonMessageInfo info)
        {
            PhotonView photonView = __instance.photonView;
            bool IsMine = photonView ? photonView.IsMine : false;

            if (IsMine)
            {
                VRRigHatJSON hatJSON = new VRRigHatJSON();
                hatJSON.material = GorillaCosmetics.selectedMaterial.Value;
                hatJSON.hat = ___hat;

                if (___hat.Contains("}") && ___hat.Contains("{"))
                {
                    // it's probably json. I really should implement a better check for this.
                    var json = JsonConvert.DeserializeObject<VRRigHatJSON>(___hat);
                    hatJSON.hat = json.hat;
                }
                string hatMessage = JsonConvert.SerializeObject(hatJSON);

                photonView.RPC("UpdateCosmetics", RpcTarget.All, new object[] { ___badge, ___face, hatMessage });
                PhotonNetwork.SendAllOutgoingCommands();
            }
            // unsure if i need to do something like this too
            //     VRRig_RequestCosmetics(self, info);
        }
    }

    [HarmonyPatch(typeof(VRRig))]
    [HarmonyPatch("InitializeNoobMaterial", MethodType.Normal)]
    internal class InitializeNoobMaterialPatch
    {
        private static void Postfix(VRRig __instance, float red, float green, float blue)
        {
            PhotonView photonView = __instance.photonView;
            Player owner = photonView ? photonView.Owner : null;
            string UserID = "";
            if (owner != null && owner.UserId != null && owner.UserId != "")
            {
                UserID = owner.UserId;
            }

            string material = PlayerCosmeticsList.GetMaterial(UserID);
            CosmeticUtils.ChangeMaterial(__instance, __instance.setMatIndex, material);
        }
    }


}
