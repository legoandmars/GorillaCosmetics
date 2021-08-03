using GorillaCosmetics.Data;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEngine;
using Photon.Pun;
using Newtonsoft.Json;

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
            if (!AssetLoader.Loaded) return;
            if (!__instance) return;

            if (materialIndex == 0)
            {
                // default mat
                GorillaMaterial material = AssetLoader.SelectedMaterial();
                if (material != null && material.Material != null)
                {
                    Material instantiatedMat = UnityEngine.Object.Instantiate(material.Material);
                    if (material.Descriptor.CustomColors)
                    {
                        Debug.Log("Material Had custom colors, setting them");
                        Material mat0 = __instance.materialsToChangeTo[0];
                        Color color = mat0.color;

                        instantiatedMat.color = color;
                    }
                    __instance.mainSkin.material = instantiatedMat;
                }
            }
            else if (materialIndex > 0)
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

        public static void ChangeMaterial(VRRig rig, int materialIndex, string materialName)
        {
            // this method is used for all players, and uses materialName to select the material to use
            if (!AssetLoader.Loaded) return;
            if (!rig) return;

            // if this is actually the local player, use the local player method instead
            if (IsLocalPlayer(rig))
            {
                ChangeMaterial(rig, materialIndex);
                return;
            }

            // get the selected material index
            int selectedMaterial = AssetLoader.SelectedMaterialFromConfig(materialName);
            // get that material
            GorillaMaterial material = AssetLoader.GetMaterial(selectedMaterial);

            // if not it or infected
            if (materialIndex == 0)
            {
                // default mat
                Material instantiatedMat;
                if (material != null && material.Material != null)
                {
                    instantiatedMat = Object.Instantiate(material.Material);
                }
                else // default material time boi
                {
                    instantiatedMat = Object.Instantiate(new GorillaMaterial("Default").Material);
                }

                // also here, custom colors need to be done differently now
                if (material.Descriptor.CustomColors)
                {
                    Debug.Log("Material Had custom colors, setting them");
                    Material mat0 = rig.materialsToChangeTo[0];
                    Color color = mat0.color;

                    instantiatedMat.color = color;
                }
                rig.mainSkin.material = instantiatedMat;
            }
            // skipping custom infected materials because that seems sketch to make custom (just select default and then boom you're unaware of what they are)
        }

        public static void ChangeHat(VRRig rig)
        {
            // local player method
            if (!AssetLoader.Loaded) return;
            if (!rig) return;

            VRMap head = rig.head;
            Transform rigTarget = head.rigTarget;

            //static Il2CppString* hatNameCS = il2cpp_utils::createcsstr("Hat", il2cpp_utils::StringType::Manual);
            // i have no idea what this does. wtf c++? i'm just going to guess...

            // destroy originals
            Transform[] existingHats = rigTarget.GetComponentsInChildren<Transform>();
            foreach (Transform existingHat in existingHats)
            {
                if (existingHat && existingHat.gameObject.name == "Hat")
                {
                    UnityEngine.Object.Destroy(existingHat.gameObject);
                }
            }

            GorillaHat hat = AssetLoader.SelectedHat();
            string name = hat?.Descriptor?.HatName;
            if (name != null && name != "None" && name != "none" && name != "Default")
            {
                if (!hat.Hat) return;
                GameObject hatObject = UnityEngine.Object.Instantiate(hat.Hat);
                hatObject.SetActive(true);
                hatObject.name = "Hat";

                hatObject.transform.SetParent(rigTarget);
                hatObject.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
                hatObject.transform.localPosition = new Vector3(0.0f, 0.365f, 0.04f);
                hatObject.transform.localRotation = Quaternion.identity;
                hatObject.transform.Rotate(new Vector3(0.0f, 90.0f, 10.0f));
            }

        }

        public static void ChangeHat(VRRig rig, string hatName)
        {
            Debug.Log("PUTTING ON THE HAT NAMED");
            Debug.Log(hatName);
            // used for anything besides the local player, just makes it a bit easier to differentiate the 2
            if (!AssetLoader.Loaded) return;
            if (!rig) return;

            // if local player, run the other method
            if (IsLocalPlayer(rig))
            {
                ChangeHat(rig);
                return;
            }

            VRMap head = rig.head;
            Transform rigTarget = head.rigTarget;

            //static Il2CppString* hatNameCS = il2cpp_utils::createcsstr("Hat", il2cpp_utils::StringType::Manual);
            // i have no idea what this does. wtf c++? i'm just going to guess...

            // destroy originals
            Transform[] existingHats = rigTarget.GetComponentsInChildren<Transform>();
            foreach(Transform existingHat in existingHats)
            {
                if (existingHat && existingHat.gameObject.name == "Hat") {
                    UnityEngine.Object.Destroy(existingHat.gameObject);
                }
            }

            int index = AssetLoader.SelectedHatFromConfig(hatName);
            GorillaHat hat = AssetLoader.GetHat(index);
            string name = hat?.Descriptor?.HatName;
            if (name != null && name != "None" && name != "none" && name != "Default")
            {
                if (!hat.Hat) return;
                GameObject hatObject = UnityEngine.Object.Instantiate(hat.Hat);
                hatObject.SetActive(true);
                hatObject.name = "Hat";

                hatObject.transform.SetParent(rigTarget);
                hatObject.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
                hatObject.transform.localPosition = new Vector3(0.0f, 0.365f, 0.04f);
                hatObject.transform.localRotation = Quaternion.identity;
                hatObject.transform.Rotate(new Vector3(0.0f, 90.0f, 10.0f));
            }
        }

        public static void LocalLoadingCallback()
        {
            string material = GorillaCosmetics.selectedMaterial.Value;
            string hat = GorillaCosmetics.selectedHat.Value;

            GorillaTagger gorillaTagger = GorillaTagger.Instance;
            VRRig offlineVRRig = gorillaTagger.offlineVRRig;
            if (offlineVRRig == null) offlineVRRig = gorillaTagger.myVRRig; // this will probably break stuff. TOO BAD!

            string hatCS = offlineVRRig.hat;
            string face = offlineVRRig.face;
            string badge = offlineVRRig.badge;

            VRRigHatJSON hatJSON = new VRRigHatJSON();
            hatJSON.hat = hatCS;
            if (hat != "None") hatJSON.hat = hat;
            // I don't know if this is right, but I'm not sure how red is doing it so i'm taking my best guess.
            Debug.Log(hatCS);
            if (hatCS.Contains("}") && hatCS.Contains("{"))
            {
                // it's probably json. I really should implement a better check for this.
                var json = JsonConvert.DeserializeObject<VRRigHatJSON>(hatCS);
                hatJSON.hat = json.hat;
            }

            hatJSON.material = material;
            string hatMessage = JsonConvert.SerializeObject(hatJSON);
            if (offlineVRRig)
            {
                // locally update it
                offlineVRRig.LocalUpdateCosmetics(badge, face, hatMessage);
            }
            VRRig myVRRig = gorillaTagger.myVRRig;
            if (myVRRig)
            {
                PhotonView photonView = myVRRig.photonView;

                photonView.RPC("UpdateCosmetics", RpcTarget.All, new object[] { badge, face, hatMessage });
                PhotonNetwork.SendAllOutgoingCommands();
            }
        }
    }

}
