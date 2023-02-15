using UnityEngine;

namespace ATBS.MenuSystem
{
    [CreateAssetMenu(menuName = "MenuSystem/BoolCheck")]
    public class BoolCheck : ValidationMethod
    {
        [SerializeField] private bool expectedValue;
        public override bool Validate(string input)
        {
            if(bool.TryParse(input, out bool result))
                return result == expectedValue;
            return false;
        }
    }
}