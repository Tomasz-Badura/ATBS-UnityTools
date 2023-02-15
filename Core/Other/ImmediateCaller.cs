using UnityEngine;
using UnityEngine.Events;
namespace ATBS.Core
{
    public class ImmediateCaller : MonoBehaviour
    {
        [SerializeField] private UnityEvent awakeEvent;
        [SerializeField] private UnityEvent onEnableEvent;
        [SerializeField] private UnityEvent startEvent;
        [SerializeField] private UnityEvent onDestroyEvent;

        private void Awake()
        {
            awakeEvent?.Invoke();
        }

        private void OnEnable()
        {
            onEnableEvent?.Invoke();
        }

        private void Start()
        {
            startEvent?.Invoke();
        }

        private void OnDestroy()
        {
            onDestroyEvent?.Invoke();
        }
    }
}