using UnityEngine;
namespace ATBS.Core
{
    [CreateAssetMenu(menuName = "Core/Variables/int")]
    public class IntVariable : GenericVariable<int>
    {
        public void Add(int amount)
        {
            Value += amount;
        }

        public void Add(IntVariable amount)
        {
            Value += amount.Value;
        }
    }
}