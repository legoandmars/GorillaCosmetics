using System;
using UnityEngine;
using GorillaCosmetics.Utils;

namespace GorillaCosmetics.Data
{
    public class GorillaMaterial : IAsset
    {
        public string FileName { get; }
        public AssetBundle AssetBundle { get; }
        public CosmeticDescriptor Descriptor { get; }

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
                    Descriptor = PackageUtils.ConvertJsonToDescriptor(json);
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
                Descriptor = new CosmeticDescriptor();
                Descriptor.Name = "Default";
                Descriptor.CustomColors = true;
                Material = Resources.Load<Material>("objects/treeroom/materials/lightfur");
            }
        }

        public GameObject GetPreviewOrb(Transform parent)
		{
            GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            gameObject.layer = 18;
            gameObject.transform.SetParent(parent);
            gameObject.transform.localScale = Vector3.one * 0.5f;
            gameObject.transform.localPosition = new Vector3(0, 0, 0.2f);
            gameObject.transform.rotation = Quaternion.identity;
			gameObject.GetComponent<Renderer>().material = Material;
            return gameObject;
		}
    }
}
