using GorillaCosmetics.Data;
using GorillaNetworking;
using Photon.Realtime;
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
		Material defaultMaterial;

		VRRig rig;
		string NickName => rig.photonView?.Owner?.NickName ?? "SELF";

		void Start()
		{
			rig = GetComponent<VRRig>();
			defaultMaterial = rig.mainSkin.material;

			Player player = rig.photonView?.Owner;
			if (player != null)
			{
				Plugin.CosmeticsNetworker.OnPlayerPropertiesUpdate(player, player.CustomProperties);
			}
		}

		public void SetHat(GorillaHat hat)
		{
			if (hat == null)
			{
				ResetHat();
				return;
			}

			Plugin.Log($"Player: {NickName} switching hat from {CurrentHat?.Descriptor?.Name} to {hat?.Descriptor?.Name}");
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
			Plugin.Log($"Player: {NickName} resetting hat");

			if (currentHatObject != null)
			{
				GameObject.Destroy(currentHatObject);
				currentHatObject = null;
			}

			CurrentHat = null;
		}

		public void SetMaterial(GorillaMaterial material)
		{
			if (material == null)
			{
				ResetMaterial();
				return;
			}

			Plugin.Log($"Player: {NickName} switching material from {CurrentMaterial?.Descriptor?.Name} to {material?.Descriptor?.Name}");

			CurrentMaterial = material;
			SetVRRigMaterial(material.GetMaterial());
		}

		public void ResetMaterial()
		{
			Plugin.Log($"Player: {NickName} resetting material");

			if (defaultMaterial != null)
			{
				SetVRRigMaterial(defaultMaterial);
			}

			CurrentMaterial = null;
		}

		public void SetColor(float red, float blue, float green)
		{
			Plugin.Log($"Player: {NickName} changing color to {red}, {blue}, {green}");
			defaultMaterial.color = new Color(red, blue, green);
		}

		void SetVRRigMaterial(Material material)
		{
			rig.materialsToChangeTo[0] = material;
			if (rig.currentMatIndex == 0)
			{
				rig.ChangeMaterialLocal(0);
			}

			rig.InitializeNoobMaterialLocal(defaultMaterial.color.r, defaultMaterial.color.g, defaultMaterial.color.b);
		}
	}
}
