using GorillaCosmetics.Data;
using GorillaNetworking;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace GorillaCosmetics.UI
{
	public class SelectionManager : ISelectionManager
	{
		const string PlayerPrefsPrefix = "MOD_";

		public Action OnEnable { get; set; }
		public Action OnDisable { get; set; }

		public GorillaHat CurrentHat { get; private set; }
		public GorillaMaterial CurrentMaterial { get; private set; }

		ICustomCosmeticsController offlineCustomCosmeticsController;
		ICustomCosmeticsController onlineCustomCosmeticsController => GorillaTagger.Instance.myVRRig.GetComponent<ICustomCosmeticsController>(); // TODO: ew

		public SelectionManager()
		{
			offlineCustomCosmeticsController = GorillaTagger.Instance.offlineVRRig.GetComponent<ICustomCosmeticsController>();

			var hatString = PlayerPrefs.GetString("hatCosmetic", "NOTHING");
			var matString = PlayerPrefs.GetString("materialCosmetic", "NOTHING");

			if (hatString.StartsWith(PlayerPrefsPrefix))
			{
				// TODO: Apply hat
			}
			
			// TODO: Apply mat
		}

		public void Disable()
		{
			OnDisable?.Invoke();
			throw new NotImplementedException();
		}

		public void Enable()
		{
			OnEnable?.Invoke();
			throw new NotImplementedException();
		}

		public void NextPage()
		{
			throw new NotImplementedException();
		}

		public void PreviousPage()
		{
			throw new NotImplementedException();
		}

		public void SetView(ISelectionManager.SelectionView view)
		{
			throw new NotImplementedException();
		}

		public void SetHat(GorillaHat hat)
		{
			ResetGameHat();
			PlayerPrefs.SetString("hatCosmetic", PlayerPrefsPrefix + hat.Descriptor.Name);
			PlayerPrefs.Save();

			CurrentHat = hat;
			offlineCustomCosmeticsController.SetHat(hat);
			onlineCustomCosmeticsController.SetHat(hat);

			UpdateWardrobeDisplay();
		}

		public void ResetHat()
		{
			PlayerPrefs.SetString("hatCosmetic", CosmeticsController.instance.nullItem.itemName);
			PlayerPrefs.Save();

			CurrentHat = null;
			offlineCustomCosmeticsController.ResetHat();
			onlineCustomCosmeticsController.ResetHat();

			UpdateWardrobeDisplay();
		}

		public void SetMaterial(GorillaMaterial material)
		{
			throw new NotImplementedException();
		}

		public void ResetMaterial()
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

		void UpdateWardrobeDisplay()
		{
			throw new NotImplementedException();
		}
	}
}
