using System;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Combat.Particles
{
    public static class ParticleManager
    {
        public static void SpawnParticle(GameObject particle, Transform target,
            Transform parent,
            Action onParticleFollowedTarget)
        {
            if (particle is null || target is null)
            {
                Debug.LogError("Particle or target is null.");
                return;
            }
            var particleInstance = Object.Instantiate(particle, parent.position, Quaternion.identity).GetComponent<ParticleComponent>();
            particleInstance.FollowTarget(target);
            particleInstance.OnParticleFollowedTarget += onParticleFollowedTarget;
            //Debug.Log($"Spawned particle at {parent.position}");
        }
    }
}