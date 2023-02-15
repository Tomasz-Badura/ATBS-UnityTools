// Unite 2017 - Game Architecture with Scriptable Objects
// Author: Ryan Hipple
// Date:   10/04/17
using UnityEngine;
namespace ATBS.Core
{
    public class GenericVariable<T> : ScriptableObject
    {
#if UNITY_EDITOR
        [Multiline]
        public string DeveloperDescription = "";
#endif
        [field: SerializeField] public T Value { get; protected set; }
        [SerializeField] private T defaultValue;

        public void SetDefault()
        {
            Value = defaultValue;
        }

        public void SetAsDefault()
        {
            defaultValue = Value;
        }

        public void SetValue(T value)
        {
            Value = value;
        }

        public void SetValue(GenericVariable<T> value)
        {
            Value = value.Value;
        }
    }
}