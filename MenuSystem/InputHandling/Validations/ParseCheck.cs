using System;
using UnityEngine;

namespace ATBS.MenuSystem
{
    [CreateAssetMenu(menuName = "MenuSystem/ParseCheck")]
    public class ParseCheck : ValidationMethod
    {
        private enum ParseType
        {
            Float,
            Decimal,
            Double,
            UInt,
            Int,
            Date,
            Bool
        }

        [SerializeField] ParseType parseType;
        public override bool Validate(string input)
        {
            switch (parseType)
            {
                case ParseType.Float:
                    return float.TryParse(input, out float floatResult);
                case ParseType.Decimal:
                    return decimal.TryParse(input, out decimal decimalResult);
                case ParseType.Double:
                    return double.TryParse(input, out double doubleResult);
                case ParseType.UInt:
                    return uint.TryParse(input, out uint uintResult);
                case ParseType.Int:
                    return int.TryParse(input, out int intResult);
                case ParseType.Date:
                    return DateTime.TryParse(input, out DateTime dateResult);
                case ParseType.Bool:
                    return bool.TryParse(input, out bool result);
                default:
                    return false;
            }
        }
    }
}