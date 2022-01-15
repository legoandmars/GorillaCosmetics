using System;
using System.Collections.Generic;
using System.Text;

namespace GorillaCosmetics.UI
{
	public class BaseCosmeticButton : GorillaPressableButton
	{
		public ICosmeticManager cosmeticManager;
		 
		WardrobeItemButton wardrobeItemButton;

		public override void Start()
		{
			cosmeticManager = GorillaCosmetics.CosmeticManager;

			wardrobeItemButton = GetComponent<WardrobeItemButton>();
			wardrobeItemButton.enabled = false;

			pressedMaterial = wardrobeItemButton.pressedMaterial ;
			unpressedMaterial = wardrobeItemButton.unpressedMaterial;
			buttonRenderer = wardrobeItemButton.buttonRenderer;
			debounceTime = wardrobeItemButton.debounceTime;
			offText = wardrobeItemButton.offText;
			onText = wardrobeItemButton.onText;
			myText = wardrobeItemButton.myText;

			base.Start();
		}

		void OnDestroy()
		{
			wardrobeItemButton.enabled = true;
		}

		public override void ButtonActivation()
		{
			base.ButtonActivation();
		}
	}
}
