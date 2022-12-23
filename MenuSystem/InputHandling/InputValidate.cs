using System;
using System.Text.RegularExpressions;
namespace ATBS.MenuSystem
{
    public static class InputValidate
    {
        // put here your own validation methods
        #region string
        /// <summary>
        /// Determines whether the specified string contains any alphabet letters (including Unicode).
        /// </summary>
        /// <param name="str">The string to check.</param>
        /// <returns>True if the string contains alphabet letters, otherwise, false.</returns>
        public static bool HasAlphabetLetters(string str, bool invert = false)
        {
            if(invert)
                return Regex.IsMatch(str, @"[^a-zA-Z\p{L}]");
            else
                return Regex.IsMatch(str, @"[a-zA-Z\p{L}]");
        }

        /// <summary>
        /// Determines whether the specified string contains any numbers.
        /// </summary>
        /// <param name="str">The string to check.</param>
        /// <returns>True if the string contains numbers, otherwise, false.</returns>
        public static bool HasNumbers(string str, bool invert = false)
        {
            if(invert)
                return Regex.IsMatch(str, @"^\d");
            else
                return Regex.IsMatch(str, @"\d");
        }   

        /// <summary>
        /// Determines whether the specified string contains any special characters (including Unicode, besides numbers and whitespaces).
        /// </summary>
        /// <param name="str">The string to check.</param>
        /// <returns>True if the string contains special characters, otherwise, false.</returns>
        public static bool HasSpecialCharactersUnicode(string str, bool invert = false)
        {
            if(invert)
                return Regex.IsMatch(str, @"[a-zA-Z\d\p{L}\s]");
            else
                return Regex.IsMatch(str, @"[^a-zA-Z\d\p{L}\s]");
        }

        /// <summary>
        /// Determines whether the specified string contains any special characters (not including Unicode, besides numbers and whitespaces).
        /// </summary>
        /// <param name="str">The string to check.</param>
        /// <returns>True if the string contains special characters, otherwise, false.</returns>
        public static bool HasSpecialCharacters(string str, bool invert = false)
        {
            if(invert)
                return Regex.IsMatch(str, @"[a-zA-Z\d\s]");
            else
                return Regex.IsMatch(str, @"[^a-zA-Z\d\s]");
        }
        
        /// <summary>
        /// Determines whether the given string contains only uppercase alphabetical characters that are part of the Unicode character set.
        /// </summary>
        /// <param name="str">The string to validate.</param>
        /// <returns>True if the string contains only uppercase alphabetical characters or is empty, false otherwise.</returns>
        public static bool UpperCaseOnly(string str)
        {
            return Regex.IsMatch(str, @"^[\p{Lu}]*$");
        }

        /// <summary>
        /// Determines whether the given string contains only lowercase alphabetical characters that are part of the Unicode character set.
        /// </summary>
        /// <param name="str">The string to validate.</param>
        /// <returns>True if the string contains only lowercase alphabetical characters or is empty, false otherwise.</returns>
        public static bool LowerCaseOnly(string str)
        {
            return Regex.IsMatch(str, @"^[\p{Ll}]*$");
        }

        /// <summary>
        /// Determines whether the given string is within the specified length range.
        /// </summary>
        /// <param name="str">The string to validate.</param>
        /// <param name="minLength">The minimum allowed length for the string.</param>
        /// <param name="maxLength">The maximum allowed length for the string.</param>
        /// <returns>True if the string is within the specified length range (inclusive), false otherwise.</returns>
        public static bool CheckLength(string str, int minLength, int maxLength)
        {
            return str.Length >= minLength && str.Length <= maxLength;
        }
        #endregion
        #region number
        /// <summary>
        /// Checks if the input value is within the specified range.
        /// </summary>
        /// <typeparam name="T">The type of the input value. Must implement the IComparable interface.</typeparam>
        /// <param name="input">The input value to check.</param>
        /// <param name="min">The minimum value in the range.</param>
        /// <param name="max">The maximum value in the range.</param>
        /// <param name="inclusive">indicates whether the check is inclusive or exclusive</param>
        /// <returns>True if the input value is within the specified range, or false if it is not.</returns>
        public static bool CheckRange<T>(T input, T min, T max, bool inclusive) where T : IComparable
        {
            if(inclusive)
            {
                int inputValue = Convert.ToInt32(input);

                int minValue;
                if(int.TryParse(min.ToString(), out int minimum))
                {
                    minValue = minimum;
                }
                else
                {
                    minValue = int.MinValue;
                }
                int maxValue;
                if(int.TryParse(max.ToString(), out int maximum))
                {
                    maxValue = maximum;
                }
                else
                {
                    maxValue = int.MaxValue;
                }
                return (inputValue >= minValue && inputValue <= maxValue);
            }
            else
                return input.CompareTo(min) >= 0 && input.CompareTo(max) <= 0;
        }
        #endregion
    }
}