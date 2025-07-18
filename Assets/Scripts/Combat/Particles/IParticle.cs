using System.Collections;
using UnityEngine;

namespace Combat.Particles
{
    public interface IParticle
    {
        public float Speed { get; set; } // Speed of the particle
        public void FollowTarget(Transform target); // Method to follow a target
    }
}