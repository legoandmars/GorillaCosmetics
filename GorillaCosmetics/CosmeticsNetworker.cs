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
			StartCoroutine(OnJoinedRoomCoroutine());
		}

		IEnumerator OnJoinedRoomCoroutine() {
			yield return 1;
			UpdatePlayerCosmetics();
			foreach (Player player in PhotonNetwork.CurrentRoom.Players.Values)
			{
				Debug.Log($"Player {player.NickName} joined room with custom properties {String.Join(", ", player.CustomProperties.Keys)}");
				OnPlayerPropertiesUpdate(player, player.CustomProperties);
			}
		}

		public void UpdatePlayerCosmetics()
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
				Debug.Log($"Player {targetPlayer.NickName} updated custom properties {String.Join(", ", changedProps.Keys)}");

				var customCosmeticsControllerObject = GorillaGameManager.instance.FindVRRigForPlayer(targetPlayer);
				if (customCosmeticsControllerObject == null)
				{
					Debug.LogWarning($"Player {targetPlayer.NickName}'s VRRig not found");
					return;
				}
				var customCosmeticsController = customCosmeticsControllerObject.GetComponent<ICustomCosmeticsController>();
				if (customCosmeticsController == null)
				{
					Debug.LogWarning($"Player {targetPlayer.NickName} has no CustomCosmeticsController");
					return;
				}

				if (changedProps.TryGetValue(CustomHatKey, out var hatObj))
				{
					if (hatObj is string hatName)
					{
						Debug.Log($"Player {targetPlayer.NickName} changed hat to {hatObj}");
						var hat = Plugin.AssetLoader.GetAsset<GorillaHat>(hatObj as string);
						if (hat != default)
						{
							Debug.Log($"Player {targetPlayer.NickName} actually changed hat to {hat.Descriptor.Name}");
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
						Debug.Log($"Player {targetPlayer.NickName} changed material to {matObj}");
						var material = Plugin.AssetLoader.GetAsset<GorillaMaterial>(matObj as string);
						if (material != default)
						{
							Debug.Log($"Player {targetPlayer.NickName} actually changed material to {material.Descriptor.Name}");
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
