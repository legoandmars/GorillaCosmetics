using GorillaCosmetics.Data;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace GorillaCosmetics.UI
{
	public class MaterialButton : BaseCosmeticButton
	{
		public GorillaMaterial Material { get; set; }

		GameObject previewOrb;

		public override void ButtonActivation()
		{
			if (Material != null)
			{
				base.ButtonActivation();
				if (Plugin.SelectionManager.CurrentMaterial == Material)
				{
					Plugin.SelectionManager.ResetMaterial();
				} else
				{
					Plugin.SelectionManager.SetMaterial(Material);
				}
			}
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
		}
	}
}
