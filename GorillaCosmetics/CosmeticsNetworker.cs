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
		public override void OnJoinedRoom()
		{
			base.OnJoinedRoom();
			// Add custom player props
		}

		public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
		{
			base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);

			// TODO: do actual logic using the changed properties, same stuff for material
			string hatName = "rainbow"; 

			var customCosmeticsController = GorillaGameManager.instance.FindVRRigForPlayer(targetPlayer).GetComponent<ICustomCosmeticsController>();
			var hat = Plugin.AssetLoader.GetAsset<GorillaHat>(hatName);

			// If we have the hat locally
			if (hat != default)
			{
				customCosmeticsController.SetHat(hat);
			}
		}
	}
}
