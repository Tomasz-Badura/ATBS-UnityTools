using UnityEngine;
namespace ATBS.Core
{
    [CreateAssetMenu(menuName = "Core/Variables/float")]
    public class FloatVariable : GenericVariable<float>
    {
        public void Add(float amount)
        {
            Value += amount;
        }

        public void Add(FloatVariable amount)
        {
            Value += amount.Value;
        }
    }
}