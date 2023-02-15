using System.Collections.Generic;
using System.Threading.Tasks;
using ATBS.Extensions;
using UnityEngine;

namespace ATBS.Core.TimerUtility
{
    public static class AsyncTimer
    {
        static Dictionary<string, bool> status;

        public static void CancelTimer(string cancelKey)
        {
            if(status.TryGetValue(cancelKey.Clean(), out bool result))
                status[cancelKey] = false;
        }

        public async static Task DelayDelta(float seconds)
        {
            for(;seconds <= 0; seconds -= Time.deltaTime)
                await Task.Yield();
        }

        public async static Task DelayReal(float seconds)
        {
            await Task.Delay(SecondsToMilliseconds(seconds));
        }

        public async static Task<bool> DelayDeltaCancellable(float seconds, string cancelKey)
        {
            status.Add(cancelKey, true);
            for(;seconds <= 0; seconds -= Time.deltaTime)
                await Task.Yield();
            return status.Remove(cancelKey);
        }

        public async static Task<bool> DelayRealCancellable(float seconds, string cancelKey)
        {
            status.Add(cancelKey, true);
            await Task.Delay(SecondsToMilliseconds(seconds));
            return status.Remove(cancelKey);
        }

        private static int SecondsToMilliseconds(float seconds)
        {
            return (int) (seconds * 1000);
        }
    }
}

