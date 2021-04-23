using GorillaCosmetics.Data.Descriptors;
using System;
using UnityEngine;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Text;
using GorillaCosmetics.Utils;

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
                    var bundleAndJson = PackageUtils.AssetBundleAndJSONFromPackage(FileName);
                    AssetBundle = bundleAndJson.bundle;
                    PackageJSON json = bundleAndJson.json;

                    // get material object and stuff
                    GameObject materialObject = AssetBundle.LoadAsset<GameObject>("_Material");
                    Material = materialObject.GetComponent<Renderer>().material;

                    // Make Descriptor
                    Descriptor = PackageUtils.ConvertJsonToMaterial(json);
                }
                catch (Exception err)
                {
                    // loading failed. that's not good.
                    Debug.Log(err);
                    throw new Exception($"Loading material at {path} failed.");
                }
            }
            else
            {
                // try to load the default material
                Descriptor = new GorillaMaterialDescriptor();
                Descriptor.MaterialName = "Default";
                Descriptor.CustomColors = true;
                Material = Resources.Load<Material>("objects/treeroom/materials/lightfur");
            }
        }
    }
}
