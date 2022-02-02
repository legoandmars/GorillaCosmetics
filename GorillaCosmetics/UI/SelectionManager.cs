using GorillaCosmetics.Data;
using GorillaNetworking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GorillaCosmetics.UI
{
	public class SelectionManager : ISelectionManager
	{
		const string PlayerPrefsPrefix = "MOD_";
		const int PageSize = 3;
		private const string HatPlayerPrefKey = "hatCosmetic";
		private const string MaterialPlayerPrefKey = "materialCosmetic";

		public Action OnCosmeticsUpdated { get; set; }

		public GorillaHat CurrentHat { get; private set; }
		public GorillaMaterial CurrentMaterial { get; private set; }

		ISelectionManager.SelectionView view;

		ICustomCosmeticsController offlineCustomCosmeticsController;
		ICustomCosmeticsController onlineCustomCosmeticsController => GorillaTagger.Instance?.myVRRig?.GetComponent<ICustomCosmeticsController>(); // TODO: ew

		CosmeticsController.Wardrobe wardrobe;

		List<GorillaHat> hats;
		List<GorillaMaterial> materials;

		// TODO: Private
		internal List<HatButton> hatButtons;
		internal List<MaterialButton> materialButtons;

		GameObject previewHat;
		GameObject previewOrb;

		int page;
		int PageCount => Mathf.CeilToInt((view == ISelectionManager.SelectionView.Hat ? hats.Count : materials.Count) / (float)PageSize);

		public SelectionManager()
		{
			offlineCustomCosmeticsController = GorillaTagger.Instance.offlineVRRig.GetComponent<ICustomCosmeticsController>();
			hats = Plugin.AssetLoader.GetAssets<GorillaHat>();
			materials = Plugin.AssetLoader.GetAssets<GorillaMaterial>();

			wardrobe = CosmeticsController.instance.wardrobes[1];

			var hatString = PlayerPrefs.GetString(HatPlayerPrefKey, "NOTHING");
			var matString = PlayerPrefs.GetString(MaterialPlayerPrefKey, "NOTHING");

			if (hatString.StartsWith(PlayerPrefsPrefix))
			{
				var hat = Plugin.AssetLoader.GetAsset<GorillaHat>(hatString.Substring(PlayerPrefsPrefix.Length));
				if (hat != null)
				{
					SetHat(hat);
				}
			}

			if (matString.StartsWith(PlayerPrefsPrefix))
			{
				var mat = Plugin.AssetLoader.GetAsset<GorillaMaterial>(matString.Substring(PlayerPrefsPrefix.Length));
				if (mat != null)
				{
					SetMaterial(mat);
				}
			}
		}

		public void Disable()
		{
			// TODO: Show normal cosmetics
			// TODO: Destroy page view buttons and navigation buttons
			foreach (var button in wardrobe.wardrobeItemButtons)
			{
				var oldButton = button.GetComponent<BaseCosmeticButton>();
				if (oldButton != null)
				{
					UnityEngine.Object.DestroyImmediate(oldButton);
				}
			}

			// TODO: Fix
			CosmeticsController.instance.PressWardrobeFunctionButton("hat");
			CosmeticsController.instance.PressWardrobeFunctionButton("right");
			CosmeticsController.instance.PressWardrobeFunctionButton("left");
		}

		public void Enable()
		{
			// TODO: Create page view buttons and navigation buttons
			//SetView(ISelectionManager.SelectionView.Hat);
			SetView(ISelectionManager.SelectionView.Material);
		}

		public void NextPage()
		{
			if (page < PageCount - 1)
			{
				page++;
			}

			UpdateView();
		}

		public void PreviousPage()
		{
			if (page > 0)
			{
				page--;
			}

			UpdateView();
		}

		public void SetView(ISelectionManager.SelectionView view)
		{
			this.view = view;
			page = 0;

			hatButtons = new();
			materialButtons = new();

			foreach (var button in wardrobe.wardrobeItemButtons)
			{
				var oldButton = button.GetComponent<BaseCosmeticButton>();
				if (oldButton != null)
				{
					UnityEngine.Object.DestroyImmediate(oldButton);
				}

				switch (view)
				{
					case ISelectionManager.SelectionView.Hat:
						var hatButton = button.gameObject.AddComponent<HatButton>();
						hatButtons.Add(hatButton);
						break;
					case ISelectionManager.SelectionView.Material:
						var matButton = button.gameObject.AddComponent<MaterialButton>();
						materialButtons.Add(matButton);
						break;
				}
			}

			UpdateView();
		}

		void UpdateView()
		{
			switch (view)
			{
				case ISelectionManager.SelectionView.Hat:
					UpdateHatView();
					break;
				case ISelectionManager.SelectionView.Material:
					UpdateMaterialView();
					break;
			}
			UpdateHeadModel();
		}

		void UpdateHatView()
		{
			var currentPageHats = hats.Skip(page * PageSize).Take(PageSize).ToList();
			for(int i = 0; i < hatButtons.Count; i++)
			{
				hatButtons[i].SetHat(i < currentPageHats.Count ? currentPageHats[i] : null);
			}
		}

		void UpdateMaterialView()
		{
			var currentPageMats = materials.Skip(page * PageSize).Take(PageSize).ToList();
			for(int i = 0; i < materialButtons.Count; i++)
			{
				materialButtons[i].SetMaterial(i < currentPageMats.Count ? currentPageMats[i] : null);
			}
		}

		void UpdateHeadModel()
		{
			Transform parent = wardrobe.selfDoll.gameObject.transform;

			if (previewHat != null)
			{
				UnityEngine.Object.Destroy(previewHat);
			}

			if (CurrentHat != null)
			{
				previewHat = CurrentHat.GetAsset();
				previewHat.transform.parent = parent;
				previewHat.transform.localPosition = Constants.PreviewHatLocalPosition;
				previewHat.transform.localRotation = Constants.PreviewHatLocalRotation;
				previewHat.transform.localScale = Constants.PreviewHatLocalScale;
			}

			if (previewOrb != null)
			{
				UnityEngine.Object.Destroy(previewOrb);
			}

			if (CurrentMaterial != null)
			{
				previewOrb = CurrentMaterial.GetPreviewOrb(parent);
				previewOrb.transform.localPosition += Constants.PreviewOrbHeadModelLocalPositionOffset;
			}
		}

		public void SetHat(GorillaHat hat)
		{
			ResetGameHat();
			if (hat == null)
			{
				ResetHat();
			} else
			{
				PlayerPrefs.SetString(HatPlayerPrefKey, PlayerPrefsPrefix + hat.Descriptor.Name);
				PlayerPrefs.Save();

				CurrentHat = hat;
				offlineCustomCosmeticsController.SetHat(hat);
				onlineCustomCosmeticsController?.SetHat(hat);

				UpdateHeadModel();

				OnCosmeticsUpdated?.Invoke();
			}
		}

		public void ResetHat()
		{
			PlayerPrefs.SetString(HatPlayerPrefKey, CosmeticsController.instance.nullItem.itemName);
			PlayerPrefs.Save();

			CurrentHat = null;
			offlineCustomCosmeticsController.ResetHat();
			onlineCustomCosmeticsController?.ResetHat();

			UpdateHeadModel();

			OnCosmeticsUpdated?.Invoke();
		}

		public void SetMaterial(GorillaMaterial material)
		{
			if (material == null)
			{
				ResetMaterial();
			} else
			{
				PlayerPrefs.SetString(MaterialPlayerPrefKey, PlayerPrefsPrefix + material.Descriptor.Name);
				PlayerPrefs.Save();

				CurrentMaterial = material;
				offlineCustomCosmeticsController.SetMaterial(material);
				onlineCustomCosmeticsController?.SetMaterial(material);

				UpdateHeadModel();

				OnCosmeticsUpdated?.Invoke();
			}
		}

		public void ResetMaterial()
		{
			PlayerPrefs.SetString(MaterialPlayerPrefKey, CosmeticsController.instance.nullItem.itemName);
			PlayerPrefs.Save();

			CurrentMaterial = null;
			offlineCustomCosmeticsController.ResetMaterial();
			onlineCustomCosmeticsController?.ResetMaterial();

			UpdateHeadModel();

			OnCosmeticsUpdated?.Invoke();
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
	}
}
