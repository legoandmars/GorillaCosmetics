using GorillaCosmetics.Data;
using GorillaNetworking;
using System;
using System.Collections.Generic;
using System.Text;

namespace GorillaCosmetics.UI
{
	public class SelectionManager : ISelectionManager
	{
		public Action OnEnable { get; set; }
		public Action OnDisable { get; set; }

		public void Disable()
		{
			OnDisable?.Invoke();
			throw new NotImplementedException();
		}

		public void Enable()
		{
			OnEnable?.Invoke();
			throw new NotImplementedException();
		}

		public void NextPage()
		{
			throw new NotImplementedException();
		}

		public void PreviousPage()
		{
			throw new NotImplementedException();
		}

		public void SetView(ISelectionManager.SelectionView view)
		{
			throw new NotImplementedException();
		}
	}
}
