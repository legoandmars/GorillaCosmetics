using GorillaCosmetics.Utils;
using UnityEngine;

namespace GorillaCosmetics.Data.Behaviours
{
    public class MaterialPreviewButton : GorillaTriggerBox
    {
		public GorillaMaterial material;
		private void OnTriggerEnter(Collider collider)
		{
			if (collider.GetComponentInParent<GorillaTriggerColliderHandIndicator>() != null)
			{
				GorillaTriggerColliderHandIndicator component = collider.GetComponent<GorillaTriggerColliderHandIndicator>();
				// do stuff
				if(material != null)
				{
					if(material.Descriptor.MaterialName != null)
					{
						Debug.Log("Swapping to: " + material.Descriptor.MaterialName);
						GorillaCosmetics.selectedMaterial.Value = material.Descriptor.MaterialName;
						AssetLoader.selectedMaterial = AssetLoader.SelectedMaterialFromConfig(material.Descriptor.MaterialName);
					}
					else
					{
						Debug.Log("Swapping to default material");
						GorillaCosmetics.selectedMaterial.Value = "Default";
						AssetLoader.selectedMaterial = 0;
					}
					CosmeticUtils.RefreshAllPlayers();
					//GorillaTagger.Instance.offlineVRRig.ChangeMaterial(GorillaTagger.Instance.offlineVRRig.setMatIndex);
				}
				if (component != null)
				{
					GorillaTagger.Instance.StartVibration(component.isLeftHand, GorillaTagger.Instance.tapHapticStrength / 2f, GorillaTagger.Instance.tapHapticDuration);
				}
			}
		}
	}
}
