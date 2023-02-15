using UnityEngine;
using UnityEngine.InputSystem;

namespace ATBS.InputSystem.UserInputHandling
{
    [CreateAssetMenu(fileName = "InputValueRequest", menuName = "InputSystem/InputValueRequest")]
    public class InputValueRequest : InputRequest
    {
        [SerializeField] InputActionReference inputAction;
        protected override T OnGetValue<T>(GameObject caller)
        {
            return inputAction.action.ReadValue<T>();
        }
    }
}