using UnityEngine;
using UnityEngine.Events;

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
			onPressButton = new UnityEvent();
        }

		public void OnDestroy()
		{
			wardrobeItemButton.enabled = true;
			foreach (Transform child in wardrobeItemButton.controlledModel.transform)
			{
				if (!child.name.ToLower().Contains("coming soon"))
				{
					child.gameObject.SetActive(true);
				}
			}
		}
	}
}
