using GorillaCosmetics.Data;
using GorillaNetworking;
using HarmonyLib;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace GorillaCosmetics
{
	[DisallowMultipleComponent]
	public class CustomCosmeticsController : MonoBehaviourPunCallbacks, ICustomCosmeticsController
	{
		public int MatIndex { get; private set; }

		public GorillaHat CurrentHat { get; private set; }

		public GorillaMaterial CurrentMaterial { get; private set; }

		GameObject currentHatObject;
		Material defaultMaterial;

		VRRig Rig;
		bool Initalized;
        public Player Player;

        void Start()
		{
			if (Initalized) return;
			Initalized = true;
            TryGetComponent(out Rig);

			var tempMatArray = Rig.materialsToChangeTo;
			Rig.materialsToChangeTo = new Material[tempMatArray.Length + 1];

			for (int index = 0; index < tempMatArray.Length; index++) {
				Rig.materialsToChangeTo[index] = tempMatArray[index];
			}

			MatIndex = Rig.materialsToChangeTo.Length - 1;
			defaultMaterial = Rig.materialsToChangeTo[0];
			Rig.materialsToChangeTo[MatIndex] = tempMatArray[0];

			if (Player != null) Plugin.CosmeticsNetworker.OnPlayerPropertiesUpdate(Player, Player.CustomProperties);
        }

		public void SetHat(GorillaHat hat)
		{
			if (hat == null)
			{
				ResetHat();
				return;
			}

			Plugin.Log($"Player: {Rig.playerText.text} switching hat from {CurrentHat?.Descriptor?.Name} to {hat?.Descriptor?.Name}");
			CurrentHat = hat;

			if (currentHatObject != null)
			{
				GameObject.Destroy(currentHatObject);
			}

			currentHatObject = hat.GetAsset();
			currentHatObject.transform.SetParent(Rig.head.rigTarget);
			currentHatObject.transform.localScale = Vector3.one * 0.25f;
			currentHatObject.transform.localPosition = new Vector3(0, 0.365f, 0.04f);
			currentHatObject.transform.localRotation = Quaternion.Euler(0, 90, 10);

			//throw new NotImplementedException();
		}

		public void ResetHat()
		{
			Plugin.Log($"Player: {Rig.playerText.text} resetting hat");

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

			Plugin.Log($"Player: {Rig.playerText.text} switching material from {CurrentMaterial?.Descriptor?.Name} to {material?.Descriptor?.Name}");

			CurrentMaterial = material;
			SetVRRigMaterial(material.GetMaterial());
		}

		public void ResetMaterial()
		{
			Plugin.Log($"Player: {Rig.playerText.text} resetting material");

			if (defaultMaterial != null)
			{
				SetVRRigMaterial(defaultMaterial);
			}

			CurrentMaterial = null;
		}

		public void SetColor(float red, float green, float blue)
		{
			if (Rig == null) return;
			Plugin.Log($"Player: {Rig.playerText.text} changing color to {red}, {green}, {blue}");

			Color newColor = new Color(red, green, blue);
			defaultMaterial.color = newColor;

			if (CurrentMaterial != null) 
			{
				Material myMat = Rig.materialsToChangeTo[MatIndex];
				if (myMat != null && CurrentMaterial.Descriptor.CustomColors && myMat.HasProperty("_Color")) myMat.color = newColor;
            }
		}

		void SetVRRigMaterial(Material material)
		{
			Rig.materialsToChangeTo[MatIndex] = material;

			if (Rig.setMatIndex == 0)
			{
				Rig.ChangeMaterialLocal(0);
			}

			Rig.InitializeNoobMaterialLocal(defaultMaterial.color.r, defaultMaterial.color.g, defaultMaterial.color.b, GorillaComputer.instance.leftHanded);
		}
    }
}
