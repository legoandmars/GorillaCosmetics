using System;
using System.Collections;
using UnityEngine;

namespace GorillaCosmetics.UI
{
	public class ViewButton : GorillaPressableButton
	{
		ISelectionManager.SelectionView view;

		public float buttonFadeTime = 0.25f;

		WardrobeFunctionButton wardrobeFunctionButton;

		bool sendPress = false;

		string defaultText;

		public void Awake()
		{
			wardrobeFunctionButton = GetComponent<WardrobeFunctionButton>();
			wardrobeFunctionButton.enabled = false;

			pressedMaterial = wardrobeFunctionButton.pressedMaterial;
			unpressedMaterial = wardrobeFunctionButton.unpressedMaterial;
			buttonRenderer = wardrobeFunctionButton.buttonRenderer;
			debounceTime = wardrobeFunctionButton.debounceTime;
			myText = wardrobeFunctionButton.myText;

			defaultText = myText.text;
            onPressButton = new UnityEngine.Events.UnityEvent();
        }

		public void OnDestroy()
		{
			wardrobeFunctionButton.enabled = true;
			myText.text = defaultText;
		}

		public void Update()
		{
			if (sendPress)
			{
				sendPress = false;
				Plugin.SelectionManager.SetView(view);
			}
		}

		public void SetView(ISelectionManager.SelectionView view)
		{
			this.view = view;
			myText.text = view.ToString().ToUpper();
		}

		public override void ButtonActivation()
		{
			base.ButtonActivation();
			// Breaks when run from wierd stacks (e.g. physics tick)
			sendPress = true;
			StartCoroutine(ButtonColorUpdate());
		}

		public override void UpdateColor() { }

		private IEnumerator ButtonColorUpdate()
		{
			// Calling it here so DestroyImmediate works
			try
			{

			} catch (Exception e)
			{
				Debug.LogException(e);
			}
			buttonRenderer.material = pressedMaterial;
			yield return new WaitForSeconds(buttonFadeTime);
			buttonRenderer.material = unpressedMaterial;
		}
	}
}
