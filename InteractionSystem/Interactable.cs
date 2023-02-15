using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace ATBS.InteractionSystem
{
    public sealed class Interactable : MonoBehaviour
    {
        private InteractionTaker interactionTaker;

        #region Events

        public delegate void InteractionHandler();
        public delegate void InteractibilityHandler(bool state);
        public event InteractionHandler OnInteracted;
        public event InteractibilityHandler InRange;
        public event InteractibilityHandler OnClosest;
        #endregion
        #region Methods
        public void Interact()
        {
            OnInteracted?.Invoke();
        }

        public void IsClosest(bool state)
        {
            OnClosest?.Invoke(state);
        }

        public void OnTriggerEnter2D(Collider2D collider)
        {
            interactionTaker = collider.transform.GetComponent<InteractionTaker>();
            if(interactionTaker != null)
            {
                InRange?.Invoke(true);
                interactionTaker.InRange.Add(this, this);
            }
        }
        public void OnTriggerExit2D(Collider2D collider)
        {
            InteractionTaker interactionTaker = collider.transform.GetComponent<InteractionTaker>();
            if(interactionTaker != null)
            {
                InRange?.Invoke(false);
                interactionTaker.InRange.Remove(this);
            }
        }

        private void OnDisable()
        {
            interactionTaker?.InRange.Remove(this);
        }
        #endregion
    }
}