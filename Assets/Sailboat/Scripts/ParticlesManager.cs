using System.Collections;
using UnityEngine;
using Pixelplacement;

namespace Sailboat
{
    public class ParticlesManager : Singleton<ParticlesManager>
    {
        [SerializeField]
        private GameObject splashParticlesPrefab;
        [SerializeField]
        private GameObject directedParticlesPrefab;
        public void splashParticles(Vector3 position, float lifeTime = 0.5f)
        {
            Destroy(Instantiate(splashParticlesPrefab, position, Quaternion.identity),lifeTime);
        }

        public void directedSplashParticles(Vector3 position, Vector2 up, float lifeTime = 0.5f)
        {
            Destroy(Instantiate(directedParticlesPrefab, position, Quaternion.FromToRotation(Vector2.up,up)), lifeTime);
        }
    }
}