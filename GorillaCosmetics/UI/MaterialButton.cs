using GorillaCosmetics.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace GorillaCosmetics.UI
{
	public class MaterialButton : BaseCosmeticButton
	{
		public GorillaMaterial Material { get; set; }

		public override void ButtonActivation()
		{
			if (Material != null)
			{
				base.ButtonActivation();
				if (selectionManager.CurrentMaterial == Material)
				{
					selectionManager.ResetMaterial();
				} else
				{
					selectionManager.SetMaterial(Material);
				}
			}
		}
	}
}
