using GorillaCosmetics.Utils;
using UnityEngine;

namespace GorillaCosmetics.Data.Behaviours
{
	public class HatPreviewButton : GorillaTriggerBox
	{
		public GorillaHat hat;
		private void OnTriggerEnter(Collider collider)
		{
			if (collider.GetComponentInParent<GorillaTriggerColliderHandIndicator>() != null)
			{
				GorillaTriggerColliderHandIndicator component = collider.GetComponent<GorillaTriggerColliderHandIndicator>();
				// do stuff
				if (hat != null)
				{
					if (hat.Descriptor?.HatName != null)

					{
						Debug.Log("Swapping to: " + hat.Descriptor.HatName);
						GorillaCosmetics.selectedHat.Value = hat.Descriptor.HatName;
						AssetLoader.selectedHat = AssetLoader.SelectedHatFromConfig();
					}
					else
					{
						// default hat stuff
						Debug.Log("Swapping to default hat");
						GorillaCosmetics.selectedHat.Value = "Default";
						AssetLoader.selectedHat = 0;
					}
					CosmeticUtils.RefreshAllPlayers();
				}
				if (component != null)
				{
					GorillaTagger.Instance.StartVibration(component.isLeftHand, GorillaTagger.Instance.tapHapticStrength / 2f, GorillaTagger.Instance.tapHapticDuration);
				}
			}
		}
	}
}
