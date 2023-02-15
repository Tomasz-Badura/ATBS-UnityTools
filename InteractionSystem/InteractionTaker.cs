using System.Collections.Generic;
using ATBS.InputSystem;
using UnityEngine;

namespace ATBS.InteractionSystem
{
    public sealed class InteractionTaker : MonoBehaviour
    {
        private class InteractableComparer : IComparer<Interactable>
        {
            private Transform takerTransform;

            public InteractableComparer(Transform takerTransform)
            {
                this.takerTransform = takerTransform;
            }
            public int Compare(Interactable x, Interactable y)
            {
                if(takerTransform == null) return 0;
                float distanceX = Vector3.Distance(x.transform.position, takerTransform.position);
                float distanceY = Vector3.Distance(y.transform.position, takerTransform.position);
                return distanceX.CompareTo(distanceY);
            }
        }

        public SortedList<Interactable, Interactable> InRange { get; private set; }
        public bool IsInteracting { get; set; }
        public Interactable LastClosest { get; private set; }
        [SerializeField] private InputRequestAction interactInput;
        public void InteractWithClosest()
        {
            if (!IsInteracting)
                return;
            if(InRange.Count == 0) return;
            Interactable closestInteractable = InRange.Values[0];
            closestInteractable?.Interact();
        }

        private void Awake()
        {
            IsInteracting = true;
            InRange = new SortedList<Interactable, Interactable>(new InteractableComparer(transform));
        }

        private void OnEnable() 
        {
            interactInput.Performed += InteractWithClosest;    
        }

        private void OnDisable()
        {
            interactInput.Performed -= InteractWithClosest; 
            InRange.Clear();
        }

        private void Update()
        {
            UpdateClosest();
        }


        private void UpdateClosest()
        {
            if(InRange.Count == 0) return;
            Interactable closestInteractable = InRange.Values[0];
            if (closestInteractable != LastClosest)
            {
                LastClosest?.IsClosest(false);
                closestInteractable?.IsClosest(true);
                LastClosest = closestInteractable;
            }
        }
    }
}