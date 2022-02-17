using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using ExitGames.Client.Photon;
using GorillaCosmetics.Data;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace GorillaCosmetics
{
	public class CosmeticsNetworker : MonoBehaviourPunCallbacks, ICosmeticsNetworker
	{
		private const string CustomHatKey = "GorillaCosmetics::CustomHat";
		private const string CustomMaterialKey = "GorillaCosmetics::Material";

		void Start()
		{
			Plugin.SelectionManager.OnCosmeticsUpdated += UpdatePlayerCosmetics;
		}

		public override void OnJoinedRoom()
		{
			UpdatePlayerCosmetics();
		}

		void UpdatePlayerCosmetics()
		{
			if (PhotonNetwork.InRoom)
			{
				Hashtable customProperties = new Hashtable();
				customProperties.Add(CustomHatKey, Plugin.SelectionManager.CurrentHat?.Descriptor?.Name);
				customProperties.Add(CustomMaterialKey, Plugin.SelectionManager.CurrentMaterial?.Descriptor?.Name);

				if (customProperties.Count > 0)
				{
					PhotonNetwork.LocalPlayer.SetCustomProperties(customProperties);
				}
			}
		}

		public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
		{
			try
			{
				base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);

				var customCosmeticsControllerObject = GorillaGameManager.instance.FindVRRigForPlayer(targetPlayer);
				var customCosmeticsController = customCosmeticsControllerObject.GetComponent<ICustomCosmeticsController>();

				if (changedProps.TryGetValue(CustomHatKey, out var hatObj))
				{
					if (hatObj is string hatName)
					{
						Plugin.Log($"Player {targetPlayer.NickName} changed hat to {hatObj}");
						var hat = Plugin.AssetLoader.GetAsset<GorillaHat>(hatObj as string);
						if (hat != default)
						{
							customCosmeticsController.SetHat(hat);
						}
						else
						{
							customCosmeticsController.ResetHat();
						}
					}
					else
					{
						customCosmeticsController.ResetHat();
					}

				}

				if (changedProps.TryGetValue(CustomMaterialKey, out var matObj))
				{
					if (matObj is string matName)
					{
						Plugin.Log($"Player {targetPlayer.NickName} changed material to {matObj}");
						var material = Plugin.AssetLoader.GetAsset<GorillaMaterial>(matObj as string);
						if (material != default)
						{
							customCosmeticsController.SetMaterial(material);
						}
						else
						{
							customCosmeticsController.ResetMaterial();
						}
					}
					else
					{
						customCosmeticsController.ResetMaterial();
					}
				}
			}
			catch (Exception e)
			{
				Debug.LogError($"Error while updating player cosmetics for {targetPlayer.NickName}: {e}");
			}
		}
	}
}
