using System;
using System.Collections.Generic;
using UnityEngine;

namespace GorillaCosmetics.UI
{
	public class BaseCosmeticButton : GorillaPressableButton
	{
		protected WardrobeItemButton wardrobeItemButton;

		public void Awake()
		{
			wardrobeItemButton = GetComponent<WardrobeItemButton>();
			wardrobeItemButton.enabled = false;

			foreach (Transform child in wardrobeItemButton.controlledModel.transform)
			{
				child.gameObject.SetActive(false);
			}

			pressedMaterial = wardrobeItemButton.pressedMaterial;
			unpressedMaterial = wardrobeItemButton.unpressedMaterial;
			buttonRenderer = wardrobeItemButton.buttonRenderer;
			debounceTime = wardrobeItemButton.debounceTime;
			offText = wardrobeItemButton.offText;
			onText = wardrobeItemButton.onText;
			myText = wardrobeItemButton.myText;
		}

		public void OnDestroy()
		{
			wardrobeItemButton.enabled = true;
			foreach (Transform child in wardrobeItemButton.controlledModel.transform)
			{
				child.gameObject.SetActive(true);
			}
		}

		public override void ButtonActivation()
		{
			base.ButtonActivation();
		}
	}
}
