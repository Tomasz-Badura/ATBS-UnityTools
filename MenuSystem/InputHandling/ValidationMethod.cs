using UnityEngine;
namespace ATBS.MenuSystem
{
    public abstract class ValidationMethod : ScriptableObject
    {
        public ValidationCode ValidationCode { get => validationCode; private set => validationCode = value; }
        [SerializeField] private ValidationCode validationCode;
        public abstract bool Validate(string input);
    }
}