namespace ATBS.Core.UIUtility
{
    public class AutoScrollElement : UnityEngine.MonoBehaviour, UnityEngine.EventSystems.ISelectHandler
    {
        [UnityEngine.SerializeField] AutoScrollView autoScrollView;
        public async virtual void OnSelect(UnityEngine.EventSystems.BaseEventData eventData)
        {
            await System.Threading.Tasks.Task.Yield(); // fixes dropdown positioning delay
            autoScrollView.HandleOnSelectChange(transform as UnityEngine.RectTransform);
        }
    }
}