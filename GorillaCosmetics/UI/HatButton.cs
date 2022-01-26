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
			if (Hat != null)
			{
				base.ButtonActivation();
				if (Plugin.SelectionManager.CurrentHat == Hat)
				{
					Plugin.SelectionManager.ResetHat();
				} else
				{
					Plugin.SelectionManager.SetHat(Hat);
				}
			}
		}

		public void SetHat(GorillaHat hat)
		{
			Hat = hat;
			if (previewHat != null)
			{
				Destroy(previewHat);
			}
			if (Hat != null)
			{
				previewHat = Hat.GetAsset();
				HeadModel controlledModel = wardrobeItemButton.controlledModel;
				previewHat.transform.parent = controlledModel.gameObject.transform;
				// TODO: Get the actual proper numbers
				previewHat.transform.localPosition = new Vector3(0, -0.04f, 0.54f);
				previewHat.transform.localRotation = Quaternion.Euler(0, 90, 100);
				previewHat.transform.localScale = Vector3.one * 0.25f;
			}
		}
	}
}
