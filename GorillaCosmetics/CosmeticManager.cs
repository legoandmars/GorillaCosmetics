using GorillaCosmetics.Data;
using GorillaNetworking;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace GorillaCosmetics
{
	public class CosmeticManager : ICosmeticManager
	{
		const string PlayerPrefsPrefix = "MOD_";

		public GorillaHat CurrentHat { get; private set; }

		public GorillaMaterial CurrentMaterial { get; private set; }

		public CosmeticManager()
		{
			var hatString = PlayerPrefs.GetString("hatCosmetic", "NOTHING");
			var matString = PlayerPrefs.GetString("materialCosmetic", "NOTHING");

			if (hatString.StartsWith(PlayerPrefsPrefix))
			{
				// TODO: Apply hat
			}
			
			// TODO: Apply mat
		}

		public void SetLocalHat(GorillaHat hat)
		{
			ResetGameHat();
			PlayerPrefs.SetString("hatCosmetic", PlayerPrefsPrefix + hat.Descriptor.Name);
			PlayerPrefs.Save();

			CurrentHat = hat;
            SetHat(GorillaTagger.Instance.offlineVRRig, hat);
            SetHat(GorillaTagger.Instance.myVRRig, hat);
			throw new NotImplementedException();
		}

		public void ResetLocalHat()
		{
			PlayerPrefs.SetString("hatCosmetic", CosmeticsController.instance.nullItem.itemName);
			PlayerPrefs.Save();
			CurrentHat = null;
            ResetHat(GorillaTagger.Instance.offlineVRRig);
            ResetHat(GorillaTagger.Instance.myVRRig);
		}

		public void SetHat(VRRig rig, GorillaHat hat)
		{
			// TODO: do the gameobject creation
			// var hatGO = CurrentHat.GetAsset();
			throw new NotImplementedException();
		}

		public void ResetHat(VRRig rig)
		{
			throw new NotImplementedException();
		}

		void ResetGameHat()
		{
			if (CosmeticsController.instance.currentWornSet.hat.itemName != CosmeticsController.instance.nullItem.itemName)
			{
				CosmeticsController.instance.currentWornSet.hat = CosmeticsController.instance.nullItem;
				CosmeticsController.instance.tryOnSet.hat = CosmeticsController.instance.nullItem;
				CosmeticsController.instance.UpdateShoppingCart();
			}
		}

		public void SetLocalMaterial(GorillaMaterial material)
		{
			throw new NotImplementedException();
		}

		public void ResetLocalMaterial()
		{
			throw new NotImplementedException();
		}

		public void SetMaterial(VRRig rig, GorillaMaterial material)
		{
			throw new NotImplementedException();
		}

		public void ResetMaterial(VRRig rig)
		{
			throw new NotImplementedException();
		}
	}
}
