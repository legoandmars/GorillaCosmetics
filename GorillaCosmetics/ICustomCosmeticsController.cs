using GorillaCosmetics.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace GorillaCosmetics
{
	public interface ICustomCosmeticsController
	{
		int MatIndex { get; }
		GorillaHat CurrentHat { get; }
		GorillaMaterial CurrentMaterial { get;  }

		void SetHat(GorillaHat hat);
		void ResetHat();
		void SetMaterial(GorillaMaterial material);
		void ResetMaterial();
		void SetColor(float red, float green, float blue);
	}
}
