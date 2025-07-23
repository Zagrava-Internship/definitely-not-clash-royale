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
            var lastKnownPosition = target.position;
            while (true)
            {
                if (target != null)
                    lastKnownPosition = target.position;
                
                transform.position = Vector3.MoveTowards(transform.position, lastKnownPosition, Speed * Time.deltaTime);
                
                if (Vector3.Distance(transform.position, lastKnownPosition) > 0.01f)
                {
                    var direction = (lastKnownPosition - transform.position).normalized;
                    // Calculate the angle in degrees
                    // and adjust it to point upwards (0 degrees is right, -90 degrees is up)
                    var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg-90f;
                    var targetRotation = Quaternion.Euler(0f, 0f, angle);
                    transform.rotation = targetRotation;
                }
                
                if (Vector3.Distance(transform.position, lastKnownPosition) < 0.1f)
                {
                    OnParticleFollowedTarget?.Invoke();
                    //Debug.Log($"Particle reached destination at {lastKnownPosition}");
                    Destroy(gameObject);
                    yield break;
                }
                yield return null;
            }
        }
    }
}