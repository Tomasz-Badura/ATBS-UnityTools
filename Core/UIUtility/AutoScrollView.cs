using UnityEngine;
using UnityEngine.UI;
namespace ATBS.Core.UIUtility
{
    public class AutoScrollView : MonoBehaviour
    {
        [SerializeField] RectTransform viewportRect;
        [SerializeField] RectTransform content;
        [SerializeField] float transitionDuration = 0.2f;
        VerticalLayoutGroup contentLayoutGroup;
        private TransitionHelper transitionHelper = new();

        private void Awake()
        {
            contentLayoutGroup = content.GetComponent<VerticalLayoutGroup>();
        }

        void Update()
        {
            if (transitionHelper.InProgress)
            {
                transitionHelper.Update();
                content.transform.localPosition = transitionHelper.PosCurrent;
            }
        }

        public void HandleOnSelectChange(RectTransform elementRectTransform)
        {
            float viewportLowestPoint = viewportRect.rect.yMin;
            float viewportHighestPoint = viewportRect.rect.yMax;
            Vector2 elementPosition = viewportRect.InverseTransformPoint(elementRectTransform.position);
            float elementTopEdge = elementPosition.y + elementRectTransform.rect.height / 2;
            float elementBottomEdge = elementPosition.y - elementRectTransform.rect.height / 2;

            if (elementTopEdge > viewportHighestPoint)
            {
                float scrollAmount = viewportHighestPoint - elementTopEdge;
                MoveContentObjectByAmount(scrollAmount + contentLayoutGroup.padding.top);
            }
            
            if (elementBottomEdge < viewportLowestPoint)
            {
                float scrollAmount = viewportLowestPoint - elementBottomEdge;
                MoveContentObjectByAmount(scrollAmount + contentLayoutGroup.padding.bottom);
            }
        }

        private void MoveContentObjectByAmount(float amount)
        {
            Vector2 pos = content.transform.localPosition;
            transitionHelper.TransitionPositionFromTo(pos, new Vector2(pos.x, pos.y + amount), transitionDuration);
        }

        private class TransitionHelper
        {
            public bool InProgress { get; private set; }
            public Vector2 PosCurrent { get => posCurrent; private set => posCurrent = value; }
            float duration;
            float timeElapsed;
            float progress;
            Vector2 posFrom;
            Vector2 posTo;
            private Vector2 posCurrent;

            public void Update()
            {
                Tick();
                CalculatePosition();
            }

            public void Clear()
            {
                duration = 0f;
                timeElapsed = 0f;
                progress = 0f;
                InProgress = false;
            }

            public void TransitionPositionFromTo(Vector2 posFrom, Vector2 posTo, float duration)
            {
                Clear();
                this.posFrom = posFrom;
                this.posTo = posTo;
                this.duration = duration;

                InProgress = true;
            }

            private void CalculatePosition()
            {
                posCurrent.x = Mathf.Lerp(posFrom.x, posTo.x, progress);
                posCurrent.y = Mathf.Lerp(posFrom.y, posTo.y, progress);
            }

            private void Tick()
            {
                if (!InProgress) return;

                timeElapsed += Time.unscaledDeltaTime;
                progress = timeElapsed / duration;
                if (progress >= 1f)
                {
                    progress = 1f;
                    InProgress = false;
                }
            }
        }

    }
}