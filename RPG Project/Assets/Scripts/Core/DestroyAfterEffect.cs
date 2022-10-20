using UnityEngine;

namespace Core
{
    [RequireComponent(typeof(ParticleSystem))]
    public class DestroyAfterEffect : MonoBehaviour
    {
        private ParticleSystem _particleSystem;

        private void Awake()
        {
            _particleSystem = GetComponent<ParticleSystem>();
        }

        void Update()
        {
            if (!_particleSystem.IsAlive())
            {
                Destroy(gameObject);
            }
        }
    }
}