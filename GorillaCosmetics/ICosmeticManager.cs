using GorillaCosmetics.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace GorillaCosmetics
{
	public interface ICosmeticManager
	{
		GorillaHat CurrentHat { get; }
		GorillaMaterial CurrentMaterial { get;  }

		void SetLocalHat(GorillaHat hat);
		void ResetLocalHat();
		void SetHat(VRRig rig, GorillaHat hat);
		void ResetHat(VRRig rig);
		void SetLocalMaterial(GorillaMaterial material);
		void ResetLocalMaterial();
		void SetMaterial(VRRig rig, GorillaMaterial material);
		void ResetMaterial(VRRig rig);
	}
}
