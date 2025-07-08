using UnityEngine;

namespace Units
{
    /// <summary>
    /// Contract for all entities that can be targeted and damaged by units (e.g., units, buildings, etc.).
    /// </summary>
    public interface ITargetable
    {
        // World position of the target 
        Vector3 Position { get; }
       
        // Apply damage to the target
        void TakeDamage(float amount);
        
        // Is the target dead?
        bool IsDead { get; }

    }
}