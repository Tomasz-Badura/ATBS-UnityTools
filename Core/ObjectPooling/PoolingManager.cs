using System.Collections.Generic;
using ATBS.Core.TimerUtility;
using UnityEngine;
namespace ATBS.Core.Pooling
{
    public static class PoolingManager
    {
        static List<IPoolHandler> objectPool;
        public static PoolGameObject GetGameObject(string name)
        {
            var go = new GameObject(name);
            PoolGameObject result = go.AddComponent<PoolGameObject>();
            objectPool.Add(result as IPoolHandler);
            return result;
        }

        public static PoolGameObject GetGameObject(string name, float disposeAfter)
        {
            PoolGameObject poolObject = GetGameObject(name);
            DisposeDelayed(poolObject as IPoolHandler, disposeAfter);
            return poolObject;
        }

        public static PoolObject GetPoolObject()
        {
            var po = new PoolObject();
            objectPool.Add(po as IPoolHandler);
            return po;
        }

        public static void DisposeAll()
        {
            for(int i = 0; i < objectPool.Count; i++)
                objectPool[i].Dispose();
        }

        public static void Dispose(IPoolHandler poolHandler)
        {
            objectPool.Find((handler) => handler == poolHandler)?.Dispose();
        }

        public async static void DisposeDelayed(IPoolHandler poolHandler, float disposeAfter)
        {
            await AsyncTimer.DelayReal(disposeAfter);
            objectPool.Find((handler) => handler == poolHandler)?.Dispose();
        }
    }
}