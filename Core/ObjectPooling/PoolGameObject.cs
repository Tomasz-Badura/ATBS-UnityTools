namespace ATBS.Core.Pooling
{
    public class PoolGameObject : UnityEngine.MonoBehaviour, IPoolHandler
    {
        public void Dispose() => Destroy(gameObject);
    }
}