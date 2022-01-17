using GorillaCosmetics.Data;
using System;

namespace GorillaCosmetics.UI
{
	public interface ISelectionManager
	{
		enum SelectionView
		{
			Hat,
			Material
		}

		Action OnEnable { get; set; }
		Action OnDisable { get; set; }
		GorillaHat CurrentHat { get; }
		GorillaMaterial CurrentMaterial { get; }

		void Enable();
		void Disable();

		void SetView(SelectionView view);

		void NextPage();
		void PreviousPage();
		void SetHat(GorillaHat hat);
		void ResetHat();
		void SetMaterial(GorillaMaterial material);
		void ResetMaterial();
	}
}