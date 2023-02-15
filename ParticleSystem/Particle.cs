using UnityEngine;
using ATBS.Core.TimerUtility;
namespace ATBS.ParticleManagement
{
    [RequireComponent(typeof(ParticleSystem))]
    public class Particle : MonoBehaviour
    {
        [SerializeField] ParticleManager particleManager;
        float lifeTimeDuration;
        ParticleSystem system;

        private void Awake() 
        {
            system = GetComponent<ParticleSystem>();
            lifeTimeDuration = system.main.duration;
        }

        public void Play()
        {
            system.Play();
        }

        public void Pause()
        {
            system.Pause();
        }

        public async void Lifetime()
        {
            particleManager.Particles.Add(this);
            if(system.main.loop) return;
            await AsyncTimer.DelayDelta(lifeTimeDuration);
            particleManager.Particles.Remove(this);
            Destroy(gameObject);
        }
    }
}