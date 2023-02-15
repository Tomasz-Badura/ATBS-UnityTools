using UnityEngine;
using UnityEngine.InputSystem;

namespace ATBS.InputSystem.UserInputHandling
{
    [CreateAssetMenu(fileName = "InputActionRequest", menuName = "InputSystem/InputActionRequest")]
    public class InputActionRequest : InputRequestAction
    {
        [SerializeField] InputActionReference inputAction;
        private void Call(InputAction.CallbackContext callbackContext)
        {
            switch (callbackContext.phase)
            {
                case InputActionPhase.Started:
                    OnStarted();
                    break;
                case InputActionPhase.Performed:
                    OnPerformed();
                    break;
                case InputActionPhase.Canceled:
                    OnCanceled();
                    break;
            }
        }

        private void OnEnable()
        {
            if (inputAction == null) return;
            inputAction.action.started += Call;
            inputAction.action.performed += Call;
            inputAction.action.canceled += Call;
        }

        private void OnDisable()
        {
            if (inputAction == null) return;
            inputAction.action.started -= Call;
            inputAction.action.performed -= Call;
            inputAction.action.canceled -= Call;
        }
    }
}