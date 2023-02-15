using System.Threading.Tasks;
using ATBS.Core.TimerUtility;
using UnityEngine;

namespace ATBS.SceneControlSystem
{
    public abstract class TransitionElement : ScriptableObject 
    {
        [SerializeField] float duration;

        /// <summary>
        /// Starts the transition for this element
        /// </summary>
        /// <param name="transitionObject">helper object</param>
        public virtual async Task StartTransition(GameObject transitionObject)
        {
            transitionObject.AddComponent<UnityEngine.UI.Image>().color = Color.black;
            await AsyncTimer.DelayReal(duration);
            Destroy(transitionObject.GetComponent<UnityEngine.UI.Image>());
        }
    }
}