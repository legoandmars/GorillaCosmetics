using GorillaCosmetics.Data.Descriptors;
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
                    // load
                    FileName = path;
                    AssetBundle = AssetBundle.LoadFromFile(path);
                    Hat = AssetBundle.LoadAsset<GameObject>("_Hat");
                    foreach(Collider collider in Hat.GetComponentsInChildren<Collider>())
                    {
                        collider.enabled = false; // Disable colliders. They can be left in accidentally and cause some really weird issues.
                    }
                    Descriptor = Hat.GetComponent<HatDescriptor>();
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
