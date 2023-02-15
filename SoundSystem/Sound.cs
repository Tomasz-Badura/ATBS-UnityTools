using UnityEngine;
namespace ATBS.Audio
{
    public abstract class Sound : ScriptableObject
    {
        [field: SerializeField] public string SoundName { get; protected set; }
        [field: SerializeField] public System.Collections.Generic.List<AudioClip> Clips { get; protected set; }
        [field: SerializeField] public SoundPlayOrder PlayOrder { get; private set; }
        [field: SerializeField] public int PlayIndex { get; private set; }

        #region Virtual methods
        public virtual void Play(AudioSource source) { }
        public virtual void PlayOneShot(AudioSource source) { }
        public virtual void PlayDelayed(AudioSource source, float delay) { }
        public virtual void PlayScheduled(AudioSource source, double time) { }
        #endregion

        /// <summary>
        /// Gets an audio clip based on current PlayOrder
        /// </summary>
        /// <returns> AudioClip from Clips list </returns>
        public AudioClip GetAudioClip()
        {
            var clip = Clips[PlayIndex >= Clips.Count - 1 ? 0 : PlayIndex];
            switch (PlayOrder)
            {
                case SoundPlayOrder.Random:
                    PlayIndex = Random.Range(0, Clips.Count);
                    break;
                case SoundPlayOrder.Reverse:
                    PlayIndex = (PlayIndex + Clips.Count - 1) % Clips.Count;
                    break;
                case SoundPlayOrder.InOrder:
                    PlayIndex = (PlayIndex + 1) % Clips.Count;
                    break;
                case SoundPlayOrder.RandomNonRepeating:
                    if(Clips.Count < 2) break;
                    int x = PlayIndex;
                    while (PlayIndex == x)
                        PlayIndex = Random.Range(0, Clips.Count);
                    break;
            }
            return clip;
        }
    }
}