using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat.Particles
{
    public class ParticleComponent:MonoBehaviour,IParticle
    {
        [field:Header("Particle Properties")]
        [field:SerializeField]public float Speed { get; set; }
        
        public event Action OnParticleFollowedTarget;
        public void FollowTarget(Transform target)
        {
            if (target == null)
            {
                Debug.LogWarning("Target is null, cannot follow.");
                return;
            }
            StopAllCoroutines();
            StartCoroutine(FollowTargetCoroutine(target));
            
        }
        private IEnumerator FollowTargetCoroutine(Transform target)
        {
            while (target is not null)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, Speed * Time.deltaTime);
                var direction = (target.position - transform.position).normalized;
                var lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Speed * Time.deltaTime);
                
                if (Vector3.Distance(transform.position, target.position) < 0.1f)// Check if the particle is close enough to the target
                {
                    OnParticleFollowedTarget?.Invoke();
                    Debug.Log($"Particle reached target at {target.position}");
                    Destroy(gameObject); // Destroy the particle after reaching the target
                    yield break;
                }
                yield return null;
            }
        }
    }
}