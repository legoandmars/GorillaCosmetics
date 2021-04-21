using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using UnityEngine;

namespace GorillaCosmetics.Utils
{
    public static class PlayerCosmeticsList
    {
        public struct CosmeticNames
        {
            public string hat;
            public string material;
            public static CosmeticNames Default => new CosmeticNames("custom:None", "default");
            public CosmeticNames(string inputHat, string inputMaterial)
            {
                hat = inputHat;
                material = inputMaterial;
            }
        }

        public static Dictionary<string, CosmeticNames> PlayerToCosmeticsMap = new Dictionary<string, CosmeticNames>();

        public static string GetHat(string userID)
        {
            if (userID == "") return GorillaCosmetics.selectedHat.Value;
            foreach (KeyValuePair<string, CosmeticNames> entry in PlayerToCosmeticsMap)
            {
                if (entry.Key == userID) return entry.Value.hat;
                // do something with entry.Value or entry.Key
            }
            return "None";
        }

        public static string GetMaterial(string userID)
        {
            if (userID == "") return GorillaCosmetics.selectedMaterial.Value;
            foreach (KeyValuePair<string, CosmeticNames> entry in PlayerToCosmeticsMap)
            {
                if (entry.Key == userID) return entry.Value.material;
            }
            return "default";
        }

        public static void SetPlayer(string UserID, string hat, string material)
        {
            PlayerToCosmeticsMap[UserID] = new CosmeticNames(hat, material);
            Debug.Log("SETTING " + UserID);
            Debug.Log(hat);
            Debug.Log(material);
        }
    }
}
