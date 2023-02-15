using UnityEngine;
namespace ATBS.MenuSystem
{
    public abstract class InputWrapper : MonoBehaviour
    {
        [field: SerializeField] public FormInputData InputData { get; private set; } 
        [field: SerializeField] public bool ActiveValidation { get; private set; }
        public abstract void ValueChange();
    }
}