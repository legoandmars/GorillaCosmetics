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
		GameObject mirror;

		List<GorillaHat> hats;
		List<GorillaMaterial> materials;

#if DEBUG
		internal List<HatButton> hatButtons = new();
		internal List<MaterialButton> materialButtons = new();
#else
		List<HatButton> hatButtons = new();
		List<MaterialButton> materialButtons = new();
#endif
		List<PageButton> pageButtons = new();
		List<ViewButton> viewButtons = new();
		WardrobeFunctionButton badgeButton;
		WardrobeFunctionButton holdableButton;

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

			CreateEnableButton();
		}

		void CreateEnableButton()
		{
			GameObject template = null;

			foreach (Transform transform in wardrobe.wardrobeItemButtons[0].transform.parent)
			{
				if (transform.name.ToLower().Contains("hat"))
				{
					template = transform.gameObject;
					break;
				}
			}
			if (template == null)
			{
				Debug.LogError("GorillaCosmetics: Could not find enable button template");
				return;
			}

			GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
			cube.SetActive(false);
			MeshFilter meshFilter = cube.GetComponent<MeshFilter>();

			GameObject button = GameObject.Instantiate(template, template.transform.parent);
			button.name = "ToggleEnableButton";
			button.GetComponent<MeshFilter>().mesh = meshFilter.mesh;
			button.GetComponent<Renderer>().material = Resources.Load<Material>("objects/treeroom/materials/plastic");
			button.transform.localPosition = template.transform.localPosition + Constants.EnableButtonLocalPositionOffset;
			button.transform.localRotation = template.transform.localRotation;
			button.transform.localScale = template.transform.localScale;

			WardrobeFunctionButton hatFunctionButton = button.GetComponent<WardrobeFunctionButton>();
			var templateText = hatFunctionButton.myText;
			var newText = GameObject.Instantiate(templateText, templateText.transform.parent);
			newText.transform.localPosition = templateText.transform.localPosition + Constants.EnableButtonLocalPositionOffset;
			newText.transform.localRotation = templateText.transform.localRotation;
			newText.transform.localScale = templateText.transform.localScale;
			hatFunctionButton.myText = newText;

			button.AddComponent<ToggleEnableButton>();

			GameObject.Destroy(cube);
		}

		public void Disable()
		{
			foreach (var button in wardrobe.wardrobeItemButtons)
			{
				var oldButton = button.GetComponent<BaseCosmeticButton>();
				if (oldButton != null)
				{
					UnityEngine.Object.DestroyImmediate(oldButton);
				}
			}

			foreach (PageButton pageButton in pageButtons)
			{
				GameObject.Destroy(pageButton);
			}
			pageButtons = new();

			foreach (ViewButton viewButton in viewButtons)
			{
				GameObject.Destroy(viewButton);
			}
			viewButtons = new();

			badgeButton.enabled = true;
			badgeButton.myText.enabled = true;
			holdableButton.enabled = true;
			holdableButton.myText.enabled = true;

			mirror?.SetActive(false);

			CosmeticsController.instance.PressWardrobeFunctionButton("hat");
			CosmeticsController.instance.PressWardrobeFunctionButton("right");
			CosmeticsController.instance.PressWardrobeFunctionButton("left");
		}

		public void Enable()
		{
			try
			{
				// This is as resilient as I can think to make it,
				// but it is likely to break with any update that moves the wardrobe around.
				foreach (Transform transform in wardrobe.wardrobeItemButtons[0].transform.parent)
				{
					string lowerName = transform.name.ToLower();
					if (lowerName.Contains("left"))
					{
						var pageButton = transform.gameObject.AddComponent<PageButton>();
						pageButton.Forward = false;
						pageButtons.Add(pageButton);
					} else if (lowerName.Contains("right"))
					{
						var pageButton = transform.gameObject.AddComponent<PageButton>();
						pageButton.Forward = true;
						pageButtons.Add(pageButton);
					} else if (lowerName.Contains("hat"))
					{
						var viewButton = transform.gameObject.AddComponent<ViewButton>();
						viewButton.SetView(ISelectionManager.SelectionView.Hat);
						viewButtons.Add(viewButton);
					} else if (lowerName.Contains("face"))
					{
						var viewButton = transform.gameObject.AddComponent<ViewButton>();
						viewButton.SetView(ISelectionManager.SelectionView.Material);
						viewButtons.Add(viewButton);
					} else if (lowerName.Contains("badge"))
					{
						badgeButton = transform.GetComponent<WardrobeFunctionButton>();
						badgeButton.enabled = false;
						badgeButton.myText.enabled = false;
					} else if (lowerName.Contains("holdable"))
					{
						holdableButton = transform.GetComponent<WardrobeFunctionButton>();
						holdableButton.enabled = false;
						holdableButton.myText.enabled = false;
					}
				}
			} catch (Exception e)
			{
				Debug.LogError($"GorillaCosmetics: Failed to create direction buttons: {e}");
			}

			try
			{
				if (mirror == null)
				{
					mirror = GameObject.Find("Level/forest/lower level/mirror (1)");
				}

				if (mirror != null)
				{
					var colliders = mirror.GetComponentsInChildren<Collider>();
					foreach (var collider in colliders)
					{
						collider.enabled = false;
					}
				}

				mirror?.SetActive(true);
			} catch (Exception e)
			{
				Debug.LogError($"GorillaCosmetics: Failed to show mirror: {e}");
			}

			SetView(ISelectionManager.SelectionView.Hat);
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
				previewHat = CurrentHat.GetCleanAsset();
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
			if (CosmeticsController.instance.currentWornSet.items[0].itemCategory == CosmeticsController.CosmeticCategory.Hat)
			{
				if (CosmeticsController.instance.currentWornSet.items[0].itemName != CosmeticsController.instance.nullItem.itemName)
				{
                    CosmeticsController.instance.currentWornSet.items[0] = CosmeticsController.instance.nullItem;

					if (CosmeticsController.instance.tryOnSet.items[0].itemCategory == CosmeticsController.CosmeticCategory.Hat)
                        CosmeticsController.instance.tryOnSet.items[0] = CosmeticsController.instance.nullItem;

                    CosmeticsController.instance.UpdateShoppingCart();
                }

            }
            /*if (CosmeticsController.instance.currentWornSet.hat.itemName != CosmeticsController.instance.nullItem.itemName)
            {
				CosmeticsController.instance.currentWornSet.hat = CosmeticsController.instance.nullItem;
				CosmeticsController.instance.tryOnSet.hat = CosmeticsController.instance.nullItem;
				CosmeticsController.instance.UpdateShoppingCart();
			}*/
        }
	}
}
