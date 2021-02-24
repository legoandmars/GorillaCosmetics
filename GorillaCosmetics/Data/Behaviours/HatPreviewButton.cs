using GorillaCosmetics.HarmonyPatches.Patches;
using GorillaCosmetics.Utils;
using System;
using System.Collections.Generic;
using System.Text;
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
					if (hat.Descriptor != null)

					{
						Debug.Log("Swapping to: " + hat.Descriptor.HatName);
						GorillaCosmetics.selectedHat.Value = hat.Descriptor.HatName;
						AssetLoader.selectedHat = AssetLoader.SelectedHatFromConfig();
					}
					else
					{
						// default hat stuff
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
