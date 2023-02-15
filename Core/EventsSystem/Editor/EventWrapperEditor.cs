using UnityEngine;
using UnityEditor;
namespace ATBS.Core.EventSystem
{
    [CustomEditor(typeof(EventWrapper))]
    public class EventWrapperEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EventWrapper e = target as EventWrapper;
            if (GUILayout.Button("Raise"))
                e.Invoke(null, null);
        }
    }
}