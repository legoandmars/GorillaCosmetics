using System;
using System.Collections.Generic;
using System.Text;

namespace GorillaCosmetics.UI
{
	public class ToggleEnableButton : GorillaPressableButton
	{
		bool sendButton = false;

		public void Awake()
		{
			var wardrobeFunctionButton = GetComponent<WardrobeFunctionButton>();

			pressedMaterial = wardrobeFunctionButton.pressedMaterial;
			unpressedMaterial = wardrobeFunctionButton.unpressedMaterial;
			buttonRenderer = wardrobeFunctionButton.buttonRenderer;
			debounceTime = wardrobeFunctionButton.debounceTime;
			myText = wardrobeFunctionButton.myText;

			Destroy(wardrobeFunctionButton);

			offText = "CUSTOM";
			onText = "CUSTOM";
			myText.text = "CUSTOM";
			onPressButton = new UnityEngine.Events.UnityEvent();
        }

		public void Update()
		{
			if (sendButton)
			{
				sendButton = false;
				if (isOn)
				{
					Plugin.SelectionManager.Enable();
				} else
				{
					Plugin.SelectionManager.Disable();
				}
			}
		}

		public override void ButtonActivation()
		{
			base.ButtonActivation();

			sendButton = true;

			isOn = !isOn;
			UpdateColor();
		}
	}
}
