using GorillaCosmetics.Data;
using GorillaNetworking;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace GorillaCosmetics
{
	public class CustomCosmeticsController : MonoBehaviour, ICustomCosmeticsController
	{
		public GorillaHat CurrentHat { get; private set; }

		public GorillaMaterial CurrentMaterial { get; private set; }

		VRRig rig;

		void Start()
		{

		}

		public void SetHat(GorillaHat hat)
		{
			throw new NotImplementedException();
		}

		public void ResetHat()
		{
			throw new NotImplementedException();
		}

		public void SetMaterial(GorillaMaterial material)
		{
			throw new NotImplementedException();
		}

		public void ResetMaterial()
		{
			throw new NotImplementedException();
		}
	}
}
