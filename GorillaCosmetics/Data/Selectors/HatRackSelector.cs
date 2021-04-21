using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace GorillaCosmetics.Data.Selectors
{
    public class HatRackSelector : MonoBehaviour
    {
        int selectedIndex = 0;
        public List<GameObject> racks = new List<GameObject>();

        public void Next()
        {
            selectedIndex++;
            if (selectedIndex >= racks.Count)
            {
                selectedIndex = 0;
            }
            UpdateRack();
        }
        public void Previous()
        {
            selectedIndex--;
            if (selectedIndex < 0)
            {
                selectedIndex = racks.Count - 1;
            }
            UpdateRack();
        }

        public void UpdateRack()
        {
            // make sure the right rack from the list is active
            Debug.Log("Updating racks");
            for (int i = 0; i < racks.Count; i++)
            {
                GameObject obj = racks[i];
                obj.SetActive(selectedIndex == i);
            }
        }
    }
}
