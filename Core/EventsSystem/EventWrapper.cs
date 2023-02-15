using System;
using ATBS.Extensions;
using UnityEngine;
namespace ATBS.Core.EventSystem
{
    [CreateAssetMenu(fileName = "EventWrapper", menuName = "EventsSystem/EventWrapper")]
    public class EventWrapper : ScriptableObject
    {
#if UNITY_EDITOR
        [SerializeField] string eventText;
#endif
        public event EventHandler<object> eventHandler;

        public void Invoke(object sender, object data)
        {
#if UNITY_EDITOR
            this.LogEvent(eventText);
#endif
            eventHandler?.Invoke(sender, data);
        }
    }
}