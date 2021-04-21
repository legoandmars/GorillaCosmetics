using System;
using System.Collections;
using System.Text;
using UnityEngine;

namespace GorillaCosmetics.Data.Selectors
{
    public class HatRackSelectorButton : MonoBehaviour
    {
        bool canPress = true;
        bool next;

        public HatRackSelector selector;

        void Awake()
        {
            if (gameObject.name == "Left") next = false;
            else next = true;
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (!canPress) return;

            if(collider.GetComponentInParent<GorillaTriggerColliderHandIndicator>() != null && selector != null)
            {
                GorillaTriggerColliderHandIndicator component = collider.GetComponent<GorillaTriggerColliderHandIndicator>();

                canPress = false;
                if (next) selector.Next();
                else selector.Previous();

                if(component != null)
                {
                    GorillaTagger.Instance.StartVibration(component.isLeftHand, GorillaTagger.Instance.tapHapticStrength / 2f, GorillaTagger.Instance.tapHapticDuration);
                }

                StartCoroutine(ButtonDelay(this));
            }
        }
        private IEnumerator ButtonDelay(HatRackSelectorButton button)
        {
            yield return new WaitForSeconds(0.150f);
            button.canPress = true;
        }

    }
}
