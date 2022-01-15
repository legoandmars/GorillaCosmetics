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

		void Enable();
		void Disable();

		void SetView(SelectionView view);

		void NextPage();
		void PreviousPage();
	}
}