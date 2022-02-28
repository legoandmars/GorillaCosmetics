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
		public int MatIndex { get; private set; }

		public GorillaHat CurrentHat { get; private set; }

		public GorillaMaterial CurrentMaterial { get; private set; }

		GameObject currentHatObject;
		Material defaultMaterial;
		Material customMaterial;

		VRRig rig;
		string NickName => rig.photonView?.Owner?.NickName ?? "SELF";

		void Start()
		{
			rig = GetComponent<VRRig>();
			defaultMaterial = rig.mainSkin.material;
			MatIndex = rig.materialsToChangeTo.Length - 1;

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

		public void SetColor(float red, float green, float blue)
		{
			Plugin.Log($"Player: {NickName} changing color to {red}, {green}, {blue}");

			Color newColor = new Color(red, green, blue);
			defaultMaterial.color = newColor;

			if (CurrentMaterial != null) 
			{
				Material myMat = rig.materialsToChangeTo[MatIndex];
				if(myMat != null && (CurrentMaterial.Descriptor.CustomColors || myMat.HasProperty("_Color"))) 
				{
					myMat.color = newColor;
				}		
			}
		}

		void SetVRRigMaterial(Material material)
		{
			rig.materialsToChangeTo[MatIndex] = material;

			if (rig.currentMatIndex == 0)
			{
				rig.ChangeMaterialLocal(MatIndex);
			}

			rig.InitializeNoobMaterialLocal(defaultMaterial.color.r, defaultMaterial.color.g, defaultMaterial.color.b);
		}
	}
}
