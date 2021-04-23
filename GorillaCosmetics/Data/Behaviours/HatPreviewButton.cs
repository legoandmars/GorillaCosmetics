using GorillaCosmetics.Utils;
using UnityEngine;
using Newtonsoft.Json;
using Photon.Pun;
using System.Reflection;
using System.Collections;

namespace GorillaCosmetics.Data.Behaviours
{
	public class HatPreviewButton : GorillaTriggerBox
	{
		public GorillaHat hat;

		static bool canPress = true;

		private void OnTriggerEnter(Collider collider)
		{
			if (!canPress) return;

			if (collider.GetComponentInParent<GorillaTriggerColliderHandIndicator>() != null)
			{
				GorillaTriggerColliderHandIndicator component = collider.GetComponent<GorillaTriggerColliderHandIndicator>();
				// do stuff
				if (hat != null)
				{
					canPress = false;
					string hatName = hat.Descriptor.HatName != null && hat.Descriptor.HatName != "None" ? hat.Descriptor.HatName : "None";
					Debug.Log("Swapping to: " + hatName);
					AssetLoader.SelectHat(hatName);
					GorillaCosmetics.selectedHat.Value = hatName;
					StartCoroutine(ButtonDelay());
					try
					{
						UpdateHatValue();
					}
					catch
                    {
						Debug.Log("Error selecting hat.");
                    }
				}
				if (component != null)
				{
					GorillaTagger.Instance.StartVibration(component.isLeftHand, GorillaTagger.Instance.tapHapticStrength / 2f, GorillaTagger.Instance.tapHapticDuration);
				}
			}
		}

		void UpdateHatValue()
        {
			string name = hat.Descriptor.HatName;
			string hatString = "custom:" + name;

			GorillaTagger gorillaTagger = GorillaTagger.Instance;
			VRRig offlineVRRig = gorillaTagger.offlineVRRig;
			if (offlineVRRig == null) offlineVRRig = gorillaTagger.myVRRig; // this will probably break stuff. TOO BAD!

			string hatCS = typeof(VRRig).GetField("hat", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(offlineVRRig) as string;
			string face = typeof(VRRig).GetField("face", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(offlineVRRig) as string;
			string badge = typeof(VRRig).GetField("badge", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(offlineVRRig) as string;

			VRRigHatJSON hatJSON = new VRRigHatJSON();
			hatJSON.hat = hatString;
			hatJSON.material = AssetLoader.SelectedMaterial().Descriptor.MaterialName != null ? AssetLoader.SelectedMaterial().Descriptor.MaterialName : "Default";
			string hatMessage = JsonConvert.SerializeObject(hatJSON);

			if (offlineVRRig)
			{
				// locally update it
				offlineVRRig.LocalUpdateCosmetics(badge, face, hatMessage);
			}
			VRRig myVRRig = gorillaTagger.myVRRig;
			if (myVRRig)
			{
				PhotonView photonView = myVRRig.photonView;

				photonView.RPC("UpdateCosmetics", RpcTarget.All, new object[] { badge, face, hatMessage });
				PhotonNetwork.SendAllOutgoingCommands();
			}
		}

		private void OnDisable() => canPress = true;
		private void OnDestroy() => canPress = true;

		private static IEnumerator ButtonDelay()
		{
			yield return new WaitForSeconds(2f);
			canPress = true;
		}
	}
}
