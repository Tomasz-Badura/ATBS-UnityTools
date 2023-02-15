using UnityEngine;

namespace ATBS.Core.SavingSystem
{
    /// <summary>
    /// Saveable scriptable objects
    /// </summary>
    /// <typeparam name="T">Data container backing this scriptable object</typeparam>
    public abstract class ScriptableContainer<T> : ScriptableObject, IResolver where T : DataContainer
    {
        [field: SerializeField] public T Data { get; private set; }
        public void Load(DataContainer data) => Data = data as T;
        public DataContainer Save() => Data;
    }
}