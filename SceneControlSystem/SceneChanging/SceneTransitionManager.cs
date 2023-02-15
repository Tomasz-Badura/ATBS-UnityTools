using UnityEngine.SceneManagement;
using UnityEngine;
using ATBS.Extensions;
using ATBS.Core.EventSystem;

namespace ATBS.SceneControlSystem
{
    [CreateAssetMenu(fileName = "SceneTransitionManager", menuName = "SceneManagement/TransitionManager")]
    public class SceneTransitionManager : ScriptableObject
    {
        [field: SerializeField] public EventWrapper OnSceneChange { get; private set; }
        public bool InProgress { get; private set; }
        public string CurrentlyLoadingScene { get; private set; }
        
        private Transition currentTransition;
        private bool waitBeforeEnd;

        /// <summary>
        /// Starts a scene transition
        /// </summary>
        /// <param name="sceneName">scene to transition to</param>
        /// <param name="transition">transition to use</param>
        /// <param name="await">wait after scene change</param>
        public void StartTransition(string sceneName, Transition transition, bool await = false)
        {
            if (InProgress) return;
            if (transition.InProgress) return;
            if (!Scenes.SceneList.Contains(sceneName))
            {
                this.LogError("Scene: " + sceneName + ", does not exist.");
                return;
            }
            // setup
            currentTransition = transition;
            waitBeforeEnd = await;
            InProgress = true;
            CurrentlyLoadingScene = sceneName;
            // start transition
            transition.StartTransition();
            transition.OnMiddle += StartSceneChange;
        }

        /// <summary>
        /// 
        /// </summary>
        public void FinishTransition()
        {
            if (currentTransition.InProgress) return;
            currentTransition.FinishTransition();
            InProgress = false;
        }
        
        /// <summary>
        /// 
        /// </summary>
        private void StartSceneChange()
        {
            currentTransition.OnMiddle -= StartSceneChange;
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(CurrentlyLoadingScene);
            if (!waitBeforeEnd)
                asyncOperation.completed += (_) => FinishTransition();
            asyncOperation.completed += (_) => OnSceneChange.Invoke(this, CurrentlyLoadingScene);
        }
    }
}