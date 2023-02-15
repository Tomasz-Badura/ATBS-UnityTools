using System.Linq;
using UnityEngine;

namespace ATBS.MenuSystem
{   
    [CreateAssetMenu(menuName = "MenuSystem/IncludeCharacters")]
    public class IncludeCharacters : ValidationMethod
    {
        [SerializeField] private char[] includedChars;
        [SerializeField] private bool ignoreCapitalization;

        public override bool Validate(string input)
        {
            foreach (char c in input.ToCharArray())
            {
                bool isValidChar = ignoreCapitalization 
                    ? includedChars.Contains(char.ToUpper(c)) || includedChars.Contains(char.ToLower(c))
                    : includedChars.Contains(c);
                if (!isValidChar) return false;
            }
            return true;
        }
    }
}