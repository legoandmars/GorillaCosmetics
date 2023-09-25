﻿using System;
using GorillaCosmetics.Data;
using GorillaExtensions;
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

				if (GorillaGameManager.instance == null) return;
				var customCosmeticsControllerObject = GorillaGameManager.instance.FindPlayerVRRig(targetPlayer);
				var customCosmeticsController = customCosmeticsControllerObject.gameObject.GetOrAddComponent<CustomCosmeticsController>();
				//customCosmeticsController ??= customCosmeticsControllerObject.gameObject.AddComponent<CustomCosmeticsController>();

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
