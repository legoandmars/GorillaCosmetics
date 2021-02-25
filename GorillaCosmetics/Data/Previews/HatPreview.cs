using GorillaCosmetics.Data.Behaviours;
using UnityEngine;
using System.IO;

namespace GorillaCosmetics.Data.Previews
{
    public class HatPreview
    {
        private GorillaHat hat;
        private GameObject gameObject;
        private HatPreviewButton button;
        public HatPreview(GorillaHat baseHat, Collider collider)
        {
            hat = baseHat;
            if (hat != null && hat.Hat != null)
            {
                gameObject = UnityEngine.Object.Instantiate(hat.Hat);
            }
            else
            {
                // fake hat time
                string folder = Path.GetDirectoryName(typeof(GorillaCosmetics).Assembly.Location);

                gameObject = UnityEngine.Object.Instantiate(AssetBundle.LoadFromFile($"{folder}\\Misc\\None").LoadAsset<GameObject>("_Hat"));
            }
            gameObject.transform.SetParent(collider.transform);
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.transform.localRotation = Quaternion.identity;
            gameObject.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
            gameObject.layer = 18;
            gameObject.transform.SetParent(null);
            gameObject.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
            UnityEngine.Object.DontDestroyOnLoad(gameObject);
            collider.isTrigger = true;
            collider.gameObject.layer = 18;
            button = collider.gameObject.AddComponent<HatPreviewButton>();
            button.hat = hat;
        }
    }
}
