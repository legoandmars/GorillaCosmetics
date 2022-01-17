using GorillaCosmetics.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace GorillaCosmetics.UI
{
	public class HatButton : BaseCosmeticButton
	{
		public GorillaHat Hat { get; set; }

		public override void ButtonActivation()
		{
			if (Hat != null)
			{
				base.ButtonActivation();
				if (selectionManager.CurrentHat == Hat)
				{
					selectionManager.ResetHat();
				} else
				{
					selectionManager.SetHat(Hat);
				}
			}
		}
	}
}
