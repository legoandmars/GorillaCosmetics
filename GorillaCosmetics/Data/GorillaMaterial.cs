using GorillaCosmetics.Data.Descriptors;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace GorillaCosmetics.Data
{
    public class GorillaMaterial
    {
        public string FileName { get; }
        public AssetBundle AssetBundle { get; }
        public GorillaMaterialDescriptor Descriptor { get; }

        public Material Material;

        public GorillaMaterial(string path)
        {
            if (path != "Default")
            {
                try
                {
                    // load
                    FileName = path;
                    AssetBundle = AssetBundle.LoadFromFile(path);
                    GameObject materialObject = AssetBundle.LoadAsset<GameObject>("_Material");
                    Material = materialObject.GetComponent<Renderer>().material;
                    Descriptor = materialObject.GetComponent<GorillaMaterialDescriptor>();
                }
                catch (Exception err)
                {
                    // loading failed. that's not good.
                    Debug.Log(err);
                }
            }
            else
            {
                // try to load the default material
                Descriptor = new GorillaMaterialDescriptor();
                Descriptor.MaterialName = "Default";
                Descriptor.CustomColors = true;
                Material = Resources.Load<Material>("objects/materials/lightfur");
            }
        }
    }
}
