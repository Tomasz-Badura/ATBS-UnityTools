using UnityEngine;
namespace ATBS.Core
{
    public class ScenePersistent : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}