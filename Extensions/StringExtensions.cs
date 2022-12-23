using System.Text.RegularExpressions;
namespace ATBS.Extensions
{
    public static partial class StringExtensions
    {
        /// <summary>
        /// normalizes a string into a more consistent format.
        /// </summary>
        /// <param name="str"> string to Normalize </param>
        /// <returns> normalized string </returns>
        public static string Clean(this string str)
        {
            // check if null
            if (str == null)
                return "";
            // Remove all whitespaces, special characters, and numbers
            str = Regex.Replace(str, @"[^A-Za-z]", "");
            // Convert the remaining characters to uppercase
            return str.ToUpperInvariant();
        }
    }
}