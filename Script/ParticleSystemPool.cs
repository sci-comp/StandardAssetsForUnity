using System.Collections.Generic;
using UnityEngine;

namespace Toolbox
{
    /// <summary>
    /// Manages a pool of particle systems, allowing for efficient reuse of the systems by
    /// activating and deactivating them as needed. The pool is populated with pre-configured
    /// particle systems, and provides a method to trigger a particle system at a specified position.
    /// </summary>
    public class ParticleSystemPool : MonoBehaviour
    {
        [SerializeField] private ParticleSystem[] pool;
        private readonly Queue<ParticleSystem> particleSystemsQueue = new();

        private void Start()
        {
            foreach (ParticleSystem system in pool)
            {
                system.gameObject.SetActive(false);
                particleSystemsQueue.Enqueue(system);
            }
        }

        public void TriggerParticleSystemAt(Vector3 position)
        {
            if (particleSystemsQueue.Count == 0)
            {
                return;
            }

            ParticleSystem systemToUse = particleSystemsQueue.Dequeue();

            if (systemToUse.gameObject.activeSelf)
            {
                systemToUse.Stop();
                systemToUse.Clear();
            }

            systemToUse.transform.position = position;
            systemToUse.gameObject.SetActive(true);
            systemToUse.Play();

            particleSystemsQueue.Enqueue(systemToUse);
        }
    }
}

