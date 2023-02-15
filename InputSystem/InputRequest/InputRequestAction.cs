using System;
using ATBS.Core.PauseControl;
using UnityEngine;
namespace ATBS.InputSystem
{
    public class InputRequestAction : ScriptableObject
    {
        [SerializeField] bool ignorePause = true;
        public event Action Started;
        public event Action Performed;
        public event Action Canceled;

        public void OnStarted()
        {
            if(ignorePause ? true : !PauseAgent.IsPaused)
                Started?.Invoke();
        }
        public void OnPerformed()
        {
            if(ignorePause ? true : !PauseAgent.IsPaused)
                Performed?.Invoke();
        }
        public void OnCanceled()
        {
            if(ignorePause ? true : !PauseAgent.IsPaused)
                Canceled?.Invoke();
        }
    }
}