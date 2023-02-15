namespace ATBS.Core.PauseControl
{
    public static class PauseAgent
    {
        public static bool IsPaused { get; private set; } = false;
        public static event System.EventHandler OnPaused;
        public static event System.EventHandler OnUnPaused;

        /// <summary>
        /// Changes the current state to paused if not already paused
        /// </summary>
        public static void Pause()
        {
            if (IsPaused) return;
            IsPaused = true;
            OnPaused?.Invoke(null, null);
        }

        /// <summary>
        /// Changes the current state to unpaused if not already unpaused
        /// </summary>
        public static void UnPause()
        {
            if (!IsPaused) return;
            IsPaused = false;
            OnUnPaused?.Invoke(null, null);
        }

        /// <summary>
        /// Changes the current state to paused if not already paused without calling events
        /// </summary>
        public static void SilentPause() => IsPaused = true;

        /// <summary>
        /// Changes the current state to unpaused if not already unpaused without calling events
        /// </summary>
        public static void SilentUnPause() => IsPaused = false;
    }
}