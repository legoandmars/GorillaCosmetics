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

		GameObject currentHatObject;

		VRRig rig;

		void Start()
		{
			rig = GetComponent<VRRig>();
		}

		public void SetHat(GorillaHat hat)
		{
			Debug.Log($"Player: {rig.playerName} switching hat from {CurrentHat?.Descriptor?.Name} to {hat?.Descriptor?.Name}");
			CurrentHat = hat;

			if (currentHatObject != null)
			{
				GameObject.Destroy(currentHatObject);
			}

			currentHatObject = hat.GetAsset();
			currentHatObject.transform.SetParent(rig.head.rigTarget);
			currentHatObject.transform.localScale = Vector3.one * 0.25f;
			currentHatObject.transform.localPosition = new Vector3(0, 0.365f, 0.04f);
			currentHatObject.transform.localRotation = Quaternion.Euler(0, 90, 10);

			//throw new NotImplementedException();
		}

		public void ResetHat()
		{
			Debug.Log($"Player: {rig.playerName} resetting hat");

			if (currentHatObject != null)
			{
				GameObject.Destroy(currentHatObject);
				currentHatObject = null;
			}

			CurrentHat = null;
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
