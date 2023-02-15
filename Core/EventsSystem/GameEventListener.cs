// Unite 2017 - Game Architecture with Scriptable Objects
// Author: Ryan Hipple
// Date:   10/04/17
using UnityEngine;
using UnityEngine.Events;
namespace ATBS.Core.EventSystem
{
    public class GameEventListener : MonoBehaviour
    {
        [SerializeField, Tooltip("Event to register with.")]
        private GameEvent Event;

        [SerializeField, Tooltip("Response to invoke when Event is raised.")]
        private UnityEvent Response;

        private void OnEnable() => Event.RegisterListener(this);

        private void OnDisable() => Event.UnregisterListener(this);

        public void OnEventRaised() => Response.Invoke();
    }
}