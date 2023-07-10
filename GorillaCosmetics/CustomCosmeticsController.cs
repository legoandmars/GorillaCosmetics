using GorillaCosmetics.Data;
using GorillaNetworking;
using HarmonyLib;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace GorillaCosmetics
{
	public class CustomCosmeticsController : MonoBehaviourPunCallbacks, ICustomCosmeticsController
	{
		public int MatIndex { get; private set; }

		public GorillaHat CurrentHat { get; private set; }

		public GorillaMaterial CurrentMaterial { get; private set; }

		GameObject currentHatObject;
		Material defaultMaterial;

		VRRig rig;
		Player currentPlayer;

        void Start()
		{
			rig = GetComponent<VRRig>();

			var tempMatArray = rig.materialsToChangeTo;
			rig.materialsToChangeTo = new Material[tempMatArray.Length + 1];

			for (int index = 0; index < tempMatArray.Length; index++) {
				rig.materialsToChangeTo[index] = tempMatArray[index];
			}

			MatIndex = rig.materialsToChangeTo.Length - 1;
			defaultMaterial = rig.materialsToChangeTo[0];
			rig.materialsToChangeTo[MatIndex] = tempMatArray[0];

            currentPlayer = Traverse.Create(rig).Field("creator").GetValue() as Player;
            if (currentPlayer != null)
			{
				Plugin.CosmeticsNetworker.OnPlayerPropertiesUpdate(currentPlayer, currentPlayer.CustomProperties);
			}
		}

		public void SetHat(GorillaHat hat)
		{
			if (hat == null)
			{
				ResetHat();
				return;
			}

			Plugin.Log($"Player: {rig.playerText.text} switching hat from {CurrentHat?.Descriptor?.Name} to {hat?.Descriptor?.Name}");
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
			Plugin.Log($"Player: {rig.playerText.text} resetting hat");

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

			Plugin.Log($"Player: {rig.playerText.text} switching material from {CurrentMaterial?.Descriptor?.Name} to {material?.Descriptor?.Name}");

			CurrentMaterial = material;
			SetVRRigMaterial(material.GetMaterial());
		}

		public void ResetMaterial()
		{
			Plugin.Log($"Player: {rig.playerText.text} resetting material");

			if (defaultMaterial != null)
			{
				SetVRRigMaterial(defaultMaterial);
			}

			CurrentMaterial = null;
		}

		public void SetColor(float red, float green, float blue)
		{
			if (rig == null) return;
			Plugin.Log($"Player: {rig.playerText.text} changing color to {red}, {green}, {blue}");

			Color newColor = new Color(red, green, blue);
			defaultMaterial.color = newColor;

			if (CurrentMaterial != null) 
			{
				Material myMat = rig.materialsToChangeTo[MatIndex];
				if (myMat != null && CurrentMaterial.Descriptor.CustomColors && myMat.HasProperty("_Color")) myMat.color = newColor;
            }
		}

		void SetVRRigMaterial(Material material)
		{
			rig.materialsToChangeTo[MatIndex] = material;

			if (rig.currentMatIndex == 0)
			{
				rig.ChangeMaterialLocal(0);
			}

			rig.InitializeNoobMaterialLocal(defaultMaterial.color.r, defaultMaterial.color.g, defaultMaterial.color.b, GorillaComputer.instance.leftHanded);
		}

        public override void OnLeftRoom()
        {
            base.OnLeftRoom();

			if (currentPlayer != null && !currentPlayer.IsLocal)
			{
                ResetHat();
                ResetMaterial();
			}
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            base.OnPlayerLeftRoom(otherPlayer);

			if (otherPlayer == currentPlayer)
			{
                ResetHat();
                ResetMaterial();
            }
        }
    }
}
