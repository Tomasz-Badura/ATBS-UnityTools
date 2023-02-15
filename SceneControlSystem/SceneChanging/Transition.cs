using System;
using ATBS.Core.Pooling;
using UnityEngine;

namespace ATBS.SceneControlSystem
{
    [CreateAssetMenu(menuName = "SceneManagement/Transition")]
    public class Transition : ScriptableObject
    {
        public bool InProgress { get; private set; }
        private GameObject transitionObject;
        public event Action OnStart;
        public event Action OnMiddle;
        public event Action OnEnd;
        public event Action OnEnded;

        [SerializeField] TransitionElement Start;
        [SerializeField] TransitionElement Middle;
        [SerializeField] TransitionElement End;

        /// <summary>
        /// Starts the transition sequence
        /// </summary>
        public async void StartTransition()
        {
            if(InProgress) return;
            InProgress = true;
            OnStart.Invoke();
            await Start.StartTransition(GetGameObject());

            OnMiddle.Invoke();
            await Middle.StartTransition(GetGameObject());
        }

        /// <summary>
        /// Finishes the transition
        /// </summary>
        public async void FinishTransition()
        {
            OnEnd.Invoke();
            await End.StartTransition(GetGameObject());
            InProgress = false;
            OnEnded.Invoke();
            PoolingManager.Dispose(transitionObject.GetComponent<PoolGameObject>() as IPoolHandler);
        }

        /// <summary>
        /// Creates a dont destroy on load game object
        /// </summary>
        /// <returns>created gameobject</returns>
        private GameObject GetGameObject()
        {
            if(transitionObject != null) return transitionObject;
            GameObject go = PoolingManager.GetGameObject("Transition Helper").gameObject;
            DontDestroyOnLoad(go);
            transitionObject = go;
            return go;
        }
    }
}