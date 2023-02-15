using UnityEngine;
namespace ATBS.Core.UIUtility
{
    public abstract class Displayer : MonoBehaviour
    {
        public virtual void UpdateVisuals() { }
        public virtual void Show() { }
        public virtual void Hide() { }
        protected virtual void OnEnable() { UpdateVisuals(); }
        protected virtual void OnDisable() { }
    }
}