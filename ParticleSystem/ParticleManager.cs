using System.Collections.Generic;
using UnityEngine;
namespace ATBS.ParticleManagement
{
    [CreateAssetMenu(fileName = "ParticleManager", menuName = "ParticleSystem/ParticleManager")]
    public class ParticleManager : ScriptableObject
    {
        public List<Particle> Particles { get => particles; private set => particles = value; }
        private List<Particle> particles = new();
        public Particle CreateParticle(Vector3 position, Quaternion rotation, GameObject prefab)
        {
            GameObject go = Instantiate(prefab, position, rotation);
            Particle particle = go.GetComponent<Particle>();
            if (particle == null)
            {
                Debug.LogWarning("given prefab doesn't contain Particle component");
                Destroy(go);
                return null;
            }
            particle.Lifetime();
            return particle;
        }

        public void DestroyAllParticles()
        {
            foreach (Particle particle in Particles)
            {
                Particles.Remove(particle);
                Destroy(particle.gameObject);
            }
        }

        private void OnDisable()
        {
            particles = null;
        }
    }
}