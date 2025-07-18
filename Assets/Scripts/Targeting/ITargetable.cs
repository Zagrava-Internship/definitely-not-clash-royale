using UnityEngine;

namespace Targeting
{
    /// <summary>
    /// Contract for all entities that can be targeted and damaged by units (e.g., units, buildings, etc.).
    /// </summary>
    public interface ITargetable
    {
        // Transform of the target
        Transform Transform { get; }
        
        // Is the target dead?
        bool IsDead { get; }
        
        // Method to apply damage to the target
        void TakeDamage(int damage);
    }
}