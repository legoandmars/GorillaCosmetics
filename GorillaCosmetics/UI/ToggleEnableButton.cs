namespace GorillaCosmetics.UI
{
	public class ToggleEnableButton : GorillaPressableButton
	{
		bool sendButton = false;
		public static bool isEnabled;

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
					isEnabled = true;
                } else
				{
					Plugin.SelectionManager.Disable();
                    isEnabled = false;
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
