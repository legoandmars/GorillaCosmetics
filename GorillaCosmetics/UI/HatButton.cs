using GorillaCosmetics.Data;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace GorillaCosmetics.UI
{
	public class HatButton : BaseCosmeticButton
	{
		public GorillaHat Hat { get; private set; }

		GameObject previewHat;

		public override void ButtonActivation()
		{
			base.ButtonActivation();
			if (Hat != null)
			{
				if (Plugin.SelectionManager.CurrentHat == Hat)
				{
					Plugin.SelectionManager.ResetHat();
				} else
				{
					Plugin.SelectionManager.SetHat(Hat);
				}
			} else
			{
				Plugin.SelectionManager.ResetHat();
			}
			UpdateButton();
		}

		new void Awake()
		{
			base.Awake();

			Plugin.SelectionManager.OnCosmeticsUpdated += UpdateButton;
		}

		public new void OnDestroy()
		{
			base.OnDestroy();
			
			if (previewHat != null)
			{
				Destroy(previewHat);
			}
		}

		public void SetHat(GorillaHat hat)
		{
			Hat = hat;
			if (previewHat != null)
			{
				Destroy(previewHat);
				previewHat = null;
			}
			if (Hat != null)
			{
				previewHat = Hat.GetAsset();
				HeadModel controlledModel = wardrobeItemButton.controlledModel;
				previewHat.transform.parent = controlledModel.gameObject.transform;
				// TODO: Get the actual proper numbers
				previewHat.transform.localPosition = Constants.PreviewHatLocalPosition;
				previewHat.transform.localRotation = Constants.PreviewHatLocalRotation;
				previewHat.transform.localScale = Constants.PreviewHatLocalScale;
			}

			UpdateButton();
		}

		void UpdateButton()
		{
			if (Plugin.SelectionManager.CurrentHat == Hat)
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
