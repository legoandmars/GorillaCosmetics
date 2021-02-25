using GorillaCosmetics.Data.Behaviours;
using UnityEngine;

namespace GorillaCosmetics.Data
{
    public class MaterialPreview
    {
        private GorillaMaterial material;
        private GameObject gameObject;
        private MaterialPreviewButton button;
        public MaterialPreview(GorillaMaterial baseMaterial, Vector3 position, float scale) {
            material = baseMaterial;
            gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            gameObject.layer = 18;
            gameObject.transform.localScale = new Vector3(scale, scale, scale);
            gameObject.transform.position = position;
            gameObject.transform.rotation = Quaternion.identity;
            UnityEngine.Object.DontDestroyOnLoad(gameObject);

            gameObject.GetComponent<Collider>().isTrigger = true;
            button = gameObject.AddComponent<MaterialPreviewButton>();
            button.material = material;

            if (baseMaterial != null && baseMaterial.Material != null)
            {
                // handle default material more later
                gameObject.GetComponent<Renderer>().material = baseMaterial.Material;
            }
        }
    }
}
