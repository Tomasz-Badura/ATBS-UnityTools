using UnityEngine;
using UnityEngine.InputSystem;

namespace ATBS.InputSystem.UserInputHandling
{
    [CreateAssetMenu(menuName = "InputSystem/UIActionMap", fileName = "UIActionMap")]
    public class UIActionMap : ScriptableObject
    {
        [field: SerializeField] public InputActionReference Point { get; private set; }
        [field: SerializeField] public InputActionReference LeftClick { get; private set; }
        [field: SerializeField] public InputActionReference MiddleClick { get; private set; }
        [field: SerializeField] public InputActionReference RightClick { get; private set; }
        [field: SerializeField] public InputActionReference ScrollWheel { get; private set; }
        [field: SerializeField] public InputActionReference Move { get; private set; }
        [field: SerializeField] public InputActionReference Submit { get; private set; }
        [field: SerializeField] public InputActionReference Cancel { get; private set; }
        [field: SerializeField] public InputActionReference TrackedPosition { get; private set; }
        [field: SerializeField] public InputActionReference TrackedOrientation { get; private set; }
    }
}