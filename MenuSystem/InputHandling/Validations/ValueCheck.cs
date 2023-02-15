using UnityEngine;

namespace ATBS.MenuSystem
{   
    [CreateAssetMenu(menuName = "MenuSystem/ValueCheck")]
    public class ValueCheck : ValidationMethod
    {
        private enum ParseType
        {
            Float,
            Int
        }

        [SerializeField] ParseType parseType;
        [SerializeField] Vector2 minMax;
        [SerializeField] bool expectValue;
        [SerializeField] int expectedIntValue;
        [SerializeField] float expectedFloatValue;
        [SerializeField] float precisionLeeway;

        public override bool Validate(string input)
        {
            switch(parseType)
            {
                case ParseType.Float:
                {
                    float value = float.Parse(input);
                    if(expectValue)
                        return value >= expectedFloatValue - precisionLeeway || value <= expectedFloatValue + precisionLeeway;

                    return value >= minMax.x && value <= minMax.y;
                }
                case ParseType.Int:
                {
                    if(expectValue)
                        return int.Parse(input) == expectedIntValue;

                    return int.Parse(input) >= minMax.x && int.Parse(input) <= minMax.y;
                }
            }
            return true;
        }
    }
}