using ATBS.Core.PauseControl;
using UnityEngine;
namespace ATBS.InputSystem
{
    public abstract class InputRequest : ScriptableObject
    {
        [SerializeField] bool ignorePause = true;
        public T GetValue<T>(GameObject caller) where T : struct
        {
            if(ignorePause ? true : !PauseAgent.IsPaused)
                return OnGetValue<T>(caller);
            else
                return default(T);
        }
        protected abstract T OnGetValue<T>(GameObject caller) where T : struct;
    }
}