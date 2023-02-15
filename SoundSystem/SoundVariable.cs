using UnityEngine;
namespace ATBS.Audio
{
    [CreateAssetMenu(fileName = "Sound", menuName = "SoundSystem/SoundVariable")]
    public class SoundVariable : Sound
    {
        #region Methods
        public override void Play(AudioSource source)
        {
            source.clip = GetAudioClip();
            source.Play();
        }

        public override void PlayOneShot(AudioSource source)
        {
            source.PlayOneShot(GetAudioClip());
        }

        public override void PlayDelayed(AudioSource source, float delay)
        {
            source.clip = GetAudioClip();
            source.PlayDelayed(delay);
        }

        public override void PlayScheduled(AudioSource source, double time)
        {
            source.clip = GetAudioClip();
            source.PlayScheduled(time);
        }
        #endregion
    }
}