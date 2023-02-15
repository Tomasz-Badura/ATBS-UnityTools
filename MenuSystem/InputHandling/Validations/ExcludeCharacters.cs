using System.Linq;
using UnityEngine;

namespace ATBS.MenuSystem
{
    [CreateAssetMenu(menuName = "MenuSystem/ExcludeCharacters")]
    public class ExcludeCharacters : ValidationMethod
    {
        [SerializeField] private char[] excludedChars;
        [SerializeField] private bool ignoreCapitalization;

        public override bool Validate(string input)
        {
            foreach (char c in input.ToCharArray())
            {
                bool isExcludedChar = ignoreCapitalization
                    ? excludedChars.Contains(char.ToUpper(c)) || excludedChars.Contains(char.ToLower(c))
                    : excludedChars.Contains(c);
                if (isExcludedChar) return false;
            }
            return true;
        }
    }
}