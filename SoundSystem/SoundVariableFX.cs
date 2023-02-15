using ATBS.Editor;
using UnityEngine;
namespace ATBS.Audio
{
    [CreateAssetMenu(fileName = "Sound", menuName = "SoundSystem/SoundVariable")]
    public class SoundVariableFX : Sound
    {
        #region Variables
        [field: MinMaxSlider(0, 1), SerializeField] public Vector2 Volume { get; private set; }
        [field: MinMaxSlider(0, 1), SerializeField] public Vector2 SpatialBlend { get; private set; }
        [field: MinMaxSlider(-3, 3), SerializeField] public Vector2 Pitch { get; private set; }
        [field: MinMaxSlider(-1, 1), SerializeField] public Vector2 StereoPan { get; private set; }
        [field: SerializeField] public bool Loop { get; private set; }
        #endregion
        #region Methods
        #region Play
        public override void Play(AudioSource source)
        {
            SetupSource(source);
            source.clip = GetAudioClip();
            source.Play();
        }

        public override void PlayOneShot(AudioSource source)
        {
            SetupSource(source);
            source.PlayOneShot(GetAudioClip());
        }

        public override void PlayDelayed(AudioSource source, float delay)
        {
            SetupSource(source);
            source.clip = GetAudioClip();
            source.PlayDelayed(delay);
        }

        public override void PlayScheduled(AudioSource source, double time)
        {
            SetupSource(source);
            source.clip = GetAudioClip();
            source.PlayScheduled(time);
        }
        #endregion
        public void GetRandomValues(out float pitch, out float volume, out float stereoPan, out float spatialBlend)
        {
            pitch = Random.Range(Pitch.x, Pitch.y);
            volume = Random.Range(Volume.x, Volume.y);
            stereoPan = Random.Range(StereoPan.x, StereoPan.y);
            spatialBlend = Random.Range(SpatialBlend.x, SpatialBlend.y);
        }

        public void SetupSource(AudioSource source)
        {
            GetRandomValues(out float pitch, out float volume, out float stereoPan, out float spatialBlend);
            source.clip = GetAudioClip();
            source.pitch = pitch;
            source.volume = volume;
            source.panStereo = stereoPan;
            source.spatialBlend = spatialBlend;
            source.loop = Loop;
        }
        #endregion
    }
}