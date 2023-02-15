using System;
using System.Collections;
using System.Collections.Generic;
using ATBS.Core.Pooling;
using UnityEngine;

namespace ATBS.Core.TimerUtility
{
    public static class CoroutineTimer
    {
        static Dictionary<string, bool> status;
        public delegate IEnumerator Delay(float seconds, params Action[] callback);
        public delegate IEnumerator DelayCancellable(float seconds, string cancelKey, params Action[] callback);
        public static void CancelTimer(string cancelKey)
        {
            if(status.TryGetValue(cancelKey, out bool result))
                status[cancelKey] = false;
        }

        public static void Run(Delay delayType, float seconds, params Action[] callback)
        {
            PoolingManager.GetGameObject("CoroutineTimer").StartCoroutine(delayType(seconds, callback));
        }

        public static void Run(DelayCancellable delayType, float seconds, string cancelKey, params Action[] callback)
        {
            PoolingManager.GetGameObject("CoroutineTimerCancellable", seconds + 1f).StartCoroutine(delayType(seconds, cancelKey, callback));
        }

        public static IEnumerator DelayDelta(float seconds, params Action[] callback)
        {
            for(; seconds >= 0f; seconds -= Time.deltaTime)
                yield return null;
            foreach(Action call in callback)
                call();
        }

        public static IEnumerator DelayReal(float seconds, params Action[] callback)
        {
            yield return new WaitForSeconds(seconds);
            foreach(Action call in callback)
                call();
        }

        public static IEnumerator DelayDeltaCancellable(float seconds, string cancelKey, params Action[] callback)
        {
            status.Add(cancelKey, true);
            for(; seconds >= 0f; seconds -= Time.deltaTime)
                yield return null;

            if(status.Remove(cancelKey))
            {
                foreach(Action call in callback)
                    call();
            }
        }

        public static IEnumerator DelayRealCancellable(float seconds, string cancelKey, params Action[] callback)
        {
            status.Add(cancelKey, true);
            yield return new WaitForSeconds(seconds);

            if(status.Remove(cancelKey))
            {
                foreach(Action call in callback)
                    call();
            }
        }
    }
}