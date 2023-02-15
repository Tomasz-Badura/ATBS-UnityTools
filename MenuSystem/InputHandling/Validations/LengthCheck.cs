using UnityEngine;

namespace ATBS.MenuSystem
{   
    [CreateAssetMenu(menuName = "MenuSystem/LengthCheck")]
    public class LengthCheck : ValidationMethod
    {
        [SerializeField] int maxLength;
        int expectedLength;
        public override bool Validate(string input)
        {
            int length = input.Length;
            if(expectedLength != 0)
                return length == expectedLength;

            return length <= maxLength;
        }
    }
}