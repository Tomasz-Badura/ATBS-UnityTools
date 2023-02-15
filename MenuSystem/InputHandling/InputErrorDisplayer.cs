using UnityEngine;

namespace ATBS.MenuSystem
{
    public abstract class InputErrorDisplayer : MonoBehaviour
    {
        [SerializeField] protected InputWrapper inputWrapper;
        protected abstract void DisplayError(ValidationCode validationCode); 
        private void OnEnable() 
        {
            inputWrapper.InputData.OnValidationError += DisplayError;
        }
        private void OnDisable() 
        {
            inputWrapper.InputData.OnValidationError -= DisplayError;
        }
    }
}