using GorillaCosmetics.Data;
using UnityEngine;

namespace GorillaCosmetics.UI
{
	public class MaterialButton : BaseCosmeticButton
	{
		public GorillaMaterial Material { get; set; }

		GameObject previewOrb;

		public override void ButtonActivation()
		{
			base.ButtonActivation();
			if (Material != null)
			{
				if (Plugin.SelectionManager.CurrentMaterial == Material)
				{
					Plugin.SelectionManager.ResetMaterial();
				} else
				{
					Plugin.SelectionManager.SetMaterial(Material);
				}
			} else
			{
				Plugin.SelectionManager.ResetMaterial();
			}
		}

		new void Awake()
		{
			base.Awake();

			Plugin.SelectionManager.OnCosmeticsUpdated += UpdateButton;
		}

		public new void OnDestroy()
		{
			base.OnDestroy();
			
			if (previewOrb != null)
			{
				Destroy(previewOrb);
			}
		}

		public void SetMaterial(GorillaMaterial material)
		{
			Material = material;
			if (previewOrb != null)
			{
				Destroy(previewOrb);
				previewOrb = null;
			}
			if (Material != null)
			{
				GameObject head = wardrobeItemButton.controlledModel.gameObject;
				previewOrb = Material.GetPreviewOrb(head.transform);
			}

			UpdateButton();
		}

		void UpdateButton()
		{
			if (Material != null && Plugin.SelectionManager.CurrentMaterial == Material)
			{
				myText.text = onText;
				buttonRenderer.material = pressedMaterial;
			} else
			{
				myText.text = offText;
				buttonRenderer.material = unpressedMaterial;
			}
		}
	}
}
