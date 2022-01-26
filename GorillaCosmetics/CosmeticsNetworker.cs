using System;
using System.Collections.Generic;
using System.Text;
using ExitGames.Client.Photon;
using GorillaCosmetics.Data;
using Photon.Pun;
using Photon.Realtime;

namespace GorillaCosmetics
{
	public class CosmeticsNetworker : MonoBehaviourPunCallbacks
	{
		private const string CustomHatKey = "GorillaCosmetics::CustomHat";
		private const string CustomMaterialKey = "GorillaCosmetics::Material";

		void Start()
		{
			Plugin.SelectionManager.OnCosmeticsUpdated += UpdatePlayerCosmetics;
		}

		public override void OnJoinedRoom()
		{
			base.OnJoinedRoom();
			UpdatePlayerCosmetics();
		}

		public void UpdatePlayerCosmetics()
		{
			if (PhotonNetwork.InRoom)
			{
				Hashtable customProperties = new Hashtable();
				if (Plugin.SelectionManager.CurrentHat != null)
				{
					customProperties.Add(CustomHatKey, Plugin.SelectionManager.CurrentHat.Descriptor.Name);
				}
				if (Plugin.SelectionManager.CurrentMaterial != null)
				{
					customProperties.Add(CustomMaterialKey, Plugin.SelectionManager.CurrentMaterial.Descriptor.Name);
				}

				if (customProperties.Count > 0)
				{
					PhotonNetwork.LocalPlayer.SetCustomProperties(customProperties);
				}
			}
		}

		public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
		{
			base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
			var customCosmeticsController = GorillaGameManager.instance.FindVRRigForPlayer(targetPlayer).GetComponent<ICustomCosmeticsController>();

			if (changedProps.TryGetValue(CustomHatKey, out var hatObj))
			{
				var hat = Plugin.AssetLoader.GetAsset<GorillaHat>(hatObj as string);
				if (hat != default)
				{
					customCosmeticsController.SetHat(hat);
				}
			}

			if (changedProps.TryGetValue(CustomMaterialKey, out var matObj))
			{
				var mat = Plugin.AssetLoader.GetAsset<GorillaMaterial>(matObj as string);
				if (mat != default)
				{
					customCosmeticsController.SetMaterial(mat);
				}
			}
		}
	}
}
