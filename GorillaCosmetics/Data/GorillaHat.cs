using GorillaCosmetics.Data.Descriptors;
using GorillaCosmetics.Utils;
using System;
using UnityEngine;

namespace GorillaCosmetics.Data
{
    public class GorillaHat
    {
        public string FileName { get; }
        public AssetBundle AssetBundle { get; }
        public HatDescriptor Descriptor { get; }

        public GameObject Hat;

        public GorillaHat(string path)
        {
            if (path != "Default")
            {
                try
                {
                    FileName = path;
                    var bundleAndJson = PackageUtils.AssetBundleAndJSONFromPackage(FileName);
                    AssetBundle = bundleAndJson.bundle;
                    PackageJSON json = bundleAndJson.json;

                    // get material object and stuff
                    Hat = AssetBundle.LoadAsset<GameObject>("_Hat");
                    foreach (Collider collider in Hat.GetComponentsInChildren<Collider>())
                    {
                        collider.enabled = false; // Disable colliders. They can be left in accidentally and cause some really weird issues.
                    }

                    // Make Descriptor
                    Descriptor = PackageUtils.ConvertJsonToHat(json);
                    Debug.Log(Descriptor.AuthorName);
                }
                catch (Exception err)
                {
                    // loading failed. that's not good.
                    Debug.Log(err);
                }
            }
        }
    }
}
